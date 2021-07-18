namespace PropertyAds.Scraper.Core.XPath
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class XPathBuilder
    {
        public static XPathBuilder FromCSSQuery(string cssQuery)
        {
            bool isAddingDirectDescendant = false;

            XPathBuilder xPathBuilder = new XPathBuilder();

            string[] pieces = cssQuery.Split(" ");

            if (pieces.Length == 1)
            {
                string[] elementClasses = cssQuery.Split(".");

                if (!string.IsNullOrEmpty(elementClasses.First()))
                {
                    xPathBuilder.Element(elementClasses.First());
                }

                foreach (var elementClass in elementClasses.Skip(1))
                {
                    xPathBuilder.ContainsClass(elementClass);
                }
            }
            else
            {
                xPathBuilder = FromCSSQuery(pieces.First());

                foreach (var piece in pieces.Skip(1))
                {
                    if (piece == ">")
                    {
                        isAddingDirectDescendant = true;
                        continue;
                    }

                    if (isAddingDirectDescendant)
                    {
                        xPathBuilder.HasDirectDescendant(FromCSSQuery(piece));
                    }
                    else
                    {
                        xPathBuilder.HasDescendant(FromCSSQuery(piece));
                    }

                    isAddingDirectDescendant = false;
                }
            }

            return xPathBuilder;
        }

        private string element = "*";
        private List<string> attributes = new List<string>();
        private List<XPathBuilder> descendants = new List<XPathBuilder>();

        public bool IsDirectDescendant { get; set; }

        private XPathBuilder AddAttributeQuery(string attribute)
        {
            this.attributes.Add(attribute);

            return this;
        }

        public XPathBuilder Element(string element)
        {
            this.element = element;

            return this;
        }

        public XPathBuilder ContainsClass(params string[] classNames)
        {
            foreach (var className in classNames)
            {
                this.HasAttributeContaining("class", className);
            }

            return this;
        }

        public XPathBuilder HasId(string id)
        {
            return this.HasAttribute("id", id);
        }

        public XPathBuilder HasAttribute(string attribute)
        {
            return this.AddAttributeQuery($"@{attribute}");
        }

        public XPathBuilder HasAttribute(string attribute, string value)
        {
            return this.AddAttributeQuery($"@{attribute}='{value}'");
        }

        public XPathBuilder HasAttributeContaining(string attribute, string containedText)
        {
            return this.AddAttributeQuery($"contains(@{attribute}, '{containedText}')");
        }

        public XPathBuilder HasDescendant(XPathBuilder descendant)
        {
            descendant.IsDirectDescendant = false;

            this.descendants.Add(descendant);

            return this;
        }

        public XPathBuilder HasDirectDescendant(XPathBuilder descendant)
        {
            descendant.IsDirectDescendant = true;

            this.descendants.Add(descendant);

            return this;
        }

        public string Result()
        {
            var sb = new StringBuilder();

            if (IsDirectDescendant)
            {
                sb.Append($"/{this.element}");
            }
            else
            {
                sb.Append($"//{this.element}");
            }

            if (this.attributes.Count > 0)
            {
                var fullAttributesQuery = string.Join(" and ", this.attributes);

                sb.Append($"[{fullAttributesQuery}]");
            }

            if (this.descendants.Count > 0)
            {
                string descendantsQuery = this.descendants
                    .Aggregate("", (acc, descendant) =>
                    {
                        return $"{acc}{descendant.Result()}";
                    });

                sb.Append(descendantsQuery);
            }

            return sb.ToString();
        }
    }
}
