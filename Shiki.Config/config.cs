using Newtonsoft.Json;

namespace Shiki.Data
{
    public class ConnectionConfig
    {
        [JsonProperty("DiscordToken")]
        public string DiscordToken { get; set; }

        [JsonProperty("IpAdress")]
        public string IpAdress { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("LavalinkPassword")]
        public string LavalinkPassword { get; set; }
        
        [JsonProperty("GeniusToken")]
        public string GeniusToken { get; set; }
    }
}