using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Presentation.ActionFilter
{
    public class ValidateMediaTypeAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");
            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing.");
            return;
            }
            string? mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present.Please add Accept header with the required media type.");
            return;
            }
            bool mediaExist = context.HttpContext.Items.Keys.Any(k => k.Equals("AcceptHeaderMediaType"));
            if (!mediaExist) 
                context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }

}
