using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.XmlUI
{
	/// <summary>
    ///	Author:					Joe Audette
    /// Last Modified:			2009-01-02
    /// 
	/// </summary>
	public partial  class XmlModule : SiteModuleControl 
	{
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        
        protected void Page_Load(object sender, EventArgs e) 
		{
			Title1.EditUrl = SiteRoot + "/XmlXsl/XmlEdit.aspx";
            Title1.EditText = XmlResources.XmlEditButton;

            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
            }

            if (
                (Settings.Contains("XmlModuleXmlSourceSetting"))
                && (Settings.Contains("XmlModuleXslSourceSetting"))
                )
            {
                string xmlPath =
                    "~/Data/Sites/" + SiteSettings.SiteId.ToString()
                    + "/xml/"
                    + Settings["XmlModuleXmlSourceSetting"].ToString();

                string xslPath =
                    "~/Data/Sites/" + SiteSettings.SiteId.ToString()
                    + "/xsl/"
                    + Settings["XmlModuleXslSourceSetting"].ToString();

                if (xmlPath.ToLower().EndsWith(".xml"))
                {
                    if (File.Exists(Server.MapPath(xmlPath)))
                    {
                        xml1.DocumentSource = xmlPath;
                    }
                    else
                    {
                        Controls.Add(new LiteralControl("<br /><span class='txterror'>File " + xmlPath + " not found.<br />"));
                    }
                }

                if (xslPath.ToLower().EndsWith(".xsl"))
                {
                    if (File.Exists(Server.MapPath(xslPath)))
                    {
                        xml1.TransformSource = xslPath;
                    }
                    else
                    {
                        Controls.Add(new LiteralControl("<br /><span class='txterror'>File " + xslPath + " not found.<br />"));
                    }
                }
            }
		}

    }
}
