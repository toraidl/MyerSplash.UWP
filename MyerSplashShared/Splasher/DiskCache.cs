using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyerSplashShared.Splasher
{
    public class DiskCache : ICache<StorageFile>
    {
        public async Task<bool> ContainsAsync(ICacheKey cacheKey)
        {
            var item = await GetAsync(cacheKey);
            return item != null;
        }

        public async Task<StorageFile> GetAsync(ICacheKey cacheKey)
        {
            var folder = CacheUtil.GetCachedFileFolder();
            var item = await folder.TryGetItemAsync(cacheKey.Key);
            return item as StorageFile;
        }

        public Task PutAsync(ICacheKey cacheKey, StorageFile t)
        {
            return Task.CompletedTask;
        }
    }
}
