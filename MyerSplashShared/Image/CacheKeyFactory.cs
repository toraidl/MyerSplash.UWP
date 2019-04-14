namespace MyerSplashShared.Image
{
    public static class CacheKeyFactory
    {
        private static readonly object O = new object();

        private static ICacheKeyFactory _default;

        public static ICacheKeyFactory GetDefault()
        {
            if (_default == null)
            {
                lock (O)
                {
                    if (_default == null)
                    {
                        return _default = new DefaultCacheKeyFactory();
                    }
                    else
                    {
                        return _default;
                    }
                }
            }
            else
            {
                return _default;
            }
        }
    }
}
