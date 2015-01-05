using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace K2.KPRXAnalyzer.Classes
{
    /// <summary>
    /// Types of the process artifacts
    /// </summary>
    public enum Artifacts
    {
        Process,
        ProcessDependency,
        ProcessEscalation,
        Line,
        StartActivity,
        Activity,
        ActivityEscalation,
        EventEscalation,


        FinishRule,
        EscalationRule,
        
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
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="xNode"></param>
        /// <param name="nodeType"></param>
        public Node(XmlNode xNode, Artifacts nodeType)
        {
            //Cloning the node in order not to have the parent ones
            XmlNode xn = xNode.Clone();
            this._outerXml = xn;
            this._type = nodeType;
            this._name = xn["Name"].InnerText;
            this._guid = Guid.Parse(xn["Guid"].InnerText);
            this.ChildNodes = new List<Node>();
            this.GetKids();
        }

        /// <summary>
        /// Abstract method for building the tree of elements
        /// </summary>
        protected abstract void GetKids();
        #endregion
    }
}

