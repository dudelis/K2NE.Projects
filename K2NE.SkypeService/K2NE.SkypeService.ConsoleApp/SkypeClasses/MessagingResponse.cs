using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2NE.SkypeService.ConsoleApp.SkypeClasses
{
    public class MessagingResponse
    {
        public string State { get; set; }
        public string[] NegotiatedMessageFormats { get; set; }
        public string Rel { get; set; }
        public Dictionary<string, Link> _links { get; set; } 
    }
}
