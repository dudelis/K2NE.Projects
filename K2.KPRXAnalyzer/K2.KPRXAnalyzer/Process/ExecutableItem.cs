using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace K2.KPRXAnalyzer.Process
{
    /// <summary>
    /// Object which contains main properties of the executable items from kprx file and References fields
    /// </summary>
    public class ExecutableItem
    {
        #region Private properties
        private Guid _parentGuid;
        private XmlNode _outerXml;
        private bool _containsString;
        #endregion
        #region Public properties
        public Guid ParentGuid { get { return _parentGuid; } }
        public XmlNode OuterXml { get { return _outerXml; } }
        public bool ContainsString { get { return _containsString; } }
        #endregion
        #region Methods
        /// <summary>
        /// Gets the list of all executable items and checks if any node contains the searched string
        /// </summary>
        /// <param name="xNode">XmlNode whete to look for the string</param>
        /// <param name="searchString">The string which is searched inside the XmlNode</param>
        /// <returns>List of executable items</returns>
        public static List<ExecutableItem> GetExecutableItems(XmlNode xNode, string searchString)
        {
            List<ExecutableItem> execItems = new List<ExecutableItem>();
            //Getting all executable items
            foreach (XmlNode xn in xNode.SelectNodes("//ExecutableItems/*"))
            {
                ExecutableItem execItem = new ExecutableItem();
                XmlNode newNode = xn.Clone();
                execItem._outerXml = newNode;
                execItem._parentGuid = Guid.Parse(newNode.SelectSingleNode("Parent").Attributes[0].InnerText);
                execItem._containsString = ExecutableItem.ItemExists(searchString, newNode);
                execItems.Add(execItem);
            }
            //Checking all the dependencies - i.e. references
            foreach (XmlNode xn in xNode.SelectNodes("//Dependencies/*"))
            {
                ExecutableItem execItem = new ExecutableItem();
                XmlNode newNode = xn.Clone();
                execItem._outerXml = newNode;
                execItem._parentGuid = Guid.Parse(newNode["Guid"].InnerText);
                execItem._containsString = ExecutableItem.ItemExists(searchString, newNode);
                execItems.Add(execItem);
            }

            return execItems;
        }

        /// <summary>
        /// Checks if the item exists inside the given XmlNode
        /// </summary>
        /// <param name="s">Searched string</param>
        /// <param name="xNode">XmlNode where the item is searched</param>
        /// <returns>Boolean value - if the item exists ir not</returns>
        private static bool ItemExists(string s, XmlNode xNode)
        {
            bool exist = false;
            foreach (XmlNode xn in xNode.SelectNodes("//*"))
            {
                if (xn.InnerText.ToString() == s)
                {
                    exist = true;
                }
            }
            return exist;
        }
        #endregion
    }
}
