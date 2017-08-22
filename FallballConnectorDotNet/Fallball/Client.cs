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
        
        [JsonProperty("users_amount", NullValueHandling = NullValueHandling.Ignore)]
        public int? UsersAmount { get; set; }
        
        public static string GetId(Tenant tenant)
        {
            return tenant.Id;
        }

        public static Usage GetUsage(Setting setting, Tenant tenant)
        {
            var fbClient =  Fallball.Call<FbClient>(
                setting,
                HttpMethod.Get,
                string.Format("resellers/{0}/clients/{1}", 
                    FbReseller.GetId(tenant.App), GetId(tenant)));
            
            Usage u = new Usage();
            u["USERS"] = fbClient.UsersAmount;
            u["DISKSPACE"] = fbClient.Storage.Usage;

            return u;
        }
        
        public static string Create(Setting setting, Tenant tenant)
        {
            var c = new FbClient {Name = GetId(tenant), Storage = new Storage {Limit = tenant.DiskspaceLimit}};
            var body = JsonConvert.SerializeObject(c);

            var fbReseller = Fallball.Call<FbReseller>(
                setting,
                HttpMethod.Get,
                string.Format("resellers/{0}", FbReseller.GetId(tenant.App)));
            
            var fbClient = Fallball.Call<FbClient>(
                setting, 
                HttpMethod.Post, 
                string.Format("resellers/{0}/clients/", FbReseller.GetId(tenant.App)),
                body, 
                fbReseller.Token);
            
            return fbClient.Name;
        }
        
        public static void Update(Setting setting, Tenant tenant)
        {
            var c = new FbClient {Name = GetId(tenant), Storage = new Storage {Limit = tenant.DiskspaceLimit}};
            var body = JsonConvert.SerializeObject(c);

            var fbReseller = Fallball.Call<FbReseller>(
                setting,
                HttpMethod.Get,
                string.Format("resellers/{0}", FbReseller.GetId(tenant.App)));
            
            Fallball.Call<FbClient>(
                setting, 
                HttpMethod.Put, 
                string.Format("resellers/{0}/clients/{1}/", FbReseller.GetId(tenant.App), GetId(tenant)),
                body, 
                fbReseller.Token);
        }
        
        public static void Delete(Setting setting, Tenant tenant)
        {
            var fbReseller = Fallball.Call<FbReseller>(
                setting,
                HttpMethod.Get,
                string.Format("resellers/{0}", FbReseller.GetId(tenant.App)));
            
            Fallball.Call<string>(
                setting, 
                HttpMethod.Delete, 
                string.Format("resellers/{0}/clients/{1}", FbReseller.GetId(tenant.App), GetId(tenant)),
                "", 
                fbReseller.Token);
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

            return Fallball.Call<string>(setting, HttpMethod.Get, string.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FbReseller.GetId(tenant.App),
                GetId(tenant),
                userId
            ));
        }
    }
}