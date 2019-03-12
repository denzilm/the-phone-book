using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ThePhoneBook.WebApi.Api.PhoneBooks.Dtos;

namespace ThePhoneBook.WebApi.OperationsFilters
{
    public class CreatePhoneBookOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "CreatePhoneBook")
            {
                return;
            }

            operation.RequestBody.Content
                .Add("application/vnd.the-phone-book.phonebookforcreationwithentries+json", new OpenApiMediaType
                {
                    Schema = context.SchemaRegistry.GetOrRegister(typeof(PhoneBookWithEntriesCreateRequest))
                });
        }
    }
}