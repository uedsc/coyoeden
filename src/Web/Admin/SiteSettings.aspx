<%@ Page Language="C#" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SiteSettings.aspx.cs"
    Inherits="Cynthia.Web.AdminUI.SiteSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlSiteSettings" runat="server" DefaultButton="btnSave" CssClass="panelwrapper admin sitesettings">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
                        EnableViewState="false" />&nbsp;&gt;
                    <asp:HyperLink ID="lnkSiteSettings" runat="server" EnableViewState="false" />
                </legend>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblSiteTitle" ForControl="txtSiteName" runat="server" CssClass="settinglabel "
                        ConfigKey="SiteSettingsSiteTitleLabel" EnableViewState="false"> </cy:SiteLabel>
                    <asp:TextBox ID="txtSiteName" TabIndex="10" runat="server" CssClass="forminput widetextbox"
                        Columns="45" />
                    <asp:DropDownList ID="ddSiteList" runat="server" AutoPostBack="True" EnableViewState="true"
                        EnableTheming="false" DataTextField="SiteName" DataValueField="SiteID" Visible="False"
                        OnSelectedIndexChanged="ddSiteList_SelectedIndexChanged">
                    </asp:DropDownList>
                    <portal:CHelpLink ID="hlp1" runat="server" HelpKey="sitesettingssitelisthelp" />
                </div>
                <div id="divtabs" class="yui-navset clear">
                    <ul class="yui-nav">
                        <li id="liGeneral" runat="server"><a href="#tabSettings"><em>
                            <asp:Literal ID="litSettingsTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liSecurity" runat="server" enableviewstate="false"><a href="#tabSecurity"
                            id="lnkSecurity" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litSecurityTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liCommerce" runat="server" enableviewstate="false"><a href="#tabCommerce"
                            id="lnkCommerce" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litCommerceTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liHosts" runat="server" visible="false" enableviewstate="false"><a href="#tabHosts"
                            id="lnkHosts" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litHostsTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liFolderNames" runat="server" visible="false" enableviewstate="false"><a
                            href="#tabFolderNames" id="lnkFolderNames" runat="server" enableviewstate="false">
                            <em>
                                <asp:Literal ID="litFolderNamesTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liFeatures" runat="server" visible="false" enableviewstate="false"><a href="#tabFeatureNames"
                            id="lnkFeatures" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litFeaturesTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liWebParts" runat="server" visible="false" enableviewstate="false"><a href="#tabWebParts"
                            id="lnkWebParts" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litWebPartsTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li><a href="#tabAPIKeys"><em>
                            <asp:Literal ID="litAPIKeysTab" runat="server" EnableViewState="false" /></em></a></li>
                        <li id="liMailSettings" runat="server" enableviewstate="false"><a href="#tabMailSettings"
                            id="lnkMailSettings" runat="server" enableviewstate="false"><em>
                                <asp:Literal ID="litMailSettingsTab" runat="server" EnableViewState="false" /></em></a></li>
                    </ul>
                    <div class="yui-content">
                        <div id="tabSettings">
                            <div id="divSiteId" runat="server" class="settingrow" visible="false">
                                <cy:SiteLabel ID="SiteLabel52" ForControl="ddSkins" runat="server" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsSiteIDLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:Label ID="lblSiteId" runat="server" />/<asp:Label ID="lblSiteGuid" runat="server" />
                            </div>
                            
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblSkin" ForControl="ddSkins" runat="server" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsSiteSkinLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddSkins" runat="server" TabIndex="10" EnableViewState="true"
                                    EnableTheming="false" DataValueField="Name" DataTextField="Name" CssClass="forminput">
                                </asp:DropDownList>
                                <portal:GreyBoxHyperlink ID="gbSkinPreview" runat="server" />
                                <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="sitesettingssiteskinhelp" />
                                <asp:Button ID="btnRestoreSkins" runat="server" Visible="false" />
                            </div>
                            <div class="settingrow logolist">
                                <cy:SiteLabel ID="lblLogo" ForControl="ddLogos" runat="server" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsSiteLogoLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddLogos" runat="server" TabIndex="10" EnableViewState="true"
                                    EnableTheming="false" DataValueField="Name" DataTextField="Name" CssClass="forminput">
                                </asp:DropDownList>
                                <img alt="" src="" id="imgLogo" runat="server" enableviewstate="false" />
                                <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="sitesettingssitelogohelp" />
                            </div>
                            <div id="divFriendlyUrlPattern" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblDefaultFriendlyUrlPatten" runat="server" ForControl="ddDefaultFriendlyUrlPattern"
                                    CssClass="settinglabel" ConfigKey="SiteSettingsDefaultFriendlyUrlPatternLabel"
                                    EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddDefaultFriendlyUrlPattern" EnableTheming="false" runat="server"
                                    TabIndex="10" CssClass="forminput">
                                    <asp:ListItem Value="PageNameWithDotASPX" Text="<%$ Resources:Resource, UrlFormatAspx %>"></asp:ListItem>
                                    <asp:ListItem Value="PageName" Text="<%$ Resources:Resource, UrlFormatExtensionless %>"></asp:ListItem>
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="sitesettingsdefaultfriendlyurlpatternhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel83" runat="server" ForControl="txtSlogan" CssClass="settinglabel"
                                    ConfigKey="SloganLabel" EnableViewState="false" />
                                <asp:TextBox ID="txtSlogan" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink79" runat="server" HelpKey="site-slogan-help" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel47" runat="server" ForControl="txtCompanyName" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsCompanyNameLabel" EnableViewState="false" />
                                <asp:TextBox ID="txtCompanyName" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink5" runat="server" HelpKey="sitesettingscompanynamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel19" runat="server" ForControl="txtSiteEmailFromAddress"
                                    CssClass="settinglabel" ConfigKey="SiteSettingsSiteEmailFromAddressLabel" EnableViewState="false" />
                                <asp:TextBox ID="txtSiteEmailFromAddress" runat="server" TabIndex="10" MaxLength="100"
                                    CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink6" runat="server" HelpKey="sitesettingssiteemailfromhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel29" runat="server" ForControl="ddEditorProviders" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name"
                                    EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="forminput">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink7" runat="server" HelpKey="sitesettingssiteeditorproviderhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel78" runat="server" ForControl="ddNewsletterEditor" CssClass="settinglabel"
                                    ConfigKey="NewsletterEditorLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddNewsletterEditor" DataTextField="name" DataValueField="name"
                                    EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="forminput">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink49" runat="server" HelpKey="newletter-editor-help" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel74" runat="server" ForControl="ddAvatarSystem" CssClass="settinglabel"
                                    ConfigKey="AvatarSystemLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddAvatarSystem" DataTextField="name" DataValueField="name"
                                    EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="forminput">
                                    <asp:ListItem Value="none" Text="<%$ Resources:Resource, AvatarTypeNone %>"></asp:ListItem>
                                    <asp:ListItem Value="internal" Text="<%$ Resources:Resource, AvatarTypeInternal %>"></asp:ListItem>
                                    <asp:ListItem Value="gravatar" Text="<%$ Resources:Resource, AvatarTypeGravatar %>"></asp:ListItem>
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink8" runat="server" HelpKey="avatarsystem-help" />
                            </div>
                           
                            <div id="divAllowUserSkins" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel2" runat="server" ForControl="chkAllowUserSkins" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsAllowUserSkinsLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkAllowUserSkins" runat="server" TabIndex="10" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink9" runat="server" HelpKey="sitesettingsuserskinhelp" />
                            </div>
                            <div id="divAllowPageSkins" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel2x" runat="server" ForControl="chkAllowPageSkins" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsAllowPageSkinsLabel" EnableViewState="false"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkAllowPageSkins" runat="server" TabIndex="10" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink10" runat="server" HelpKey="sitesettingspageskinhelp" />
                            </div>
                            <div id="divAllowHideMenu" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel2y" runat="server" ForControl="chkAllowHideMenuOnPages"
                                    CssClass="settinglabel" ConfigKey="SiteSettingsAllowHideMainMenuLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkAllowHideMenuOnPages" runat="server" TabIndex="10" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="sitesettingsallowhidemenuhelp" />
                            </div>
                            <div id="divMyPage" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel20" ForControl="chkEnableMyPageFeature" runat="server"
                                    CssClass="settinglabel" ConfigKey="SiteSettingsEnableMyPageFeatureLabel" EnableViewState="false">
                                </cy:SiteLabel>
                                <asp:CheckBox ID="chkEnableMyPageFeature" runat="server" TabIndex="10" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink12" runat="server" HelpKey="sitesettingsmypagehelp" />
                            </div>
                            <div id="divMyPageSkin" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel45" runat="server" CssClass="settinglabel" ConfigKey="MyPageSkinLabel">
                                </cy:SiteLabel>
                                <asp:DropDownList ID="ddMyPageSkin" runat="server" EnableTheming="false" DataValueField="Name"
                                    DataTextField="Name" CssClass="forminput" TabIndex="10">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink13" runat="server" HelpKey="mypageskinhelp" />
                            </div>
                            <div id="divSSL" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel3" runat="server" ForControl="chkRequireSSL" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsRequireSSLLabel" />
                                <asp:CheckBox ID="chkRequireSSL" runat="server" TabIndex="10" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink14" runat="server" HelpKey="sitesettingsrequiresslhelp" />
                            </div>
                            <div id="divReallyDeleteUsers" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SitelabelReallyDeleteUsers" runat="server" ForControl="chkReallyDeleteUsers"
                                    CssClass="settinglabel" ConfigKey="SiteSettingsReallyDeleteUsersLabel" />
                                <asp:CheckBox ID="chkReallyDeleteUsers" runat="server" TabIndex="10" CssClass="forminput" /><cy:SiteLabel
                                    ID="SitelabelReallyDeleteUsersExplain" runat="server" ConfigKey="SiteSettingsReallyDeleteUsersExplainLabel" />
                                <portal:CHelpLink ID="CynHelpLink15" runat="server" HelpKey="sitesettingsreallydeleteusershelp" />
                            </div>
                            <div id="divContentVersioning" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel48" runat="server" ForControl="chkForceContentVersioning"
                                    CssClass="settinglabel" ConfigKey="ForceContentVersioning" />
                                <asp:CheckBox ID="chkForceContentVersioning" runat="server" TabIndex="10" CssClass="forminput" /><cy:SiteLabel
                                    ID="Sitelabel49" runat="server" />
                                <portal:CHelpLink ID="CynHelpLink16" runat="server" HelpKey="sitesettingsforcecontentversioninghelp" />
                            </div>
                            <div id="divApprovalsWorkflow" runat="server" class="settingrow">
                                <cy:SiteLabel ID="Sitelabel59" runat="server" ForControl="chkEnableContentWorkflow"
                                    CssClass="settinglabel" ConfigKey="EnableContentWorkflow" />
                                <asp:CheckBox ID="chkEnableContentWorkflow" runat="server" TabIndex="10" CssClass="forminput" /><cy:SiteLabel
                                    ID="Sitelabel57" runat="server" />
                                <portal:CHelpLink ID="CynHelpLink67" runat="server" HelpKey="sitesettingsenablecontentworkflowhelp" />
                            </div>
                            <div class="settingrow" id="divPreferredHostName" runat="server">
                                <cy:SiteLabel ID="SiteLabel24" runat="server" ForControl="txtPreferredHostName" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsPreferredHostNameLabel" />
                                <asp:TextBox ID="txtPreferredHostName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                                    runat="server" />
                                <portal:CHelpLink ID="CynHelpLink17" runat="server" HelpKey="sitesettingspreferredhostnamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel53" runat="server" ForControl="txtPrivacyPolicyUrl" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsPrivacyUrlLabel" EnableViewState="false" />
                                <asp:Label ID="lblPrivacySiteRoot" runat="server" CssClass="forminput" />
                                <asp:TextBox ID="txtPrivacyPolicyUrl" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                                    runat="server" />
                                <portal:CHelpLink ID="CynHelpLink62" runat="server" HelpKey="sitesettingsprivacyhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel56" runat="server" ForControl="txtOpenSearchName" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsOpenSearchNameLabel" EnableViewState="false" />
                                <asp:TextBox ID="txtOpenSearchName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                                    runat="server" />
                                <portal:CHelpLink ID="CynHelpLink65" runat="server" HelpKey="sitesettings-opensearchname-help" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel79" runat="server" ForControl="txtMetaProfile" CssClass="settinglabel"
                                    ConfigKey="MetaProfileLabel" EnableViewState="false" />
                                <asp:TextBox ID="txtMetaProfile" TabIndex="10" CssClass="forminput verywidetextbox"
                                    runat="server" />
                                <portal:CHelpLink ID="CynHelpLink50" runat="server" HelpKey="meta-profile-help" />
                            </div>
                            <div class="settingrow" id="div2" runat="server">
                                &nbsp;<br />
                            </div>
                        </div>
                        <div id="tabSecurity" runat="server">
                            <div id="divSecurityTabs" class="yui-navset">
                                <ul class="yui-nav">
                                    <li class="selected" id="liGeneralSecurity" runat="server" enableviewstate="false"><a
                                        href="#tabGeneralSecurity" id="lnkGeneralSecurityTab" runat="server" enableviewstate="false">
                                        <em>
                                            <asp:Literal ID="litGeneralSecurityTab" runat="server" EnableViewState="false" /></em></a></li>
                                    <li id="liPermissions" runat="server" enableviewstate="false"><a href="#tabPermissions"
                                        id="lnkPermissions" runat="server" enableviewstate="false"><em>
                                            <asp:Literal ID="litPermissionsTab" runat="server" EnableViewState="false" /></em></a></li>
                                    <li id="liLDAP" runat="server" enableviewstate="false"><a href="#tabLDAP" id="lnkLDAP"
                                        runat="server" enableviewstate="false"><em>
                                            <asp:Literal ID="litLDAPTab" runat="server" EnableViewState="false" /></em></a></li>
                                    <li id="liOpenID" runat="server" enableviewstate="false"><a href="#tabOpenID" id="lnkOpenIDTab"
                                        runat="server" enableviewstate="false"><em>
                                            <asp:Literal ID="litOpenIDTab" runat="server" EnableViewState="false" /></em></a></li>
                                    <li id="liWindowsLive" runat="server" enableviewstate="false"><a href="#tabWindowsLiveID"
                                        id="lnkWindowsLive" runat="server" enableviewstate="false"><em>
                                            <asp:Literal ID="litWindowsLiveTab" runat="server" EnableViewState="false" /></em></a></li>
                                    <li id="liCaptcha" runat="server"><a href="#tabAntiSpam"><em>
                                        <asp:Literal ID="litAntiSpamTab" runat="server" EnableViewState="false" /></em></a></li>
                                </ul>
                                <div class="yui-content">
                                    <div id="tabGeneralSecurity" runat="server">
                                        <asp:Panel ID="pnlUserSecurity" runat="server" SkinID="plain">
                                            <div id="divAllowRegistration" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkAllowRegistration" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsAllowRegistrationLabel" />
                                                <asp:CheckBox ID="chkAllowRegistration" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink18" runat="server" HelpKey="sitesettingsallowregistrationhelp" />
                                            </div>
                                            <div id="divUseEmailForLogin" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabelemailforlogin" runat="server" ForControl="chkUseEmailForLogin"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsUseEmailForLoginLabel" />
                                                <asp:CheckBox ID="chkUseEmailForLogin" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink19" runat="server" HelpKey="sitesettingsuseemailforloginhelp" />
                                            </div>
                                            <div id="divSecureRegistration" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblSecureRegistration" runat="server" ForControl="chkSecureRegistration"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsSecureRegistrationLabel" />
                                                <asp:CheckBox ID="chkSecureRegistration" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink20" runat="server" HelpKey="sitesettingssecureregistrationhelp" />
                                            </div>
                                            <div id="divAllowUserToChangeName" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="SitelabelAllowUserToChangeName" runat="server" ForControl="chkAllowUserToChangeName"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsAllowUsersToChangeNameLabel" />
                                                <asp:CheckBox ID="chkAllowUserToChangeName" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink21" runat="server" HelpKey="sitesettingsallowusernamechangehelp" />
                                            </div>
                                            <div id="divDisableDbAuthentication" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel76" runat="server" ForControl="chkDisableDbAuthentication"
                                                    CssClass="settinglabel" ConfigKey="DisableDbAuthentication" />
                                                <asp:CheckBox ID="chkDisableDbAuthentication" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink76" runat="server" HelpKey="sitesettings-DisableDbAuthentication-help" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel11" runat="server" ForControl="ddPasswordFormat" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsPasswordFormatLabel"> </cy:SiteLabel>
                                                <asp:DropDownList ID="ddPasswordFormat" EnableTheming="false" runat="server" TabIndex="10"
                                                    CssClass="forminput">
                                                </asp:DropDownList>
                                                <portal:CHelpLink ID="CynHelpLink22" runat="server" HelpKey="sitesettingspasswordformathelp" />
                                            </div>
                                            <div id="divEncryptPasswords" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lbl1" runat="server" ForControl="chkAllowPasswordRetrieval" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsAllowPasswordRetrievalLabel" />
                                                <asp:CheckBox ID="chkAllowPasswordRetrieval" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink23" runat="server" HelpKey="sitesettingsallowpasswordretrievalhelp" />
                                            </div>
                                            <div id="div1" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel16" runat="server" ForControl="chkRequiresQuestionAndAnswer"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsRequiresQuestionAndAnswerLabel" />
                                                <asp:CheckBox ID="chkRequiresQuestionAndAnswer" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink24" runat="server" HelpKey="sitesettingsrequirequestionandanswerhelp" />
                                            </div>
                                            <div id="divAllowPasswordReset" runat="server" visible="false" class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel12" runat="server" ForControl="chkAllowPasswordReset"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsAllowPasswordResetLabel" />
                                                <asp:CheckBox ID="chkAllowPasswordReset" runat="server" TabIndex="10" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink25" runat="server" HelpKey="sitesettingsallowpasswordresethelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel17" runat="server" ForControl="txtMaxInvalidPasswordAttempts"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsMaxInvalidPasswordAttemptsLabel" />
                                                <asp:TextBox ID="txtMaxInvalidPasswordAttempts" TabIndex="10" MaxLength="2" Columns="10"
                                                    CssClass="forminput smalltextbox" runat="server" />
                                                <portal:CHelpLink ID="CynHelpLink26" runat="server" HelpKey="sitesettingsmaxincalidpasswordhelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel18" runat="server" ForControl="txtPasswordAttemptWindowMinutes"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsPasswordAttemptWindowMinutesLabel" />
                                                <asp:TextBox ID="txtPasswordAttemptWindowMinutes" TabIndex="10" MaxLength="2" Columns="10"
                                                    CssClass="forminput smalltextbox" runat="server" />
                                                <portal:CHelpLink ID="CynHelpLink27" runat="server" HelpKey="sitesettingspasswordattemptwindowhelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel13" runat="server" ForControl="txtMinimumPasswordLength"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsMinimumPasswordLengthLabel" />
                                                <asp:TextBox ID="txtMinimumPasswordLength" TabIndex="10" MaxLength="2" Columns="10"
                                                    CssClass="forminput smalltextbox" runat="server" Text="7" />
                                                <portal:CHelpLink ID="CynHelpLink28" runat="server" HelpKey="sitesettingspasswordlengthhelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtMinRequiredNonAlphaNumericCharacters"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsMinRequiredNonAlphaNumericCharactersLabel" />
                                                <asp:TextBox ID="txtMinRequiredNonAlphaNumericCharacters" TabIndex="10" MaxLength="2"
                                                    CssClass="forminput smalltextbox" Columns="10" runat="server" Text="0" />
                                                <portal:CHelpLink ID="CynHelpLink29" runat="server" HelpKey="sitesettingspasswordnonalphacharactershelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel15" runat="server" ForControl="txtPasswordStrengthRegularExpression"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsPasswordStrengthExpressionLabel" />
                                                <asp:TextBox ID="txtPasswordStrengthRegularExpression" TabIndex="10" TextMode="MultiLine"
                                                    Rows="3" Columns="45" runat="server" CssClass="forminput" />
                                                <portal:CHelpLink ID="CynHelpLink30" runat="server" HelpKey="sitesettingspasswordstrengthhelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel63" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                                </cy:SiteLabel>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div id="tabPermissions" runat="server">
                                        <div class="C-accordion">
                                            <h3 id="h3SiteEditRoles" runat="server" visible="false">
                                                <a href="#">
                                                <cy:SiteLabel ID="Sitelabel42" runat="server" ConfigKey="SiteEditRolesLabel" UseLabelTag="false">
                                                </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div id="divSiteEditRoles" runat="server" visible="false">
                                                <p>
                                                <asp:CheckBoxList ID="chkListEditRoles" runat="server" SkinID="Roles" CssClass="forminput">
                                                </asp:CheckBoxList>
                                                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="sitesettingseditroleshelp" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel77" runat="server" ConfigKey="RolesThatCanCreateRootPages"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanCreateRootPages" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink77" runat="server" HelpKey="RolesThatCanCreateRootPages-help" /></p>
                                            </div>
                                        
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="lblAuthorizedRoles" runat="server" ConfigKey="GeneralBrowseAndUploadRoles"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkGeneralBrowseAndUploadRoles" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink66" runat="server" HelpKey="GeneralBrowseAndUploadRoles-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel66" runat="server" ConfigKey="UserFilesBrowseAndUploadRoles"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkUserFilesBrowseAndUploadRoles" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink68" runat="server" HelpKey="UserFilesBrowseAndUploadRoles-help" /></p>
                                            </div>
                                            
                                            
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel75" runat="server" ConfigKey="RolesThatCanDeleteFilesInEditor"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanDeleteFilesInEditor" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink75" runat="server" HelpKey="RolesThatCanDeleteFilesInEditor-help" /></p>
                                            </div>
                                            
                                            
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel67" runat="server" ConfigKey="RolesThatCanEditContentTemplates"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanEditContentTemplates" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink69" runat="server" HelpKey="RolesThatCanEditContentTemplates-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel68" runat="server" ConfigKey="RolesNotAllowedToEditModuleSettings"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesNotAllowedToEditModuleSettings" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink70" runat="server" HelpKey="RolesNotAllowedToEditModuleSettings-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel69" runat="server" ConfigKey="RolesThatCanManageUsers"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanManageUsers" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink71" runat="server" HelpKey="RolesThatCanManageUsers-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel70" runat="server" ConfigKey="RolesThatCanLookupUsers"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanLookupUsers" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink72" runat="server" HelpKey="RolesThatCanLookupUsers-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel71" runat="server" ConfigKey="RolesThatCanViewMemberList"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanViewMemberList" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink73" runat="server" HelpKey="RolesThatCanViewMemberList-help" /></p>
                                            </div>
                                            <h3>
                                                <a href="#">
                                                    <cy:SiteLabel ID="SiteLabel72" runat="server" ConfigKey="RolesThatCanViewMyPage"
                                                        UseLabelTag="false"> </cy:SiteLabel>
                                                </a>
                                            </h3>
                                            <div>
                                                <p>
                                                    <asp:CheckBoxList ID="chkRolesThatCanViewMyPage" runat="server" CssClass="forminput">
                                                    </asp:CheckBoxList>
                                                    <portal:CHelpLink ID="CynHelpLink74" runat="server" HelpKey="RolesThatCanViewMyPage-help" /></p>
                                            </div>
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel73" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                            </cy:SiteLabel>
                                        </div>
                                    </div>
                                    <div id="tabLDAP" runat="server">
                                        <asp:Panel ID="pnlLdapSettings" runat="server" SkinID="plain">
                                            <div id="divUseLdap" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblUseLdapAuth" ForControl="chkUseLdapAuth" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsUseLdapAuth" runat="server"> </cy:SiteLabel>
                                                <asp:CheckBox ID="chkUseLdapAuth" runat="server" TabIndex="10" CssClass="forminput">
                                                </asp:CheckBox>
                                                <portal:CHelpLink ID="CynHelpLink31" runat="server" HelpKey="sitesettingsuseldaphelp" />
                                            </div>
                                            <div id="divLdapTestPassword" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel9" ForControl="txtLdapTestPassword" ConfigKey="SiteSettingsLdapTestPassword"
                                                    CssClass="settinglabel" runat="server"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtLdapTestPassword" Columns="55" TabIndex="10" runat="server" TextMode="password"
                                                    CssClass="forminput normaltextbox" MaxLength="255"></asp:TextBox>
                                                <portal:CHelpLink ID="CynHelpLink32" runat="server" HelpKey="sitesettingsldappasswordhelp" />
                                                <br />
                                                <br />
                                            </div>
                                            <div id="divAutoCreateLdapUsers" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblAutoCreateLdapUser" ForControl="chkAutoCreateLdapUserOnFirstLogin"
                                                    CssClass="settinglabel" ConfigKey="SiteSettingsAutoCreateLdapUserOnFirstLoginLabel"
                                                    runat="server"> </cy:SiteLabel>
                                                <asp:CheckBox ID="chkAutoCreateLdapUserOnFirstLogin" runat="server" TabIndex="10"
                                                    CssClass="forminput"></asp:CheckBox>
                                                <portal:CHelpLink ID="CynHelpLink33" runat="server" HelpKey="sitesettingsautocreateldapuserhelp" />
                                            </div>
                                            <div id="divLdapServer" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblLdapServer" ForControl="txtLdapServer" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsLdapServer" runat="server"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtLdapServer" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                                    CssClass="forminput widetextbox"></asp:TextBox>
                                                <portal:CHelpLink ID="CynHelpLink34" runat="server" HelpKey="sitesettingsldapserverhelp" />
                                            </div>
                                            <div id="divLdapPort" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblLdapPort" ForControl="txtLdapPort" CssClass="settinglabel" ConfigKey="SiteSettingsLdapPort"
                                                    runat="server"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtLdapPort" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                                    CssClass="forminput smalltextbox"></asp:TextBox>
                                                <portal:CHelpLink ID="CynHelpLink35" runat="server" HelpKey="sitesettingsldapporthelp" />
                                            </div>
                                            <div id="divLdapDomain" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel26" ForControl="txtLdapDomain" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsLdapDomain" runat="server"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtLdapDomain" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                                    CssClass="forminput widetextbox"></asp:TextBox>
                                                <portal:CHelpLink ID="CynHelpLink36" runat="server" HelpKey="sitesettingsldapdomainhelp" />
                                            </div>
                                            <div id="divLdapRootDn" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="lblLdapRootDN" ForControl="txtLdapRootDN" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsLdapRootDN" runat="server"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtLdapRootDN" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                                    CssClass="forminput widetextbox"></asp:TextBox>
                                                <portal:CHelpLink ID="CynHelpLink37" runat="server" HelpKey="sitesettingsldaprootdnhelp" />
                                            </div>
                                            <div id="divLdapUserDNKey" runat="server" class="settingrow">
                                                <cy:SiteLabel ID="Sitelabel8" ForControl="ddLdapUserDNKey" CssClass="settinglabel"
                                                    ConfigKey="SiteSettingsLdapUserDNKey" runat="server"> </cy:SiteLabel>
                                                <asp:DropDownList ID="ddLdapUserDNKey" runat="server" EnableTheming="false" TabIndex="10"
                                                    CssClass="forminput">
                                                    <asp:ListItem Value="uid">uid (OpenLDAP)</asp:ListItem>
                                                    <asp:ListItem Value="CN">CN (Active Directory)</asp:ListItem>
                                                </asp:DropDownList>
                                                <portal:CHelpLink ID="CynHelpLink38" runat="server" HelpKey="sitesettingsldapuserdnkeyhelp" />
                                            </div>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="SiteLabel62" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                                </cy:SiteLabel>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <div id="tabOpenID" runat="server">
                                        <div id="divOpenID" runat="server" class="settingrow">
                                            <cy:SiteLabel ID="Sitelabel31" runat="server" ForControl="chkAllowOpenIDAuth" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsAllowOpenIDAuthenticationLabel" />
                                            <asp:CheckBox ID="chkAllowOpenIDAuth" runat="server" TabIndex="10" CssClass="forminput" />
                                            <portal:CHelpLink ID="CynHelpLink39" runat="server" HelpKey="sitesettingsopenidhelp" />
                                        </div>
                                        <div id="divOpenIDSelector" runat="server" visible="false" class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel27" ForControl="txtOpenIdSelectorCode" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsOpenIdSelectorLabel" runat="server"> </cy:SiteLabel>
                                            <asp:TextBox ID="txtOpenIdSelectorCode" Columns="55" runat="server" TabIndex="10"
                                                MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
                                            <portal:CHelpLink ID="CynHelpLink40" runat="server" HelpKey="sitesettingsopenidselectorhelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel54" ForControl="txtRpxNowApiKey" CssClass="settinglabel"
                                                ConfigKey="RpxNowApiKeyLabel" runat="server"> </cy:SiteLabel>
                                            <asp:TextBox ID="txtRpxNowApiKey" Columns="55" runat="server" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
                                            <portal:CHelpLink ID="CynHelpLink63" runat="server" HelpKey="rpxnow-apikey-help" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel55" ForControl="txtRpxNowApplicationName" CssClass="settinglabel"
                                                ConfigKey="RpxNowApplicationNameLabel" runat="server"> </cy:SiteLabel>
                                            <asp:TextBox ID="txtRpxNowApplicationName" Columns="55" runat="server" MaxLength="255"
                                                CssClass="forminput widetextbox"></asp:TextBox>
                                            <portal:CHelpLink ID="CynHelpLink64" runat="server" HelpKey="rpxnow-applicationname-help" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel61" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                            </cy:SiteLabel>
                                        </div>
                                        <div class="settingrow">
                                            <asp:HyperLink ID="lnkRpxAdmin" runat="server" Visible="false" />
                                            <asp:Button ID="btnSetupRpx" runat="server" />
                                        </div>
                                    </div>
                                    <div id="tabWindowsLiveID" runat="server">
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="Sitelabel32" runat="server" ForControl="chkAllowWindowsLiveAuth"
                                                CssClass="settinglabel" ConfigKey="SiteSettingsAllowWindowsLiveAuthLabel"> </cy:SiteLabel>
                                            <asp:CheckBox ID="chkAllowWindowsLiveAuth" runat="server" TabIndex="10" CssClass="forminput" />
                                            <portal:CHelpLink ID="CynHelpLink41" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                        </div>
                                        <div id="divLiveMessenger" runat="server" class="settingrow">
                                            <cy:SiteLabel ID="Sitelabel50" runat="server" ForControl="chkAllowWindowsLiveMessengerForMembers"
                                                CssClass="settinglabel" ConfigKey="AllowLiveMessengerOnProfilesLabel"> </cy:SiteLabel>
                                            <asp:CheckBox ID="chkAllowWindowsLiveMessengerForMembers" runat="server" TabIndex="10"
                                                CssClass="forminput" />
                                            <portal:CHelpLink ID="CynHelpLink42" runat="server" HelpKey="livemessenger-admin-help" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel33" runat="server" ForControl="txtWindowsLiveAppID" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsWindowsLiveAppIDLabel" />
                                            <asp:TextBox ID="txtWindowsLiveAppID" TabIndex="10" MaxLength="100" Columns="45"
                                                CssClass="forminput widetextbox" runat="server" />
                                            <portal:CHelpLink ID="CynHelpLink43" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel34" runat="server" ForControl="txtWindowsLiveKey" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsWindowsLiveKeyLabel" />
                                            <asp:TextBox ID="txtWindowsLiveKey" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                                CssClass="forminput widetextbox" />
                                            <portal:CHelpLink ID="CynHelpLink44" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel51" runat="server" ForControl="txtAppLogoForWindowsLive"
                                                CssClass="settinglabel" ConfigKey="WindowsLiveAppLogoLabel" />
                                            <asp:Label ID="lblSiteRoot" runat="server" />
                                            <asp:TextBox ID="txtAppLogoForWindowsLive" TabIndex="10" MaxLength="100" Columns="45"
                                                runat="server" CssClass="forminput widetextbox" />
                                            <portal:CHelpLink ID="CynHelpLink45" runat="server" HelpKey="windowslive-applogo-help" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel60" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                            </cy:SiteLabel>
                                        </div>
                                    </div>
                                    <div id="tabAntiSpam" runat="server">
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel6" runat="server" ForControl="ddCaptchaProviders" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsCaptchaProviderLabel"> </cy:SiteLabel>
                                            <asp:DropDownList ID="ddCaptchaProviders" DataTextField="name" DataValueField="name"
                                                EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="forminput">
                                            </asp:DropDownList>
                                            <portal:CHelpLink ID="CynHelpLink46" runat="server" HelpKey="sitesettingscaptchaproviderhelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtRecaptchPrivateKey" CssClass="settinglabel"
                                                ConfigKey="SiteSettingsSiteRecaptchaPrivateKeyLabel" />
                                            <asp:TextBox ID="txtRecaptchaPrivateKey" TabIndex="10" MaxLength="100" Columns="45"
                                                CssClass="forminput verywidetextbox" runat="server" />
                                            <portal:CHelpLink ID="CynHelpLink47" runat="server" HelpKey="sitesettingsrecaptchahelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel30" runat="server" ForControl="txtRecaptchaPublicKey"
                                                CssClass="settinglabel" ConfigKey="SiteSettingsSiteRecaptchaPublicKeyLabel" />
                                            <asp:TextBox ID="txtRecaptchaPublicKey" TabIndex="10" MaxLength="100" Columns="45"
                                                CssClass="forminput verywidetextbox" runat="server" />
                                            <portal:CHelpLink ID="CynHelpLink48" runat="server" HelpKey="sitesettingsrecaptchahelp" />
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel58" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                            </cy:SiteLabel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tabCommerce" runat="server">
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblDefaultCountry" runat="server" CssClass="settinglabel" ConfigKey="DefaultCountryStateLabel" />
                                <div>
                                    <asp:UpdatePanel ID="upCountryState" UpdateMode="Conditional" runat="server" EnableViewState="true">
                                        <ContentTemplate>
                                            <div>
                                                <asp:DropDownList ID="ddDefaultCountry" runat="server" AutoPostBack="true" EnableTheming="false"
                                                    DataValueField="Guid" DataTextField="Name" EnableViewState="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddDefaultGeoZone" runat="server" EnableTheming="false" DataValueField="Guid"
                                                    DataTextField="Name" EnableViewState="true">
                                                </asp:DropDownList>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel44" runat="server" ForControl="txtDefaultPageKeywords"
                                    CssClass="settinglabel" ConfigKey="CurrencyLabel" />
                                <portal:CurrencySetting ID="SiteCurrencySetting" runat="server" />
                            </div>
                            <div id="divCommerceRoles" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel43" runat="server" ForControl="txtDefaultPageKeywords"
                                    CssClass="settinglabel" ConfigKey="RolesThatCanViewCommerceReportsLabel" />
                                <asp:CheckBoxList ID="chkCommerceReportRoles" runat="server" SkinID="Roles" CssClass="forminput">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        
                        <div id="tabHosts" runat="server" visible="false">
                            <asp:UpdatePanel ID="upHosts" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlHostNames" runat="server" DefaultButton="btnAddHost">
                                        <asp:TextBox ID="txtHostName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
                                        <asp:Button ID="btnAddHost" runat="server"></asp:Button>
                                        <portal:CHelpLink ID="CynHelpLink53" runat="server" HelpKey="sitesettingshostnamehelp" />
                                        <br />
                                        <br />
                                        <portal:CLabel ID="lblHostMessage" runat="server" CssClass="txterror" EnableViewState="false" />
                                    </asp:Panel>
                                    <h3>
                                        <asp:Literal ID="litHostListHeader" runat="server" /></h3>
                                    <asp:Repeater ID="rptHosts" runat="server">
                                        <HeaderTemplate>
                                            <ul class="simplelist">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                                    CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HostID") %>'
                                                    AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>" runat="server"
                                                    ID="btnDeleteHost" />&nbsp;
                                                <%# DataBinder.Eval(Container.DataItem, "HostName") %>
                                            </li>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <li>
                                                <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                                    CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HostID") %>'
                                                    AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>" runat="server"
                                                    ID="btnDeleteHost" />&nbsp;
                                                <%# DataBinder.Eval(Container.DataItem, "HostName") %>
                                            </li>
                                            </ItemTemplate>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tabFolderNames" runat="server" visible="false">
                            <asp:UpdatePanel ID="upFolderNames" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlAddFolder" runat="server" DefaultButton="btnAddFolder">
                                        <asp:TextBox ID="txtFolderName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
                                        <asp:Button ID="btnAddFolder" runat="server"></asp:Button>
                                        <portal:CHelpLink ID="CynHelpLink54" runat="server" HelpKey="sitesettingsfoldernamehelp" />
                                        <br />
                                        <br />
                                        <portal:CLabel ID="lblFolderMessage" runat="server" CssClass="txterror" />
                                    </asp:Panel>
                                    <h3>
                                        <asp:Literal ID="litFolderNamesListHeading" runat="server" EnableViewState="false" /></h3>
                                    <asp:Repeater ID="rptFolderNames" runat="server">
                                        <HeaderTemplate>
                                            <ul class="simplelist">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                                    CommandName="delete" ToolTip="<%# Resources.Resource.SiteSettingsDeleteFolderMapping %>"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>"
                                                    runat="server" ID="btnDeleteFolder" />&nbsp;
                                                <%# DataBinder.Eval(Container.DataItem, "FolderName") %>
                                            </li>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <li>
                                                <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                                    CommandName="delete" ToolTip="<%# Resources.Resource.SiteSettingsDeleteFolderMapping %>"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>"
                                                    runat="server" ID="btnDeleteFolder" />&nbsp;
                                                <%# DataBinder.Eval(Container.DataItem, "FolderName") %>
                                            </li>
                                            </ItemTemplate>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tabSiteFeatures" runat="server" visible="false" class="minheightpanel">
                            <div style="height: 250px;">
                                <asp:UpdatePanel ID="upFeatures" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <portal:CHelpLink ID="CynHelpLink55" runat="server" HelpKey="sitesettingschildsitefeatureshelp" />
                                        <div class="floatpanel">
                                            <div>
                                                <h3>
                                                    <cy:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="SiteSettingsSiteAvailableFeaturesLabel"
                                                        UseLabelTag="false" />
                                                </h3>
                                            </div>
                                            <div>
                                                <asp:ListBox ID="lstAllFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple" />
                                            </div>
                                        </div>
                                        <div class="floatpanel">
                                            <div>
                                                <asp:Button Text="<-" runat="server" ID="btnRemoveFeature" />
                                                <asp:Button Text="->" runat="server" ID="btnAddFeature" />
                                            </div>
                                        </div>
                                        <div class="floatpanel">
                                            <div>
                                                <h3>
                                                    <cy:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="SiteSettingsSiteSelectedFeaturesLabel"
                                                        UseLabelTag="false" />
                                                </h3>
                                            </div>
                                            <div>
                                                <asp:ListBox ID="lstSelectedFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple" />
                                            </div>
                                            <div class="clearpanel">
                                                <asp:Label ID="lblFeatureMessage" runat="server" CssClass="txterror" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="tabWebParts" runat="server" visible="false" class="minheightpanel">
                            <asp:UpdatePanel ID="upWebParts" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <portal:CHelpLink ID="CynHelpLink56" runat="server" HelpKey="sitesettingschildsitefeatureshelp" />
                                    <div class="floatpanel">
                                        <div>
                                            <h3>
                                                <cy:SiteLabel ID="Sitelabel21" runat="server" ConfigKey="WebPartAdminAllWebParts"
                                                    UseLabelTag="false" />
                                            </h3>
                                        </div>
                                        <div>
                                            <asp:ListBox ID="lstAllWebParts" runat="Server" Width="150" />
                                        </div>
                                    </div>
                                    <div class="floatpanel">
                                        <div>
                                            <asp:Button Text="<-" runat="server" ID="btnRemoveWebPart" />
                                            <asp:Button Text="->" runat="server" ID="btnAddWebPart" />
                                        </div>
                                    </div>
                                    <div class="floatpanel">
                                        <div>
                                            <h3>
                                                <cy:SiteLabel ID="Sitelabel22" runat="server" ConfigKey="WebPartAdminSelectedWebParts"
                                                    UseLabelTag="false" />
                                            </h3>
                                        </div>
                                        <div>
                                            <asp:ListBox ID="lstSelectedWebParts" runat="Server" Width="150" />
                                        </div>
                                        <div>
                                            <asp:Label ID="lblWebPartMessage" runat="server" CssClass="txterror" EnableViewState="false" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tabApiKeys" runat="server">
                            <div id="divGAnalytics" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel25" runat="server" ForControl="txtGoogleAnayticsAccountCode"
                                    CssClass="settinglabel" ConfigKey="GoogleAnalyticsAccountCodeLabel" />
                                <asp:TextBox ID="txtGoogleAnayticsAccountCode" TabIndex="10" MaxLength="100" Columns="45"
                                    runat="server" CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink57" runat="server" HelpKey="googleanalyticsaccountcodehelp" />
                            </div>
                            <div id="divWoopra" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel46" runat="server" ForControl="chkEnableWoopra" CssClass="settinglabel"
                                    ConfigKey="EnableWoopraLabel" />
                                <asp:CheckBox ID="chkEnableWoopra" runat="server" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink58" runat="server" HelpKey="wooprahelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel10" runat="server" ForControl="txtGmapApiKey" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsGmapApiKeyLabel" />
                                <asp:TextBox ID="txtGmapApiKey" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                    CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink59" runat="server" HelpKey="sitesettingsgmapapikeyhelp" />
                            </div>
                            <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel80" runat="server" ForControl="ddCommentSystem" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsCommentSystem" />
                                <asp:DropDownList ID="ddCommentSystem" runat="server" CssClass="forminput">
                                    <asp:ListItem Value="intensedebate" Text="IntenseDebate" />
                                    <asp:ListItem Value="disqus" Text="Disqus" />
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink51" runat="server" HelpKey="comment-system-help" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel81" runat="server" ForControl="txtIntenseDebateAccountId" CssClass="settinglabel"
                                    ConfigKey="IntenseDebateAccountId" />
                                <asp:TextBox ID="txtIntenseDebateAccountId" TabIndex="10" MaxLength="255"  runat="server"
                                    CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink52" runat="server" HelpKey="intensedebate-accoutid-help" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel82" runat="server" ForControl="txtDisqusSiteShortName" CssClass="settinglabel"
                                    ConfigKey="DisqusSiteShortName" />
                                <asp:TextBox ID="txtDisqusSiteShortName" TabIndex="10" MaxLength="255"  runat="server"
                                    CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink78" runat="server" HelpKey="disqus-siteshortname-help" />
                            </div>
                            
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel23" runat="server" ForControl="txtAddThisUserId" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsAddThisAccountIdLabel" />
                                <asp:TextBox ID="txtAddThisUserId" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                    CssClass="forminput widetextbox" />
                                <portal:CHelpLink ID="CynHelpLink60" runat="server" HelpKey="sitesettingsaddthisuseridhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel64" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                </cy:SiteLabel>
                            </div>
                        </div>
                        <div id="tabMailSettings" runat="server" class="minheightpanel" visible="false">
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel28" runat="server" ForControl="txtSMTPUser" CssClass="settinglabel"
                                    ConfigKey="SMTPUser" />
                                <asp:TextBox ID="txtSMTPUser" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                    CssClass="forminput widetextbox" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel36" runat="server" ForControl="txtSMTPPassword" CssClass="settinglabel"
                                    ConfigKey="SMTPPassword" />
                                <asp:TextBox ID="txtSMTPPassword" TabIndex="10" TextMode="Password" MaxLength="100"
                                    Columns="45" runat="server" CssClass="forminput widetextbox" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel37" runat="server" ForControl="txtSMTPServer" CssClass="settinglabel"
                                    ConfigKey="SMTPServer" />
                                <asp:TextBox ID="txtSMTPServer" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                    CssClass="forminput widetextbox" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel38" runat="server" ForControl="txtSMTPPort" CssClass="settinglabel"
                                    ConfigKey="SMTPPort" />
                                <asp:TextBox ID="txtSMTPPort" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                    CssClass="forminput widetextbox" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel41" runat="server" ForControl="chkSMTPRequiresAuthentication"
                                    CssClass="settinglabel" ConfigKey="SMTPRequiresAuthentication" />
                                <asp:CheckBox ID="chkSMTPRequiresAuthentication" runat="server" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel40" runat="server" ForControl="chkSMTPUseSsl" CssClass="settinglabel"
                                    ConfigKey="SMTPUseSsl" />
                                <asp:CheckBox ID="chkSMTPUseSsl" runat="server" CssClass="forminput" />
                            </div>
                            <div class="settingrow" id="divSMTPEncoding" runat="server">
                                <cy:SiteLabel ID="SiteLabel39" runat="server" ForControl="txtSMTPPreferredEncoding"
                                    CssClass="settinglabel" ConfigKey="SMTPPreferredEncoding" />
                                <asp:TextBox ID="txtSMTPPreferredEncoding" TabIndex="10" MaxLength="100" Columns="45"
                                    runat="server" CssClass="forminput widetextbox" />
                            </div>
                            <div class="settingrow">
                                <portal:CHelpLink ID="CynHelpLink61" runat="server" HelpKey="smtphelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel65" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                </cy:SiteLabel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="sitesettings" />
                    <asp:RegularExpressionValidator ID="regexMaxInvalidPasswordAttempts" runat="server"
                        ControlToValidate="txtMaxInvalidPasswordAttempts" ValidationGroup="sitesettings"
                        Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="regexPasswordAttemptWindow" runat="server" ControlToValidate="txtPasswordAttemptWindowMinutes"
                        ValidationGroup="sitesettings" Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="regexMinPasswordLength" runat="server" ControlToValidate="txtMinimumPasswordLength"
                        ValidationGroup="sitesettings" Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="regexPasswordMinNonAlphaNumeric" runat="server"
                        ControlToValidate="txtMinRequiredNonAlphaNumericCharacters" ValidationGroup="sitesettings"
                        Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="regexWinLiveSecret" runat="server" ControlToValidate="txtWindowsLiveKey"
                        ValidationGroup="sitesettings" Display="None" ValidationExpression=".{16}.*"></asp:RegularExpressionValidator>
                    <portal:CLabel ID="lblErrorMessage" runat="server" CssClass="txterror" EnableViewState="false"></portal:CLabel>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <portal:CButton ID="btnSave" Text="Apply Changes" runat="server" />&nbsp;&nbsp;
                    <portal:CButton ID="btnDelete" runat="server" Visible="false" />&nbsp;&nbsp;
                </div>
            </fieldset>
        </div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
    runat="server" >
</asp:Content>

