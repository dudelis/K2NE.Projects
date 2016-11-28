using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2NE.SkypeService.ConsoleApp.SkypeClasses
{
    public class OauthToken
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Ms_rtc_identityscope { get; set; }
        public string Token_type { get; set; }
    }
}
