using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories
{
    public static class ApiResposeFactory
    {
        public static IActionResult GenerateApiValidationRespose(ActionContext actionContext)
        {
            var errors = actionContext.ModelState.Where(X => X.Value.Errors.Count > 0)
                         .ToDictionary(
                             X => X.Key,
                             X => X.Value.Errors.Select(X => X.ErrorMessage).ToArray()
                          );

            var problem = new ProblemDetails()
            {
                Title = "Validation Error",
                Detail = "One Or More Validation Errors Occured",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                        {
                            {"Errors" , errors}
                        }
            };

            return new BadRequestObjectResult(problem);
        }
    }
}
