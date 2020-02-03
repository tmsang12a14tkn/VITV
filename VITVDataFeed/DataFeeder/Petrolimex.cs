using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VITVDataFeed
{
    public static class Petrolimex
    {
        public static void GetData()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://www.petrolimex.com.vn/default.aspx");
            var items = doc.DocumentNode.Descendants().Where(x => x.Name == "tr" && x.Attributes.Contains("class") && x.Attributes["class"].Value.StartsWith("blueseaDocsItem")).ToList();
            foreach (HtmlNode item in items)
            {
                var childs = item.ChildNodes;
                if (childs.Count == 3)
                {
                    var name = childs[0].InnerText;
                    var val1 = childs[1].InnerText;
                    var val2 = childs[2].InnerText;
                }
            }
        }
    }
}
