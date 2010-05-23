/// Last Modified:      2009-05-01
/// 
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Cynthia.Net;

namespace Cynthia.Web.GroupUI
{
	
    public partial class UnsubscribeGroup : CBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeGroup));

 
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
            
			if (!Request.IsAuthenticated)
			{
				lblUnsubscribe.Text = ResourceHelper.GetMessageTemplate("AccessDeniedMessage.config");
				return;
			}

            int forumID = WebUtils.ParseInt32FromQueryString("itemid", -1);

            if (forumID > -1)
            {
                UnsubscribeUser(forumID);
                return;
            }

            if ((WebUser.IsAdmin)&&(Request.Params.Get("ue") != null))
            {
                UnsubscribeUserFromAll(Request.Params.Get("ue"));
            }
            
			
					
		}


        private void UnsubscribeUser(int forumId)
        {
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null) return;
            Group forum = new Group(forumId);
            if (!forum.Unsubscribe(siteUser.UserId))
            {
                log.ErrorFormat("Group.UnSubscribe({0}, {1}, ) failed", forumId, siteUser.UserId);
                lblUnsubscribe.Text = Resources.GroupResources.GroupUnsubscribeFailed;
                return;
            }
            lblUnsubscribe.Text = Resources.GroupResources.GroupUnsubscribeCompleted;	


        }

        private void UnsubscribeUserFromAll(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) { return; }
            if(!Email.IsValidEmailAddressSyntax(userEmail)){ return;}

            
            SiteUser user = SiteUser.GetByEmail(siteSettings, userEmail);
            if(user == null) { return;}
            if(user.UserGuid == Guid.Empty){ return;}

            GroupThread.UnsubscribeAll(user.UserId);
            Group.UnsubscribeAll(user.UserId);

            lblUnsubscribe.Text = Resources.GroupResources.AdminUnsubscribeUserComplete;	

        }


		
	}
}
