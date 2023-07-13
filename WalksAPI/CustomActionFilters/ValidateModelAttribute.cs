using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WalksAPI.CustomActionFilters {
    public class ValidateModelAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            if (!context.ModelState.IsValid) {
                context.Result = new BadRequestResult();
            }
        }
    }
}
