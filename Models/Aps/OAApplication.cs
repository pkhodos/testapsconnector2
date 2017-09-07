using Newtonsoft.Json;

namespace FallballConnectorDotNet.Models.Aps
{
    public class OaApplication
    {
        [JsonProperty("aps")]
        public OaAps Aps { get; set; }
        
        [JsonProperty("tenants")]
        public OaLink TenantsLink { get; set; }
    }

    public class OaAps
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("revision")]
        public string Revision { get; set; }
    }

    public class OaLink
    {
        [JsonProperty("aps")]
        public OaApsLink ApsLink { get; set; }
    }
    
    public class OaApsLink
    {
        [JsonProperty("link")]
        public string Link { get; set; }
        
        [JsonProperty("href")]
        public string Href { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}