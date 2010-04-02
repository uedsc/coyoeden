/// Last Modified:		2009-12-27
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
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class SecurityRoles : CBasePage
	{
		private int  roleID = -1;
        private bool isAdmin = false;
        private Role role;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        

        private void Page_Load(object sender, EventArgs e)
		{
           
            if (!WebUser.IsAdminOrRoleAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            SecurityHelper.DisableBrowserCache();
           
            LoadParams();
            role = new Role(roleID);
            EnforceSecurity();
            SetupScript();
			PopulateLabels();

            if (!Page.IsPostBack) 
			{
                BindData();
      
            }
        }

        void btnSetUserFromGreyBox_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (hdnUserID.Value.Length == 0) { return; }
            try
            {
                int userId = Convert.ToInt32(hdnUserID.Value);
                SiteUser user = new SiteUser(siteSettings, userId);

                Role.AddUser(roleID, userId, role.RoleGuid, user.UserGuid);

                WebUtils.SetupRedirect(this, Request.RawUrl);

            }
            catch (FormatException) { }
            
        }

        void rptRoleMembers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                Role.RemoveUser(roleID, userId);

                
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        void rptRoleMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            if (btnDelete != null)
            {
                btnDelete.AlternateText = Resource.ManageUsersRemoveFromRoleButton;
                btnDelete.ToolTip = Resource.ManageUsersRemoveFromRoleButton;
                UIHelper.AddConfirmationDialog(btnDelete, Resource.RolesRemoveUserWarning);
            }

        }
    

        private void BindData()
		{
            
            spnTitle.InnerText = Resource.SecurityRolesTitle + " " + role.DisplayName;

            using (IDataReader reader = Role.GetRoleMembers(roleID))
            {
                rptRoleMembers.DataSource = reader;
                rptRoleMembers.DataBind();
            }

            
        }

        private void EnforceSecurity()
        {
            if (role.RoleId == -1)
            {
                pnlSecurity.Visible = false;
                return;
            }
            if (role.SiteId != siteSettings.SiteId)
            {
                pnlSecurity.Visible = false;
                return;
            }

            if (!isAdmin)
            {
                if ((role.Equals("Admins")) || (role.Equals("Role Admins")))
                {
                    pnlSecurity.Visible = false;
                    return;
                }
            }
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuRoleAdminLink);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";
            lnkRoleManager.Text = Resource.AdminMenuRoleAdminLink;
            lnkRoleManager.ToolTip = Resource.AdminMenuRoleAdminLink;
            lnkRoleManager.NavigateUrl = SiteRoot + "/Admin/RoleManager.aspx";
           
            lnkUserLookup.Text = Resource.SecurityAddExistingButton;
            lnkUserLookup.ToolTip = Resource.SecurityAddExistingButton;
            lnkUserLookup.DialogCloseText = Resource.CloseDialogButton;
            lnkUserLookup.NavigateUrl = SiteRoot + "/Dialog/RoleUserSelectDialog.aspx?r=" + roleID.ToInvariantString();
            btnSetUserFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnSetUserFromGreyBox.AlternateText = " ";
        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("function SelectUser(userId) {");

            script.Append("GB_hide();");
           
            script.Append("var hdnUI = document.getElementById('" + this.hdnUserID.ClientID + "'); ");
            script.Append("hdnUI.value = userId; ");

            script.Append("var btn = document.getElementById('" + this.btnSetUserFromGreyBox.ClientID + "');  ");
            script.Append("btn.click(); ");

            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectUserHandler", script.ToString());

        }


        private void LoadParams()
        {
       
            roleID = WebUtils.ParseInt32FromQueryString("roleid", -1);
            isAdmin = WebUser.IsAdmin;
            
            
        }

      
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            

            btnSetUserFromGreyBox.Click += new System.Web.UI.ImageClickEventHandler(btnSetUserFromGreyBox_Click);
            rptRoleMembers.ItemCommand += new RepeaterCommandEventHandler(rptRoleMembers_ItemCommand);
            rptRoleMembers.ItemDataBound += new RepeaterItemEventHandler(rptRoleMembers_ItemDataBound);
            

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        
    }
}
