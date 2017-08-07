using APSConnector.Controllers;
using APSConnector.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace APSConnector.Fallball
{
    public class FBReseller
    {
        public string name { get; set; }
        public string rid { get; set; }
        public Storage storage { get; set; }

        public static string GetID(Application oa)
        {
            return oa.apsID;
        }

        public static string Create(Config config, Application app)
        {
            FBReseller r = new FBReseller { name = FBReseller.GetID(app), rid = FBReseller.GetID(app), storage = new Storage { limit = 100000 } };
            string body = JsonConvert.SerializeObject(r);
            dynamic response = Fallball.Call(config, "POST", "resellers/", body);

            return Convert.ToString(response.name);
        }
    }
}