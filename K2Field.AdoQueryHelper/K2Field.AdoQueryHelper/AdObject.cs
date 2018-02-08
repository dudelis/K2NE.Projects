using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Field.AdoQueryHelper
{
    public class AdObject
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string OperatingSystem { get; set; }
        public string SamAccountName { get; set; }
        public string CN { get; set; }
        public string DistinguishedName { get; set; }
        public string ObjectCategory { get; set; }
    }
}
