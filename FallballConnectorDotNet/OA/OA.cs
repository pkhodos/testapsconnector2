using OAuth;
using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using APSConnector.Controllers;
using Microsoft.AspNetCore.Http;

namespace APSConnector.Models
{
    public class OA
    {
        static public dynamic GetResource(Config _config, HttpRequest request, string resource)
        {
            string url = "aps/2/resources/" + resource;

            return OA.SendRequest(_config, request, "GET", url);
        }

        static public dynamic SendRequest(Config _config, HttpRequest r, string method, string path, string body = "" )
        {
            string oa_host = "https://api-isv-oa.aps.odin.com/";
            string url = oa_host + path;

            string header = "";

            {
               OAuthBase oauthBase = new OAuthBase();
                string timestamp = oauthBase.GenerateTimeStamp();
                string nonce = oauthBase.GenerateNonce();
                string consumerKey = _config.config["oauth_key"];
                string consumerSecret = _config.config["oauth_secret"];
                string normalizedUrl;
                string normalizedRequestParameters;

                // generating signature based on requst parameters
                string sig = oauthBase.GenerateSignature(
                        new Uri(url),
                        consumerKey,
                        consumerSecret,
                        string.Empty, string.Empty,
                        method,
                        timestamp,
                        nonce,
                        out normalizedUrl, out normalizedRequestParameters);

                header = String.Format(
                    "oauth_consumer_key=\"{0}\"" +
                   ",oauth_signature_method=\"HMAC-SHA1\"" +
                   ",oauth_timestamp=\"{1}\"" +
                   ",oauth_nonce=\"{2}\"" +
                   ",oauth_version=\"1.0\"" +
                   ",oauth_signature=\"{3}\"",
                   consumerKey, timestamp, nonce, sig);
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", header);
            request.Headers.Add("Aps-Transaction-Id", r.Headers["Aps-Transaction-Id"].ToString() );

            request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            _config.logger.LogInformation("OA BEGIN REQUEST to {0}", url);
            _config.logger.LogInformation("OA BODY: {0}", body);

            var response = client.SendAsync(request).Result;
                

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                _config.logger.LogInformation("OA RESPONSE OK: {0}", result);
                return JsonConvert.DeserializeObject(result);
            }
            else
            {
                string result = response.Content.ReadAsStringAsync().Result;
                _config.logger.LogInformation("OA RESPONSE FAIL: {0}", result);
                throw new WebException(response.Content.ReadAsStringAsync().Result);
            }

        }
    }
}