using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UBC_Course_Web_Scrapping_Script
{
    internal class Course
    {

        private static string courseCodeRegex = "[A-Z]{3,4}(_V)?\\s?[0-9]{3}";
        private string major { get; set; }

        private string code { get; set; }

        private string courseCode => major + " " + code;

        private int credits { get; set; }

        private string title { get; set; }

        private string description { get; set; }
        private string lowercaseDescription { get; set; }

        private string prerequisiteString { get; set; }
        private string corequisiteString { get; set; }
        private string equivalentString { get; set; }

        private List<string> prerequisites  { get; set; }
        private List<string> corequisites  { get; set; }
        private List<string> equivalents { get; set; }

        public Course(string courseCode, int? credits, string title, string description) {

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
            int codeLength = 3;

            try
            {
                this.major = courseCode.Substring(0, spaceIndex);
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
            } catch (ArgumentOutOfRangeException)
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

        private void findCourseRelations()
        {

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
        }
    }
}
