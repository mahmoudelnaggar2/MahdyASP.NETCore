using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MahdyASP.NETCore.Filters
{
    public class LogSensitiveActionAttribute : ActionFilterAttribute
    {        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Sensitive Action Executed!!!!!!");    
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Sensitive Action Executed!!!!!!");
        }
    }
}
