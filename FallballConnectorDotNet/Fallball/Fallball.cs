using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FallballConnectorDotNet.Controllers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Fallball
{
    public class Fallball
    {
        public static string Call(Setting setting, HttpMethod method, string url, string body)
        {
            return Call(setting, method, url, body, null);
        }
        
        public static string Call(Setting setting, HttpMethod method, string url)
        {
            return Call(setting, method, url, "", null);
        }
        
        public static string Call(Setting setting, HttpMethod method, string url, string body, string token)
        {
            var client = new HttpClient();

            url = setting.Config["fallball_service_url"] + url;
            client.BaseAddress = new Uri(url);

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Token",
                token == null ? setting.Config["fallball_service_authorization_token"] : token);

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            setting.Logger.LogInformation("FALLBALL BEGIN REQUEST to {0}", url);
            setting.Logger.LogInformation("FALLBALL BODY: {0}", body);

            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                setting.Logger.LogInformation("FALLBALL RESPONSE: {0}", result);
                return result;
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                setting.Logger.LogInformation("FALLBALL FAIL: {0}", result);

                var error = string.Format("Call to Fallball failed. URL: {0}, BODY: {1}, RESPONSE: {2}",
                    url, body, result);

                throw new WebException(error);
            }
        }
    }
}