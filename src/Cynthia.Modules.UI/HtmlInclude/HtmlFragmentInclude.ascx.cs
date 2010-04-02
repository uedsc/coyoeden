using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.ContentUI 
{
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

			string includePath = HttpContext.Current.Server.MapPath(String.Format("{0}/Data/Sites/{1}/htmlfragments", WebUtils.GetApplicationRoot(), siteSettings.SiteId));

			string includeFileName = (string) Settings["HtmlFragmentSourceSetting"];
			string includeContentFile = String.Format("{0}{1}{2}", includePath, Path.DirectorySeparatorChar, includeFileName);
			
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
					Controls.Add(new LiteralControl(String.Format("<br /><span class='txterror'>File {0} not found.<br />", includeContentFile)));
				}
			}
		}

	}
}
