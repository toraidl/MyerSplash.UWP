using Newtonsoft.Json;

namespace MyerSplashShared.Data
{
    public class UnsplashLinks
    {
        [JsonProperty("download_location")]
        public string DownloadLocation { get; set; }
    }
}
