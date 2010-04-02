<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="RoleUserSelectDialog.aspx.cs" Inherits="Cynthia.Web.AdminUI.RoleUserSelectDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<portal:ScriptLoader ID="ScriptInclude" runat="server"  />
<div  style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">

<asp:Panel ID="pnlLookup" runat="server" Visible="false">
	<div class="AspNet-GridView">	
	<table  cellspacing="0" width="100%">
		<thead>
		<tr>
			<th id='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>' >
				<cy:SiteLabel id="lblUserNameLabel" runat="server" ConfigKey="MemberListUserNameLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="MemberListEmailLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel2" runat="server" ConfigKey="MemberListLoginNameLabel" UseLabelTag="false"> </cy:SiteLabel>
			</th>
			<th></th>
		</tr></thead><tbody>
		<asp:Repeater id="rptUsers" runat="server" EnableViewState="False">
			<ItemTemplate>
				<tr>
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
					    <a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td headers='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td>
					<asp:Button ID="btnSelect" runat="server" Text='<%# Resources.Resource.UserLookupDialogSelectButton %>' CommandName="selectUser" 
                        CommandArgument='<%# Eval("UserID") %>' />
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="AspNet-GridView-Alternate">
					<td headers='<%# Resources.Resource.MemberListUserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.Resource.MemberListEmailLabel.Replace(" ", "") %>'>
					    <a href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td headers='<%# Resources.Resource.MemberListLoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td>
					<asp:Button ID="btnSelect" runat="server" Text='<%# Resources.Resource.UserLookupDialogSelectButton %>' CommandName="selectUser" 
                        CommandArgument='<%# Eval("UserID") %>' />
					</td>
				</tr>
			</AlternatingItemTemplate>
		</asp:Repeater></tbody>
	</table>
	</div>	
	<div class="modulepager">
		<portal:CCutePager ID="pgrMembers" runat="server" />
	</div>
</asp:Panel>
<asp:Panel ID="pnlNotAllowed" runat="server" Visible="false">
<cy:SiteLabel ID="lblNotAllowed" runat="server" CssClass="txterror" UseLabelTag="false" ConfigKey="NotInUserLookupRolesWarning" />
</asp:Panel>


</div>
</asp:Content>
