namespace PropertyAds.Scraper.Contracts
{
    public interface IFinder<T>
        where T : IElement<T>
    {
        T ByClass(string className);

        T ByQuery(string query);

        T ByXPath(string xPath);
    }
}
