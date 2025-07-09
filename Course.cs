using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UBC_Course_Web_Scrapping_Script
{
    internal class Course
    {

        // these HAVE to be public for the serializer.
        public static string courseCodeRegex = "[A-Z]{3,4}(_V)?\\s?[0-9]{3}";
        public string major { get; set; }

        public string code { get; set; }

        public string courseCode => major + " " + code;

        public int credits { get; set; }

        public string title { get; set; }

        public string pageLink { get; set; }

        public string description { get; set; }
        public string lowercaseDescription { get; set; }

        public string prerequisiteString { get; set; }
        public string corequisiteString { get; set; }
        public string equivalentString { get; set; }

        public List<string> prerequisites { get; set; }
        public List<string> corequisites { get; set; }
        public List<string> equivalents { get; set; }

        public Course(string courseCode, int? credits, string title, string description, string link)
        {
            pageLink = link;

            assignCourseCode(courseCode);

            this.credits = credits != null ? (int)credits : -1;
            this.title = title;
            this.description = description;
            this.lowercaseDescription = description.ToLower();

            prerequisiteString = "";
            corequisiteString = "";
            equivalentString = "";

            findCourseRelationStrings();
            findCourseRelations();
        }

        private void assignCourseCode(string courseCode)
        {
            int spaceIndex = courseCode.IndexOf(' ');
            int _VIndex = courseCode.IndexOf("_V");
            int codeLength = 3;

            try
            {
                int endIndex = Math.Min(spaceIndex, _VIndex);
                this.major = courseCode.Substring(0, endIndex);
                this.code = courseCode.Substring(spaceIndex + 1, codeLength);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Course Code: " + courseCode + " out of range exception");
            }
            catch (Exception)
            {
                Console.WriteLine("Course Code: " + courseCode + " unknown error");
            }
        }


        private void findCourseRelationStrings()
        {
            int prereqStart = lowercaseDescription.IndexOf("prerequisite");
            int coreqStart = lowercaseDescription.IndexOf("corequisite");
            int equivalentStart = lowercaseDescription.IndexOf("equivalency");

            try
            {
                findPrerequisiteString(prereqStart, coreqStart, equivalentStart);
                findCorequisiteString(prereqStart, coreqStart, equivalentStart);
                findEquivalentString(prereqStart, coreqStart, equivalentStart);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("ERROR: Out of range for string finding " + courseCode);
            }
        }

        private void findPrerequisiteString(int pre, int co, int eq)
        {
            if (pre == -1)
            {
                prerequisiteString = "";
                return;
            }

            int[] candidateNumbers = { co, eq, description.Length - 1 };
            int prereqEnd = findSmallestGreaterNumber(pre, candidateNumbers);

            int prereqStringLength = prereqEnd - pre;
            prerequisiteString = description.Substring(pre, prereqStringLength);
        }

        private void findCorequisiteString(int pre, int co, int eq)
        {
            if (co == -1)
            {
                corequisiteString = "";
                return;
            }

            int[] candidateNumbers = { pre, eq, description.Length - 1 };
            int coreqEnd = findSmallestGreaterNumber(co, candidateNumbers);

            int coreqStringLength = coreqEnd - co;
            corequisiteString = description.Substring(co, coreqStringLength);
        }

        private void findEquivalentString(int pre, int co, int eq)
        {
            if (eq == -1)
            {
                equivalentString = "";
                return;
            }

            int[] candidateNumbers = { pre, co, description.Length - 1 };
            int equivalentEnd = findSmallestGreaterNumber(eq, candidateNumbers);

            int equivalentStringLength = equivalentEnd - eq;
            equivalentString = description.Substring(eq, equivalentStringLength);
        }

        // finds smallest greater number.
        private int findSmallestGreaterNumber(int original, params int[] numbers)
        {
            int result = 99999;
            foreach (int number in numbers)
            {
                if (number <= original) continue;
                if (number < result) result = number;
            }

            return result;
        }



        private static Func<Match, string> formatMatch = (match) =>
        {
            string value = match.Value;

            // first, add a space if there is no space
            int spaceIndex = value.IndexOf(' ');
            if (spaceIndex == -1)
            {
                int courseCodeLength = 3;
                value = value.Substring(0, value.Length - 3) + " " + value.Substring(value.Length - courseCodeLength);
            }

            // second, remove the _V if there is a _V
            int index = value.IndexOf("_V");
            if (index != -1)
            {
                value = value.Substring(0, index) + value.Substring(index + 2);
            }

            return value;
        };

        private void findCourseRelations()
        {
            string pattern = courseCodeRegex;
            prerequisites = Regex.Matches(prerequisiteString, pattern)
                .Cast<Match>()
                .Select(match => formatMatch(match))
                .Distinct()
                .ToList();

            corequisites = Regex.Matches(corequisiteString, pattern)
                .Cast<Match>()
                .Select(match => formatMatch(match))
                .Distinct()
                .ToList();

            equivalents = Regex.Matches(equivalentString, pattern)
               .Cast<Match>()
               .Select(match => formatMatch(match))
               .Distinct()
               .ToList();
        }


        public void print()
        {
            Console.Write(major + " " + code + " ");
            Console.Write(credits + " ");
            Console.WriteLine(title);
            Console.WriteLine(description);

            Console.WriteLine("---------------------------------");
            Console.WriteLine("Prereq String: " + prerequisiteString);
            Console.WriteLine("Coreq String: " + corequisiteString);
            Console.WriteLine("Equivalent String: " + equivalentString);
            Console.WriteLine("---------------------------------");

            printList("Prerequisites", prerequisites);
            printList("Coerequisites", corequisites);
            printList("Equivalents", equivalents);
        }

        private void printList(string listName, List<string> list)
        {
            Console.WriteLine("Printing " + listName);
            foreach (string prereq in list)
            {
                Console.Write(prereq + " ");
            }
            Console.WriteLine("");
        }
    }
}
