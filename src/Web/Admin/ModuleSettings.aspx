<%@ Page CodeBehind="ModuleSettings.aspx.cs" MaintainScrollPositionOnPostback="true" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.ModuleSettingsPage" EnableEventValidation="false" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
 <cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<cy:YUIPanel id="pnlModules" runat="server" CssClass="panelwrapper admin modulesettings">
<div class="modulecontent">
 <fieldset class="modulesettings">
        <legend>
        <cy:SiteLabel id="lblModuleSettings" runat="server" ConfigKey="ModuleSettingsSettingsLabel" UseLabelTag="false" EnableViewState="false"> </cy:SiteLabel>
    </legend>
    <div id="divtabs" class="yui-navset">
        <ul class="yui-nav">
            <li class="selected"><a href="#tabFeatureSpecificSettings"><em>
                <asp:Literal ID="litFeatureSpecificSettingsTab" runat="server" EnableViewState="false" /></em></a></li>
            <li id="liGeneralSettings" runat="server"><a id="lnkGeneralSettingsTab" runat="server" href="#tabGeneralSettings"><em><asp:Literal ID="litGeneralSettingsTab" runat="server" EnableViewState="false" /></em></a></li>
            <li id="liSecurity" runat="server"><a id="lnkSecurityTab" runat="server" href="#tabSecurity"><em><asp:Literal ID="litSecurityTab" runat="server" EnableViewState="false" /></em></a></li>
        </ul>
        <div class="yui-content">
        <div ID="tabFeatureSpecificSettings">
        <div  class="settingrow" id="divWebParts" runat="server" visible="false">
		    <cy:SiteLabel id="SiteLabel4" runat="server" ForControl="ddWebParts" CssClass="settinglabel" ConfigKey="WebPartModuleWebPartSetting" EnableViewState="false"> </cy:SiteLabel>
		    <asp:DropDownList id="ddWebParts" runat="server" DataValueField="WebPartID" DataTextField="ClassName"></asp:DropDownList>
		</div>
        <asp:PlaceHolder id="PlaceHolderAdvancedSettings" runat="server"></asp:PlaceHolder>
        <div class="settingrow">
         <cy:SiteLabel id="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="spacer" UseLabelTag="false"> </cy:SiteLabel>
         </div>
        </div>
        <div ID="tabGeneralSettings" runat="server" >
            <div class="settingrow">
                <cy:SiteLabel id="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="ModuleSettingsFeatureNameLabel" UseLabelTag="false"> </cy:SiteLabel>
                <asp:Label ID="lblFeatureName" runat="server" EnableViewState="false" CssClass="forminput" />
            </div>
            <div class="settingrow" id="divParentPage" runat="server" visible="false">
                <cy:SiteLabel id="lblParentPage" runat="server" ForControl="ddPages" CssClass="settinglabel" ConfigKey="PageLayoutParentPageLabel"> </cy:SiteLabel>
                <asp:DropDownList id="ddPages" runat="server" EnableTheming="false" DataTextField="PageName" DataValueField="PageID" CssClass="forminput"></asp:DropDownList>
            </div>
            <div class="settingrow">
                <cy:SiteLabel id="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="settinglabel" ConfigKey="ModuleSettingsModuleNameLabel"> </cy:SiteLabel>
                <asp:Textbox id="moduleTitle" runat="server" columns="46" EnableViewState="false" CssClass="forminput"></asp:Textbox>
            </div>
            <div id="divCacheTimeout" runat="server" class="settingrow">
                <cy:SiteLabel ID="lblCacheTime" runat="server" ForControl="cacheTime" CssClass="settinglabel" ConfigKey="ModuleSettingsCacheTimeLabel"  />
                <asp:Textbox id="cacheTime" runat="server" width="100" MaxLength="8" Text="0" EnableViewState="false" CssClass="forminput"></asp:Textbox>
            </div>
            <div  class="settingrow">
            <cy:SiteLabel id="lblShowTitle" runat="server" ForControl="chkShowTitle" CssClass="settinglabel" ConfigKey="ModuleSettingsShowTitleLabel"> </cy:SiteLabel>
            <asp:Checkbox id="chkShowTitle" runat="server" EnableViewState="false" CssClass="forminput"></asp:Checkbox>
            </div>
            <div  class="settingrow">
            <cy:SiteLabel id="SiteLabel6" runat="server" ForControl="chkHideFromAuth" CssClass="settinglabel" ConfigKey="ModuleSettingsHideFromAuthenticatedLabel"> </cy:SiteLabel>
            <asp:Checkbox id="chkHideFromAuth" runat="server" EnableViewState="false" CssClass="forminput"></asp:Checkbox>
            </div>
            <div  class="settingrow">
            <cy:SiteLabel id="SiteLabel7" runat="server" ForControl="chkHideFromUnauth" CssClass="settinglabel" ConfigKey="ModuleSettingsHideFromUnauthenticatedLabel"> </cy:SiteLabel>
            <asp:Checkbox id="chkHideFromUnauth" runat="server" EnableViewState="false" CssClass="forminput"></asp:Checkbox>
            </div>
            <div id="divMyPage" runat="server" class="settingrow">
                <cy:SiteLabel id="SiteLabel2" runat="server" ForControl="chkAvailableForMyPage" CssClass="settinglabel" ConfigKey="ModuleSettingsAvailableForMyPageLabel"> </cy:SiteLabel>
                <asp:Checkbox id="chkAvailableForMyPage" runat="server" EnableViewState="false" CssClass="forminput"></asp:Checkbox>
            </div>
            <div id="divMyPageMulti" runat="server" class="settingrow">
                <cy:SiteLabel id="SiteLabel3" runat="server" CssClass="settinglabel" ForControl="chkAllowMultipleInstancesOnMyPage" ConfigKey="ModuleSettingsAllowMultipleInstancesOnMyPageLabel"> </cy:SiteLabel>
                <asp:Checkbox id="chkAllowMultipleInstancesOnMyPage" runat="server" EnableViewState="false" CssClass="forminput"></asp:Checkbox>
            </div>
            <div  class="settingrow">
		        <cy:SiteLabel id="lblIcon" runat="server" ForControl="ddIcons" CssClass="settinglabel" ConfigKey="ModuleSettingsIconLabel" > </cy:SiteLabel>
		        <asp:DropDownList id="ddIcons" runat="server" EnableTheming="false" DataValueField="Name" DataTextField="Name" CssClass="forminput"></asp:DropDownList>
		        <img id="imgIcon" alt="" src=""  runat="server" />
		    </div>
		    <div  class="settingrow">
            <cy:SiteLabel id="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="spacer"> </cy:SiteLabel>
            </div>
        </div>
        
        <div ID="tabSecurity" runat="server">
            <div class="settingrow">
            <cy:SiteLabel id="SiteLabel8" runat="server" ForControl="authEditRoles" CssClass="settinglabel" ConfigKey="ModuleSettingsViewRolesLabel"> </cy:SiteLabel>
            </div>
            <div class="settingrow">
            <asp:CheckBoxList id="cblViewRoles" runat="server"></asp:CheckBoxList>
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="lblEditRoles" runat="server" ForControl="authEditRoles" CssClass="settinglabel" ConfigKey="ModuleSettingsEditRolesLabel"> </cy:SiteLabel>
            </div>
            <div class="settingrow">
            <asp:CheckBoxList id="authEditRoles" runat="server"></asp:CheckBoxList>
            </div>
            <asp:Panel ID="pnlDraftEditRoles" runat="server">
            <div class="settingrow">
                <cy:SiteLabel id="SiteLabel9" runat="server" ForControl="draftEditRoles" CssClass="settinglabel" ConfigKey="ModuleSettingsDraftEditRolesLabel"> </cy:SiteLabel>
            </div>
            <div class="settingrow">
                <asp:CheckBoxList id="draftEditRoles" runat="server"></asp:CheckBoxList>
            </div>  
            </asp:Panel>
            <div id="divEditUser" runat="server" class="settingrow" style="height:220px;">
                <cy:SiteLabel id="Sitelabel1" runat="server" ForControl="scUser" CssClass="settinglabel" ConfigKey="ModuleSettingsEditUserLabel" > </cy:SiteLabel>
                <cy:SmartCombo ID="scUser" runat="server" 
	                DataUrl="../Services/UserDropDownXml.aspx?query=" 
	                ShowValueField="True" 
	                ValueCssClass="TextLabel" 
	                ValueColumns="5" 
	                ValueLabelText="UserID:" 
	                ValueLabelCssClass="" 
	                ButtonImageUrl="../Data/SiteImages/DownArrow.gif" 
	                ScriptDirectory="~/ClientScript" 
	                Columns="45"  MaxLength="50" > </cy:SmartCombo>    
            </div>
        </div>
        
        </div>
    </div>
    
    <div class="modulecontent">
        <div class="settingrow">
            <portal:CLabel id="lblValidationSummary" runat="server" CssClass="txterror" EnableViewState="false" />
            <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="ModuleSettings" EnableViewState="false"></asp:ValidationSummary>
            <asp:RequiredFieldValidator id="reqCacheTime" runat="server" Display="None" ValidationGroup="ModuleSettings"
							ControlToValidate="cacheTime" Enabled="false" EnableViewState="false"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="regexCacheTime" runat="server" Display="None" ValidationGroup="ModuleSettings"
			    ControlToValidate="cacheTime" ValidationExpression="^[0-9][0-9]{0,8}$" EnableViewState="false" />
            
        </div>
        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <portal:CButton id="btnSave" runat="server" ValidationGroup="ModuleSettings" />&nbsp;
            <portal:CButton id="btnDelete" runat="server" CausesValidation="false" />
            &nbsp;<asp:HyperLink ID="lnkCancel" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkEditContent" runat="server" Visible="false" EnableViewState="false" />
            &nbsp;<asp:HyperLink ID="lnkPublishing" runat="server" Visible="false" EnableViewState="false" />
        </div>
	</div>
</fieldset>
</div>
 </cy:YUIPanel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />