<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyPageEdit.aspx.cs" Inherits="SurveyFeature.UI.PageEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPageEdit" CssClass="selectedcrumb"></asp:HyperLink>
</div>

<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper survey">
<asp:Panel ID="pnlPageEdit" runat="server" DefaultButton="btnSave" CssClass="modulecontent">
<fieldset class="htmledit">
    <legend>	
		<asp:Literal ID="litHeading" runat="server" />
	</legend>
    
    <div class="settingrow">
        <cy:SiteLabel ID="lblPageTitleLabel" runat="server" ConfigKey="PageTitleLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:TextBox runat="server" ID="txtPageTitle" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel ID="lblPageEnabledLabel" runat="server" ConfigKey="PageEnabledLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:CheckBox runat="server" ID="chkPageEnabled" Checked="true" />
    </div>
    <div class="settingrow">
        <br />
        <portal:CButton ID="btnSave" runat="server" />
        <portal:CButton ID="btnCancel" runat="server" />
    </div>
 </fieldset>
 </asp:Panel>	
</asp:Panel>	
<cy:CornerRounderBottom id="cbottom1" runat="server" />
<portal:SessionKeepAliveControl id="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
