using HtmlAgilityPack;
using System;
using System.Linq;
using System.Web;

namespace TestWebScrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            exampleStackOverFlow();
        }
        public static void exampleStackOverFlow()
        {
            // /html/body/div[3]/div[3]/div[4]/div/table[2]/tbody/tr  <-- Xpath to the first row of the table
            // //body//table[2]/tbody/tr                              <-- Same Result
            // (//body//table)[2]/tbody/tr                            <-- Same Result (maybe better)
            // //table[contains(@class, 'sortable')]//tr              <-- Same Result

            // https://stackoverflow.com/questions/12415214/scrape-data-from-wikipedia
            HtmlWeb web = new HtmlWeb();
            var doc = new HtmlDocument();
            doc = web.Load("https://en.wikipedia.org/wiki/List_of_national_parks_of_the_United_States");

            //We get all the rows from the table (except the header)
            var rows = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]//tr").Skip(1);
            foreach (var row in rows)
            {
                var name = HttpUtility.HtmlDecode(row.SelectSingleNode("./*[1]/a[@href and @title]").InnerText);
                var loc = HttpUtility.HtmlDecode(row.SelectSingleNode(".//span[@class='geo-dec']").InnerText);
                var areaNodes = row.SelectSingleNode("./*[5]").ChildNodes.Skip(1);
                string area = "";
                foreach (var a in areaNodes)
                {
                    area += HttpUtility.HtmlDecode(a.InnerText);
                }
                Console.WriteLine("{0,-30} {1,-20} {2,-10}", name, loc, area);
            }
        }
    }
}
