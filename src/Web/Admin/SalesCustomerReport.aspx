<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SalesCustomerReport.aspx.cs" Inherits="Cynthia.Web.AdminUI.SalesCustomerReportPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCommerceReports" runat="server" NavigateUrl="~/Admin/SalesSummary.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnl1" runat="server" CssClass="art-Post-inner panelwrapper ">
        <h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <cy:CGridView ID="grdUsers" runat="server" AllowPaging="false" AllowSorting="false"
                 AutoGenerateColumns="false" EnableTheming="false"
                SkinID="plain">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink Text='<%# Resources.Resource.ManageUserLink %>' ID="Hyperlink2" 
                                NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>'
                                Visible="<%# IsAdmin %>" runat="server" />
                            <%# Eval("Name") %><br />
                            <%# Eval("LoginName") %>
                            -
                            <%# Eval("Email") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# SiteRoot + "/Admin/SalesCustomerDetail.aspx?u=" + Eval("UserGuid") %>'>
                                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Revenue"))) %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            <div class="modulepager">
                <portal:CCutePager ID="pgrUsers" runat="server" />
            </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
