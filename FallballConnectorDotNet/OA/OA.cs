using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FallballConnectorDotNet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.OA
{
    public static class Oa
    {
        public static T GetResource<T>(Setting setting, HttpRequest request, string resource)
        {
            var url = "aps/2/resources/" + resource;

            return SendRequest<T>(setting, request, HttpMethod.Get, url, "");
        }

        public static T SendRequest<T>(Setting setting, HttpRequest r, HttpMethod method, string path, string body)
        {
            var oaHost = r.Headers["Aps-Controller-Uri"].ToString();
            if (oaHost == "")
                throw new WebException("Header 'Aps-Controller-Uri' is not found");
            
            var url = oaHost + path;

            string header;

            var oauthBase = new OAuthBase();
            var timestamp = oauthBase.GenerateTimeStamp();
            var nonce = oauthBase.GenerateNonce();
            var consumerKey = setting.Config["oauth_key"];
            var consumerSecret = setting.Config["oauth_secret"];
            string normalizedUrl;
            string normalizedRequestParameters;

            // generating signature based on requst parameters
            var sig = oauthBase.GenerateSignature(
                new Uri(url),
                consumerKey,
                consumerSecret,
                string.Empty, string.Empty,
                method.ToString(),
                timestamp,
                nonce,
                out normalizedUrl, out normalizedRequestParameters);

            header = string.Format(
                "oauth_consumer_key=\"{0}\"" +
                ",oauth_signature_method=\"HMAC-SHA1\"" +
                ",oauth_timestamp=\"{1}\"" +
                ",oauth_nonce=\"{2}\"" +
                ",oauth_version=\"1.0\"" +
                ",oauth_signature=\"{3}\"",
                consumerKey, timestamp, nonce, sig);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                var request = new HttpRequestMessage(method, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", header);
                request.Headers.Add("Aps-Transaction-Id", r.Headers["Aps-Transaction-Id"].ToString());

                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                setting.Logger.LogInformation("OA BEGIN HOST {0}", r.Headers["Aps-Controller-Uri"].ToString());
                setting.Logger.LogInformation("OA Aps-Transaction-Id: {0}", r.Headers["Aps-Transaction-Id"].ToString());
                setting.Logger.LogInformation("OA REQUEST {0} to {1}", method.ToString(), url);
                setting.Logger.LogInformation("OA BODY: {0}", body);

                using (var response = client.SendAsync(request).Result)
                using (var content = response.Content)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = content.ReadAsStringAsync().Result;

                        setting.Logger.LogInformation("OA RESPONSE OK: {0}", result);
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                    else
                    {
                        var result = content.ReadAsStringAsync().Result;
                        setting.Logger.LogInformation("OA RESPONSE FAIL: {0}", result);
                        
                        var error = string.Format("Call to OA failed. URL: {0} RESPONSE: {1}",
                            url, result);

                        throw new WebException(error);
                    }
                }
            }
        }
        
        public static void ValidateRequest (IConfiguration config, ActionExecutingContext actionContext)
        {
            var context = actionContext.HttpContext;
            
            // OAuth validation
            if(!context.Request.Headers.ContainsKey("Authorization"))
                throw new WebException("Header 'Authorization' is absent.");
                
                
            string s = context.Request.Headers["Authorization"];
            s = s.Replace("OAuth ", "");
            s = s.Replace("\"", "");
            s = s.Replace(" ", "");

            var header = s.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);


            string normalizedUrl;
            string normalizedRequestParameters;

            // generating signature based on requst parameters
            var oauthBase = new OAuthBase();

            var url = context.Request.GetDisplayUrl();
            
            // use https if it was terminated by some tool
            if(context.Request.Headers.ContainsKey("X-Forwarded-Proto"))
                url = url.Replace("http:", "https:");
            
            var generatedSig = oauthBase.GenerateSignature(
                new Uri(url),
                header["oauth_consumer_key"],
                config["oauth_secret"],
                string.Empty, string.Empty,
                context.Request.Method,
                header["oauth_timestamp"],
                header["oauth_nonce"],
                out normalizedUrl, out normalizedRequestParameters);

            var incomingSig = WebUtility.UrlDecode(header["oauth_signature"]);
            
            if(generatedSig != incomingSig )
                throw new WebException("Authentication is failed.");
        }
    }
}