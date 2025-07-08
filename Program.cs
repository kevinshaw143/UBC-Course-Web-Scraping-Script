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
        static void Main(string[] args)
        {
            var url = "https://vancouver.calendar.ubc.ca/course-descriptions/courses-subject";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            IEnumerable<string> hrefs = doc.DocumentNode.SelectNodes("//a")
                .Where(node => node.Attributes["href"] != null)                         
                .Select(node => node.Attributes["href"].Value);

            IEnumerable<string> links = hrefs.Where(link => link.Contains("https://vancouver.calendar.ubc.ca/course-descriptions/subject/"));

            parseLinkedPages(links);
        }

        static void printLinks(IEnumerable<string> links)
        {
            foreach (var link in links)
            {
                Console.WriteLine(link);
            }
        }

        static void parseLinkedPages(IEnumerable<string> links)
        {
            foreach (string link in links)
            {
                parseLinkedPage(link);
                break;
            }
        }

        static void parseLinkedPage(string link)
        {
            var url = link;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var courseTags = doc.DocumentNode.SelectNodes("//div[contains(@class, 'flow-root text-formatted node__content relative')]");

            foreach (var courseTag in courseTags)
            {
                var h3Tag = courseTag.Descendants("h3").First();

                string courseCodeInfo = h3Tag.InnerHtml;
                int endOfCourseCode = courseCodeInfo.IndexOf('(');

                string courseCode = courseCodeInfo.Substring(0, endOfCourseCode - 1);
                string courseCredits = courseCodeInfo.Substring(endOfCourseCode + 1, 1);
                string courseTitle = h3Tag.Descendants("strong").First().InnerHtml;
                string courseDescription = courseTag.Descendants("p").First().InnerHtml;

                Console.Write(courseCode + " ");
                Console.Write(courseCredits + " ");
                Console.WriteLine(courseTitle);
                Console.WriteLine(courseDescription);
            }
        }
    }
}
