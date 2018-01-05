using DeafX.Richter.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModelValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    new ValidationErrorsViewModel()
                    {
                        errors = context.ModelState.SelectMany(kvp => kvp.Value.Errors.Select(e => new ValidationErrorViewModel()
                        {
                            field = kvp.Key,
                            errorMessage = e.ErrorMessage
                        })).ToArray()
                    });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
