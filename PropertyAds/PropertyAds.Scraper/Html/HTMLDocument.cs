namespace PropertyAds.Scraper.Core
{
    using HtmlAgilityPack;
    using PropertyAds.Scraper.Contracts;
    using System.Text;
    using System.Threading.Tasks;

    public class HTMLDocument : HTMLElement, IDocument
    {
        public static async Task<HTMLDocument> LoadFromUrlAsync(string url)
        {
            var web = new HtmlWeb();
            var dom = await web.LoadFromWebAsync(url, Encoding.GetEncoding(1251));

            return new HTMLDocument(dom.DocumentNode);
        }

        public HTMLDocument(HtmlNode node)
            : base(node) { }
    }
}
