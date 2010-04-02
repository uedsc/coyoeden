<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminOffer.aspx.cs" Inherits="WebStore.UI.AdminOfferPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop id="ctop1" runat="server" />
    <asp:Panel ID="pnlOffer" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreadminoffer">
        <h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent yui-skin-sam">
        <cy:CGridView ID="grdOffer" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false"  
            DataKeyNames="Guid" 
            EnableTheming="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" Text='<%# Resources.WebStoreResources.OfferEditButton %>'
                            ToolTip='<%# Resources.WebStoreResources.OfferEditButton %>' NavigateUrl='<%# SiteRoot + "/WebStore/AdminOfferEdit.aspx" + BuildOfferQueryString(Eval("Guid").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IsVisible") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IsSpecial") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cy:CGridView>
        <div class="settingrow">
            <asp:HyperLink ID="lnkAddNew" runat="server" />
        </div>
        <div class="modulepager">
           <portal:CCutePager ID="pgrOffer" runat="server" />
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
