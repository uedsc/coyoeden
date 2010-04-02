<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminProduct.aspx.cs" Inherits="WebStore.UI.AdminProductPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop id="ctop1" runat="server" />
    <asp:Panel ID="pnlProduct" runat="server" CssClass="art-Post-inner panelwrapper webstore webstoreadminproduct">
        <h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
        <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent">
        <cy:CGridView ID="grdProduct" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false" 
            DataKeyNames="Guid" EnableTheming="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" Text='<%# Resources.WebStoreResources.ProductGridEditButton %>'
                            ToolTip='<%# Resources.WebStoreResources.ProductGridEditButton %>' NavigateUrl='<%# SiteRoot +"/WebStore/AdminProductEdit.aspx" + BuildProductQueryString(Eval("Guid").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("ModelNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                    <portal:CRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' ShowPrompt="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cy:CGridView>
        <div class="settingrow">
            <asp:HyperLink ID="lnkNewProduct" runat="server" />
        </div>
        <div class="modulepager">
           <portal:CCutePager ID="pgrProduct" runat="server" />
        </div>
        <br class="clear" />
        </div>
        </portal:CPanel>
    </asp:Panel>
    <cy:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
