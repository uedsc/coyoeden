<%@ Page language="c#" Codebehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.ContentUI.HtmlIncludeEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper htmlincludemodule">
<asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
<fieldset class="htmlincludeedit">
    <legend>	
		 <cy:SiteLabel id="lblSettings" runat="server" ConfigKey="EditHtmlFragmentIncludeSettingsLabel" ResourceFile="HtmlIncludeResources" UseLabelTag="false"> </cy:SiteLabel>
	</legend>
        <div class="settingrow">
            <cy:SiteLabel id="lblIncludeFile" runat="server" ForControl="ddInclude" CssClass="settinglabel" ConfigKey="HtmlFragmentIncludeFileLabel" ResourceFile="HtmlIncludeResources"> </cy:SiteLabel>
            <asp:DropDownList ID="ddInclude" Runat="server" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
        </div>
        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <portal:CButton  id="btnUpdate" runat="server" Text="Update" />&nbsp;
		    <portal:CButton  id="btnCancel" runat="server" Text="Cancel"  CausesValidation="false" />
		    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="htmlincludeedithelp" />
        </div>
</fieldset>	
</asp:Panel>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />	
</asp:Panel>	
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

