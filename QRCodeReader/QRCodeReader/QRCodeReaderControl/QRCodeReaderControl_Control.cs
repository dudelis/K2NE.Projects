using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//SourceCode.Forms.Controls.Web.SDK.dll, located in the GAC of the smartforms server or in the bin folder of the K2 Designer web site
using SourceCode.Forms.Controls.Web.SDK;
using SourceCode.Forms.Controls.Web.SDK.Attributes;

//adds the client-side .js file as a web resource
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.QRCodeReaderControl_Script.js", "text/javascript", PerformSubstitution = true)]
//adds the client-side style sheet as a web resource
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.QRCodeReaderControl_Stylesheet.css", "text/css", PerformSubstitution = true)]

[assembly: WebResource("QRCodeReader.QRCodeReaderControl.grid.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.version.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.detector.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.formatinf.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.errorlevel.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.bitmat.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.datablock.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.bmparser.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.datamask.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.rsdecoder.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.gf256poly.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.gf256.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.decoder.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.qrcode.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.findpat.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.alignpat.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.databr.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("QRCodeReader.QRCodeReaderControl.QRCodeReader.js", "text/javascript", PerformSubstitution = true)]


namespace QRCodeReader.QRCodeReaderControl
{
    //specifies the location of the embedded definition xml file for the control
    [ControlTypeDefinition("QRCodeReader.QRCodeReaderControl.QRCodeReaderControl_Definition.xml")]
    //specifies the location of the embedded client-side javascript file for the control
    [ClientScript("QRCodeReader.QRCodeReaderControl.QRCodeReaderControl_Script.js")]
    //specifies the location of the embedded client-side stylesheet for the control
    [ClientCss("QRCodeReader.QRCodeReaderControl.QRCodeReaderControl_Stylesheet.css")]
    //specifies the location of the client-side resource file for the control. You will need to add a resource file to the project properties
    //[ClientResources("QRCodeReader.Resources.[ResrouceFileName]")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.grid.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.version.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.detector.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.formatinf.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.errorlevel.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.bitmat.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.datablock.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.bmparser.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.datamask.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.rsdecoder.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.gf256poly.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.gf256.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.decoder.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.qrcode.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.findpat.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.alignpat.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.databr.js")]
    [ClientScript("QRCodeReader.QRCodeReaderControl.QRCodeReader.js")]


 



    public class Control : BaseControl
    {
        #region Control Properties
        //to be able to use the control's properties in code-behind, define public properties 
        //with the same names as the properties defined in the Definition.xml file's <Properties> section
        //create get/set methods and return the property of the same name but to lower case

        //in this example, we are exposing the <Prop ID="ControlText"> property from the definition.xml file to the code-behind
        public string ControlText
        {
            get
            {
                return this.Attributes["controltext"];
            }
            set
            {
                this.Attributes["controltext"] = value;
            }
        }

        //IsVisible property
        public bool IsVisible
        {
            get
            {
                return this.GetOption<bool>("isvisible", true);
            }
            set
            {
                this.SetOption<bool>("isvisible", value, true);
            }
        }

        //IsEnabled property
        public bool IsEnabled
        {
            get
            {
                return this.GetOption<bool>("isenabled", true);
            }
            set
            {
                this.SetOption<bool>("isenabled", value, true);
            }
        }

        //Value property. 
        //"Value" is the value set with the standard getValue/getValue js methods. You can override these methods to set a different property
        public string Value
        {
            get { return this.Attributes["value"]; }
            set { this.Attributes["value"] = value; }
        }

        //IsVisible property
        public bool OutputDebugInfo
        {
            get
            {
                return this.GetOption<bool>("outputdebuginfo", true);
            }
            set
            {
                this.SetOption<bool>("outputdebuginfo", value, true);
            }
        }

        #region IDs
        public string ControlID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public override string ClientID
        {
            get
            {
                return base.ID;
            }
        }

        public override string UniqueID
        {
            get
            {
                return base.ID;
            }
        }
        #endregion

        #endregion

        #region Contructor
        public Control()
            : base("span")  //TODO: if needed, inherit from a HTML type like div or input
        {
            
            //Attributes.Add("caprture", "camera");
            //Attributes.Add("accept", "image/*");
            
            //Attributes.Add("name", "cameraInput");
            ////Attributes.Add("onchange", "handleFiles(this.files)");


        }
        #endregion

        #region Control Methods
        protected override void CreateChildControls()
        {
            base.EnsureChildControls();

            //TODO: if necessary, create child controls for the control.

            //Perform state-specific operations
            switch (base.State)
            {
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime:
                    //assign a temp unique Id for the control
                    this.ID = Guid.NewGuid().ToString();
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Preview:
                    //do any Preview-time manipulation here
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime:
                    //do any runtime manipulation here
                    this.Attributes.Add("enabled", this.IsEnabled.ToString());
                    this.Attributes.Add("visible", this.IsVisible.ToString());
                    break;
            }

           
            // Call base implementation last
            base.CreateChildControls();
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            LiteralControl ctrl = new LiteralControl();
            ctrl.Text = "<input type=\"file\" capture=\"camera\" accept=\"image/*\" id=\"cameraInput\" name=\"cameraInput\" onchange=\"handleFiles(this.files)\">";
            ctrl.RenderControl(writer);
            //TODO: if needed, implement a method to render contents
        }
        #endregion

        /// <summary>
        /// this helper method outputs a label control with various properties for the Smartforms control
        /// it is intended for development and debugging purposes so that you can output the various properties of your custom control
        /// Feel free to add code and properties to the output element
        /// </summary>
        
    }
}
