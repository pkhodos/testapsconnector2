using APSConnector.Controllers;
using APSConnector.Models;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace APSConnector.Fallball
{
    public class FBClient
    {
        public string name { get; set; }
        public Storage storage { get; set; }

        public static string GetID(Tenant oa)
        {
            return oa.apsID;
        }

        public static string Create(Config config, Tenant tenant)
        {
            FBClient c = new FBClient { name = FBClient.GetID(tenant), storage = new Storage { limit = 10 } };
            string body = JsonConvert.SerializeObject(c);

            dynamic fbReseller = Fallball.Call(config, "GET", String.Format("resellers/{0}", FBReseller.GetID(tenant.app) ));
            dynamic response = Fallball.Call(config,  "POST", String.Format("resellers/{0}/clients/", FBReseller.GetID(tenant.app)), body, Convert.ToString(fbReseller.token));

            return Convert.ToString(response.name);
        }

        public static string GetAdminLogin(Config _config,  Tenant tenant)
        {
            string adminlogin = String.Format("admin@{0}.{1}.fallball.io", FBClient.GetID(tenant), FBReseller.GetID(tenant.app));

            string user_id = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(adminlogin));
                Guid result = new Guid(hash);
                user_id = result.ToString();
            }

            dynamic url = Fallball.Call(_config, "GET", String.Format("resellers/{0}/clients/{1}/users/{2}/link",
                FBReseller.GetID(tenant.app),
                FBClient.GetID(tenant),
                user_id
                ) );

            return Convert.ToString(url);
        }
    }
}