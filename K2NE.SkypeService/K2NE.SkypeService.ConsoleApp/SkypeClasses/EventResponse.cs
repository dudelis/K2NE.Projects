using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2NE.SkypeService.ConsoleApp.SkypeClasses
{
    public class EventResponse
    {
        public Dictionary<string, Link> _links { get; set; }
 
        public Sender[] Sender { get; set; }
    }

    public class Sender
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public Event[] Events { get; set; }
    }

    public class Event
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public Link Link { get; set; }
        public EmbeddedResource _Embedded { get; set; }
    }

    public class EmbeddedResource
    {
        public MessagingInvitation MessagingInvitation { get; set; }
        public Conversation Conversation { get; set; }
    }

    public class MessagingInvitation
    {
        public string Rel { get; set; }
        public string Direction { get; set; }
        public string Importance { get; set; }
        public string ThreadId { get; set; }
        public string State { get; set; }
        public string OperationId { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, Link> _links { get; set; } 

    }

    public class Conversation
    {
        public string State { get; set; }
        public string ThreadId { get; set; }
        public string Subject { get; set; }
        public string Rel { get; set; }
        public string Importance { get; set; }
        public bool Recording { get; set; }
        public string[] ActiveModalities { get; set; }
        
        public Dictionary<string, Link> _links { get; set; } 

    }
}
