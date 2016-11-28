using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2NE.SkypeService.ConsoleApp.SkypeClasses
{

    public class Me
    {
        public string name { get; set; }
        public string uri { get; set; }
        public List<string> emailAddresses { get; set; }
        public string title { get; set; }
        public Dictionary<string, Link> _links { get; set; }
        public string rel { get; set; }
    }

    public class People
    {
        public Dictionary<string, Link> _links { get; set; }
        public string rel { get; set; }
    }

    public class OnlineMeetings
    {
        public Dictionary<string, Link> _links { get; set; }
        public string rel { get; set; }
    }

    public class Communication
    {
        public List<object> supportedModalities { get; set; }
        public List<string> supportedMessageFormats { get; set; }
        public Dictionary<string, Link> _links { get; set; }
        public string rel { get; set; }
        public string etag { get; set; }
    }

    public class Embedded
    {
        public Me me { get; set; }
        public People people { get; set; }
        public OnlineMeetings onlineMeetings { get; set; }
        public Communication communication { get; set; }
    }

    public class ApplicationsResponse
    {
        public string Culture { get; set; }
        public string UserAgent { get; set; }
        public Dictionary<string, Link> _links { get; set; }
        public Embedded _embedded { get; set; }
        public string rel { get; set; }
    }
}
