using MyerSplash.View.Uc;
using MyerSplashCustomControl;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;

namespace MyerSplash.Common
{
    public static class WallpaperSettingHelper
    {
        public static async Task SetAsBackgroundAsync(StorageFile savedFile)
        {
            var uc = new LoadingTextControl() { LoadingText = "Setting background and lockscreen..." };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false);

            var file = await PrepareImageFileAsync(savedFile);
            if (file != null)
            {
                var result = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);

                if (!result)
                {
                    ToastService.SendToast("Fail to set as wallpaper. #API ERROR.");
                }
            }

            PopupService.Instance.TryHide(500);
        }

        public static async Task SetAsLockscreenAsync(StorageFile savedFile)
        {
            var uc = new LoadingTextControl() { LoadingText = "Setting background and lockscreen..." };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false);

            var file = await PrepareImageFileAsync(savedFile);
            if (file != null)
            {
                var result = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);

                if (!result)
                {
                    ToastService.SendToast("Fail to set both. #API ERROR.");
                }
            }

            PopupService.Instance.TryHide(500);
        }

        public static async Task SetBothAsync(StorageFile savedFile)
        {
            var uc = new LoadingTextControl() { LoadingText = "Setting background and lockscreen..." };
            await PopupService.Instance.ShowAsync(uc, solidBackground: false);

            var file = await PrepareImageFileAsync(savedFile);
            if (file != null)
            {
                var result0 = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);
                var result1 = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file);

                if (!result0 || !result1)
                {
                    ToastService.SendToast("Fail to set both. #API ERROR.");
                }
            }

            PopupService.Instance.TryHide(500);
        }

        private static async Task<StorageFile> PrepareImageFileAsync(StorageFile resultFile)
        {
            if (!UserProfilePersonalizationSettings.IsSupported())
            {
                ToastService.SendToast("Your device can't set wallpaper.");
                return null;
            }
            if (resultFile != null)
            {
                StorageFile file = null;

                //WTF, the file should be copy to LocalFolder to make the setting wallpaer api work.
                var folder = ApplicationData.Current.LocalFolder;
                if (await folder.TryGetItemAsync(resultFile.Name) is StorageFile oldFile)
                {
                    await resultFile.CopyAndReplaceAsync(oldFile);
                    file = oldFile;
                }
                else
                {
                    file = await resultFile.CopyAsync(folder);
                }
                return file;
            }
            return null;
        }
    }
}