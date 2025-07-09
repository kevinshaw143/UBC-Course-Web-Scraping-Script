using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;
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

            List<Course> courses = new List<Course>();
            foreach (string link in links)
            {

                // https://vancouver.calendar.ubc.ca/course-descriptions/subject/econv
                // https://vancouver.calendar.ubc.ca/course-descriptions/subject/mathv
                // https://vancouver.calendar.ubc.ca/course-descriptions/subject/hebrv
                if (link.Equals(@"https://vancouver.calendar.ubc.ca/course-descriptions/subject/mathv"))
                {
                    parseLinkedPage(link, courses);
                }
            }

            // uncomment to write to file
            /*
            var options = new JsonSerializerOptions { WriteIndented = true };
            string fileName = "UBC-Course-Info.json";
            string json = JsonSerializer.Serialize(courses, options);
            File.WriteAllText(fileName, json);
            */
        }

        static void parseLinkedPage(string link, List<Course> courses)
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

                Int32.TryParse(courseCredits, out int credits);

                Course current = new Course(courseCode, credits, courseTitle, courseDescription);
                courses.Add(current);
                current.print();
                Console.WriteLine("");
            }
        }
    }
}
