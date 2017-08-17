using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Authorization;


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
                _logger.LogInformation(
                    $"===>>> REQUEST METHOD: {context.Request.Method}, REQUEST URL: {url}, REQUEST BODY: {requestBodyText}, ");

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;
                
                await _next(context);
                //context.Request.Body = originalRequestBody;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(new {error = exception.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
    
    public class OutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OutMiddleware> _logger;

        public OutMiddleware(RequestDelegate next, ILogger<OutMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            _logger.LogInformation($"<<<== RESPONSE LOG: {responseBody}");
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(bodyStream);
        }
    }
}
