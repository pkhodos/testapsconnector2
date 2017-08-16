using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Models;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Fallball
{
    public class FbClient
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("storage")]
        public Storage Storage { get; set; }

        public static string GetId(Tenant oa)
        {
            return oa.ApsId;
        }

        public static string Create(Setting setting, Tenant tenant)
        {
            var c = new FbClient {Name = GetId(tenant), Storage = new Storage {Limit = 10}};
            var body = JsonConvert.SerializeObject(c);

            string sReseller = Fallball.Call(
                setting,
                HttpMethod.Get,
                string.Format("resellers/{0}", FbReseller.GetId(tenant.App)));
            
            FbReseller fbReseller = JsonConvert.DeserializeObject<FbReseller>(sReseller);
            
            string sFbClient = Fallball.Call(
                setting, 
                HttpMethod.Post, 
                string.Format("resellers/{0}/clients/", FbReseller.GetId(tenant.App)),
                body, 
                fbReseller.Token);
            
            FbClient fbClient = JsonConvert.DeserializeObject<FbClient>(sFbClient);

            return Convert.ToString(fbClient.Name);
        }

        public static string GetAdminLogin(Setting setting, Tenant tenant)
        {
            var adminlogin = string.Format("admin@{0}.{1}.fallball.io", GetId(tenant), FbReseller.GetId(tenant.App));

            string userId;
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(adminlogin));
                var result = new Guid(hash);
                userId = result.ToString();
            }

            var url = Fallball.Call(setting, HttpMethod.Get, string.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FbReseller.GetId(tenant.App),
                GetId(tenant),
                userId
            ));

            return Convert.ToString(url);
        }
    }
}