using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace K2.KPRXAnalyzer.Classes
{
    public enum Artifacts
    {
        Process,
        ProcessDependency,
        StartRule,
        FinishRule,
        EscalationRule,
        Activity,
        Event,
        LineRule
    }

    public abstract class Node
    {
        #region Private Properties
        public string _name;
        private XmlNode _outerXml;
        private Artifacts _type;
        private Guid _guid;
        #endregion

        #region Public Properties
        public string Name
        {
            get { return _name; }
        }
        public Artifacts Type
        {
            get { return _type; }
        }
        public Guid ItemGuid
        {
            get { return _guid; }
        }
        public XmlNode OuterXml
        {
            get { return _outerXml; }
        }
        public List<Node> ChildNodes {get; set; }
        #endregion
        
        #region Methods
        //public Node(){ }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="xNode">XmlNode for parsing</param>
        /// <param name="nodeType"> Type of the current class</param>
        /// <param name="xPath">XPath to the properties</param>
        public Node(XmlNode xNode, Artifacts nodeType, string xPathItem)
        {
            this._outerXml = xNode;
            this._type = nodeType;
            foreach (XmlNode xn in xNode.SelectNodes(xPathItem))
            {
                this._name = xn["Name"].InnerText;
                this._guid = Guid.Parse(xn["Guid"].InnerText);
            }
        }
        public abstract void GetKids();

        #endregion
    }
}

