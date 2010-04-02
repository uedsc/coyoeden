<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiveListControl.ascx.cs" Inherits="Cynthia.Web.BlogUI.BlogArchiveList" %>
<h3><cy:sitelabel id="Sitelabel5" runat="server" ConfigKey="BlogArchivesLabel" ResourceFile="BlogResources" EnableViewState="false" UseLabelTag="false"> </cy:sitelabel></h3>
<asp:repeater id="dlArchive" runat="server" EnableViewState="False" SkinID="plain">
    <HeaderTemplate><ul class="blognav"></HeaderTemplate>
    <ItemTemplate>
        <li>
        <asp:HyperLink id="Hyperlink6" runat="server" EnableViewState="false"
            Text='<%# DateTimeHelper.FormatArchiveLinkText(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Month")),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Year")),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Count"))) %>' 
            NavigateUrl='<%# this.SiteRoot + "/Blog/ViewArchive.aspx?month=" + DataBinder.Eval(Container.DataItem,"Month") + "&amp;year=" + DataBinder.Eval(Container.DataItem,"Year").ToString() + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + PageId.ToString() %>'>
        </asp:HyperLink></li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:repeater>
