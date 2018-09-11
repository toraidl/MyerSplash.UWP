namespace MyerSplashShared.Splasher
{
    public class DefaultCacheKeyFactory : ICacheKeyFactory
    {
        public ICacheKey CreateCacheKey(ImageRequest request)
        {
            var key = new SimpleCacheKey();
            key.GenerateKey(request);
            return key;
        }
    }
}
