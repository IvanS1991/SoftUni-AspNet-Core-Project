namespace PropertyAds.Scraper.Core
{
    using HtmlAgilityPack;
    using PropertyAds.Scraper.Contracts;
    using PropertyAds.Scraper.Core.XPath;
    using System.Collections.Generic;
    using System.Linq;

    public class HTMLCollectionFinder : ICollectionFinder<HTMLElement>
    {
        private readonly HtmlNode node;

        public HTMLCollectionFinder(HtmlNode node)
        {
            this.node = node;
        }

        public ICollection<HTMLElement> ByClass(string className)
        {
            var xpathQuery = new XPathBuilder()
                .ContainsClass(className)
                .Result();

            return this.ByXPath(xpathQuery);
        }

        public ICollection<HTMLElement> ByQuery(string query)
        {
            var xPathQuery = XPathBuilder.FromCSSQuery(query)
                .Result();

            return this.ByXPath(xPathQuery);
        }

        public ICollection<HTMLElement> ByXPath(string xPath)
        {
            var nodes = this.node.SelectNodes($".{xPath}");

            if (nodes is ICollection<HtmlNode>)
            {
                return nodes
                    .Select(x => new HTMLElement(x))
                    .ToList();
            }

            return null;
        }
    }
}
