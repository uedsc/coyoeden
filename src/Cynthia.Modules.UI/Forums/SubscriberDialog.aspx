<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="SubscriberDialog.aspx.cs" Inherits="Cynthia.GroupUI.SubscriberDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeYuiDataTable="true" />
<div  style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">

<asp:Panel ID="pnlSubscribers" runat="server" CssClass="AspNet-GridView">		
	<table  cellspacing="0" width="100%">
		<thead>
		<tr>
			<th id='<%# Resources.GroupResources.UserNameLabel.Replace(" ", "") %>' >
				<cy:SiteLabel id="lblUserNameLabel" runat="server" ConfigKey="UserNameLabel" UseLabelTag="false" ResourceFile="GroupResources"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.GroupResources.LoginNameLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel2" runat="server" ConfigKey="LoginNameLabel" UseLabelTag="false" ResourceFile="GroupResources"> </cy:SiteLabel>
			</th>
			<th id='<%# Resources.GroupResources.EmailLabel.Replace(" ", "") %>'>
				<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="EmailLabel" UseLabelTag="false" ResourceFile="GroupResources" Visible='<%# isAdmin %>'> </cy:SiteLabel>
			</th>
			<th></th>
		</tr></thead><tbody>
		<asp:Repeater id="rptUsers" runat="server">
			<ItemTemplate>
				<tr>
					<td headers='<%# Resources.GroupResources.UserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.GroupResources.LoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td headers='<%# Resources.GroupResources.EmailLabel.Replace(" ", "") %>'>
					    <a id="lnkEmail" runat="server" Visible='<%# isAdmin %>' href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td>
					<asp:Button ID="btnUnsubscribe" runat="server" Text='<%# Resources.GroupResources.UnsubscribeButton %>' CommandName="unsubscribe" 
                        CommandArgument='<%# Eval("SubscriptionID").ToString() %>' />
					</td>
				</tr>
			</ItemTemplate>
			<alternatingItemTemplate>
				<tr class="AspNet-GridView-Alternate">
					<td headers='<%# Resources.GroupResources.UserNameLabel.Replace(" ", "") %>'>
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
					</td>
					<td headers='<%# Resources.GroupResources.LoginNameLabel.Replace(" ", "") %>'> 
						<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LoginName").ToString())%>
					</td>
					<td headers='<%# Resources.GroupResources.EmailLabel.Replace(" ", "") %>'>
					    <a id="lnkEmail" runat="server" Visible='<%# isAdmin %>' href='<%# "mailto:" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%>'><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Email").ToString())%></a>
					</td>
					<td>
					<asp:Button ID="btnUnsubscribe" runat="server" Text='<%# Resources.GroupResources.UnsubscribeButton %>' CommandName="unsubscribe" 
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
