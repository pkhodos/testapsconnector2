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
    public static class Fallball
    {
        public static T Call<T>(Setting setting, HttpMethod method, string url, string body)
        {
            return Call<T>(setting, method, url, body, null);
        }
        
        public static T Call<T>(Setting setting, HttpMethod method, string url)
        {
            return Call<T>(setting, method, url, "", null);
        }
        
        public static T Call<T>(Setting setting, HttpMethod method, string url, string body, string token)
        {
            var client = new HttpClient();

            url = setting.Config["fallball_service_url"] + url;
            client.BaseAddress = new Uri(url);

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Token",
                token == null ? setting.Config["fallball_service_authorization_token"] : token);

            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            setting.Logger.LogInformation("FALLBALL REQUEST {0} to {1}",method.ToString(), url);
            setting.Logger.LogInformation("FALLBALL BODY: {0}", body);

            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                setting.Logger.LogInformation("FALLBALL RESPONSE: {0}", result);
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                setting.Logger.LogInformation("FALLBALL FAIL: {0}", result);

                var error = string.Format("Call to Fallball failed. METHOD: {0}, URL: {1}, BODY: {2}, RESPONSE: {3}",
                    method.ToString(), url, body, result);

                throw new WebException(error);
            }
        }
    }
}