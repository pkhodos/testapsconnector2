using APSConnector.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APSConnector.Fallball
{
    public class Fallball
    {
        public static dynamic Call(Config _config, string method, string url, string body = "", string token=null)
        {
            HttpClient client = new HttpClient();

            url = _config.config["fallball_service_url"] + url;
            client.BaseAddress = new Uri( url );

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", token == null ? _config.config["fallball_service_authorization_token"]: token);

            request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            _config.logger.LogInformation("FALLBALL BEGIN REQUEST to {0}", url);
            _config.logger.LogInformation("FALLBALL BODY: {0}", body);

            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                _config.logger.LogInformation("FALLBALL RESPONSE: {0}", result);
                return JsonConvert.DeserializeObject(result);
            }
            else
            {
                string result = response.Content.ReadAsStringAsync().Result;
                _config.logger.LogInformation("FALLBALL FAIL: {0}", result);

                throw new WebException(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}