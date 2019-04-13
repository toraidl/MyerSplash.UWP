using MyerSplashShared.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MyerSplashShared.Image
{
    public static class ImageDownloader
    {
        public static int TIMEOUT_MILLIS => 30_000;

        private static readonly HttpClient _client = new HttpClient();

        public static async Task<IRandomAccessStream> GetEncodedImageFromUrlAsync(string url, CancellationToken? token)
        {
            if (token == null) token = CancellationTokenSourceFactory.CreateDefault(TIMEOUT_MILLIS).Create().Token;
            return await GetEncodedImageFromUrlInternalAsync(url, token.Value);
        }

        private static async Task<IRandomAccessStream> GetEncodedImageFromUrlInternalAsync(string url, CancellationToken token)
        {
            if (string.IsNullOrEmpty(url)) throw new UriFormatException("The url is null or empty.");

            var downloadTask = _client.GetAsync(new Uri(url), token);

            token.ThrowIfCancellationRequested();

            var response = await downloadTask;
            response.EnsureSuccessStatusCode();

            var streamTask = response.Content.ReadAsStreamAsync();

            token.ThrowIfCancellationRequested();

            var stream = await streamTask;

            return stream.AsRandomAccessStream();
        }
    }
}