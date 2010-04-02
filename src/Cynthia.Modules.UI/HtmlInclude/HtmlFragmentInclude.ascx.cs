using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ContentUI 
{
	/// <summary>
	///	Author:				Joe Audette
	/// Created:			2005-11-19
	/// Last Modified:		2008-11-09
	///		
	/// </summary>
	public partial class HtmlFragmentInclude : SiteModuleControl 
	{
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }
        

        protected void Page_Load(object sender, EventArgs e)
		{
			Title1.EditUrl = SiteRoot + "/HtmlInclude/Edit.aspx";
            Title1.EditText = HtmlIncludeResources.HtmlFragmentIncludeEditLink;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;

            }

			string includePath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() 
				+ "/Data/Sites/" + siteSettings.SiteId.ToString() + "/htmlfragments");

			string includeFileName = (string) Settings["HtmlFragmentSourceSetting"];
			string includeContentFile = includePath + Path.DirectorySeparatorChar + includeFileName;
			
			if (includeFileName != null)
			{
				if  (File.Exists(includeContentFile)) 
				{
					FileInfo file = new FileInfo(includeContentFile);
					StreamReader sr  = file.OpenText();
					this.lblInclude.Text = sr.ReadToEnd();
					sr.Close();
				}
				else 
				{
					Controls.Add(new LiteralControl("<br /><span class='txterror'>File " + includeContentFile + " not found.<br />"));
				}
			}
		}

	}
}
