using Autofac.Features.Indexed;

namespace SMARTII.Domain.DI
{
    public static class AutofacUtility
    {
        public static V TryGetService<K, V>(this IIndex<K, V> index, K key, K nonExistMappedKey = default(K))
        {
            if (key == null)
                return default(V);

            if (index.TryGetValue(key, out V service))
            {
                return service;
            }

            if (nonExistMappedKey ==null || nonExistMappedKey.Equals(default(K)))
                return default(V);

            return index[nonExistMappedKey];
        }
    }
}
