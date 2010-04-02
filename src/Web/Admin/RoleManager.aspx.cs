/// Author:					Joe Audette
/// Created:				2004-09-12
/// Last Modified:			2009-06-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.Web.UI.WebControls;
using log4net;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RoleManagerPage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RoleManagerPage));

        protected string EditPropertiesImage = "~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        //protected bool IsAdmin = false;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebUser.IsAdminOrRoleAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindRoleList();

        }

        private void BindRoleList()
        {
            Collection<Role> siteRoles = Role.GetbySite(siteSettings.SiteId, WebConfigSettings.UseRelatedSiteMode);
            if (!WebUser.IsAdmin)
            {
                // must be only Role Admin
                // remove admins role and role admins role
                // from list. Role Admins can't edit or manage
                // membership in Admins role or Role Admins role
                foreach (Role r in siteRoles)
                {
                    if (r.Equals("Admins"))
                    {
                        siteRoles.Remove(r);
                        break;
                    }

                }

                foreach (Role r in siteRoles)
                {
                    if (r.Equals("Role Admins"))
                    {
                        siteRoles.Remove(r);
                        break;
                    }
                }


            }
            
            rolesList.DataSource = siteRoles;
            rolesList.DataBind();
            
            

        }

        protected void btnAddRole_Click(Object sender, EventArgs e)
        {
            if (this.txtNewRoleName.Text.Length > 0)
            {
                Role role = new Role();
                role.SiteId = siteSettings.SiteId;
                role.SiteGuid = siteSettings.SiteGuid;
                role.RoleName = this.txtNewRoleName.Text;
                role.EnforceRelatedSitesMode = WebConfigSettings.UseRelatedSiteMode;
                role.Save();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }


        protected void RolesList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("fired event RolesList_ItemCommand");

            }
            int roleID = (int)rolesList.DataKeys[e.Item.ItemIndex];
            Role role = new Role(roleID);

            switch (e.CommandName)
            {
                case "edit":
                    rolesList.EditItemIndex = e.Item.ItemIndex;
                    BindRoleList();
                    break;

                case "apply":
                    role.RoleName = ((TextBox)e.Item.FindControl("roleName")).Text;
                    role.Save();
                    rolesList.EditItemIndex = -1;
                    BindRoleList();
                    break;

                case "delete":
                    Role.DeleteRole(roleID);
                    rolesList.EditItemIndex = -1;
                    BindRoleList();
                    break;

                //case "members":
                //    roleName = ((TextBox)e.Item.FindControl("roleName")).Text;
                //    role.RoleName = roleName;
                //    role.Save();
                //    string redirectUrl 
                //        = SiteRoot + "/Admin/SecurityRoles.aspx?roleId=" 
                //        + roleID + "&rolename=" + roleName;

                //    WebUtils.SetupRedirect(this, redirectUrl);
                //    break;

                case "cancel":
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;
            }

        }


        void rolesList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.RolesDeleteWarning);
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuRoleAdminLink);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkRoleAdmin.Text = Resource.AdminMenuRoleAdminLink;
            lnkRoleAdmin.ToolTip = Resource.AdminMenuRoleAdminLink;
            lnkRoleAdmin.NavigateUrl = SiteRoot + "/Admin/RoleManager.aspx";
           
            btnAddRole.Text = Resource.RolesAddButton;
            btnAddRole.ToolTip = Resource.RolesAddButton;
            SiteUtils.SetButtonAccessKey(btnAddRole, AccessKeys.RolesAddButtonAccessKey);

            

        }

        private void LoadSettings()
        {
            //IsAdmin = WebUser.IsAdmin;

            
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnAddRole.Click += new EventHandler(btnAddRole_Click);
            this.rolesList.ItemCommand += new DataListCommandEventHandler(RolesList_ItemCommand);
            this.rolesList.ItemDataBound += new DataListItemEventHandler(rolesList_ItemDataBound);

            SuppressMenuSelection();
            SuppressPageMenu();
            
        }

        #endregion
    }
}
