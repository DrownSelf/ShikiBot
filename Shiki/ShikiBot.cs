using DSharpPlus;
using DSharpPlus.Lavalink;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Shiki.Data;
using Microsoft.Extensions.DependencyInjection;
using Shiki.Service;
using DSharpPlus.CommandsNext;
using Shiki.Commands;

namespace Shiki
{
    public class ShikiBot
    {
        public DiscordClient Discord { get; set; }

        public LavalinkExtension LavalinkExtension { get; set; }

        public ShikiBot()
        {
            var discordToken = Environment.GetEnvironmentVariable("DiscordToken");
            var ipPort = Environment.GetEnvironmentVariable("IpAdress");
            var password = Environment.GetEnvironmentVariable("LavalinkPassword");
            var geniusToken = Environment.GetEnvironmentVariable("GeniusToken");
            var port = int.Parse(Environment.GetEnvironmentVariable("Port"));
            
            var connectionConfig = new ConnectionConfig
            {
                DiscordToken = discordToken,
                IpAdress = ipPort,
                LavalinkPassword = password,
                Port = port,
                GeniusToken = geniusToken
            };
            
            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = discordToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            });

            var service = new ServiceCollection()
                .AddSingleton(new LyricsService(connectionConfig))
                .AddSingleton(new LavalinkService(connectionConfig, Discord))
                .AddScoped<Player>()
                .AddSingleton<MusicService>()
                .BuildServiceProvider();

            var commands = Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] {"?"},
                Services = service
            });

            commands.RegisterCommands<MusicCommands>();
        }

        public async Task MainAsync()
        {
            await Discord.ConnectAsync();
            LavalinkExtension = Discord.UseLavalink();
            await Task.Delay(-1);
        }
    }
}