using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ThePhoneBook.WebApi.Services
{
    public class UserInfoService : IUserInfoService
    {
        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            UserId = httpContext
                .User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            FirstName = httpContext.User
                .Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;

            LastName = httpContext
                .User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
        }

        public string UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }
    }
}