using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyerSplashShared.Splasher
{
    public interface ICache<T>
    {
        Task PutAsync(ICacheKey cacheKey, T t);

        Task<T> GetAsync(ICacheKey cacheKey);

        Task<bool> ContainsAsync(ICacheKey cacheKey);
    }
}
