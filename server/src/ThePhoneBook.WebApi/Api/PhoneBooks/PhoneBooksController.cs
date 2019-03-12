using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePhoneBook.Core.Entities;
using ThePhoneBook.Core.Repositories;
using ThePhoneBook.WebApi.Api.Helpers;
using ThePhoneBook.WebApi.Api.PhoneBookEntries.Dtos;
using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;
using ThePhoneBook.WebApi.Helpers;
using ThePhoneBook.WebApi.Services;

namespace ThePhoneBook.WebApi.Api.PhoneBooks
{
    [Route("api/v{version:apiVersion}/phone-books")]
    [ApiController]
    [ApiVersion("1")]
    public class PhoneBooksController : ControllerBase
    {
        private readonly ILogger<PhoneBooksController> _logger;
        private readonly IUserInfoService _userInfoService;
        private readonly IPhoneBookRepository _phoneBookRepository;
        private readonly IPhoneBookEntryRepository _phoneBookEntryRepository;
        private readonly IMapper _mapper;

        public PhoneBooksController(
            ILogger<PhoneBooksController> logger,
            IUserInfoService userInfoService,
            IPhoneBookRepository phoneBookRepository,
            IPhoneBookEntryRepository phoneBookEntryRepository,
            IMapper mapper)
        {
            _logger = logger;
            _userInfoService = userInfoService;
            _phoneBookRepository = phoneBookRepository;
            _phoneBookEntryRepository = phoneBookEntryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retreives all phone books for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books?page=1
        /// </remarks>
        /// <response code="200">Returns a paged list of the users phone books</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept,
            new[] { "application/vnd.the-phone-book.phonebooks+json", "application/json" })]
        [Produces("application/json", "application/vnd.the-phone-book.phonebooks+json")]
        public async Task<ActionResult<PhoneBookResponse>> GetPhoneBooks([FromQuery]PagingRequest pagingRequest)
        {
            Guid userId = Guid.Parse(_userInfoService.UserId);
            int phoneBooksCount = await _phoneBookRepository.CountForUserAsync(userId)
                .ConfigureAwait(false);

            IReadOnlyList<PhoneBook> phoneBooks = await _phoneBookRepository
                .GetPhoneBooksForUser(userId, pagingRequest.Page, pagingRequest.PageSize)
                .ConfigureAwait(false);

            PagingInfo pagingInfo = new PagingInfo(phoneBooksCount, pagingRequest.Page, pagingRequest.PageSize);
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(pagingInfo,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            return Ok(_mapper.Map<IEnumerable<PhoneBookResponse>>(phoneBooks));
        }

        /// <summary>
        /// Retreives all phone books with their entries
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books?page=1
        /// </remarks>
        /// <response code="200">Returns a paged list of the users phone books with related entries</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept,
            new[] { "application/vnd.the-phone-book.phonebookswithentries+json" })]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Produces("application/vnd.the-phone-book.phonebookswithentries+json")]
        public async Task<ActionResult<IEnumerable<PhoneBookWithEntriesResponse>>> GetPhoneBooksWithEntries(
            [FromQuery]PagingRequest pagingRequest)
        {
            Guid userId = Guid.Parse(_userInfoService.UserId);
            int phoneBooksCount = await _phoneBookRepository.CountForUserAsync(userId)
                .ConfigureAwait(false);

            IReadOnlyList<PhoneBook> phoneBooks = await _phoneBookRepository
                .GetPhoneBooksForUserWithEntries(userId, pagingRequest.Page, pagingRequest.PageSize)
                .ConfigureAwait(false);

            PagingInfo pagingInfo = new PagingInfo(phoneBooksCount, pagingRequest.Page, pagingRequest.PageSize);
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(pagingInfo,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));

            return Ok(_mapper.Map<IEnumerable<PhoneBookWithEntriesResponse>>(phoneBooks));
        }

        /// <summary>
        /// Retreives all phone books with their entries
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books?page=1
        /// </remarks>
        /// <response code="200">Returns a paged list of the users phone books with related entries</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PhoneBookWithEntriesResponse>>> SearchPhoneBooksForContact(
            [FromQuery]string query)
        {
            Guid userId = Guid.Parse(_userInfoService.UserId);

            IReadOnlyList<PhoneBookEntry> phoneBookEntries = await _phoneBookEntryRepository
                .SearchPhoneBookEntries(userId, query)
                .ConfigureAwait(false);

            return Ok(_mapper.Map<IEnumerable<PhoneBookEntryWithPhoneBookResponse>>(phoneBookEntries));
        }

        /// <summary>
        /// Retreives a specific phone book for user
        /// </summary>
        /// <param name="id">The id of the phone book</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93
        /// </remarks>
        /// <response code="200">Returns the requested phone book</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpGet("{id}", Name = "GetPhoneBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept,
            new[] { "application/vnd.the-phone-book.phonebook+json", "application/json" })]
        [Produces("application/json", "application/vnd.the-phone-book.phonebook+json")]
        public async Task<ActionResult<PhoneBookResponse>> GetPhoneBook(Guid id)
        {
            // Check if phone book exists
            PhoneBook phoneBook = await _phoneBookRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (phoneBook == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBook.UserId == userId)
            {
                return Ok(_mapper.Map<PhoneBookResponse>(phoneBook));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to request a phone book owned by {OwningUser}",
                userId, phoneBook.UserId);

            // phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Retreives a specific phone book with entries for user
        /// </summary>
        /// <param name="id">The id of the phone book</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /phone-books/c7ba6add-09c4-45f8-8dd0-eaca221e5d93
        /// </remarks>
        /// <response code="200">Returns the requested phone book with related entries</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpGet("{id}", Name = "GetPhoneBookWithEntries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept,
            new[] { "application/vnd.the-phone-book.phonebookwithentries+json" })]
        [Produces("application/vnd.the-phone-book.phonebookwithentries+json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<PhoneBookWithEntriesResponse>> GetPhoneBookWithEntries(Guid id)
        {
            // Check if the phone book exists
            PhoneBook phoneBook = await _phoneBookRepository
                .GetPhoneBookWithEntries(id)
                .ConfigureAwait(false);

            phoneBook.PhoneBookEntries = phoneBook.PhoneBookEntries
                .OrderBy(e => e.LastName)
                .ToList();

            if (phoneBook == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBook.UserId == userId)
            {
                return Ok(_mapper.Map<PhoneBookWithEntriesResponse>(phoneBook));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to request a phone book owned by {OwningUser}",
                userId, phoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Deletes an existing phone book
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /phone-books/id
        ///
        /// </remarks>
        /// <param name="id">The id of the phone book to delete</param>
        /// <response code="204">No content in response body</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePhoneBook(Guid id)
        {
            // Check if the phone book exists
            PhoneBook phoneBook = await _phoneBookRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (phoneBook == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBook.UserId == userId)
            {
                _phoneBookRepository.Delete(phoneBook);
                await _phoneBookRepository.SaveChangesAsync().ConfigureAwait(false);

                return NoContent();
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to delete a phone book owned by {OwningUser}",
                userId, phoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Updates an existing phone book
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /phone-books/id
        ///     {
        ///         "title": "The phone book title",
        ///         "description": "The phone book description"
        ///     }
        /// </remarks>
        /// <param name="id">The id of the phone book to update</param>
        /// <param name="phoneBookUpdateRequest">The info to update the phone book with</param>
        /// <response code="200">Returns the updated phone book</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        /// <response code="404">The requested resource does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/vnd.the-phone-book.phonebookforupdate+json")]
        [Produces("application/vnd.the-phone-book.phonebook+json")]
        public async Task<ActionResult<PhoneBookResponse>> UpdatePhoneBook(Guid id,
            [FromBody] PhoneBookUpdateRequest phoneBookUpdateRequest)
        {
            // Check if the phone book exists
            PhoneBook phoneBook = await _phoneBookRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (phoneBook == null)
            {
                return NotFound();
            }

            // Check if the phone book belongs to the current user
            Guid userId = Guid.Parse(_userInfoService.UserId);

            if (phoneBook.UserId == userId)
            {
                _mapper.Map(phoneBookUpdateRequest, phoneBook);
                _phoneBookRepository.Update(phoneBook);
                await _phoneBookRepository.SaveChangesAsync();

                return Ok(_mapper.Map<PhoneBookResponse>(phoneBook));
            }

            _logger.LogWarning("User with id {ForbiddenUser} attempted to update a phone book owned by {OwningUser}",
                userId, phoneBook.UserId);

            // the phone book does not belong to the user
            // forbidden request
            return Forbid();
        }

        /// <summary>
        /// Creates a new phone book
        /// </summary>
        /// <param name="phoneBookCreateRequest">The info to create the phone book with</param>
        /// <response code="201">The phone book successfully created</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [RequestHeaderMatchesMediaType(HeaderNames.ContentType,
            new[] { "application/vnd.the-phone-book.phonebookforcreation+json", "application/json" })]
        [Consumes("application/vnd.the-phone-book.phonebookforcreation+json", "application/json")]
        public async Task<ActionResult<PhoneBookResponse>> CreatePhoneBook(
            [FromBody] PhoneBookCreateRequest phoneBookCreateRequest)
        {
            PhoneBook phoneBook = _mapper.Map<PhoneBook>(phoneBookCreateRequest);

            phoneBook = await _phoneBookRepository
                .CreatePhoneBookForUser(Guid.Parse(_userInfoService.UserId), phoneBook)
                .ConfigureAwait(false);

            await _phoneBookRepository.SaveChangesAsync();

            return CreatedAtRoute("GetPhoneBook", new { id = phoneBook.Id }, _mapper.Map<PhoneBookResponse>(phoneBook));
        }

        /// <summary>
        /// Creates a new phone book with related entries
        /// </summary>
        /// <param name="phoneBookWithEntriesCreateRequest">The info to create the phone book with</param>
        /// <response code="201">The phone book with associated entries successfully created</response>
        /// <response code="403">The user is not allowed to access this resource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [RequestHeaderMatchesMediaType(HeaderNames.ContentType,
            new[] { "application/vnd.the-phone-book.phonebookforcreationwithentries+json" })]
        [Consumes("application/vnd.the-phone-book.phonebookforcreationwithentries+json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<PhoneBookWithEntriesResponse>> CreatePhoneBookWithEntries(
            [FromBody] PhoneBookWithEntriesCreateRequest phoneBookWithEntriesCreateRequest)
        {
            PhoneBook phoneBook = _mapper.Map<PhoneBook>(phoneBookWithEntriesCreateRequest);

            phoneBook = await _phoneBookRepository
                .CreatePhoneBookForUser(Guid.Parse(_userInfoService.UserId), phoneBook)
                .ConfigureAwait(false);

            await _phoneBookRepository.SaveChangesAsync();

            return CreatedAtRoute("GetPhoneBookWithEntries", new { id = phoneBook.Id },
                _mapper.Map<PhoneBookWithEntriesResponse>(phoneBook));
        }
    }
}