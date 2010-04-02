<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SendLog.aspx.cs" Inherits="Cynthia.Web.ELetterUI.SendLogPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span>
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
    <asp:HyperLink ID="lnkArchive" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
    <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlSendLog" runat="server" CssClass="art-Post-inner panelwrapper newsletteradmin">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<cy:CGridView ID="grdSendLog" runat="server" 
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
            <asp:HyperLink Text='<%# Eval("EmailAddress")%>' id="Hyperlink2" 
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?u=" + Eval("UserGuid")   %>' 
						    Visible="<%# IsAdmin %>" runat="server" />
            <asp:Literal ID="litUser" runat="server" Text='<%# Eval("EmailAddress") %>' Visible="<%# !IsAdmin %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("UTC"), TimeOffset)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("ErrorMessage") %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
 </cy:CGridView>
<div class="modulepager" >
    <portal:CCutePager ID="pgrSendLog" runat="server" />
</div>
<div class="settingrow">
<portal:CButton ID="btnPurgeSendLog" runat="server" />
</div>
<br class="clear" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
