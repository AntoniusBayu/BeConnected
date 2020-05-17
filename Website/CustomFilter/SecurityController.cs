using Microsoft.AspNetCore.Mvc.Filters;

namespace Website
{
    public class SecurityController : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do Something After Method being executed
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do Something before Method being executed
        }
    }
}