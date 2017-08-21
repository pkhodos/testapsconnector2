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
        
        private const int ResellersStorage = 100000;

        public static string GetId(Application app)
        {
            return app.Id;
        }

        public static string Create(Setting setting, Application app)
        {
            var r = new FbReseller {Name = GetId(app), Rid = GetId(app), Storage = new Storage {Limit = ResellersStorage}};
            var body = JsonConvert.SerializeObject(r);
            
            var fbReseller = Fallball.Call<FbReseller>(setting, HttpMethod.Post, "resellers/", body);

            return fbReseller.Name;
        }
    }
}