using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MyerSplashShared.Splasher
{
    public class MemoryCache : ICache<BitmapImage>
    {
        private readonly IDictionary<string, WeakReference<BitmapImage>> _cacheData =
            new Dictionary<string, WeakReference<BitmapImage>>();

        public Task<bool> ContainsAsync(ICacheKey cacheKey)
        {
            var bitmap = GetAsync(cacheKey);
            return Task.FromResult(bitmap != null);
        }

        public Task<BitmapImage> GetAsync(ICacheKey cacheKey)
        {
            BitmapImage result = null;
            var key = cacheKey.Key;
            if (!_cacheData.ContainsKey(key))
            {
                return Task.FromResult(null as BitmapImage);
            }
            var refContent = _cacheData[key];
            refContent?.TryGetTarget(out result);
            return Task.FromResult(result);
        }

        public Task PutAsync(ICacheKey cacheKey, BitmapImage t)
        {
            if (_cacheData.ContainsKey(cacheKey.Key))
            {
                _cacheData.Remove(cacheKey.Key);
            }
            _cacheData.Add(cacheKey.Key, new WeakReference<BitmapImage>(t));
            return Task.CompletedTask;
        }
    }
}
