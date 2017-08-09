using APSConnector.Controllers;
using APSConnector.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace APSConnector.Fallball
{
    public class FBUser
    {
        public string email { get; set; }
        public Boolean admin { get; set; }
        public Storage storage { get; set; }

        public static string GetID(User oa)
        {
            return oa.userId;
        }

        public static string Create(Config _config, User user)
        {
            FBUser u = new FBUser { email = user.email, admin = true, storage = new Storage { limit = 1 } };

            dynamic fbUser = Fallball.Call(_config, "POST",
                String.Format("resellers/{0}/clients/{1}/users/", 
                    FBReseller.GetID(user.tenant.app), 
                    FBClient.GetID(user.tenant) ),
                JsonConvert.SerializeObject(u) );

            return Convert.ToString(fbUser.user_id);
        }
        
        public static void Delete(Config _config, User user)
        {
            Fallball.Call(_config, "DELETE",
                String.Format("resellers/{0}/clients/{1}/users/{2}", 
                    FBReseller.GetID(user.tenant.app), 
                    FBClient.GetID(user.tenant),
                    FBUser.GetID(user) ) );
        }

        public static string GetUserLogin(Config _config, User user )
        {
            dynamic url = Fallball.Call(_config, "GET", String.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FBReseller.GetID(user.tenant.app),
                FBClient.GetID(user.tenant),
                FBUser.GetID(user)
                ) );

            return Convert.ToString(url);
        }
    }
}