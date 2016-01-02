using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Models
{
    public class L
    {
        public string id { get; set; }
        public string t { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class U
    {
        public string i { get; set; }
    }

    public class F
    {
        public string id { get; set; }
        public string t { get; set; }
        public L l { get; set; }
        public U u { get; set; }
    }

    public class Version
    {
        public string id { get; set; }
        public F f { get; set; }
        public int ts { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class Results
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public List<Version> versions { get; set; }
    }

    public class FeedVersion
    {
        public string status { get; set; }
        public int ts { get; set; }
        public Results results { get; set; }
    }
}
