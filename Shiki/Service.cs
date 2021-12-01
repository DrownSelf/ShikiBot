using DSharpPlus.Lavalink;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;

namespace Shiki
{
    class Service : BaseCommandModule
    {
        private async Task PlaybackChoose(CommandContext ctx, string request, LavalinkSearchType service)
        {
            LinkContext lava = new LinkContext(ctx, request, service);
            await lava.ActivateLava(lava.Playback);
        }

        private async Task ChooseOtherMode(CommandContext ctx, Context.HandleMode mode)
        {
            LinkContext lava = new LinkContext(ctx);
            lava.otherMode = mode;
            await lava.ActivateLava(lava.SetOtherMode);
        }

        [Command("youtube")]
        public async Task YoutubeRequest(CommandContext ctx, [RemainingText] string request)
        {
            await PlaybackChoose(ctx, request, LavalinkSearchType.Youtube);
        }

        [Command("soundCloud")]
        public async Task SoundCloudRequest(CommandContext ctx, [RemainingText] string request)
        {
            await PlaybackChoose(ctx, request, LavalinkSearchType.SoundCloud);
        }

        [Command("stop")]
        public async Task StopPlayback(CommandContext ctx)
        {
            await ChooseOtherMode(ctx, Context.HandleMode.Stop);
        }

        [Command("pause")]
        public async Task Pause(CommandContext ctx)
        {
            await ChooseOtherMode(ctx, Context.HandleMode.Pause);
        }

        [Command("resume")]
        public async Task Resume(CommandContext ctx)
        {
            await ChooseOtherMode(ctx, Context.HandleMode.Resume);
        }


    }
}