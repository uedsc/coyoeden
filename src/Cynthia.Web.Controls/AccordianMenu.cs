using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2006-06-27
    /// Last Modified:			2007-07-08
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// This is a .NET wrapper around Rico Accordian Panel, extended
    /// to serve as a menu. also relies on dojo and prototype
    /// 
    /// Example usage:
    /// 
    ///<mp:AccordianMenu id="mnu1" runat="server">
    ///<mp:AccordianMenuSection ID="AccordianMenuSection1" runat="server"
    /// TitleResourceFile="ExternalMailResources"
    /// TitleResourceKey="MenuSectionHead"
    /// TitleNavigateUrl="/SiteOffice/Office.aspx?section=externalmail&view=popinbox"
    ///>
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem1" runat="server"
    ///   MenuTextResourceFile="ExternalMailResources"
    ///   MenuTextResourceKey="MenuInBoxLink"
    ///   ImageUrl="/Data/SiteImages/office/Envelope.gif"
    ///   NavigateUrl="/SiteOffice/Office.aspx?section=externalmail&view=popinbox"
    ///  />
    /// 
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem2" runat="server"
    ///     MenuTextResourceFile="ExternalMailResources"
    ///     MenuTextResourceKey="MenuComposeLink"
    ///     ImageUrl="/Data/SiteImages/office/texteditor.png"
    ///     NavigateUrl="/SiteOffice/Office.aspx?section=externalmail&view=popcompose"
    ///  />
    ///  
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem3" runat="server"
    ///       MenuTextResourceFile="ExternalMailResources"
    ///       MenuTextResourceKey="MenuAddEmailAccountLink"
    ///       ImageUrl="/Data/SiteImages/office/add.gif"
    ///       NavigateUrl="/SiteOffice/Office.aspx?section=externalmail&view=addpopaccount"
    /// />
    ///  
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem4" runat="server"
    ///         MenuTextResourceFile="ExternalMailResources"
    ///         MenuTextResourceKey="MenuContactsLink"
    ///         ImageUrl="/Data/SiteImages/office/contact.png"
    ///        NavigateUrl="/SiteOffice/Office.aspx?section=externalmail&view=externalcontacts"
    ///  />
    ///  </mp:AccordianMenuSection>
    ///
    ///  <mp:AccordianMenuSection ID="AccordianMenuSection2" runat="server"
    ///     TitleResourceFile="SiteMailResources"
    ///    TitleResourceKey="MenuSectionHead"
    ///     TitleNavigateUrl="/SiteOffice/Office.aspx?section=sitemail&view=siteinbox"
    ///    >
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem5" runat="server"
    ///   MenuTextResourceFile="SiteMailResources"
    ///   MenuTextResourceKey="MenuInBoxLink"
    ///   ImageUrl="/Data/SiteImages/office/Envelope.gif"
    ///   NavigateUrl="/SiteOffice/Office.aspx?section=sitemail&view=siteinbox"
    /// />
    ///  
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem6" runat="server"
    ///    MenuTextResourceFile="SiteMailResources"
    ///     MenuTextResourceKey="MenuComposeLink"
    ///     ImageUrl="/Data/SiteImages/office/texteditor.png"
    ///     NavigateUrl="/SiteOffice/Office.aspx?section=sitemail&view=sitecompose"
    ///  /> 
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem8" runat="server"
    ///         MenuTextResourceFile="SiteMailResources"
    ///         MenuTextResourceKey="MenuContactsLink"
    ///         ImageUrl="/Data/SiteImages/office/contact.png"
    ///         NavigateUrl="/SiteOffice/Office.aspx?section=sitemail&view=sitecontacts"
    ///  />
    ///  
    ///</mp:AccordianMenuSection>
    ///<mp:AccordianMenuSection ID="AccordianMenuSection3" runat="server"
    ///     TitleResourceFile="SiteOfficeResources"
    ///     TitleResourceKey="MyStuffMenuSectionHead"
    ///     TitleNavigateUrl="/SiteOffice/Office.aspx?section=mystuff&view=files"
    ///   >
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem7" runat="server"
    ///   MenuTextResourceFile="SiteOfficeResources"
    ///   MenuTextResourceKey="MyStuffFilesMenu"
    ///   ImageUrl="/Data/SiteImages/office/drive-s.gif"
    ///   NavigateUrl="/SiteOffice/Office.aspx?section=mystuff&view=files"
    ///  />
    /// 
    ///  <mp:AccordianMenuItem ID="AccordianMenuItem9" runat="server"
    ///     MenuTextResourceFile="SiteOfficeResources"
    ///     MenuTextResourceKey="MyStuffSQLMenu"
    ///     ImageUrl="/Data/SiteImages/office/SQL.png"
    ///     NavigateUrl="/SiteOffice/Office.aspx?section=mystuff&view=gearsdbadmin"
    ///  />
    ///</mp:AccordianMenuSection>
    ///</mp:AccordianMenu>
    /// </summary>
    [ParseChildren(ChildrenAsProperties = false)]
    public class AccordianMenu : Control
    {
        string containerID = null;
        List<AccordianMenuSection> menuSections = new List<AccordianMenuSection>();

        private string containerDivStyle = "margin-top:6px; border-top-width:1px; border-top-style:solid;";
        private string sectionHeadCSSClass = "accordionTabTitleBar";
        private string sectionBodyCSSClass = "accordionTabContentBox";
        private string sectionULCSSClass = "officenav";
        private string startSectionIndex = "0";


        public AccordianMenu()
        {

        }

        public List<AccordianMenuSection> MenuSections
        {
            get { return menuSections; }
            set { menuSections = value; }
        }

        public string ContainerID
        {
            get { return containerID; }
            set { containerID = value; }
        }

        public string StartSectionIndex
        {
            get { return startSectionIndex; }
            set { startSectionIndex = value; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript")]
        public string ScriptDirectory
        {
            get { return (ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript"); }
            set { ViewState["ScriptDirectory"] = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {

            if (obj is AccordianMenuSection) menuSections.Add((AccordianMenuSection)obj);
            
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

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "prototype", "\n<script type=\"text/javascript\" src=\""
                + ResolveUrl("~/ClientScript/prototype.js") + "\"></script>");


            this.Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), 
                "rico", 
                "\n<script type=\"text/javascript\" src=\""
                + ResolveUrl("~/ClientScript/rico.js") + "\"></script>");


            StringBuilder accoridanSetupScript = new StringBuilder();
            accoridanSetupScript.Append("<script language='javascript' type='text/javascript'>\n<!--\n");


            //accoridanSetupScript.Append("var onloads = new Array();");
            

            //accoridanSetupScript.Append("function bodyOnLoad() {");
            //accoridanSetupScript.Append("alert('body onload fired');");
            //accoridanSetupScript.Append("for ( var i = 0 ; i < onloads.length ; i++ )");
            //accoridanSetupScript.Append("onloads[i]();");
            //accoridanSetupScript.Append("}");

            //accoridanSetupScript.Append("bodyOnLoad();");

            //accoridanSetupScript.Append("onloads.push( accord ); function accord()");

            

            //;

            accoridanSetupScript.Append("function accord" + this.ClientID + "(){ new Rico.Accordion( '" 
                + this.ClientID 
                + "', {panelHeight:400, onLoadShowTab:"
                + startSectionIndex + "} ); }");

            accoridanSetupScript.Append("dojo.addOnLoad(accord" + this.ClientID + ") ");

            accoridanSetupScript.Append("\n//-->\n</script>");


            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "accordian-setup" + this.ClientID,
                accoridanSetupScript.ToString());
        
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
     
                base.DataBind();

                writer.WriteBeginTag("div"); //start container div
                writer.WriteAttribute("id", this.ClientID);
                writer.WriteAttribute("style", containerDivStyle);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteLine();

                foreach (AccordianMenuSection section in menuSections)
                {

                    writer.WriteBeginTag("div"); //start section div
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteLine();
                    writer.WriteBeginTag("div"); //start section head div
                    writer.WriteAttribute("class", sectionHeadCSSClass);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteLine();
                    writer.WriteBeginTag("a"); //start head link

                    string sectionTitle = section.TitleResourceKey;
                    try
                    {
                        sectionTitle = HttpContext.GetGlobalResourceObject(section.TitleResourceFile, section.TitleResourceKey).ToString();

                    }
                    catch { }
                    writer.WriteAttribute("title", sectionTitle);
                    writer.WriteAttribute("onclick", "return false;");
                    writer.WriteAttribute("href", section.TitleNavigateUrl);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEncodedText(sectionTitle);
                    writer.WriteEndTag("a"); //end head link
                    writer.WriteLine();
                    writer.WriteEndTag("div"); //end section head div

                    writer.WriteBeginTag("div"); //start section content div
                    writer.WriteAttribute("class", sectionBodyCSSClass);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteLine();

                    writer.WriteBeginTag("ul"); //start section content ul
                    writer.WriteAttribute("class", sectionULCSSClass);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteLine();

                    foreach (AccordianMenuItem menuItem in section.MenuItems)
                    {
                        writer.WriteFullBeginTag("li");
                        writer.WriteLine();

                        writer.WriteBeginTag("a"); //start item link

                        string menuTitle = menuItem.MenuTextResourceKey;
                        try
                        {
                            menuTitle
                                = HttpContext.GetGlobalResourceObject(
                                menuItem.MenuTextResourceFile,
                                menuItem.MenuTextResourceKey).ToString();

                        }
                        catch { }
                        writer.WriteAttribute("title", menuTitle);
                        writer.WriteAttribute("href", menuItem.NavigateUrl);
                        writer.Write(HtmlTextWriter.TagRightChar);

                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("alt", menuTitle);
                        writer.WriteAttribute("src", menuItem.ImageUrl);
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                        writer.WriteEndTag("a"); //end item link
                        writer.WriteLine();

                        writer.WriteBreak();
                        writer.WriteLine();

                        writer.WriteBeginTag("a"); //start item link
                        writer.WriteAttribute("title", menuTitle);
                        writer.WriteAttribute("href", menuItem.NavigateUrl);
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteEncodedText(menuTitle);
                        writer.WriteEndTag("a"); //end item link
                        writer.WriteLine();

                        writer.WriteEndTag("li");
                        writer.WriteLine();
                    }

                    writer.WriteEndTag("ul"); //end section content ul
                    writer.WriteLine();
                    writer.WriteEndTag("div"); //end section content div
                    writer.WriteLine();

                    writer.WriteEndTag("div"); //end section div
                    writer.WriteLine();
                }

                writer.WriteLine();
                writer.WriteEndTag("div"); //end container div
            }

        }


       

    }

    public class AccordianMenuSection : Control
    {
        string titleResourceFile = string.Empty;
        string titleResourceKey = string.Empty;
        string titleNavigateUrl = string.Empty;
        List<AccordianMenuItem> menuItems = new List<AccordianMenuItem>();

        public List<AccordianMenuItem> MenuItems
        {
            get { return menuItems; }
            set { menuItems = value; }
        }

        public string TitleResourceFile
        {
            get { return titleResourceFile; }
            set { titleResourceFile = value; }
        }

        public string TitleResourceKey
        {
            get { return titleResourceKey; }
            set { titleResourceKey = value; }
        }

        public string TitleNavigateUrl
        {
            get { return titleNavigateUrl; }
            set { titleNavigateUrl = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {

            if (obj is AccordianMenuItem) menuItems.Add((AccordianMenuItem)obj);

        }

        



    }

    public class AccordianMenuItem : Control
    {
        string menuTextResourceFile = string.Empty;
        string menuTextResourceKey = string.Empty;
        string imageUrl = string.Empty;
        string navigateUrl = string.Empty;

        public string MenuTextResourceFile
        {
            get { return menuTextResourceFile; }
            set { menuTextResourceFile = value; }
        }

        public string MenuTextResourceKey
        {
            get { return menuTextResourceKey; }
            set { menuTextResourceKey = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string NavigateUrl
        {
            get { return navigateUrl; }
            set { navigateUrl = value; }
        }



    }

}
