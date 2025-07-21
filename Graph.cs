using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBC_Course_Web_Scrapping_Script
{
    internal class Graph
    {
        public List<Node> nodes { get; set; }

        public List<Link> links { get; set; }
        public Graph() { 
            nodes = new List<Node>();
            links = new List<Link>();
       }
    }
}
