namespace MyerSplashShared.Splasher
{
    public class SimpleCacheKey : ICacheKey
    {
        public string Key { get; private set; }

        public void GenerateKey(ImageRequest request)
        {
            Key = request.Url.GetHashCode().ToString() + ".jpg";
        }
    }
}
