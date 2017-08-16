using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FallballConnectorDotNet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OAuth;

namespace FallballConnectorDotNet.OA
{
    public class Oa
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

            {
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
            }

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
    }
}