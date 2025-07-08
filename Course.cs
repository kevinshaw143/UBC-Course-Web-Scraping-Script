using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBC_Course_Web_Scrapping_Script
{
    internal class Course
    {
        public string major { get; set; }

        public string code { get; set; }

        public int credits { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Course(string courseCode, int? credits, string title, string description) { 

            int spaceIndex = courseCode.IndexOf(' ');
            int codeLength = 3;

            try
            {
                this.major = courseCode.Substring(0, spaceIndex);
                this.code = courseCode.Substring(spaceIndex + 1, codeLength);
            } catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Course Code: " + courseCode + " out of range exception");
            } catch (Exception)
            {
                Console.WriteLine("Course Code: " + courseCode + " unknown error");
            }


            this.credits = credits != null ? (int)credits : -1;
            this.title = title;
            this.description = description;
        }

        public void print()
        {
            Console.Write(major + " " + code + " ");
            Console.Write(credits + " ");
            Console.WriteLine(title);
            Console.WriteLine(description);
        }
    }
}
