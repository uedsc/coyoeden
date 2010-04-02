/// Author:				        Joe Audette
/// Created:			        2004-10-03
/// Last Modified:		        2009-10-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Configuration;
using Cynthia.Web.Controls;
using Cynthia.Web.Framework;
using Resources;
using ConsentToken = Cynthia.Web.WindowsLiveLogin.ConsentToken;

namespace Cynthia.Web.UI.Pages
{
	
    public partial class ProfileView : CBasePage
	{
        private Guid userGuid = Guid.Empty;
        private int userID = -1;
        private Double timeOffset = 0;
        //Gravatar public enum RatingType { G, PG, R, X }
        private Gravatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private string avatarPath = string.Empty;
        private SiteUser siteUser = null;
        private bool allowView = false;
        

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            SuppressMenuSelection();
            SuppressPageMenu();
        }
        #endregion


		private void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();

            if (!allowView)
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }
                else
                {
                    WebUtils.SetupRedirect(this, SiteRoot);
                    return;
                }
            }

            if (SiteUtils.SslIsAvailable() && WebConfigSettings.ForceSslOnProfileView) { SiteUtils.ForceSsl(); }

            

			PopulateControls();
		}

		private void PopulateControls()
		{
           
            if (siteUser != null)
            {
                this.lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
                this.lblTotalPosts.Text = siteUser.TotalPosts.ToString(CultureInfo.InvariantCulture);
                
                this.lblUserName.Text = Server.HtmlEncode(siteUser.Name);

                Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
                    Resource.PageTitleFormatProfilePage, siteUser.Name));

                MetaDescription = string.Format(CultureInfo.InvariantCulture,
                    Resource.ProfileViewMetaFormat, Server.HtmlEncode(siteUser.Name));
                

                if (allowGravatars)
                {
                    imgAvatar.Visible = false;
                    gravatar1.Visible = true;
                    gravatar1.Email = siteUser.Email;
                    gravatar1.MaxAllowedRating = MaxAllowedGravatarRating;
                }
                else
                {
                    gravatar1.Visible = false;
                    if (disableAvatars)
                    {
                        divAvatar.Visible = false;
                    }
                    else
                    {
                        if (siteUser.AvatarUrl.Length > 0)
                        {
                            this.imgAvatar.Src = avatarPath + siteUser.AvatarUrl;
                        }
                    }
                }

                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;

                if (WebConfigSettings.UseRelatedSiteMode)
                {
                    // this can't be used in related site mode
                    // because we can't assume forum posts were in this site.
                    divForumPosts.Visible = false;
                }

                if (Request.IsAuthenticated)
                {
                    ShowAuthenticatedProperties(siteUser);
                }
                else
                {
                    ShowAnonymousProperties(siteUser);
                }


                PopulateMessenger();
            }
            else
            {
                this.lblUserName.Text = "User not found";
                imgAvatar.Visible = false;
                gravatar1.Visible = false;
            }
		
            

		}

        private void LoadSettings()
        {
            avatarPath = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");

            allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);
            userID = WebUtils.ParseInt32FromQueryString("userid", true, userID);
            timeOffset = SiteUtils.GetUserTimeOffset();
            userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);

            if (userID > -1)
            {
                siteUser = new SiteUser(siteSettings, userID);
                if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
            }
            else if(userGuid != Guid.Empty)
            {
                siteUser = new SiteUser(siteSettings, userGuid);
                if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
            }

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = true;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

        }

        private void PopulateMessenger()
        {
            if (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger) { return; }
            if (!siteSettings.AllowWindowsLiveMessengerForMembers) { return; }
            if (siteUser == null) { return; }
            if (!siteUser.EnableLiveMessengerOnProfile) { return; }
            if (siteUser.LiveMessengerId.Length == 0) { return; }

            divLiveMessenger.Visible = true;
            chat1.Invitee = siteUser.LiveMessengerId;
            //chat1.InviteeDisplayName = siteUser.Name;

            if (WebConfigSettings.TestLiveMessengerDelegation)
            {
                WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
                WindowsLiveMessenger m = new WindowsLiveMessenger(wl);
                ConsentToken token = m.DecodeToken(siteUser.LiveMessengerDelegationToken);
                ConsentToken refreshedToken = m.RefreshConsent(token);
                if (refreshedToken != null)
                {


                    chat1.DelegationToken = refreshedToken.DelegationToken;
                    string signedParams = WindowsLiveMessenger.SignParameters(
                        refreshedToken.SessionKey,
                        siteUser.Name,
                        string.Empty,
                        string.Empty);
                    chat1.SignedParams = signedParams;

                }
                else
                {
                    //chat1.DelegationToken = siteUser.LiveMessengerDelegationToken;
                    chat1.DelegationToken = token.DelegationToken;
                    string signedParams = WindowsLiveMessenger.SignParameters(
                        token.SessionKey,
                        siteUser.Name,
                        string.Empty,
                        string.Empty);

                    chat1.SignedParams = signedParams;
                }


            }
            

        }

        private void ShowAuthenticatedProperties(SiteUser siteUser)
        {
            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                         (propertyDefinition.VisibleToAuthenticated)
                            && (
                                    (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                                    || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                                )
                                &&(
                                    (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                                        || (WebUser.IsInRoles(propertyDefinition.OnlyVisibleForRoles))
                                  )
                            
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                timeOffset);
                        }
                        else
                        {
                            CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                               propertyDefinition.DefaultValue,
                                timeOffset);

                        }
                    }

                }
            }

        }

        private void ShowAnonymousProperties(SiteUser siteUser)
        {
            bool wouldSeeMoreIfAuthenticated = false;

            CProfileConfiguration profileConfig = CProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (CProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                        (propertyDefinition.VisibleToAnonymous)
                        && (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                        &&(
                        (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                            || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                timeOffset);
                        }
                        else
                        {
                            CProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                                propertyDefinition.DefaultValue,
                                timeOffset);
                        }
                    }
                    else
                    {
                        if (
                            (propertyDefinition.VisibleToAuthenticated)
                            && (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                            &&(
                            (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                                || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                                )
                            )
                        {
                            wouldSeeMoreIfAuthenticated = true;
                        }

                    }

                }
            }

            if (wouldSeeMoreIfAuthenticated)
            {
                lblMessage.Text = ProfileResource.WouldSeeMoreIfAuthenticatedMessage;
            }

        }

		
	}
}
