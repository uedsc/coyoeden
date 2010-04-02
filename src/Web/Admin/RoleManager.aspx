<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RoleManager.aspx.cs" Inherits="Cynthia.Web.AdminUI.RoleManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin rolemanager">
<div class="modulecontent">
<fieldset>
<legend>
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkRoleAdmin" runat="server" />
</legend>
<div class="settingrow">
    <portal:CDataList id="rolesList" DataKeyField="RoleID" runat="server">
		<ItemTemplate>
			<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.RolesEditLabel %>" ToolTip="<%# Resources.Resource.RolesEditLabel %>" runat="server" ID="btnEdit"  />
			<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.RolesDeleteLabel %>" ToolTip="<%# Resources.Resource.RolesDeleteLabel %>" runat="server" ID="btnDelete"  />
			&nbsp;&nbsp;
			<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" ID="Label1"  />
			<asp:HyperLink ID="lnkMembers" runat="server" Text='<%# Resources.Resource.RolesChangeMemberLabel%>' ToolTip='<%# Resources.Resource.RolesChangeMemberLabel%>' 
			NavigateUrl='<%# SiteRoot + "/Admin/SecurityRoles.aspx?roleid=" + DataBinder.Eval(Container.DataItem, "RoleID") %>' />
		</ItemTemplate>
		<EditItemTemplate>
			<asp:Textbox id="roleName" width="200" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" />
			&nbsp;
			<asp:Button Text="<%# Resources.Resource.RolesApplyLabel %>" ToolTip="<%# Resources.Resource.RolesApplyLabel %>" CommandName="apply"  runat="server" ID="Button1" />
			&nbsp;
			<asp:Button Text="<%# Resources.Resource.RoleManagerCancelButton %>" ToolTip="<%# Resources.Resource.RoleManagerCancelButton %>" CommandName="cancel"  runat="server" ID="Button2" />
			
		</EditItemTemplate>
	</portal:CDataList>
</div>
<div class="settingrow">
    <portal:CLabel ID="lblError" Runat="server" CssClass="txterror" />
</div>
<asp:Panel ID="pnlAddRole" runat="server" CssClass="settingrow" DefaultButton="btnAddRole">
    <portal:CButton  runat="server" id="btnAddRole" />
	<asp:TextBox ID="txtNewRoleName" Runat="server" MaxLength="50" CssClass="mediumtextbox"></asp:TextBox>
	<portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="roleadministrationhelp" />	
</asp:Panel>
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
