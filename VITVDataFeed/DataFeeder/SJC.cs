using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace VITVDataFeed
{
    public static class SJC
    {
        public static void GetData()
        {
            XPathDocument doc = new XPathDocument("http://www.sjc.com.vn/xml/tygiavang.xml");
            XPathNavigator nav = doc.CreateNavigator();
            var cityIter = nav.Select("/root/ratelist/city");

            try
            {
                while (cityIter.MoveNext())
                {
                    Console.WriteLine(cityIter.Current.GetAttribute("name", ""));
                    XPathNavigator nav2 = cityIter.Current.Clone();
                    var itemIter = nav2.SelectChildren("item", "");
                    while (itemIter.MoveNext())
                    {
                        Console.WriteLine(itemIter.Current.GetAttribute("buy", ""));
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
