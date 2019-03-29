using MyerSplashShared.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MyerSplashShared.Utils
{
    public static class ImageDownloader
    {
        public static async Task<IRandomAccessStream> GetEncodedImageFromUrlAsync(string url, CancellationToken? token)
        {
            if (string.IsNullOrEmpty(url)) throw new UriFormatException("The url is null or empty.");

            using (var client = new HttpClient())
            {
                if (token == null) token = CancellationTokenSourceFactory.CreateDefault(15000).Create().Token;

                var downloadTask = client.GetAsync(new Uri(url), token.Value);

                token?.ThrowIfCancellationRequested();

                var response = await downloadTask;
                response.EnsureSuccessStatusCode();

                var streamTask = response.Content.ReadAsStreamAsync();

                token?.ThrowIfCancellationRequested();

                var stream = await streamTask;

                return stream.AsRandomAccessStream();
            }
        }

        public static async Task<IRandomAccessStream> GetIRandomAccessStreamFromUrlAsync(string url)
        {
            return await GetEncodedImageFromUrlAsync(url, null);
        }
    }
}