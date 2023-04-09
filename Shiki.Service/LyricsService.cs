using System.Text;
using Newtonsoft.Json;
using Shiki.Data;

namespace Shiki.Service;

public class Lyrics
{
    [JsonProperty("lyrics")]
    public string lyrics;
}

public class LyricsResponse
{
    [JsonProperty("data")] 
    public Lyrics data;
}

public class LyricsService
{
    private readonly string _token;

    public LyricsService(ConnectionConfig config)
    {
        _token = config.GeniusToken;
    }
    
    public async Task<string> GetLyricsAsync(string title)
    {
        using (HttpClient client = new HttpClient())
        {
            var values = new Dictionary<string, string>
            {
                { "title", title },
                { "genius_token", _token }
            };
            string json = JsonConvert.SerializeObject(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://lyrics:8000/api/v1/lyrics", content);
            
            var responseString = await response.Content.ReadAsStringAsync();
            LyricsResponse lyricsResponse = JsonConvert.DeserializeObject<LyricsResponse>(responseString);
            
            return lyricsResponse.data.lyrics;
        }
    }
}