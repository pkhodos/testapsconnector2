using System;
using System.Linq;
using System.Reflection;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet
{
    public class OAuthValidationFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
 
        public OAuthValidationFilter(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ClassConsoleLogActionOneFilter");
            _config = config;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if ((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo
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