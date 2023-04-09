using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Shiki.Data;
using System.Threading.Tasks;

namespace Shiki.Service
{
    public class LavalinkService
    {
        public DiscordClient DiscordClient { get; set; }
        
        public LavalinkNodeConnection LavalinkNodeConnection { get; set; }

        public ConnectionConfig ConnectionConfig { get; set; }

        public LavalinkService(ConnectionConfig connectionConfig, DiscordClient discordClient)
        {
            this.ConnectionConfig = connectionConfig;
            this.DiscordClient = discordClient;
            this.DiscordClient.Ready += this.ConnectLavalink;
        }

        private Task ConnectLavalink(DiscordClient sender, ReadyEventArgs eventArgs)
        {
            if (LavalinkNodeConnection == null)
                Task.Run(async () =>
                {
                    var lavalink = sender.GetLavalink();
                    var endpoint = new ConnectionEndpoint
                    {
                        Port = ConnectionConfig.Port,
                        Hostname = ConnectionConfig.IpAdress
                    };
                    var config = new LavalinkConfiguration
                    {
                        RestEndpoint = endpoint,
                        SocketEndpoint = endpoint,
                        Password = ConnectionConfig.LavalinkPassword
                    };
                    LavalinkNodeConnection = await lavalink.ConnectAsync(config);
                });
            return Task.CompletedTask;
        }

    }
}