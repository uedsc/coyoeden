<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminDiscounts.aspx.cs" Inherits="WebStore.UI.AdminDiscountsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<cy:CGridView ID="grdDiscount" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false" 
            DataKeyNames="DiscountGuid" 
            EnableTheming="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" Text='<%# Resources.WebStoreResources.DiscountEditLink %>'
                            ToolTip='<%# Resources.WebStoreResources.DiscountEditLink %>' NavigateUrl='<%# SiteRoot + "/WebStore/AdminDiscountEdit.aspx" + BuildQueryString(Eval("DiscountGuid").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("DiscountCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cy:CGridView>
        <div class="modulepager">
           <portal:CCutePager ID="pgrDiscounts" runat="server" />
        </div>
        <br class="clear" />
        <div class="settingrow">
            <asp:HyperLink ID="lnkNewDiscount" runat="server" />
        </div>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
