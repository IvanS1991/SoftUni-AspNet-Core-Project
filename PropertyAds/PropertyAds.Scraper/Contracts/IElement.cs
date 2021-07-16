namespace PropertyAds.Scraper.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IElement<T>
        where T : IElement<T>
    {
        string InnerText { get; }

        T Find(Func<IFinder<T>, T> finderCb);

        ICollection<T> FindAll(Func<ICollectionFinder<T>, ICollection<T>> finderCb);

        Task<T> FindAsync(Func<IFinder<T>, T> finderCb);

        Task<ICollection<T>> FindAllAsync(Func<ICollectionFinder<T>, ICollection<T>> finderCb);
    }
}
