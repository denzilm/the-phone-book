using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ThePhoneBook.WebApi.Helpers.Validation
{
    public class ValidationProblemDetailsResult : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            KeyValuePair<string, ModelStateEntry>[] modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();

            List<ValidationError> errors = new List<ValidationError>(modelStateEntries.Length);
            string details = "See ValidationErrors for details";

            if (modelStateEntries.Any())
            {
                if (modelStateEntries.Length == 1 && modelStateEntries[0].Value.Errors.Count == 1 &&
                    modelStateEntries[0].Key == string.Empty)
                {
                    details = modelStateEntries[0].Value.Errors[0].ErrorMessage;
                }
                else
                {
                    foreach (KeyValuePair<string, ModelStateEntry> modelStateEntry in modelStateEntries)
                    {
                        foreach (ModelError modelError in modelStateEntry.Value.Errors)
                        {
                            ValidationError error = new ValidationError
                            {
                                Name = modelStateEntry.Key,
                                Description = modelError.ErrorMessage
                            };

                            errors.Add(error);
                        }
                    }
                }
            }

            ValidationProblemDetails problemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request Validation Error",
                Instance = context.HttpContext.Request.Path,
                Detail = details,
                ValidationErrors = errors
            };

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }
    }
}
