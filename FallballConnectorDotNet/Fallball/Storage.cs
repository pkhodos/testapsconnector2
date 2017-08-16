using Newtonsoft.Json;

namespace FallballConnectorDotNet.Fallball
{
    public class Storage
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}