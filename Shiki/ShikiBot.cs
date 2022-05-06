using DSharpPlus;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiki.Data;
using DSharpPlus.Net;
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
            string json = File.ReadAllText("C:\\Ryougi\\Shiki\\ConnectionConfig.json");
            var connectionConfig = JsonConvert.DeserializeObject<ConnectionConfig>(json);
            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = connectionConfig.DiscordToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            });

            var endpoint = new ConnectionEndpoint()
            {
                Hostname = connectionConfig.IpAdress,
                Port = connectionConfig.Port
            };

            var service = new ServiceCollection()
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
