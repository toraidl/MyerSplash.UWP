namespace MyerSplashShared.Image
{
    public static class CacheKeyFactory
    {
        private static readonly object o = new object();

        private static ICacheKeyFactory _default;

        public static ICacheKeyFactory GetDefault()
        {
            if (_default == null)
            {
                lock (o)
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
