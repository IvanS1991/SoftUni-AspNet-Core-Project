namespace PropertyAds.Scraper.Core
{
    using HtmlAgilityPack;
    using PropertyAds.Scraper.Contracts;
    using PropertyAds.Scraper.Core.XPath;

    public class HTMLFinder : IFinder<HTMLElement>
    {
        private readonly HtmlNode node;

        public HTMLFinder(HtmlNode node)
        {
            this.node = node;
        }

        public HTMLElement ByClass(string className)
        {
            var xpathQuery = new XPathBuilder()
                .ContainsClass(className)
                .Result();

            return this.ByXPath(xpathQuery);
        }

        public HTMLElement ByQuery(string query)
        {
            var xpathQuery = XPathBuilder.FromCSSQuery(query)
                .Result();

            return this.ByXPath(xpathQuery);
        }

        public HTMLElement ByXPath(string xPath)
        {
            var nodes = this.node.SelectSingleNode($".{xPath}");

            if (node == null)
            {
                return null;
            }

            return new HTMLElement(node);
        }
    }
}
