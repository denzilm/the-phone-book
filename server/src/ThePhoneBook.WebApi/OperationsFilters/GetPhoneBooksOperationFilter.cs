using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;

namespace ThePhoneBook.WebApi.OperationsFilters
{
    public class GetPhoneBooksOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "GetPhoneBooks")
            {
                return;
            }

            operation.Responses[StatusCodes.Status200OK.ToString()]
                .Content.Add("application/vnd.the-phone-book.phonebookswithentries+json", new OpenApiMediaType
                {
                    Schema = context.SchemaRegistry.GetOrRegister(typeof(PhoneBookWithEntriesResponse))
                });
        }
    }
}