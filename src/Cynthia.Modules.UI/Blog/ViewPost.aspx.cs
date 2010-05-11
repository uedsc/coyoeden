
using System;
using Cynthia.Web.Framework;

namespace Cynthia.Web.BlogUI
{
	
    public partial class BlogView : CBasePage
    {
        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            
            base.OnInit(e);
            
            
        }

        
        #endregion

        

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
        }

        private int moduleId = -1;

        private void Page_Load(object sender, EventArgs e)
		{
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            pnlContainer.ModuleId = moduleId;
        
		}

        protected override void OnError(EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            if ((lastError != null) && (lastError is NullReferenceException) && Page.IsPostBack)
            {
                if (lastError.StackTrace.Contains("Recaptcha"))
                {
                    Server.ClearError();
                    WebUtils.SetupRedirect(this, Request.RawUrl);

                }

            }
           

        }

        

	}
}
