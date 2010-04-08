// Author:					Joe Audette
// Created:					2009-04-14
// Last Modified:			2009-04-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using Cynthia.Web;
using Cynthia.Web.Framework;
using Cynthia.Business;
using Resources;

namespace Cynthia.Modules.UI
{

    public partial class LiveMessengerModule : SiteModuleControl
    {
        // FeatureGuid 23339B5D-C51C-4a69-941E-D1C451BE73DA

        private string messengerCid = string.Empty;
        private string displayName = string.Empty;
        private string messengerTheme = string.Empty;
        private int messengerHeight = 300;
        private int messengerWidth = 300;
        private SiteUser currentUser = null;

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnCopyFromProfile.Click += new EventHandler(btnCopyFromProfile_Click);

        }

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (messengerCid.Length > 0)
            {
                ShowChat();
            }
            else
            {
                if (IsEditable)
                {
                    // user has edit permission lets try and use his live messenger info
                    currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser.LiveMessengerId.Length > 0)
                    {
                        pnlCopyCidFromUser.Visible = true;

                    }

                }
                
            }

        }

        private void ShowChat()
        {
            chat1.Invitee = messengerCid;
            //chat1.InviteeDisplayName = displayName;
            chat1.ThemeName = messengerTheme;
            chat1.Height = messengerHeight;
            chat1.Width = messengerWidth;

        }

        void btnCopyFromProfile_Click(object sender, EventArgs e)
        {
            if (currentUser == null) { currentUser = SiteUtils.GetCurrentSiteUser(); }

            if (currentUser.LiveMessengerId.Length > 0)
            {
                ModuleSettings.UpdateModuleSetting(this.ModuleGuid, this.ModuleId, "MessengerCIDSetting", currentUser.LiveMessengerId);
                ModuleSettings.UpdateModuleSetting(this.ModuleGuid, this.ModuleId, "MessengerDisplayNameSetting", currentUser.Name);

            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        private void PopulateLabels()
        {
            lblCopyFromProfileInstructions.Text = LiveResources.MessengerCopyFromProfileInstructions;
            btnCopyFromProfile.Text = LiveResources.MessengerCopyFromProfileButton;
        }

        private void LoadSettings()
        {
            TitleControl.Visible = !this.RenderInWebPartMode;

            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (Settings.Contains("MessengerCIDSetting"))
            {
                messengerCid = Settings["MessengerCIDSetting"].ToString();
            }

            if (Settings.Contains("MessengerDisplayNameSetting"))
            {
                displayName = Settings["MessengerDisplayNameSetting"].ToString();
            }

            if (Settings.Contains("MessengerThemeSetting"))
            {
                messengerTheme = Settings["MessengerThemeSetting"].ToString();
            }


            messengerHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "MessengerHeightSetting", messengerHeight);

            messengerWidth = WebUtils.ParseInt32FromHashtable(
                Settings, "MessengerWidthSetting", messengerWidth);

        }


    }
}