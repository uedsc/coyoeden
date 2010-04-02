<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminStoreSettings.aspx.cs" Inherits="WebStore.UI.AdminStoreSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlStore" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoresettings">
        <h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div id="divtabs" class="yui-navset">
            <ul class="yui-nav">
                <li class="selected"><a href="#tab1"><em><asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                <li><a href="#tab2"><em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                <li><a href="#tab3"><em><asp:Literal ID="litClosedTab" runat="server" /></em></a></li>
            </ul>
            <div class="yui-content">
                <div id="tab1">
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblName" runat="server" CssClass="settinglabel" ConfigKey="StoreNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtName" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblOwnerName" runat="server" CssClass="settinglabel" ConfigKey="StoreOwnerNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOwnerName" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblOwnerEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreOwnerEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOwnerEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCountryGuid" runat="server" CssClass="settinglabel" ConfigKey="StoreCountryLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddCountryGuid" runat="server" EnableTheming="false" CssClass="forminput" DataTextField="Name" DataValueField="Guid"
                            AutoPostBack="true" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblAddress" runat="server" CssClass="settinglabel" ConfigKey="StoreAddressLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCity" runat="server" CssClass="settinglabel" ConfigKey="StoreCityLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCity"  runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblZoneGuid" runat="server" CssClass="settinglabel" ConfigKey="StoreZoneLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddZoneGuid" runat="server" EnableTheming="false" CssClass="forminput" DataTextField="Name" DataValueField="Guid" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblPostalCode" runat="server" CssClass="settinglabel" ConfigKey="StorePostalCodeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtPostalCode"  runat="server" MaxLength="50" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblPhone" runat="server" CssClass="settinglabel" ConfigKey="StorePhoneLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="32" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblFax" runat="server" CssClass="settinglabel" ConfigKey="StoreFaxLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="32" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblSalesEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreSalesEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtSalesEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblSupportEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreSupportEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtSupportEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblEmailFrom" runat="server" CssClass="settinglabel" ConfigKey="StoreEmailFromLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtEmailFrom" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblOrderBCCEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreOrderBCCEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOrderBCCEmail"  runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">&nbsp;</div>
                </div>
                <div id="tab2">
                    <div class="settingrow">
                        <cye:EditorControl ID="edDescription" runat="server">
                        </cye:EditorControl>
                    </div>
                </div>
                <div id="tab3">
                     <div class="settingrow">
                     <cy:SiteLabel ID="lblClosedMessage" runat="server" CssClass="settinglabel" ConfigKey="StoreClosedMessageLabel"
                            ResourceFile="WebStoreResources" />
                     </div>
                    <div class="settingrow">
                        <cye:EditorControl ID="edClosedMessage" runat="server">
                        </cye:EditorControl>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblIsClosed" runat="server" CssClass="settinglabel" ConfigKey="StoreIsClosedLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:CheckBox ID="chkIsClosed" runat="server" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                         <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    </div>
                </div>
            </div>
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="lblspacer1" runat="server" ConfigKey="spacer" />
                <portal:CButton ID="btnSave" runat="server" Text="Update" />&nbsp;
            </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
