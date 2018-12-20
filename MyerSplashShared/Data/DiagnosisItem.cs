using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyerSplashShared.Data
{
    public class DiagnosisItem
    {
        private const int TIME_OUT_MILLIS = 6000;

        private HttpClient _client;
        private readonly string _uri;
        private readonly string _desc;
        private readonly int _timeoutMillis;

        public bool IsStatusCodeSuccessful { get; private set; } = false;

        public DiagnosisItem(HttpClient client, string uri, string desc = "", int timeoutMillis = TIME_OUT_MILLIS)
        {
            _client = client;
            _uri = uri;
            _desc = desc;
            _timeoutMillis = timeoutMillis;
        }

        public override string ToString()
        {
            return $"------Resolving {(string.IsNullOrEmpty(_desc) ? _uri : _desc)}------";
        }

        public virtual async Task<DiagnosisResult> RunAsync()
        {
            try
            {
                var cts = new CancellationTokenSource(_timeoutMillis);
                var request = new HttpRequestMessage(HttpMethod.Get, _uri);
                var watch = new Stopwatch();
                watch.Start();
                var result = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                watch.Stop();
                IsStatusCodeSuccessful = result.IsSuccessStatusCode;
                return new DiagnosisResult($"Result: {result.StatusCode}, elapsed time: {watch.ElapsedMilliseconds}millis", result);
            }
            catch (TaskCanceledException)
            {
                return new DiagnosisResult($"The task is cancelled after {_timeoutMillis}millis", null);
            }
            catch (Exception e)
            {
                return new DiagnosisResult(e.Message, null);
            }
        }
    }
}
