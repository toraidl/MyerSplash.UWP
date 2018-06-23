using MyerSplash.Data;
using MyerSplashShared.Utils;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class WallpaperAutoChangeTask : IBackgroundTask
    {
        private const string KEY = "BackgroundWallpaperSource";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("===========background task run==============");
            var defer = taskInstance.GetDeferral();
            var url = UnsplashImageFactory.CreateTodayHighlightImage().Urls.Full;
            var result = await SimpleWallpaperSetter.DownloadAndSetAsync(url);
            Debug.WriteLine($"===========result {result}==============");
            defer.Complete();
        }
    }
}