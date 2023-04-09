using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shiki.Service
{
    public class Player
    {
        public LavalinkService LavalinkService { get; set; }

        public Queue<LavalinkTrack> Tracks { get; set; }

        public LavalinkGuildConnection Connection { get; set; }

        public Player(LavalinkService lavalinkService)
        {
            LavalinkService = lavalinkService;
            Tracks = new Queue<LavalinkTrack>();
        }

        private async Task PlaybackNext(LavalinkGuildConnection lavalinkGuildConnection, TrackFinishEventArgs trackFinishEventArgs)
            =>await Skip();

        public bool CurrentlyPlaying()
        {
            if (!Connection.IsConnected)
                return false;

            if (Connection.CurrentState.CurrentTrack == null)
                return false;

            TimeSpan trackBegin = Connection.CurrentState.CurrentTrack.Position;
            TimeSpan trackEnd = Connection.CurrentState.CurrentTrack.Length;
            TimeSpan currentPosition = Connection.CurrentState.PlaybackPosition;

            if(currentPosition > trackBegin && currentPosition < trackEnd)
                return true;
            return false;
        }

        public async Task StopPlayer()
        {
            if (Tracks != null)
                Tracks.Clear();
            await DisposePlayer();
        }

        public async Task Skip()
        {
            if (Tracks == null || Tracks.Count == 0)
            {
                await DisposePlayer();
                return;
            }
            await Connection.PlayAsync(Tracks.Dequeue());
        }

        public async Task PauseTrack()
            => await Connection.PauseAsync();

        public async Task PlaybackTrack(LavalinkTrack lavalinkTrack)
            => await Connection.PlayAsync(lavalinkTrack);

        public async Task ResumeTrack()
            => await Connection.ResumeAsync();

        public async Task StartPlayer(DiscordChannel discordChannel)
        {
            Connection = await LavalinkService.LavalinkNodeConnection.ConnectAsync(discordChannel);
            Connection.PlaybackFinished += PlaybackNext;
        }

        public async Task DisposePlayer()
        {
            if (Connection == null)
                return;
            await Connection.DisconnectAsync();
            Connection.PlaybackFinished -= PlaybackNext;
        }
    }
}