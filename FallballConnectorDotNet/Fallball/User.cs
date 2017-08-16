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
        public string UserID { get; set; }

        public static string GetId(User oa)
        {
            return oa.UserId;
        }

        public static string Create(Setting setting, User user)
        {
            var u = new FbUser {Email = user.Email, Admin = true, Storage = new Storage {Limit = 1}};

            string sFbUser = Fallball.Call(setting, HttpMethod.Post, 
                string.Format("resellers/{0}/clients/{1}/users/",
                    FbReseller.GetId(user.Tenant.App),
                    FbClient.GetId(user.Tenant)),
                JsonConvert.SerializeObject(u));
            
            FbUser fbUser = JsonConvert.DeserializeObject<FbUser>(sFbUser);

            return Convert.ToString(fbUser.UserID);
        }

        public static void Delete(Setting setting, User user)
        {
            Fallball.Call(setting, HttpMethod.Delete, 
                string.Format("resellers/{0}/clients/{1}/users/{2}",
                    FbReseller.GetId(user.Tenant.App),
                    FbClient.GetId(user.Tenant),
                    GetId(user)));
        }

        public static string GetUserLogin(Setting setting, User user)
        {
            var url = Fallball.Call(setting, HttpMethod.Get, string.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FbReseller.GetId(user.Tenant.App),
                FbClient.GetId(user.Tenant),
                GetId(user)
            ));

            return Convert.ToString(url);
        }
    }
}