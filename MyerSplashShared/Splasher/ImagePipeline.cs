using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MyerSplashShared.Splasher
{
    public class ImagePipeline
    {
        public ICache<BitmapImage> MemoryCache { get; private set; } 

        public ICache<StorageFile> DiskCache { get; private set; }

        public ICacheKeyFactory CacheKeyFactory { get; private set; }

        internal long TimeoutMillis { get; private set; }

        internal void Initialize(SplasherConfiguration configuration)
        {
            CacheKeyFactory = configuration.CacheKeyFactory;
            MemoryCache = new MemoryCache();
            DiskCache = new DiskCache();
        }

        public Task ConsumesCacheAsync<T>(Action<ICache<T>> func)
        {
            return Task.Run(() =>
            {
                if (typeof(T) == typeof(BitmapImage))
                {
                    func.Invoke((ICache<T>)MemoryCache);
                }
                else
                {
                    func.Invoke((ICache<T>)DiskCache);
                }
            });
        }
    }
}
