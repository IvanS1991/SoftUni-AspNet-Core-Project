namespace PropertyAds.WebApp.Services.Utility
{
    public interface ICache
    {
        bool TryGetValue<T>(object key, out T value);

        T Set<T>(object key, T value);

        void Remove(object key);
    }
}
