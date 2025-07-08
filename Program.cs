using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace UBC_Course_Web_Scrapping_Script
{
    internal class Program
    {

        static void printLinks(IEnumerable<string> links)
        {
            foreach (var link in links)
            {
                Console.WriteLine(link);
            }
        }

        static void parseLinkedPages(IEnumerable<string> links)
        {

        }

        static void Main(string[] args)
        {
            var url = "https://vancouver.calendar.ubc.ca/course-descriptions/courses-subject";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            IEnumerable<string> hrefs = doc.DocumentNode.SelectNodes("//a")
                .Where(node => node.Attributes["href"] != null)                         
                .Select(node => node.Attributes["href"].Value);

            IEnumerable<string> links = hrefs.Where(link => link.Contains("https://vancouver.calendar.ubc.ca/course-descriptions/subject/"));

            printLinks(links);

            parseLinkedPages(IEnumerable<string> links);
        }
    }
}
