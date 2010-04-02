<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryListControl.ascx.cs" Inherits="Cynthia.Web.BlogUI.BlogCategories" %>
<h3><cy:sitelabel id="Sitelabel4" runat="server" ConfigKey="BlogCategoriesLabel" ResourceFile="BlogResources" UseLabelTag="false"> </cy:sitelabel></h3>
<asp:repeater id="dlCategories" runat="server" EnableViewState="False" SkinID="plain">
    <HeaderTemplate><ul class="blognav"></HeaderTemplate>
    <ItemTemplate>
        <li>
        <asp:HyperLink id="Hyperlink5" runat="server" EnableViewState="false" 
            Text='<%# ResourceHelper.FormatCategoryLinkText(DataBinder.Eval(Container.DataItem,"Category").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PostCount"))) %>' 
            NavigateUrl='<%# this.SiteRoot + "/Blog/ViewCategory.aspx?cat=" + DataBinder.Eval(Container.DataItem,"CategoryID") + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + PageId.ToString() %>'>
        </asp:HyperLink></li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:repeater>
<portal:TagCloudControl ID="cloud" runat="server" />
