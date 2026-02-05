using ECommerce.Shared.CommonResposes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseControllers : ControllerBase
    {
        // Handle Result Without Value
        // If Result => Success With No Content 204
        // If Result Failure => return problem Details With desc , Status Code

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);

        }

        // Handle Result With Value
        // If Result Is Success => Wiht Value Ok 200
        // If Result Failure => return problem Details With desc , Status Code

        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                    return HandleProblem(result.Errors);
        }


        protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {

            // If No Errors Are Provided , return 500 Error

            if (errors.Count == 0)
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An Error Occured"
                    );

            // if All Errors are Validation Errors Handle Them as a Validation Problem

            if(errors.All(E => E.ErrorType == ErrorType.Validation))
            {
                return HandleValidationErrors(errors);
            }

            // If There Is Only One Error , Handle As a Single Error Problem
            return HandleSingleError(errors[0]);

        }

        private ActionResult HandleSingleError(Error error)
        {
            return Problem(

                title: error.Code,
                detail: error.Description,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeIntoStatusCode(error.ErrorType)

            );
        }

        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code , error.Description);
            }

            return ValidationProblem(modelState);
        }
        private static int MapErrorTypeIntoStatusCode(ErrorType errorType)
        => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.UnAthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.InvalidCredintals => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
