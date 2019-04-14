using System.Net.Http;

namespace MyerSplashShared.Data
{
    public class DiagnosisResult
    {
        public string BriefMessage { get; set; }

        public HttpResponseMessage Response { get; set; }

        public DiagnosisResult(string message, HttpResponseMessage response)
        {
            BriefMessage = message;
            Response = response;
        }
    }
}
