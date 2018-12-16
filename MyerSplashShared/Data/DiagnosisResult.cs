using System.Net.Http;

namespace MyerSplashShared.Data
{
    public class DiagnosisResult
    {
        public string BriefMesasge { get; set; }

        public HttpResponseMessage Response { get; set; }

        public DiagnosisResult(string message, HttpResponseMessage response)
        {
            BriefMesasge = message;
            Response = response;
        }
    }
}
