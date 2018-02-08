using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace K2Field.AdoQueryHelper
{
    public static class AdQueryHelper
    {
        public static List<AdObject> GetComputers(string ldapPath, int sizeLimit, int pageSize)
        {
            List<AdObject> adEntries = new List<AdObject>();
            using (DirectoryEntry entry = new DirectoryEntry(ldapPath))
            {
                using (DirectorySearcher mySearcher = new DirectorySearcher(entry))
                {
                    mySearcher.Filter = ("(objectClass=computer)");
                    // No size limit, reads all objects
                    mySearcher.SizeLimit = 0;
                    // Read data in pages of 250 objects. Make sure this value is below the limit configured in your AD domain (if there is a limit)
                    mySearcher.PageSize = 250;
                    // Let searcher know which properties are going to be used, and only load those
                    mySearcher.PropertiesToLoad.Add("name");
                    mySearcher.PropertiesToLoad.Add("displayName");
                    mySearcher.PropertiesToLoad.Add("operatingSystem");
                    mySearcher.PropertiesToLoad.Add("cn");
                    mySearcher.PropertiesToLoad.Add("sAMAccountName");
                    mySearcher.PropertiesToLoad.Add("distinguishedName");
                    mySearcher.PropertiesToLoad.Add("objectCategory");
                    foreach (SearchResult resEnt in mySearcher.FindAll())
                    {
                        // Note: Properties can contain multiple values.
                        if (resEnt.Properties["name"].Count > 0)
                        {
                            var adEntry = new AdObject();
                            adEntry.Name = CheckExists(resEnt.Properties, "name");
                            adEntry.DisplayName = CheckExists(resEnt.Properties, "displayName");
                            adEntry.OperatingSystem = CheckExists(resEnt.Properties, "operatingSystem");
                            adEntry.CN = CheckExists(resEnt.Properties, "cn");
                            adEntry.SamAccountName = CheckExists(resEnt.Properties, "sAMAccountName");
                            adEntry.DistinguishedName = CheckExists(resEnt.Properties, "distinguishedName");
                            adEntry.ObjectCategory = CheckExists(resEnt.Properties, "objectCategory");
                            adEntries.Add(adEntry);
                        }
                    }
                }
            }
            return adEntries;
        }
        private static string CheckExists(ResultPropertyCollection col, string propName)
        {
            string result = string.Empty;
            if (col[propName].Count > 0)
            {
                result = (string)col[propName][0];
            }
            return result;
        }
    }
}
