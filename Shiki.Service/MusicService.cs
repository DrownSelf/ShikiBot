using DSharpPlus.Lavalink;
using System.Threading.Tasks;
using System.Linq;

namespace Shiki.Service
{
    public class MusicService
    {
        public LavalinkService LavalinkService { get; set; }

        public MusicService(LavalinkService lavalinkService)
        {
            LavalinkService = lavalinkService;
        }

        public async Task<LavalinkTrack> FindTrack(string request, LavalinkSearchType searchType)
        {
            var loadedTreck = await LavalinkService.LavalinkNodeConnection.Rest.GetTracksAsync(request, searchType);
            if (!(loadedTreck.LoadResultType == LavalinkLoadResultType.LoadFailed
                  || loadedTreck.LoadResultType == LavalinkLoadResultType.NoMatches))
                return loadedTreck.Tracks.First();
            return null;
        }
    }
}