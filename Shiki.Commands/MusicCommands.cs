using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using Shiki.Service;

namespace Shiki.Commands
{
    public class MusicCommands : BaseCommandModule
    {
        public MusicService MusicService { get; set; }
        
        public LyricsService LyricsService { get; set; }
        
        public Player Player { get; set; }

        public MusicCommands(MusicService musicService, LyricsService lyricsService, Player player)
        {
            MusicService = musicService;
            Player = player;
            LyricsService = lyricsService;
        }

        private async Task<bool> CheckConnectionForDisturb(CommandContext context)
        {
            if (context.Member.VoiceState == null || context.Member.VoiceState.Channel == null)
            {
                await context.RespondAsync("You are not in voice channel");
                return false;
            }

            if (Player.Connection == null)
            {
                await context.RespondAsync("lavalink haven't connected");
                return false;
            }

            if (!Player.Connection.IsConnected)
            {
                await context.RespondAsync("You haven't started treck");
                return false;
            }
            return true;
        }

        private async Task Play(CommandContext context, [RemainingText]string request, LavalinkSearchType searchType)
        {
            if (context.Member.VoiceState == null || context.Member.VoiceState.Channel == null)
            {
                await context.RespondAsync("You are not in voice channel");
                return;
            }

            if(Player.Connection == null || (!Player.Connection.IsConnected))
                await Player.StartPlayer(context.Member.VoiceState.Channel);

            if (Player.Connection.Channel != context.Member.VoiceState.Channel)
            {
                await context.RespondAsync("You are not in the same channel");
                return;
            }

            var track = await MusicService.FindTrack(request, searchType);
            if (Player.CurrentlyPlaying())
            {
                Player.Tracks.Enqueue(track);
                await context.RespondAsync($"{track.Title} {track.Author} added to queue");
                return;
            }

            await Player.PlaybackTrack(track);
            await context.RespondAsync($"Currently playing {track.Title} {track.Author}");
        }

        [Command("youtube")]
        public async Task PlayYoutube(CommandContext context, [RemainingText]string request)
            => await Play(context, request, LavalinkSearchType.Youtube);

        [Command("soundcloud")]
        public async Task PlaySoundClound(CommandContext context, [RemainingText] string request)
            => await Play(context, request, LavalinkSearchType.SoundCloud);

        [Command("resume")]
        public async Task Resume(CommandContext context)
        {
            if (!await CheckConnectionForDisturb(context))
                return;
            await context.RespondAsync($"Resume playing {Player.Connection.CurrentState.CurrentTrack.Title} ");
            await Player.ResumeTrack();
        }
        
        [Command("skip")]
        public async Task Skip(CommandContext context)
        {
            if (!await CheckConnectionForDisturb(context))
                return;
            await context.RespondAsync("skiped");
            await Player.Skip();
        }
        

        [Command("pause")]
        public async Task Pause(CommandContext context)
        {
            if (!await CheckConnectionForDisturb(context))
                return;
            await context.RespondAsync($"{Player.Connection.CurrentState.CurrentTrack.Title} {Player.Connection.CurrentState.CurrentTrack.Author} paused");
            await Player.PauseTrack();
        }

        [Command("stop")]
        public async Task Stop(CommandContext context)
        {
            if (!await CheckConnectionForDisturb(context))
                return;
            await context.RespondAsync("Player Stopped");
            await Player.StopPlayer();
        }

        [Command("text")]
        public async Task GetText(CommandContext context)
        {
            if (!await CheckConnectionForDisturb(context))
                return;
            string nameOfTrack = Player.Connection.CurrentState.CurrentTrack.Title.Split('(')[0];
            await context.RespondAsync(await LyricsService.GetLyricsAsync(nameOfTrack));
        }
    }
}