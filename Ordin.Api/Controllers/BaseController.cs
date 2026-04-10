using ErrorOr;
using Ordin.Api.Contracts;
using Ordin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace Ordin.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IDispatcher _dispatcher;

        public BaseController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            var requestId = Activity.Current?.Id;

            var result = new ApiResponse<object>(value, requestId!, DateTimeOffset.UtcNow);

            return base.Ok(result);
        }

        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
                return Problem();

            if (errors.All(e => e.Type == ErrorType.Validation))
                return ValidationProblem(errors);

            var firstError = errors.First();
            return Problem(firstError);
        }

        private ObjectResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private ActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
                modelStateDictionary.AddModelError(error.Code, error.Description);

            return ValidationProblem(modelStateDictionary);
        }
    }
}
