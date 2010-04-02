<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="SubscriberDialog.aspx.cs" Inherits="Cynthia.ForumUI.SubscriberDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeYuiDataTable="true" />
<div  style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">

<asp:Panel ID="pnlSubscribers" runat="server" CssClass="AspNet-GridView">		
	<table  cellspacing="0" width="100%">
		<thead>
		<tr>
			<th id='<%# Resources.ForumResources.UserNameLabel.Replace(" ", "") %>' >
				<cy:SiteLabel id="lblUserNameLabel" runat="server" ConfigKey="UserNameLabel" UseLabelTag="false" ResourceFile="ForumResources"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.ForumResources.LoginNameLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel2" runat="server" ConfigKey="LoginNameLabel" UseLabelTag="false" ResourceFile="ForumResources"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.ForumResources.EmailLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="EmailLabel" UseLabelTag="false" ResourceFile="ForumResources" Visible='<%# isAdmin %>'> </cy:SiteLabel>
			</th>
			<th></th>
		</tr></thead><tbody>
		<asp:Repeater id="rptUsers" runat="server">
			<ItemTemplate>
				<tr>
					<td headers='<%# Resources.ForumResources.UserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.ForumResources.LoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td headers='<%# Resources.ForumResources.EmailLabel.Replace(" ", "") %>'>
					    <a id="lnkEmail" runat="server" Visible='<%# isAdmin %>' href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td>
					<asp:Button ID="btnUnsubscribe" runat="server" Text='<%# Resources.ForumResources.UnsubscribeButton %>' CommandName="unsubscribe" 
                        CommandArgument='<%# Eval("SubscriptionID").ToString() %>' />
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="AspNet-GridView-Alternate">
					<td headers='<%# Resources.ForumResources.UserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.ForumResources.LoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td headers='<%# Resources.ForumResources.EmailLabel.Replace(" ", "") %>'>
					    <a id="lnkEmail" runat="server" Visible='<%# isAdmin %>' href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td>
					<asp:Button ID="btnUnsubscribe" runat="server" Text='<%# Resources.ForumResources.UnsubscribeButton %>' CommandName="unsubscribe" 
                        CommandArgument='<%# Eval("SubscriptionID").ToString() %>' />
					</td>
				</tr>
			</AlternatingItemTemplate>
		</asp:Repeater></tbody>
	</table>
	<div class="modulepager">
		<portal:CCutePager ID="pgr" runat="server" />
	</div>
</asp:Panel>



</div>
</asp:Content>
