<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminOrderHistory.aspx.cs" Inherits="WebStore.UI.AdminOrderHistoryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<asp:Panel id="pnlCart" runat="server" CssClass="art-Post-inner panelwrapper cart">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<cy:CGridView ID="grdOrders" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="OrderGuid" 
    EnableTheming="false"
    >
     <Columns>
     <asp:TemplateField>
        <ItemTemplate>
                <%# Eval("CustomerFirstName") %> <%# Eval("CustomerLastName") %><br />
                <%# Eval("CustomerEmail") %><br />
                <%# Eval("CompletedFromIP") %>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OrderTotal"))) %><br />
                <a href='<%# Eval("OrderGuid", detailUrlFormat)%>'><%# Resources.WebStoreResources.OrderHistoryViewDetailLink%></a>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("Completed"), timeOffset)%>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# GetOrderStatus(Eval("StatusGuid").ToString())%>
            </ItemTemplate>
     </asp:TemplateField>
    </Columns>
 </cy:CGridView>

<div class="modulepager">
    <portal:CCutePager ID="pgrOrders" runat="server" />
</div>
<br class="clear" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
