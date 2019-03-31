using System.Threading.Tasks;

namespace MyerSplashShared.Image
{
    public interface ICacheSupplier<T>
    {
        Task<bool> CheckCacheExistAsync(string key);

        Task<T> TryGetCacheAsync(string key);

        Task<long> GetSizeAsync();

        Task ClearAsync();
    }
}
