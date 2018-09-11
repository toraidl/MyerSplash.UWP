using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MyerSplashShared.Splasher
{
    public class SplasherController
    {
        private ICacheKeyFactory CacheKeyFactory
        {
            get
            {
                return Splasher.ImagePipeline.CacheKeyFactory;
            }
        }

        private ICache<BitmapImage> MemoryCache
        {
            get
            {
                return Splasher.ImagePipeline.MemoryCache;
            }
        }

        private ICache<StorageFile> DiskCache
        {
            get
            {
                return Splasher.ImagePipeline.DiskCache;
            }
        }

        private long TimeoutMillis
        {
            get
            {
                return Splasher.ImagePipeline.TimeoutMillis;
            }
        }

        private CancellationTokenSource _cts;

        public async Task<BitmapImage> FetchBitmapImageAsync(ImageRequest request)
        {
            var uri = request.Url;
            var key = CacheKeyFactory.CreateCacheKey(request);
            var bm = await MemoryCache.GetAsync(key);
            if (bm != null)
            {
                return bm;
            }

            var file = await DiskCache.GetAsync(key);
            if (file != null)
            {
                bm = await OpenBitmapImageAsync(file, request.ResizeOption);
                if (bm != null)
                {
                    await MemoryCache.PutAsync(key, bm);
                    return bm;
                }
            }

            var save = await FetchDecodedImageAsync(request);
            if (save)
            {
                file = await DiskCache.GetAsync(key);
                if (file != null)
                {
                    bm = await OpenBitmapImageAsync(file, request.ResizeOption);
                    if (bm != null)
                    {
                        await MemoryCache.PutAsync(key, bm);
                        return bm;
                    }
                }
            }

            return null;
        }

        private async Task<BitmapImage> OpenBitmapImageAsync(StorageFile file, IImageResizeOption resize)
        {
            using (var fs = await file.OpenAsync(FileAccessMode.Read))
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(fs);
                if (resize != null)
                {
                    var outputSize = resize.GetResizeSize(bitmap.PixelWidth, bitmap.PixelHeight);
                    bitmap.DecodePixelWidth = outputSize.Item1;
                    bitmap.DecodePixelHeight = outputSize.Item2;
                }
                return bitmap;
            }
        }

        public async Task<bool> FetchDecodedImageAsync(Uri uri)
        {
            return await FetchDecodedImageAsync(new ImageRequest(uri.ToString()));
        }

        public async Task<bool> FetchDecodedImageAsync(string uri)
        {
            return await FetchDecodedImageAsync(new ImageRequest(uri));
        }

        public async Task<bool> FetchDecodedImageAsync(ImageRequest request)
        {
            _cts = new CancellationTokenSource((int)TimeoutMillis);

            var key = CacheKeyFactory.CreateCacheKey(request).Key;

            var outputFile = await CacheUtil.GetCachedFileFolder().CreateFileAsync(key,
                CreationCollisionOption.ReplaceExisting);
            return await FileDownloader.GetStreamFromUrlAsync(request.Url, null, outputFile);
        }
    }
}
