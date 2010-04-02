<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ContentTemplates.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentTemplatesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
    <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper admin ">
        <div class="modulecontent">
            <div class="settingrow">
                <asp:HyperLink ID="lnkAddNewTop" runat="server" />
            </div>
            <div id="divTopPager" runat="server" class="modulepager">
                <portal:CCutePager ID="pgrTop" runat="server" />
            </div>
            <asp:Repeater ID="rptTemplates" runat="server">
                <ItemTemplate>
                    <div class="templatewrapper">
                        <h3>
                            <%# Eval("Title") %>
                            <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# SiteRoot + "/Admin/ContentTemplateEdit.aspx?t=" + Eval("Guid") %>'
                                Text='<%# Resources.Resource.ContentTemplateEditLink %>' CssClass="ModuleEditLink" /></h3>
                        <div id="divBody" runat="server" visible='<%# showBody %>'>
                            <%# Eval("Body") %>
                        </div>
                        <div>
                            <%# Eval("Description") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="settingrow">
                <br />
                <asp:HyperLink ID="lnkAddNewBottom" runat="server" />
            </div>
            <div id="divBottomPager" runat="server" class="modulepager">
                <portal:CCutePager ID="pgrBottom" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
