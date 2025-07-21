using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBC_Course_Web_Scrapping_Script
{
    internal class Link
    {
        public string source {  get; set; }

        public string target { get; set; }
        public Link(string source, string target) {
            this.source = source;
            this.target = target;
        }
    }
}
