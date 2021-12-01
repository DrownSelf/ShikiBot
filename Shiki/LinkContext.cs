using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shiki
{
    class LinkContext : Context
    {
        private CommandContext ctx;

        private LavalinkNodeConnection node;
        private LavalinkExtension link;
        private LavalinkGuildConnection connection;

        private readonly string request;
        private readonly LavalinkSearchType service;
        private Dictionary<HandleMode, PlayerMode> playerMode;

        public HandleMode otherMode { get; set; }

        
        private void SetCommonFields(CommandContext ctx)
        {
            this.ctx = ctx;
            this.link = ctx.Client.UseLavalink();
            this.node = link.ConnectedNodes.Values.First();

            playerMode.Add(HandleMode.Resume, connection.ResumeAsync);
            playerMode.Add(HandleMode.Pause, connection.PauseAsync);
            playerMode.Add(HandleMode.Stop, node.StopAsync);
        }

        public LinkContext(CommandContext ctx)
        {
            SetCommonFields(ctx);
        }

        public LinkContext(CommandContext ctx, string request, LavalinkSearchType service)
        {
            SetCommonFields(ctx);
            this.request = request;
            this.service = service;
        }

        public async Task ActivateLava(PlayerMode mode)
        {
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("Хитрец, надо зайти на канал, чтобы остановить трек");
                return;
            }

            connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (connection == null)
            {
                await ctx.RespondAsync("Сервер не включен, попросите его включить");
                return;
            }

            await mode();
        }

        public async Task Playback()
        {
            LavalinkLoadResult load = await node.Rest.GetTracksAsync(request, service);

            if (load.LoadResultType == LavalinkLoadResultType.LoadFailed || load.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync("Не удалось найти трек (");
                return;
            }

            LavalinkTrack track = load.Tracks.First();
            await connection.PlayAsync(track);
        }

        public async Task SetOtherMode()
        {
            if (connection.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("Ты не запускал меня, чтобы совершать действие(");
                return;
            }
            await playerMode[otherMode]();
        }
    }
}