using Newtonsoft.Json;

namespace FallballConnectorDotNet.Models
{
    public class OaUser
    {
        [JsonProperty("aps")]
        public OaAps Aps { get; set; }
        
        [JsonProperty("resource")]
        public string Resource { get; set; }
        
        [JsonProperty("tenant")]
        public OaLink TenantLink { get; set; }
        
        [JsonProperty("user")]
        public OaLink AdminUserLink { get; set; }
        
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
    
    public class OaAdminUser
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}