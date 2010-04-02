///	Last Modified:              2009-05-01
///	
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Editor;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.UI.Pages
{
    public partial class HelpEdit : Page
    {
        private String helpKey = String.Empty;
        private SiteSettings siteSettings;
        private bool isSiteEditor = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            if ((!isSiteEditor)&&(!WebUser.IsAdminOrContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (Request.Params.Get("helpkey") != null)
            {
                helpKey = Request.Params.Get("helpkey");
            }

            PopulateLabels();

            

            if (!IsPostBack)
            {
                PopulateControls(); 
            }
        }


        protected void PopulateControls()
        {
            if (helpKey != String.Empty)
            {
                edContent.Text = ResourceHelper.GetHelpFileText(helpKey);
            }
        }


        protected void PopulateLabels()
        {
           
            edContent.WebEditor.ToolBar = ToolBar.Full;

            btnSave.Text = Resource.HelpEditSaveButton;
            SiteUtils.SetButtonAccessKey(btnSave, AccessKeys.HelpEditSaveButtonAccessKey);

            lnkCancel.Text = Resource.HelpEditCancelButton;
            
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            String updatedHelp = edContent.Text;
            ResourceHelper.SetHelpFileText(helpKey, updatedHelp);
            

            WebUtils.SetupRedirect(this, siteSettings.SiteRoot + "/Help.aspx?helpkey=" + helpKey);

            

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            lnkCancel.NavigateUrl = siteSettings.SiteRoot + "/Help.aspx?helpkey=" + helpKey;
        }


        

        protected override void Render(HtmlTextWriter writer)
        {
            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
            * 
            * Custom HtmlTextWriter to fix Form Action
            * Based on Jesse Ezell's "Fixing Microsoft's Bugs: URL Rewriting"
            * http://weblogs.asp.net/jezell/archive/2004/03/15/90045.aspx
            * 
            * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            // this removes the action attribute from the form
            // so that it posts back correctly when using url re-writing

            string action = Path.GetFileName(Request.RawUrl);
            if (action.IndexOf("?") == -1 && Request.QueryString.Count > 0)
            {
                action += "?" + Request.QueryString.ToString();
            }
            if (writer.GetType() == typeof(HtmlTextWriter))
            {
                writer = new CynHtmlTextWriter(writer, action);
            }
            else if (writer.GetType() == typeof(Html32TextWriter))
            {
                writer = new CynHtml32TextWriter(writer, action);
            }

            base.Render(writer);

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SiteUtils.SetupEditor(edContent);
        }

    }
}
