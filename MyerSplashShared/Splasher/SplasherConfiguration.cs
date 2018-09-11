using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyerSplashShared.Splasher
{
    public class SplasherConfiguration
    {
        internal static long DEFAULT_TIMEOUT_MILLIS => 100_000L;

        public ICacheKeyFactory CacheKeyFactory { get; internal set; }

        public long TimeoutMillis { get; internal set; } = DEFAULT_TIMEOUT_MILLIS;

        internal SplasherConfiguration()
        {

        }
    }

    public class SplasherConfigurationBuilder
    {
        public ICacheKeyFactory CacheKeyFactory { get; set; }

        public long TimeoutMillis { get; set; } = SplasherConfiguration.DEFAULT_TIMEOUT_MILLIS;

        public SplasherConfiguration Build()
        {
            return new SplasherConfiguration()
            {
                CacheKeyFactory = this.CacheKeyFactory,
                TimeoutMillis = this.TimeoutMillis,
            };
        }
    }
}
