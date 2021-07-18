namespace PropertyAds.Scraper.Contracts
{
    using System.Collections.Generic;

    public interface ICollectionFinder<T>
        where T : IElement<T>
    {
        ICollection<T> ByClass(string className);

        ICollection<T> ByQuery(string query);

        ICollection<T> ByXPath(string xPath);
    }
}
