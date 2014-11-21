using System;
using System.Linq;
using System.Xml.Linq;

namespace LinqToXml
{
    class LinqToXml
    {
        static void Main(string[] args)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Sample RSS Feed"),
                new XElement("rss",
                    new XAttribute("version", "2.0"),
                    new XElement("channel",
                        new XElement("title", "RSS Channel Title"),
                        new XElement("description", "RSS Channel Description."),
                        new XElement("link", "http://aspiring-technology.com"),
                        new XElement("item",
                            new XElement("title", "First article title"),
                            new XElement("description", "First Article Description"),
                            new XElement("pubDate", DateTime.Now.ToUniversalTime()),
                            new XElement("guid", Guid.NewGuid())
                        ),
                        new XElement("item",
                                new XElement("title", "Second article title"),
                                new XElement("description", "Second Article Description"),
                                new XElement("pubDate", DateTime.Now.ToUniversalTime()),
                                new XElement("guid", Guid.NewGuid())
                        )
                    )
                )
            );

            doc.Save("Sample.xml");

            XDocument loaded = XDocument.Load(@"Sample.xml");
            var titles = from c in loaded.Descendants("item")
                         where !string.IsNullOrEmpty((string)c.Element("title"))
                         select (string)c.Element("title");

            titles.FirstOrDefault(t =>
                {
                    Console.WriteLine(t);
                    return false;
                });
        }
    }
}