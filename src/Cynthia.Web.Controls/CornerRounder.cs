using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    ///		Author:				Joe Audette
    ///		Created:			3/29/2007
    ///		Last Modified:		3/30/2007
    ///		
    /// </summary>
    public class CornerRounder : WebControl, INamingContainer
    {
        #region Constructors

        public CornerRounder()
		{
			EnsureChildControls();
		}

		#endregion

        #region Private Properties

        private bool PropertiesValid
        {
            get
            {
                return (NamingContainer.FindControl(ControlToRound) != null);
            }
        }

        #endregion

        #region Public Properties

        
        public string ControlToRound
        {
            get
            {
                string obj = ViewState["ControlToRound"] as string;
                return (obj != null) ? obj : string.Empty;
            }
            set
            {
                ViewState["ControlToRound"] = value;
            }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(false)]
        public bool DoRounding
        {
            get { return (ViewState["DoRounding"] != null ? (bool)ViewState["DoRounding"] : false); }
            set { ViewState["DoRounding"] = value; }
        }


        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript/msajax")]
        public string ScriptDirectory
        {
            get { return (ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript/msajax"); }
            set { ViewState["ScriptDirectory"] = value; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(6)]
        public int Radius
        {
            get { return (ViewState["Radius"] != null ? (int)ViewState["Radius"] : 6); }
            set { ViewState["Radius"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string BackgroundColor
        {
            get { return (ViewState["BackgroundColor"] != null ? (string)ViewState["BackgroundColor"] : string.Empty); }
            set { ViewState["BackgroundColor"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string RoundedBorderColor
        {
            get { return (ViewState["RoundedBorderColor"] != null ? (string)ViewState["RoundedBorderColor"] : string.Empty); }
            set { ViewState["RoundedBorderColor"] = value; }
        }

        #endregion


        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // render to the designer
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                base.Render(writer);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (PropertiesValid)
            {
                SetupScripts();
                Initialize();
            }
           
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

        }

        private void Initialize()
        {

           
        }

        

        private void SetupScripts()
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MicrosoftAjax", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/MicrosoftAjax.js") + "\"></script>");

            //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MicrosoftAjaxWebForms", "<script type=\"text/javascript\" src=\""
            //    + ResolveUrl(this.ScriptDirectory + "/MicrosoftAjaxWebForms.js") + "\"></script>");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MicrosoftAjaxBaseScripts", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/BaseScripts.js") + "\"></script>");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MicrosoftAjaxCommon", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/Common.js") + "\"></script>");


            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RoundedCornersBehavior", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/RoundedCornersBehavior.js") + "\"></script>");


            string colorSetting = string.Empty;
            if (BackgroundColor != string.Empty)
            {
                colorSetting = "\"Color\":\"" + BackgroundColor + "\",";
            }

            string borderColorSetting = string.Empty;
            if (RoundedBorderColor != string.Empty)
            {
                borderColorSetting = "\"BorderColor\":\"" + RoundedBorderColor + "\",";
            }

            string initscript = "<script language=\"javascript\" type=\"text/javascript\">"
                + "Sys.Application.initialize();"
                + " });</script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "ajaxinit", initscript);


            string script = "<script language=\"javascript\" type=\"text/javascript\">"
                + "Sys.Application.add_init(function() { "
                + "$create(AjaxControlToolkit.RoundedCornersBehavior, {" 
                + colorSetting
                + borderColorSetting
                + "\"Radius\":" + this.Radius.ToString() + ",\"id\":\"" + this.UniqueID + "\"}, null, null, $get(\"" + NamingContainer.FindControl(ControlToRound).ClientID + "\"));"
                + " });</script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "roundcorner" + this.UniqueID, script);



        }

    }
}
