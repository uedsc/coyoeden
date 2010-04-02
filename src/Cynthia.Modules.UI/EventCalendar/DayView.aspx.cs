/// Author:				        Joe Audette
/// Created:			        2005-04-12
/// Last Modified:		        2009-01-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;

namespace Cynthia.Web.EventCalendarUI
{
    public partial class EventCalendarDayView : CBasePage
	{
        
		private int moduleId = 0;
        private DateTime theDate = DateTime.Now;

        
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{
            LoadParams();

            if (!UserCanViewPage())
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateControls();

		}

        private void PopulateControls()
        {
            if (!WebUser.HasEditPermissions(siteSettings.SiteId, moduleId, CurrentPage.PageId))
            {
                this.lnkNewEvent.Visible = false;
            }

            if (moduleId > 0)
            {
                Module module = new Module(moduleId);
                this.litDate.Text = module.ModuleTitle + " " + this.theDate.ToShortDateString();

                lnkNewEvent.HRef = SiteRoot + "/EventCalendar/EditEvent.aspx?"
                    + "mid=" + moduleId.ToString()
                    + "&date=" + Server.UrlEncode(this.theDate.ToString("s"))
                    + "&pageid=" + CurrentPage.PageId.ToString();

                lnkNewEvent.InnerHtml = Resources.EventCalResources.EventCalendarAddEventLabel;

                DataSet ds = CalendarEvent.GetEvents(this.moduleId, theDate, theDate);
                //				DataView dv = ds.Tables[0].DefaultView;
                //				dv.Sort = "StartTime ASC ";
                this.rptEvents.DataSource = ds;
                this.rptEvents.DataBind();

            }
        }

        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            theDate = WebUtils.ParseDateFromQueryString("date", DateTime.Now);
  
        }

	}
}
