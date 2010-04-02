<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="BannedIPAddresses.aspx.cs" Inherits="Cynthia.Web.AdminUI.BannedIPAddressesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin bannedips">
<asp:Panel id="pnlBannedIPAddresses" runat="server" CssClass="modulecontent yui-skin-sam" DefaultButton="btnAddNew">
<fieldset class="moduleadmin">
<legend>
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
<asp:HyperLink ID="lnkBannedIPs" runat="server" />
</legend>
<asp:Panel ID="pnlLookup" runat="server" DefaultButton="btnIPLookup">
<asp:TextBox ID="txtIPAddress" runat="server" CssClass="mediumtextbox" MaxLength="50" />
<portal:CButton ID="btnIPLookup" runat="server" />
</asp:Panel>
<div>&nbsp;</div>
<cy:CGridView ID="grdBannedIPAddresses" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="RowID" 
    EnableTheming="false"
    >
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.BannedIPAddressesGridEditButton %>' />&nbsp;&nbsp;&nbsp;
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Button id="btnGridUpdate" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridUpdateButton %>' CommandName="Update" />
                <asp:Button id="btnGridDelete" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridDeleteButton %>' CommandName="Delete" />
                <asp:Button id="btnGridCancel" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridCancelButton %>' CommandName="Cancel" />
            </EditItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# Eval("BannedIP") %>
            </ItemTemplate>
			<EditItemTemplate>
			<asp:TextBox ID="txtBannedIP" Columns="20" Text='<%# Eval("BannedIP") %>' runat="server" MaxLength="50" />
			</EditItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# Eval("BannedReason") %>
            </ItemTemplate>
			<EditItemTemplate>
			<asp:TextBox ID="txtBannedReason" Columns="20" Text='<%# Eval("BannedReason") %>' runat="server" MaxLength="255" />
			</EditItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("BannedUTC"), TimeOffset)%>
            </ItemTemplate>
			<EditItemTemplate>
			<asp:TextBox ID="txtBannedUTC" Columns="20" Text='<%# Eval("BannedUTC") %>' runat="server" MaxLength="30" />
			</EditItemTemplate>
		</asp:TemplateField>
</Columns>
 </cy:CGridView>
<div class="settingrow">
<portal:CButton ID="btnAddNew" runat="server"  />
</div>
<div class="modulepager" style="float:left; clear:left;">
    <portal:CCutePager ID="pgrBannedIPAddresses" runat="server" />
</div>
<br class="clear" />

</fieldset>
</asp:Panel>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
