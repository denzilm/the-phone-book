using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ThePhoneBook.WebApi.Helpers.Validation
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public ICollection<ValidationError> ValidationErrors { get; set; }
    }
}
