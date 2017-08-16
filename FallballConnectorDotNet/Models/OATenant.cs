using Newtonsoft.Json;

namespace FallballConnectorDotNet.Models
{
    public class OaTenant
    {
        [JsonProperty("aps")]
        public OaAps Aps { get; set; }
        
        [JsonProperty("app")]
        public OaLink AppLink { get; set; }
        
        [JsonProperty("account")]
        public OaLink AccountLink { get; set; }
        
        [JsonProperty("subscription")]
        public OaLink SubscriptionLink { get; set; }
        
        [JsonProperty("USERS")]
        public OaLimit UsersLimit { get; set; }
    }

    public class OaLimit
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
    
    public class OaAccount
    {
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
    }
}