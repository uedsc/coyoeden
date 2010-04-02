<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminCartBrowser.aspx.cs" Inherits="WebStore.UI.AdminCartBrowserPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlCart" runat="server" CssClass="art-Post-inner panelwrapper cart">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent ">
<cy:CGridView ID="grdCart" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="CartGuid" 
    EnableTheming="false"
    >
     <Columns>
     <asp:TemplateField>
        <ItemTemplate>
            <div class="settingrow">
            <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel" ResourceFile="WebStoreResources" />
            <%# Eval("Name") %> <%# Eval("Email") %>
            </div>
            <div class="settingrow">
            <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="IPUser" ResourceFile="WebStoreResources" />
             <%# Eval("IPUser") %>
            </div>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OrderTotal"))) %><br />
                <a href='<%# Eval("CartGuid", detailUrlFormat)%>'><%# Resources.WebStoreResources.CartBrowserViewCartLink %></a>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("Created"), timeOffset)%><br />
                 <%# Eval("CreatedFromIP") %>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("LastModified"), timeOffset)%>
            </ItemTemplate>
     </asp:TemplateField>
    </Columns>
 </cy:CGridView>

<div class="settingrow">
	<br /><asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
<div class="modulepager">
    <portal:CCutePager ID="pgrCart" runat="server" />
</div>
<br class="clear" />
<div class="settingrow">
<asp:CheckBox ID="chkOnlyAnonymous" runat="server" />
<asp:Button ID="btnDelete" runat="server" />
<asp:TextBox ID="txtDaysOld" runat="server" CssClass="smalltextbox" /> <asp:Literal ID="litDays" runat="server" />

</div>

</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
