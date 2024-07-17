using MasterKinder.Models;
using System.Text;
using System.Xml.Linq;

namespace MasterKinder.Services
{
    public class Sitemap
    {
        private readonly List<SitemapItem> _items;

        public Sitemap(List<SitemapItem> items)
        {
            _items = items;
        }

        public string ToXml()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var sitemapElement = new XElement(ns + "urlset");

            foreach (var item in _items)
            {
                var urlElement = new XElement(ns + "url",
                    new XElement(ns + "loc", item.Url));

                if (item.LastModified.HasValue)
                {
                    urlElement.Add(new XElement(ns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-ddTHH:mm:sszzz")));
                }

                sitemapElement.Add(urlElement);
            }

            var doc = new XDocument(sitemapElement);
            var sb = new StringBuilder();
            using (var writer = new Utf8StringWriter(sb))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        private class Utf8StringWriter : StringWriter
        {
            public Utf8StringWriter(StringBuilder sb) : base(sb)
            {
            }

            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}