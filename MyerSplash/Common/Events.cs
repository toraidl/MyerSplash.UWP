using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;

namespace MyerSplash.Common
{
    public static class Events
    {
        public static void LogSelected(string name)
        {
            Analytics.TrackEvent($"Tab Selected: {name}");
        }

        public static void LogRefreshList(int selectedIndex)
        {
            Analytics.TrackEvent("RefreshList", new Dictionary<string, string> { { "Index", selectedIndex.ToString() } });
        }

        public static void LogEnterDownloads()
        {
            Analytics.TrackEvent("Enter downloads");
        }

        public static void LogEnterSearch()
        {
            Analytics.TrackEvent("Enter search");
        }

        public static void LogEnterSettings()
        {
            Analytics.TrackEvent("Enter settings");
        }

        public static void LogEnterAbout()
        {
            Analytics.TrackEvent("Enter about");
        }

        public static void LogDownloadButtonOnList()
        {
            Analytics.TrackEvent("Click download button on list");
        }
        public static void LogDownloadButtonOnDetails()
        {
            Analytics.TrackEvent("Click download button on details");
        }

        public static void LogSetAsInDetails()
        {
            Analytics.TrackEvent("Click set-as button on details");
        }

        public static void LogCancelInDetails()
        {
            Analytics.TrackEvent("Click cancel button on details");
        }

        public static void LogDragToDismiss()
        {
            Analytics.TrackEvent("Drag to dismiss");
        }

        public static void LogToggleImageDetails()
        {
            Analytics.TrackEvent("Toggle image details");
        }

        public static void LogSetAsPreview()
        {
            Analytics.TrackEvent("Toggle set-as preview");
        }

        public static void LogPhotoInfoDetails()
        {
            Analytics.TrackEvent("Toggle photo info details");
        }

        public static void LogCompatMode(bool on)
        {
            LogSwitch("Swtich compat mode", on);
        }

        public static void LogTile(bool on)
        {
            LogSwitch("Swtich live tile", on);
        }

        public static void LogScaleAnimation(bool on)
        {
            LogSwitch("Swtich scale animation", on);
        }

        public static void LogBackgroundWallpapersSource(int source)
        {
            Analytics.TrackEvent("Change background wallpapers source", new Dictionary<string, string> { { "Source", source.ToString() } });
        }

        private static void LogSwitch(string name, bool on)
        {
            Analytics.TrackEvent(name, new Dictionary<string, string> { { "IsOn", on.ToString() } });
        }
    }
}
