using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.Persistence.Identity;
using ThePhoneBook.WebApi.Api.Users.Dtos;
using ThePhoneBook.WebApi.Helpers.Auth;

namespace ThePhoneBook.WebApi.Users
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiVersion("1")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public UsersController(
            ILogger<UsersController> logger,
            IMapper mapper,
            UserManager<ApplicationIdentityUser> userManager,
            IUserRepository userRepository,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
            _jwtIssuerOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtIssuerOptions);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///         "firstname": "firstname",
        ///         "lastname": "lastname",
        ///         "email": "email@example.com",
        ///         "password": "P@ssw0rd",
        ///         "confirmpassword": "P@ssword"
        ///     }
        /// </remarks>
        /// <param name="registerModel">The user to register</param>
        [HttpPost("register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest registerModel)
        {
            _logger.LogInformation("Register new user with email {Email}", registerModel.Email);

            ApplicationIdentityUser identityUser = new ApplicationIdentityUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            IdentityResult result = await _userManager
                .CreateAsync(identityUser, registerModel.Password)
                .ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            User user = _mapper.Map<User>(identityUser);

            await _userRepository.AddAsync(user).ConfigureAwait(false);
            await _userRepository.SaveChangesAsync().ConfigureAwait(false);

            _logger.LogInformation("User registration for user with email {Email} was successful",
                registerModel.Email);

            return Ok(_mapper.Map<RegisterResponse>(user));
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///         "email": "email@example.com",
        ///         "password": "P@ssw0rd"
        ///     }
        ///
        /// </remarks>
        /// <param name="loginModel">The user to log in</param>
        /// <returns>An access token to use for authenticated requests</returns>
        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginModel)
        {
            _logger.LogInformation("User with email {Email} attempting to login", loginModel.Email);

            // Check whether the user who wants to log in exists
            ApplicationIdentityUser identityUser = await _userManager
                .FindByEmailAsync(loginModel.Email).ConfigureAwait(false);

            if (identityUser != null) // if exists
            {
                // validate password
                bool correctPassword = await _userManager
                    .CheckPasswordAsync(identityUser, loginModel.Password)
                    .ConfigureAwait(false);

                if (correctPassword)
                {
                    User user = (await _userRepository
                        .GetAllByAsync(u => u.IdentityId.Equals(identityUser.Id, StringComparison.OrdinalIgnoreCase))
                        .ConfigureAwait(false))
                        .First();

                    // Create access token for successfully validated user
                    AccessToken accessToken = await GenerateAccessToken(user);

                    return Ok(new LoginResponse(accessToken));
                }

                _logger.
                    LogInformation("User with email {Email} attempted to login with the wrong credentials",
                    loginModel.Email);
            }

            _logger.LogInformation("Login failed for user with email {0} ", loginModel.Email);

            // The user does not exist in our system so the request is unauthorized
            return Unauthorized();
        }

        private async Task<AccessToken> GenerateAccessToken(User user)
        {
            ClaimsIdentity claimsIdentity = new GenericIdentity(user.EmailAddress, "Token");
            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.EmailAddress),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat,
                    ToUnixEpochDate(_jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                 new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                 new Claim("id", user.Id.ToString())
            };

            // Create the JWT security token and encode it.
            JwtSecurityToken jwt = new JwtSecurityToken(
                _jwtIssuerOptions.Issuer,
                _jwtIssuerOptions.Audience,
                claims,
                _jwtIssuerOptions.NotBefore,
                _jwtIssuerOptions.Expiration,
                _jwtIssuerOptions.SigningCredentials);

            return new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwt),
                (int)_jwtIssuerOptions.ValidFor.TotalSeconds);
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.",
                    nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}