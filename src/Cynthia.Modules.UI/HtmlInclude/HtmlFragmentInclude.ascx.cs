using System;
using System.IO;
using System.Web;
using System.Web.UI;
using Cynthia.Web.Framework;
using Resources;
using System.Web.UI.WebControls;
using Cynthia.Web.UI;
using SystemX;
namespace Cynthia.Web.ContentUI 
{
	public partial class HtmlFragmentInclude : SiteModuleControl
	{

		#region member variables
		/// <summary>
		/// Render the included html file without any additional generated html.
		/// </summary>
		protected bool NOExtraMarkUp { get; set; }
		protected string HtmlData { get; private set; }
		private string includePath;
		private string includeFileName;
		private string includeContentFile;
		#endregion

		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += Page_Load;
        }
        

        protected void Page_Load(object sender, EventArgs e)
		{
			//load module settings
			includeFileName = (string)Settings["HtmlFragmentSourceSetting"];
			NOExtraMarkUp = Convert.ToBoolean(Settings["HtmlFragmentNOExtraMarkUp"]);
			includePath = HttpContext.Current.Server.MapPath(String.Format("{0}/Data/Sites/{1}/htmlfragments", WebUtils.GetApplicationRoot(), SiteSettings.SiteId));
			includeContentFile = String.Format("{0}{1}{2}", includePath, Path.DirectorySeparatorChar, includeFileName);


			if (this.ModuleConfiguration != null)
			{
				this.Title = this.ModuleConfiguration.ModuleTitle;
				this.Description = this.ModuleConfiguration.FeatureName;

			}
			
			//reset visibility
			this.ph1.Visible = this.ph2.Visible = false;
			//rendering without additional mark ups
			if (NOExtraMarkUp) {
				this.ph2.Visible = true;
				setTitle(Title2);
				bindHtmlData(lblInclude1);
				return;
			}
			//rendering
			this.ph1.Visible = true;
			setTitle(Title1);
			bindHtmlData(lblInclude);
		}

		#region helper methods
		void bindHtmlData(Literal lbl) {
			if (string.IsNullOrEmpty(includeFileName)) return;
			if (File.Exists(includeContentFile))
			{
				var file = new FileInfo(includeContentFile);
				StreamReader sr = file.OpenText();
				HtmlData = sr.ReadToEnd();
				sr.Close();
				//parsing htmldata
				HtmlData = HtmlData.Replace("%siteroot%", SiteRoot).Replace("%datafolder%",DataFolderUrl).RemoveHtmlWhitespace();

				lbl.Text = HtmlData;		
			}
			else
			{
				Controls.Add(new LiteralControl(String.Format("<br /><span class='txterror'>File {0} not found.<br />", includeContentFile)));
			}
		}
		void setTitle(ModuleTitleControl ctr) {
			ctr.EditUrl = SiteRoot + "/HtmlInclude/Edit.aspx";
			ctr.EditText = HtmlIncludeResources.HtmlFragmentIncludeEditLink;
			ctr.Visible = !this.RenderInWebPartMode;
		}
		#endregion

	}
}
