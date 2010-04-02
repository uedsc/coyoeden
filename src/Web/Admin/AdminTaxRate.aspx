<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminTaxRate.aspx.cs" Inherits="Cynthia.Web.AdminUI.AdminTaxRatePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlTaxRate" runat="server" CssClass="art-Post-inner panelwrapper admin admintaxrate">
        <h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent yui-skin-sam">
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridCountryHeader" />
                <asp:DropDownList ID="ddCountry" runat="server" DataValueField="Guid" DataTextField="Name"
                    AutoPostBack="true" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridGeoZoneHeader" />
                <asp:DropDownList ID="ddGeoZones" runat="server" DataValueField="Guid" DataTextField="Name"
                    AutoPostBack="true" />
            </div>
            <cy:CGridView ID="grdTaxRate" runat="server" AllowPaging="false" AllowSorting="false"
                AutoGenerateColumns="false"  DataKeyNames="Guid" EnableTheming="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.TaxRateGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Description") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridDescriptionHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtDescription" Columns="20" Text='<%# Eval("Description") %>' runat="server"
                                    MaxLength="255" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridTaxClassHeader"
                                    ResourceFile="Resource" />
                                <asp:DropDownList ID="ddTaxClass" runat="server" DataValueField="Guid" DataTextField="Title"
                                    CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridPriorityHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtPriority" Columns="20" Text='<%# Eval("Priority") %>' runat="server"
                                    MaxLength="4" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridRateHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtRate" Columns="20" Text='<%# Eval("Rate") %>' runat="server"
                                    MaxLength="9" CssClass="forminput" />
                            </div>
                            <div>
                                <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.TaxRateGridUpdateButton %>'
                                    CommandName="Update" />
                                <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.TaxRateGridDeleteButton %>'
                                    CommandName="Delete" />
                                <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.TaxRateGridCancelButton %>'
                                    CommandName="Cancel" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Rate") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            <div class="settingrow">
                <portal:CButton ID="btnAddNew" runat="server" />
            </div>
            <br class="clear" />
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
