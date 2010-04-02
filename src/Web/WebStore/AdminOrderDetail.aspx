<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminOrderDetail.aspx.cs" Inherits="WebStore.UI.AdminOrderDetailPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlStoreManager" runat="server" CssClass="panelwrapper  webstore webstorecart">
        <div class="modulecontent">
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblSiteUser" runat="server" />
                <asp:HyperLink ID="lnkUser" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="OrderIdLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblOrderId" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="OrderStatusLabel"
                    ResourceFile="WebStoreResources" />
                <portal:OrderStatusSetting ID="orderStatusControl" runat="server" />
                <asp:Button ID="btnSaveStatusChange" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerName" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerNameLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerName" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerCompany" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCompanyLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCompany" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerAddressLine1" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerAddressLine1Label" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerAddressLine1" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerAddressLine2" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerAddressLine2Label" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerAddressLine2" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerCountry" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCountryLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCountry" runat="server" />
            </div>
            <div class="settingrow" id="divCustomerState" runat="server">
                <cy:SiteLabel ID="LabelCustomerState" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerStateLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerGeoZone" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabellCustomerSuburb" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerSuburbLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerSuburb" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerCity" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCityLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCity" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerPostalCode" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerPostalCodeLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerPostalCode" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerTelephoneDay" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerTelephoneDayLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerTelephoneDay" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerTelephoneNight" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerTelephoneNightLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerTelephoneNight" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="LabelCustomerEmail" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerEmailLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerEmail" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="OrderDiscountCodesLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblDiscountCodes" runat="server" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel12" runat="server" CssClass="settinglabel" ConfigKey="OrderCustomData" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomData" runat="server" />
            </div>
            <h3 class="itemsheader"><asp:Literal ID="litItemsHeader" runat="server" /></h3>
            <div class="settingrow">
                <asp:Repeater ID="rptOrderItems" runat="server">
                    <ItemTemplate>
                        <div>
                            <%# Eval("Name") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <asp:Panel ID="pnlDownloadTickets" runat="server" CssClass="yui-skin-sam">
            <h3 class="itemsheader"><asp:Literal ID="litDownloadTicketsHeading" runat="server" /></h3>
            <div class="settingrow">
                <cy:CGridView ID="grdDownloadTickets" runat="server" 
                    AllowPaging="false" 
                    AllowSorting="false"
                    CssClass="" AutoGenerateColumns="false"
                    DataKeyNames="Guid" EnableTheming="false" SkinID="plain" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("ProductName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("DownloadsAllowed")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("DownloadedCount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <portal:GreyBoxHyperlink ID="lnkDownloadss" runat="server" ClientClick="return GB_showCenter(this.title, this.href)" NavigateUrl='<%# SiteRoot + "/WebStore/AdminDownloadHistory.aspx?pageid=" + pageId + "&amp;mid=" + moduleId + "&amp;order=" + orderGuid.ToString() + "&amp;t=" + Eval("Guid")%>' Text='<%# Resources.WebStoreResources.DownloadHistoryViewLink %>' Tooltip='<%# Resources.WebStoreResources.DownloadHistoryHeading %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </cy:CGridView>
            </div>
            </asp:Panel>
            <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrow">
                <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="CartSubTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litSubTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlDiscount" runat="server" CssClass="storerow">
                    <cy:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="CartDiscountTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litDiscount" runat="server" />
                </asp:Panel>
            <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="settingrow">
                <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailShippingTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litShippingTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="settingrow">
                <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailTaxTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litTaxTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="settingrow">
                <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailOrderTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litOrderTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" CssClass="settingrow">
                <cy:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailPaymentMethodLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litPaymentMethod" runat="server" />
            </asp:Panel>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                <portal:CButton ID="btnDelete" runat="server" Visible="false" />
            </div>
            <asp:Panel ID="pnlCheckoutLog" runat="server">
            </asp:Panel>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
