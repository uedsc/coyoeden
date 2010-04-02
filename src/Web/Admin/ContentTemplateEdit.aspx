<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ContentTemplateEdit.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentTemplateEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkTemplates" runat="server" NavigateUrl="~/Admin/ContentTemplates.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <cy:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper contenttemplates yui-skin-sam">
        <div class="modulecontent">
            <div class="settingrow">
                    <cy:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabeltight"
                        ConfigKey="ContentTemplateTitleLabel" ResourceFile="Resource" />
                    <asp:TextBox ID="txtTitle" CssClass="verywidetextbox forminput" runat="server" />
                </div>
                <div class="settingrow">&nbsp;</div>
            <div id="divtabs" class="yui-navset">
                <ul class="yui-nav">
                    <li class="selected"><a href="#tabTemplate"><em><asp:Literal ID="litTemplateTab" runat="server" /></em></a></li>
                    <li id="liDescription" runat="server"><a id="lnkDescription" runat="server" href="#tabExcerpt">
                        <em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                   <li id="liSecurity" runat="server" visible="false"><a id="lnkSecurity" runat="server" href="#tabSecurity">
                        <em><asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
                </ul>
                <div class="yui-content">
                    <div id="tabTemplate">
                        <div class="settingrow">
                            <cye:EditorControl id="edTemplate" runat="server"> </cye:EditorControl>
                        </div>
                    </div>
                    <div id="tabDescription">
                        <div class="settingrow">
                            <cye:EditorControl id="edDescription" runat="server"> </cye:EditorControl>
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblLogo" ForControl="ddImage" runat="server" CssClass="settinglabel"
                                ConfigKey="ContentTemplateImageLabel" EnableViewState="false"> </cy:SiteLabel>
                            <asp:DropDownList ID="ddImage" runat="server" TabIndex="10" EnableViewState="true"
                                DataValueField="Name" DataTextField="Name" CssClass="forminput">
                            </asp:DropDownList>
                            <img alt="" src="" id="imgTemplate" runat="server" EnableViewState="false" />
                            
                        </div>
                    </div>
                    <div id="tabSecurity" runat="server" visible="false">
                        <div class="settingrow">
                        <portal:AllowedRolesSetting ID="arTemplate" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" ResourceFile="Resource" />
                <portal:CButton ID="btnSave" runat="server" />
                <portal:CButton ID="btnDelete" runat="server" />
                <asp:HyperLink ID="lnkCancel" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
