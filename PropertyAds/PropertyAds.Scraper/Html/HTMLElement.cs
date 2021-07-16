namespace PropertyAds.Scraper.Core
{
    using HtmlAgilityPack;
    using PropertyAds.Scraper.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class HTMLElement : IElement<HTMLElement>
    {
        protected readonly HtmlNode node;

        public HTMLElement(HtmlNode node)
        {
            this.node = node;
        }

        public string InnerText => this.node.InnerText;

        public HTMLElement Find(Func<IFinder<HTMLElement>, HTMLElement> finderCb)
        {
            return finderCb(new HTMLFinder(this.node));
        }

        public ICollection<HTMLElement> FindAll(Func<ICollectionFinder<HTMLElement>, ICollection<HTMLElement>> finderCb)
        {
            return finderCb(new HTMLCollectionFinder(this.node));
        }

        public Task<HTMLElement> FindAsync(Func<IFinder<HTMLElement>, HTMLElement> finderCb)
        {
            return Task.FromResult(this.Find(finderCb));
        }

        public Task<ICollection<HTMLElement>> FindAllAsync(Func<ICollectionFinder<HTMLElement>, ICollection<HTMLElement>> finderCb)
        {
            return Task.FromResult(this.FindAll(finderCb));
        }
    }
}
