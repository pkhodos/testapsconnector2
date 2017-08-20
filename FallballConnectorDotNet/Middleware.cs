using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace FallballConnectorDotNet
{
    public class InMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InMiddleware> _logger;


        public InMiddleware(RequestDelegate next, ILogger<InMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                // logging incoming requests
                var requestBodyStream = new MemoryStream();
                var originalRequestBody = context.Request.Body;

                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);

                var url = UriHelper.GetDisplayUrl(context.Request);
                var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                var auth = context.Request.Headers.ContainsKey("Authorization")? (string) context.Request.Headers["Authorization"] : "" ;
                
                _logger.LogInformation(
                    $"===>>> REQUEST METHOD: {context.Request.Method}, AUTH: {auth}, REQUEST URL: {url},\n REQUEST BODY: {requestBodyText}, ");

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;
                
                await _next(context);
                context.Request.Body = originalRequestBody;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(_logger, context, ex);
            }
        }

        private static Task HandleExceptionAsync(ILogger<InMiddleware> logger, HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(new {error = exception.Message});
            logger.LogError("<<<=== RESPONSE FAILED {0}", result);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
