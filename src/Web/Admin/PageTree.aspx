<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PageTree.aspx.cs" Inherits="Cynthia.Web.AdminUI.PageTreePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin pagetree">
<div class="modulecontent">
<fieldset>
<legend>
<span id="spnAdminLinks" runat="server">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkPageTree" runat="server" />
</span>
<asp:Literal ID="litNonAdminHeading" runat="server" Visible="false" />
</legend>
<asp:Literal id="litTest" runat="server" />
<div class="settingrow">
    <a id="lnkNewPage" runat="server" ></a>&nbsp;<portal:CLabel ID="litWarning" runat="server" CssClass="txterror" />
</div>
<div class="settingrow">
    <table cellpadding="0" cellspacing="0" border="0">
		<tr valign="top">
		    <td>
                
            </td>
			<td>
				<asp:ListBox id="lbPages" SkinID="PageTree"  DataTextField="PageName" DataValueField="PageID" Rows="30"  runat="server" />
			</td>
			
			<td>
				<asp:ImageButton id="btnUp" CommandName="up"  runat="server" />
				<br />
				<asp:ImageButton id="btnDown" CommandName="down" runat="server" />
			    <br />
				<asp:ImageButton id="btnSettings" runat="server" />
					<asp:ImageButton id="btnEdit" runat="server" />
					<asp:ImageButton ID="btnViewPage" runat="server" />
				<br />
					<asp:ImageButton id="btnDelete" runat="server" />	
				<br /><br />
				<portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="addeditpageshelp" />
			</td>
		</tr>
	</table>
</div>
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
