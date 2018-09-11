namespace MyerSplashShared.Splasher
{
    public class Splasher
    {
        private static SplasherConfiguration _configuration;

        public static ImagePipeline ImagePipeline { get; private set; }

        public static void Initialize()
        {
            Initialize(null);
        }

        public static void Initialize(SplasherConfiguration configuration)
        {
            if (configuration == null)
            {
                configuration = new SplasherConfigurationBuilder()
                {
                    CacheKeyFactory = new DefaultCacheKeyFactory()
                }.Build();
            }
            _configuration = configuration;

            ImagePipeline = new ImagePipeline();
            ImagePipeline.Initialize(_configuration);
        }
    }
}
