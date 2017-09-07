using System;
using System.Linq;
using System.Reflection;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace FallballConnectorDotNet
{
    public class OAuthValidationFilter : ActionFilterAttribute
    {
        private readonly IConfiguration _config;
 
        public OAuthValidationFilter(IConfiguration config)
        {
            _config = config;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null && controllerActionDescriptor.MethodInfo
                .GetCustomAttributes<AllowAnonimousAttribute>().Any())
            {
                // do not validate actions with 'AllowAnonimousAttribute'
                return;
            }
            
            Oa.ValidateRequest(_config, context);
            
            base.OnActionExecuting(context);
        }
    }

    public class AllowAnonimousAttribute : Attribute
    {
    }
}