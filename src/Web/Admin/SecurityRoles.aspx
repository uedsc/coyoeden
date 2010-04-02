<%@ Page language="c#" CodeBehind="SecurityRoles.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.SecurityRoles" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<asp:Panel id="pnlSecurity" runat="server"  CssClass="securityroles">
    <fieldset>
    <legend>
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkRoleManager" runat="server" NavigateUrl="~/Admin/RoleManager.aspx" />&nbsp;&gt;
        <span id="spnTitle" runat="server"></span>
    </legend>
    <div class="modulecontent">
        <portal:GreyBoxHyperlink ID="lnkUserLookup" runat="server" ClientClick="return GB_showCenter(this.title, this.href, 670,600)"  />
        <asp:HiddenField ID="hdnUserID" runat="server" />
        <asp:ImageButton ID="btnSetUserFromGreyBox" runat="server" />
    </div>
    <div class="modulecontent">
        <asp:Repeater ID="rptRoleMembers" runat="server">
            <HeaderTemplate>
                <ul class="simplelist"></ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li class="simplelist">
                    <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' 
                    AlternateText='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                     ToolTip='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                    CommandName="delete" 
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID") %>' runat="server" ID="btnDelete"  />
				<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' runat="server" ID="Label1" />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="modulecontent">
        <portal:CLabel id="Message" runat="server" CssClass="txterror" />
    </div>
    </fieldset>
</asp:Panel>
	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
