namespace PropertyAds.Scraper.Contracts
{
    using System.Threading.Tasks;

    public interface IScraper<T>
    {
        Task<T> Scrape();
    }
}
