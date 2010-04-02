<%@ Page language="c#" CodeBehind="ModuleDefinitions.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.ModuleDefinitions" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlModules" runat="server" CssClass="panelwrapper admin moduledefinitions">
<div class="modulecontent">
<fieldset class="moduledefinitions">
    <legend>
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkModuleAdmin" runat="server" NavigateUrl="~/Admin/ModuleAdmin.aspx" />&nbsp;&gt;
        <cy:SiteLabel id="lblModuleDefinition" runat="server" ConfigKey="ModuleDefinitionsModuleDefinitionLabel"> </cy:SiteLabel>
    </legend>
    <div class="settingrow">
        <cy:SiteLabel id="lblFeatureName" runat="server" ForControl="txtFeatureName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceKeyLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtFeatureName" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel4" runat="server" ForControl="txtResourceFile" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceFileLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtResourceFile" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="lblControlSource" runat="server" ForControl="txtControlSource" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDesktopSourceLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtControlSource" runat="server" Columns="50" maxlength="150" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel3" runat="server" ForControl="txtFeatureGuid" CssClass="settinglabel" ConfigKey="ModuleDefinitionsFeatureGuidLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtFeatureGuid" runat="server" Columns="50" maxlength="36" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="lblSortOrder" runat="server" ForControl="txtSortOrder" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSortOrderLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtSortOrder" runat="server" Columns="20" maxlength="3" CssClass="forminput"></asp:TextBox>
    </div>
     <div class="settingrow">
        <cy:SiteLabel id="Sitelabel5" runat="server" ForControl="chkIsCacheable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsCacheableLabel"> </cy:SiteLabel>
        <asp:CheckBox ID="chkIsCacheable" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel2" runat="server" ForControl="txtDefaultCacheDuration" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDefaultCacheDurationLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtDefaultCacheDuration" runat="server" Columns="20" maxlength="8" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="Sitelabel6" runat="server" ForControl="chkIsSearchable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsSearchableLabel"> </cy:SiteLabel>
        <asp:CheckBox ID="chkIsSearchable" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel7" runat="server" ForControl="txtSearchListName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSearchListNameLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtSearchListName" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="Sitelabel1" runat="server" ForControl="chkIsAdmin" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsAdminLabel"> </cy:SiteLabel>
        <asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
    <div  class="settingrow">
	    <cy:SiteLabel id="lblIcon" runat="server" ForControl="ddIcons" CssClass="settinglabel" ConfigKey="ModuleSettingsIconLabel" > </cy:SiteLabel>
	    <asp:DropDownList id="ddIcons" runat="server" DataValueField="Name" DataTextField="Name" CssClass="forminput"></asp:DropDownList>
	    <img id="imgIcon" alt="" src=""  runat="server" />
	</div>
    <div class="settingrow">
        <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="DefinitionSettings"></asp:ValidationSummary>
        <asp:RequiredFieldValidator id="reqFeatureName" runat="server" Display="None" ValidationGroup="DefinitionSettings" 
        ControlToValidate="txtFeatureName"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator id="reqControlSource" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtControlSource"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator id="reqSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtSortOrder"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator id="reqDefaultCache" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtDefaultCacheDuration"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="regexSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings"
		    ControlToValidate="txtSortOrder" ValidationExpression="^[0-9][0-9]{0,4}$" />
		<asp:RegularExpressionValidator ID="regexCacheDuration" runat="server" Display="None" ValidationGroup="DefinitionSettings"
		    ControlToValidate="txtDefaultCacheDuration" ValidationExpression="^[0-9][0-9]{0,8}$" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
        <portal:CButton  id="updateButton" runat="server" Text="" ValidationGroup="DefinitionSettings" />&nbsp;
	    <portal:CButton  id="cancelButton" runat="server" Text="" CausesValidation="false" />&nbsp;
	    <portal:CButton id="deleteButton" runat="server" Text="" CausesValidation="false" />&nbsp;
	    <asp:HyperLink ID="lnkConfigureSettings" runat="server" />
	    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="moduledefinitionedithelp" />	
    </div>
    <asp:Label ID="lblErrorMessage" runat="server" CssClass="txterror" /> 
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
