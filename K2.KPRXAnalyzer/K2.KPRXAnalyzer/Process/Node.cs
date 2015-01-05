using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace K2.KPRXAnalyzer.Process
{
    public class Node : INode
    {
        #region Private properties

        private string _name;
        private NodeType _type;
        private Guid _itemGuid;
        private XmlNode _outerXml;
        private bool _containsString = false;

        #endregion

        #region Public properties
        public string Name
        {
            get { return _name; }
        }
        public NodeType Type
        {
            get { return _type; }
        }
        public string TypeString
        {
            get { return _type.ToString(); }
        }
        public Guid ItemGuid
        {
            get { return _itemGuid; }
        }
        public XmlNode OuterXml
        {
            get { return _outerXml; }
        }
        public bool ContainsString
        {
            get { return _containsString; }
            set { _containsString = value; }
        }
        public List<INode> ChildNodes { get; set; }
        public INode ParentNode { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Default Constructor for building the node and child ones
        /// </summary>
        /// <param name="xNode">XmlNode class</param>
        /// <param name="nodeType">Type of the Node</param>
        public Node(XmlNode xNode, NodeType nodeType, INode parentNode = null)
        {
            //Cloning the node in order not to have the parent ones
            XmlNode xn = xNode.Clone();
            if (xn.SelectNodes("Name").Count > 0)
            {
                this.ParentNode = parentNode;
                this._outerXml = xn;
                this._type = nodeType;
                this._name = xn["Name"].InnerText;
                this._itemGuid = Guid.Parse(xn["Guid"].InnerText);
                this.ChildNodes = new List<INode>();
                this.GetKids();
            }
            else
            {
                this.ParentNode = parentNode;
                this._name = nodeType.ToString();
                this._type = nodeType;
                this._outerXml = xn;
                this.ChildNodes = new List<INode>();
                this.GetKids();
            }
        }
        
        /// <summary>
        /// The dafault method, used for creating child nodes in the default constructore
        /// </summary>
        protected void GetKids()
        {
            foreach (NodeType type in Enum.GetValues(typeof (NodeType)))
            {
                string xPath = @"/" + type.ToString();
                if (this.Type != NodeType.Line)
                {
                    foreach (XmlNode xNode in OuterXml.SelectNodes(xPath))
                    {
                        Node node = new Node(xNode, type, this);
                        this.ChildNodes.Add(node);
                    }
                }
            }

        }

        /// <summary>
        /// Creates a list of nodes in order to have the List available for treeview
        /// </summary>
        /// <param name="XmlPath">The path to the kprx file</param>
        /// <param name="SearchItem">String, which is searched inside the Workflow</param>
        /// <returns>List of Nodes.</returns>
        public static List<INode> GetNodesWithExecItems(string XmlPath, string SearchItem)
        {
            XmlDocument procXml = new XmlDocument();
            procXml.Load(XmlPath);
            Node node = new Node(procXml.ChildNodes[1], NodeType.Process);
            List<ExecutableItem> execItems = ExecutableItem.GetExecutableItems(procXml.ChildNodes[1], SearchItem);

            foreach (ExecutableItem item in execItems)
            {
                if (item.ContainsString)
                {
                    node.CompareGuids(item.ParentGuid);
                }
            }

            List<INode> nodes = new List<INode>();
            nodes.Add(node);
            return nodes;
        }

        /// <summary>
        /// Recursive call to find and update all tree which contain the searched string
        /// </summary>
        /// <param name="execItemGuid">Guid of the item from execution layer</param>
        /// <returns>Boolean value</returns>
        public void CompareGuids(Guid execItemGuid)
        {
            if (ItemGuid == execItemGuid)
            {
                _containsString = true;
                Node.UpdateParentContainsString(true, ParentNode);
            }
            else
            {
                foreach (INode n in ChildNodes)
                {
                    n.CompareGuids(execItemGuid);
                    
                }
            }
        }
        
        /// <summary>
        /// Updating all the parent nodes up the tree to mark the whole tree branch
        /// </summary>
        /// <param name="containsString">Current node property if the containsString value is true/false</param>
        /// <param name="node">Parent node</param>
        private static void UpdateParentContainsString(bool containsString, INode node = null)
        {
            if (node != null)
            {
                node.ContainsString = containsString;
                UpdateParentContainsString(containsString, node.ParentNode);
            }
        }
#endregion

    }
}