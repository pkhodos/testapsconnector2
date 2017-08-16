using System;
using System.Net.Http;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Models;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Fallball
{
    public class FbReseller
    {
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("rid")]
        public string Rid { get; set; }
        
        [JsonProperty("storage")]
        public Storage Storage { get; set; }
        
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        public static string GetId(Application oa)
        {
            return oa.ApsId;
        }

        public static string Create(Setting setting, Application app)
        {
            var r = new FbReseller {Name = GetId(app), Rid = GetId(app), Storage = new Storage {Limit = 100000}};
            var body = JsonConvert.SerializeObject(r);
            
            FbReseller fbReseller = Fallball.Call<FbReseller>(setting, HttpMethod.Post, "resellers/", body);

            return fbReseller.Name;
        }
    }
}