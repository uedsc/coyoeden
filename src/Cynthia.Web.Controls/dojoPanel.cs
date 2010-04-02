using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-06-17
    /// Last Modified:			2007-10-25
    ///		
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.		
    /// </summary>
    public class dojoPanel : Panel
    {
        private string dojoType = "dijit.layout.ContentPane";
        private string sizeShare = "50";
        private string orientation = "horizontal";
        private string sizerWidth = "5";
        private string activeSizing = "0";
        private string style = string.Empty;
        private string layoutAlign = string.Empty;
        private string title = string.Empty;
        private string tabPosition = "top"; // top bottom, left-h, right-h

       
        public string DojoType
        {
            get { return dojoType; }
            set { dojoType = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string TabPosition
        {
            get { return tabPosition; }
            set { tabPosition = value; }
        }

        public string SizeShare
        {
            get { return sizeShare; }
            set { sizeShare = value; }
        }

        public string Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public string LayoutAlign
        {
            get { return layoutAlign; }
            set { layoutAlign = value; }
        }

        public string SizerWidth
        {
            get { return sizerWidth; }
            set { sizerWidth = value; }
        }

        public string ActiveSizing
        {
            get { return activeSizing; }
            set { activeSizing = value; }
        }

        public new string Style
        {
            get { return style; }
            set { style = value; }
        }


        public string SelectedPanelClientID
        {
            get
            {
                string obj = ViewState["SelectedPanelClientID"] as string;
                return (obj != null) ? obj : string.Empty;
            }
            set
            {
                ViewState["SelectedPanelClientID"] = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Attributes.Add("dojoType", dojoType);
            if (sizeShare.Length > 0)
            {
                this.Attributes.Add("sizeShare", sizeShare);
            }

            if (title.Length > 0)
            {
                this.Attributes.Add("title", title);
            }

            if (dojoType == "dijit.layout.TabContainer")
            {

                if ((tabPosition.Length > 0)&&(tabPosition != "top"))
                {
                    this.Attributes.Add("tabPosition", tabPosition);
                }

            }

            if (dojoType == "dijit.layout.SplitContainer")
            {
                this.Attributes.Add("orientation", orientation);
                this.Attributes.Add("sizerWidth", sizerWidth);
                this.Attributes.Add("activeSizing", activeSizing);
            }

            if (layoutAlign.Length > 0)
            {
                this.Attributes.Add("layoutAlign", layoutAlign);
            }

            if (style.Length > 0)
            {
                this.Attributes.Add("style", style);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupScripts();
            
        }

        private void SetupScripts()
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo", "\n<script type=\"text/javascript\" src=\""
                + ResolveUrl("~/ClientScript/dojo0-9/dojo/dojo.js") + "\" djConfig=\"parseOnLoad: true\"></script>");

            RequireParser();

            switch (dojoType)
            {
                case "dijit.layout.LayoutContainer":
                    RequireLayoutContainer();
                    break;

                case "dijit.layout.SplitContainer":
                    RequireSplitContainer();
                    break;


                case "dijit.layout.TabContainer":
                    RequireTabContainer();
                    DoTabSelection();
                    break;

                case "dijit.layout.ContentPane":
                default:
                    RequireContentPane();
                    break;
            }

        }

        private void RequireParser()
        {
            string dojoRequireScript = @"
<script language='javascript' type='text/javascript'>
<!--
    dojo.require('dojo.parser');
//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo-parser", dojoRequireScript);

        }

       
        private void RequireLayoutContainer()
        {
            string dojoRequireScript = @"
<script language='javascript' type='text/javascript'>
<!--
    dojo.require('dijit.layout.LayoutContainer');
//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo-layoutcontainer", dojoRequireScript);

        }

        private void RequireSplitContainer()
        {
            string dojoRequireScript = @"
<script language='javascript' type='text/javascript'>
<!--
    dojo.require('dijit.layout.SplitContainer');
//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo-splitcontainer", dojoRequireScript);

        }

        private void RequireTabContainer()
        {
            string dojoRequireScript = @"
<script language='javascript' type='text/javascript'>
<!--
    dojo.require('dijit.layout.TabContainer');
//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo-tabcontainer", dojoRequireScript);

        }

        private void RequireContentPane()
        {
            string dojoRequireScript = @"
<script language='javascript' type='text/javascript'>
<!--
    dojo.require('dijit.layout.ContentPane');
//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dojo-contentpane", dojoRequireScript);

        }


        private void DoTabSelection()
        {

            if (SelectedPanelClientID.Length == 0) return;

            StringBuilder collapseScript = new StringBuilder();
            collapseScript.Append("<script language=\"javascript\" type=\"text/javascript\"> ");
            collapseScript.Append("\n<!-- \n");

            collapseScript.Append("function doSelect" + this.ClientID + "() {  ");
            collapseScript.Append("dijit.byId('"
                + this.ClientID + "').selectChild(dijit.byId('" + SelectedPanelClientID + "') ); ");

            collapseScript.Append(" }  ");

            collapseScript.Append("dojo.addOnLoad(doSelect" + this.ClientID + "); ");
            
            collapseScript.Append("\n//--> ");
            collapseScript.Append(" </script>");

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "selecttab" + this.ClientID,
                collapseScript.ToString());

            

        }

    }
}
