using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using DSharpPlus.CommandsNext;

namespace Shiki
{
    class Ryougi
    {
        private DiscordClient client = new DiscordClient(new DiscordConfiguration
        {
            TokenType = TokenType.Bot,
            Token = "Дискорд токен"
        });

        private ConnectionEndpoint endpoint = new ConnectionEndpoint
        {
            Hostname = "",
            Port = 80
        };

        private LavalinkConfiguration config;

        public Ryougi()
        {
            config = new LavalinkConfiguration
            {
                Password = "",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };
        }

        public async Task MainAsync()
        {
            LavalinkExtension lavalink = client.UseLavalink();
            CommandsNextExtension comands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "&" }
            });
            comands.RegisterCommands<Service>();
            await client.ConnectAsync();
            await lavalink.ConnectAsync(config);
            await Task.Delay(-1);
        }
    }

}