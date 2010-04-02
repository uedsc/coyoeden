<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminCurrency.aspx.cs" Inherits="Cynthia.Web.AdminUI.AdminCurrencyPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnl1" runat="server" CssClass="art-Post-inner panelwrapper admin admincurrency ">
        <h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
            <div class="settingrow">
            <cy:CGridView ID="grdCurrency" runat="server" AllowPaging="false" AllowSorting="false" 
                AutoGenerateColumns="false"  DataKeyNames="Guid" EnableTheming="false" SkinID="plain" >
                <Columns>
                    <asp:TemplateField SortExpression="Title">
                        <ItemTemplate>
                            <asp:Button ID="btnEditCurrency" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.CurrencyGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Title">
                        <ItemTemplate>
                            <%# Eval("Title") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridTitleHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtTitle" Columns="20" Text='<%# Eval("Title") %>' runat="server"
                                    MaxLength="50" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridCodeHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtCode" Columns="20" Text='<%# Eval("Code") %>' runat="server"
                                    MaxLength="3" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridSymbolLeftHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtSymbolLeft" Columns="20" Text='<%# Eval("SymbolLeft") %>' runat="server"
                                    MaxLength="15" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridSymbolRightHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtSymbolRight" Columns="20" Text='<%# Eval("SymbolRight") %>' runat="server"
                                    MaxLength="15" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridDecimalPointCharHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtDecimalPointChar" Font-Size="Large" Columns="20" Text='<%# Eval("DecimalPointChar") %>'
                                    runat="server" MaxLength="1" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridThousandsPointCharHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtThousandsPointChar" Font-Size="Large" Columns="20" Text='<%# Eval("ThousandsPointChar") %>'
                                    runat="server" MaxLength="1" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridDecimalPlacesHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtDecimalPlaces" Columns="20" Text='<%# Eval("DecimalPlaces") %>'
                                    runat="server" MaxLength="1" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridValueHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtValue" Columns="20" Text='<%# Eval("Value") %>' runat="server"
                                    MaxLength="9" CssClass="forminput" />
                            </div>
                            <div>
                                <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.CurrencyGridUpdateButton %>'
                                    CommandName="Update" />
                                <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.CurrencyGridDeleteButton %>'
                                    CommandName="Delete" />
                                <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.CurrencyGridCancelButton %>'
                                    CommandName="Cancel" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            </div>
            <div class="settingrow">
                <portal:CButton ID="btnAddNew" runat="server" />
            </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
