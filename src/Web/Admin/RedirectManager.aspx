<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RedirectManager.aspx.cs" Inherits="Cynthia.Web.AdminUI.RedirectManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server"  />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin urlmanager">
<div class="modulecontent">
<fieldset>
<legend>
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
<asp:HyperLink ID="lnkRedirectManager" runat="server" />
</legend>
		
			<asp:Panel ID="pnlAddRedirect" runat="server" DefaultButton="btnAdd">
			<div class="settingrow">
			    <asp:Label ID="lblSiteRoot" Runat="server" ></asp:Label>
				<asp:TextBox ID="txtOldUrl" Runat="server" Columns="60" MaxLength="255"></asp:TextBox>
			</div>
			<div class="settingrow">
			    <strong><cy:SiteLabel id="Sitelabel4" runat="server" ConfigKey="RedirectsToLabel" > </cy:SiteLabel></strong>
			</div>
			<div class="settingrow">
			    <asp:Label ID="lblSiteRoot2" Runat="server" ></asp:Label>
				<asp:Textbox id="txtNewUrl" Columns="60"   runat="server" />
				<portal:CButton  runat="server" ID="btnAdd"/> 
			</div>
			<div class="settingrow">
			     <cy:SiteLabel id="Sitelabel5" runat="server" ConfigKey="RedirectHelp" > </cy:SiteLabel>
			</div>
			<div>
		        <asp:Label ID="lblError" Runat="server" CssClass="txterror"></asp:Label>
		    </div>
			</asp:Panel>
		<hr />
		<portal:CDataList id="dlRedirects" DataKeyField="RowGuid" runat="server">
			<ItemTemplate>
				<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.EditLink %>" ToolTip="<%# Resources.Resource.EditLink %>"  runat="server" ID="btnEdit"/>
				<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.DeleteLink %>" ToolTip="<%# Resources.Resource.DeleteLink %>" runat="server" ID="btnDelete"/>
				&nbsp;&nbsp;
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%>'><%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%></a> &nbsp;&nbsp;
				<strong><cy:SiteLabel id="lblPageNameLayout" runat="server" ConfigKey="RedirectsToLabel" UseLabelTag="false"> </cy:SiteLabel></strong>&nbsp;&nbsp;
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%>'><%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%></a>
			    <hr />
			</ItemTemplate>
			<EditItemTemplate>
			<fieldset>
			    <div class="settingrow">
			    <asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label3"/><asp:Textbox id="txtGridOldUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "OldUrl").ToString() %>' runat="server" />
			    </div>
			    <div class="settingrow">
			        <strong><cy:SiteLabel id="Sitelabel4" runat="server" ConfigKey="RedirectsToLabel" > </cy:SiteLabel></strong>
			    </div>
			    <div class="settingrow">
			    <asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label4"/>
				<asp:Textbox id="txtGridNewUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "NewUrl").ToString() %>' runat="server" />
				 
				</div>
				<div class="settingrow">
				    <asp:Button Text="<%# Resources.Resource.SaveButton %>" ToolTip="<%# Resources.Resource.SaveButton %>" CommandName="apply" runat="server" ID="button1" />
				    <asp:Button Text="<%# Resources.Resource.CancelButton %>" ToolTip="<%# Resources.Resource.CancelButton %>" CommandName="cancel" runat="server" ID="button3" /> 
				</div>
		    </fieldset>
			</EditItemTemplate>
		</portal:CDataList>
<div class="modulepager">
    <portal:CCutePager ID="pgrFriendlyUrls" runat="server" Visible="false" />
</div>

</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
