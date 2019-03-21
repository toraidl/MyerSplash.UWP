using Windows.ApplicationModel.Resources;

namespace MyerSplash.Common
{
    public class ResourcesHelper
    {
        public static string GetResString(string key)
        {
            return ResourceLoader.GetForViewIndependentUse().GetString(key);
        }

        public static string GetFormattedResString(string key, params object[] args)
        {
            return string.Format(ResourceLoader.GetForViewIndependentUse().GetString(key), args);
        }

        public static string GetDicString(string key)
        {
            return Windows.UI.Xaml.Application.Current.Resources[key] as string;
        }

        public static double GetDimentionInPixel(string key)
        {
            return (double)Windows.UI.Xaml.Application.Current.Resources[key];
        }
    }
}