using System;
using System.Net.Http;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Models;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Fallball
{
    public class FbUser
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("admin")]
        public bool Admin { get; set; }
        
        [JsonProperty("storage")]
        public Storage Storage { get; set; }
        
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        public static string GetId(User oa)
        {
            return oa.UserId;
        }

        private const int UserLimit = 1;

        public static string Create(Setting setting, User user)
        {
            var u = new FbUser {Email = user.Email, Admin = true, Storage = new Storage {Limit = UserLimit}};

            var fbUser = Fallball.Call<FbUser>(setting, HttpMethod.Post, 
                string.Format("resellers/{0}/clients/{1}/users/",
                    FbReseller.GetId(user.Tenant.App),
                    FbClient.GetId(user.Tenant)),
                JsonConvert.SerializeObject(u));
            
            return fbUser.UserId;
        }

        public static void Delete(Setting setting, User user)
        {
            Fallball.Call<User>(setting, HttpMethod.Delete, 
                string.Format("resellers/{0}/clients/{1}/users/{2}",
                    FbReseller.GetId(user.Tenant.App),
                    FbClient.GetId(user.Tenant),
                    GetId(user)));
        }

        public static string GetUserLogin(Setting setting, User user)
        {
            var url = Fallball.Call<String>(setting, HttpMethod.Get, string.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FbReseller.GetId(user.Tenant.App),
                FbClient.GetId(user.Tenant),
                GetId(user)
            ));

            return url;
        }
    }
}