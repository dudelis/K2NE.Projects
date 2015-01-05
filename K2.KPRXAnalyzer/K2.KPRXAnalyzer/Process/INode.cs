using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace K2.KPRXAnalyzer.Process
{
    public enum NodeType
    {
        Process,
        Dependencies,
        ProcessDependency,
        Lines,
        Line,
        StartActivity,
        Activities,
        Activity,
        Escalations,
        DefaultEscalation,
        Events,
        Event
    }

    public interface INode
    {
        #region Properties
        string Name { get; }
        string TypeString { get; }
        NodeType Type { get; }
        Guid ItemGuid { get; }
        XmlNode OuterXml { get; }
        bool ContainsString { get; set; }
        List<INode> ChildNodes { get; set; }
        #endregion
        INode ParentNode { get; set; }

        void CompareGuids(Guid guid);

    }
}
