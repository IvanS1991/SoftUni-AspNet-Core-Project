namespace PropertyAds.Scraper.Core
{
    using PropertyAds.Scraper.Contracts;
    using System.Threading.Tasks;

    public abstract class HTMLScraper<T> : IScraper<T>
    {
        protected abstract Task NavigateTo();

        protected abstract Task Setup();

        protected abstract Task<T> GetResults();

        protected abstract Task Cleanup();

        public async Task<T> Scrape()
        {
            await this.Setup();
            await this.NavigateTo();

            T results = await this.GetResults();

            await this.Cleanup();

            return results;
        }
    }
}
