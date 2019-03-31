using System.Linq;
using System;

namespace MyerSplashShared.Image
{
    public class DefaultCacheKeyFactory : ICacheKeyFactory
    {
        public string ProvideKey(string key)
        {
            var uri = new Uri(key);
            return uri.Segments.LastOrDefault() ?? key.GetHashCode().ToString();
        }
    }
}
