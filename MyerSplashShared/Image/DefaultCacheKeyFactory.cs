
namespace MyerSplashShared.Image
{
    public class DefaultCacheKeyFactory : ICacheKeyFactory
    {
        public string ProvideKey(string key)
        {
            return key.GetHashCode().ToString();
        }
    }
}
