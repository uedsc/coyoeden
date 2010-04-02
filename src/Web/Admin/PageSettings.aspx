<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="PageSettings.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.PageProperties" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlSettings" runat="server" CssClass="panelwrapper admin pagesettings">
        <div class="modulecontent">
            <fieldset class="pagesettings">
                <legend>
                    <cy:SiteLabel ID="lblPageNameLayout" runat="server" ConfigKey="PageSettingsLabel">
                    </cy:SiteLabel>
                    <asp:Label ID="lblPageName" runat="server"></asp:Label>
                </legend>
                <div class="breadcrumbs pageditlinks">
                    <asp:HyperLink ID="lnkEditContent" runat="server" EnableViewState="false"></asp:HyperLink>&nbsp;
                    <asp:HyperLink ID="lnkViewPage" runat="server" EnableViewState="false"></asp:HyperLink>&nbsp;
                    <asp:HyperLink ID="lnkPageTree" runat="server" />
                </div>
                <div class="pagetabs">
                    <div id="divtabs" class="yui-navset">
                        <ul class="yui-nav">
                            <li class="selected"><a href="#tabSettings"><em>
                                <asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                            <li><a href="#tabSecurity"><em>
                                <asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
                            <li><a href="#tabMetaSettings"><em>
                                <asp:Literal ID="litMetaSettingsTab" runat="server" /></em></a></li>
                            <li><a href="#tabSEO"><em>
                                <asp:Literal ID="litSEOTab" runat="server" /></em></a></li>
                        </ul>
                        <div class="yui-content">
                            <div id="tabSettings">
                                <div class="settingrow">
                                    <cy:SiteLabel ID="lblParentPage" runat="server" ForControl="ddPages" CssClass="settinglabel"
                                        ConfigKey="PageLayoutParentPageLabel"> </cy:SiteLabel>
                                    <asp:DropDownList ID="ddPages" runat="server" EnableTheming="false" DataTextField="PageName"
                                        DataValueField="PageID" CssClass="forminput">
                                    </asp:DropDownList>
                                    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="pagesettingsparentpagehelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="lblPageNameLabel" runat="server" ForControl="txtPageName" CssClass="settinglabel"
                                        ConfigKey="PageSettingsPageNameLabel"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtPageName" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                    <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="pagesettingspagenamehelp" />
                                    <asp:HiddenField ID="hdnPageName" runat="server" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel11" runat="server" ForControl="txtPageTitle" CssClass="settinglabel"
                                        ConfigKey="PageSettingsPageTitleOverrideLabel"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                    <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="pagesettingspagetitlehelp" />
                                </div>
                                <div id="divUseUrl" runat="server" class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel2" runat="server" ForControl="chkUseUrl" CssClass="settinglabel"
                                        ConfigKey="PageLayoutUseUrlLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkUseUrl" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="pagesettingsuseurlhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel3" runat="server" ForControl="txtUrl" CssClass="settinglabel"
                                        ConfigKey="PageLayoutUrlLabel"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                    <portal:CHelpLink ID="CynHelpLink5" runat="server" HelpKey="pagesettingsurlhelp" />
                                    <span id="spnUrlWarning" runat="server" style="font-weight: normal;" class="txterror">
                                    </span>
                                </div>
                                <div class="settingrow pageicons">
                                    <cy:SiteLabel ID="lblIcon" runat="server" ForControl="ddIcons" CssClass="settinglabel"
                                        ConfigKey="PageSettingsIconLabel"> </cy:SiteLabel>
                                    <asp:DropDownList ID="ddIcons" runat="server" EnableTheming="false" DataValueField="Name"
                                        DataTextField="Name" CssClass="forminput">
                                    </asp:DropDownList>
                                    <img id="imgIcon" alt="" src="" runat="server" />
                                    <portal:CHelpLink ID="CynHelpLink6" runat="server" HelpKey="pagesettingsiconhelp" />
                                </div>
                                <div id="divSkin" runat="server" class="settingrow">
                                    <cy:SiteLabel ID="lblSkin" runat="server" ForControl="ddSkins" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsSiteSkinLabel"> </cy:SiteLabel>
                                    <asp:DropDownList ID="ddSkins" runat="server" EnableTheming="false" DataValueField="Name"
                                        DataTextField="Name" CssClass="forminput">
                                    </asp:DropDownList>
                                    <portal:CHelpLink ID="CynHelpLink7" runat="server" HelpKey="pagesettingsskinhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel12" runat="server" ForControl="chkAllowBrowserCache" CssClass="settinglabel"
                                        ConfigKey="PageSettingsAllowBrowserCacheLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkAllowBrowserCache" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink8" runat="server" HelpKey="pagesettingsallowbrowsercachehelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel10" runat="server" ForControl="chkIncludeInMenu" CssClass="settinglabel"
                                        ConfigKey="PageSettingsIncludeInMenuLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkIncludeInMenu" runat="server" CssClass="forminput" Checked="true">
                                    </asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink9" runat="server" HelpKey="pagesettingsincludeinmenuhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel16" runat="server" ForControl="chkIncludeInSiteMap" CssClass="settinglabel"
                                        ConfigKey="PageSettingsIncludeInSiteMapLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkIncludeInSiteMap" runat="server" CssClass="forminput" Checked="true">
                                    </asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink10" runat="server" HelpKey="pagesettingsincludeinsitemaphelp" />
                                </div>
                                <div id="divIsPending" runat="server" class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel19" runat="server" ForControl="chkIsPending" CssClass="settinglabel"
                                        ConfigKey="PageSettingsIsPendingLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkIsPending" runat="server" CssClass="forminput" Checked="false">
                                    </asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink29" runat="server" HelpKey="pagesettingsisdrafthelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkShowBreadcrumbs" CssClass="settinglabel"
                                        ConfigKey="PageLayoutShowBreadcrumbsLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkShowBreadcrumbs" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="pagesettingsbreadcrumbshelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel7" runat="server" ForControl="chkShowChildPageBreadcrumbs"
                                        CssClass="settinglabel" ConfigKey="PageLayoutShowChildBreadcrumbsLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkShowChildPageBreadcrumbs" runat="server" CssClass="forminput">
                                    </asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink12" runat="server" HelpKey="pagesettingschildpagebreadcrumbshelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel17" runat="server" ForControl="chkShowHomeCrumb" CssClass="settinglabel"
                                        ConfigKey="ShowHomePageCrumb"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkShowHomeCrumb" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink13" runat="server" HelpKey="pagesettingshomecrumbhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel4" runat="server" ForControl="chkNewWindow" CssClass="settinglabel"
                                        ConfigKey="PageLayoutOpenInNewWindowLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkNewWindow" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink14" runat="server" HelpKey="pagesettingsnewwindowhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel5" runat="server" ForControl="chkShowChildMenu" CssClass="settinglabel"
                                        ConfigKey="PageLayoutShowChildMenuLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkShowChildMenu" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink15" runat="server" HelpKey="pagesettingschildpagemenuhelp" />
                                </div>
                                <div id="divHideMenu" runat="server" class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel9" runat="server" ForControl="chkHideMainMenu" CssClass="settinglabel"
                                        ConfigKey="PageLayoutHideMenuLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkHideMainMenu" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink16" runat="server" HelpKey="pagesettingshidemenuhelp" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel13" runat="server" ForControl="chkHideAfterLogin" CssClass="settinglabel"
                                        ConfigKey="PageSettingstHideAfterLoginLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkHideAfterLogin" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink17" runat="server" HelpKey="pagesettingshideafterloginhelp" />
                                </div>
                                <asp:Panel ID="pnlComments" runat="server" Visible="false" CssClass="settingrow">
                                    <cy:SiteLabel ID="Sitelabel24" runat="server" ForControl="chkEnableComments" CssClass="settinglabel"
                                        ConfigKey="PageSettingsEnableComments"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkEnableComments" runat="server" CssClass="forminput"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink32" runat="server" HelpKey="pagesettings-enablecomments-help" />
                               </asp:Panel>
                                <div class="settingrow">
                                    &nbsp;</div>
                            </div>
                            <div id="tabSecurity">
                                <div id="tabSSL" runat="server" class="settingrow">
                                    <cy:SiteLabel ID="lblRequireSSL" runat="server" ForControl="chkRequireSSL" CssClass="settinglabel"
                                        ConfigKey="PageLayoutRequireSSLLabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkRequireSSL" runat="server"></asp:CheckBox>
                                    <portal:CHelpLink ID="CynHelpLink18" runat="server" HelpKey="pagesettingsrequiresslhelp" />
                                </div>
                                <div class="C-accordion">
                                    <h3 id="h3ViewRoles" runat="server">
                                        <a href="#">
                                            <cy:SiteLabel ID="lblAuthorizedRoles" runat="server" ConfigKey="PageLayoutViewRolesLabel"
                                                UseLabelTag="false" />
                                        </a>
                                    </h3>
                                    <div id="divViewRoles" runat="server">
                                        <p>
                                            <asp:CheckBoxList ID="chkListAuthRoles" runat="server" CssClass="forminput">
                                            </asp:CheckBoxList>
                                            <portal:CHelpLink ID="CynHelpLink19" runat="server" HelpKey="pagesettingsviewroleshelp" />
                                        </p>
                                    </div>
                                    <h3 id="h3EditRoles" runat="server">
                                        <a href="#">
                                            <cy:SiteLabel ID="SiteLabel21" runat="server" ConfigKey="PageLayoutEditRolesLabel"
                                                UseLabelTag="false" />
                                        </a>
                                    </h3>
                                    <div id="divEditRoles" runat="server">
                                        <p>
                                            <asp:CheckBoxList ID="chkListEditRoles" runat="server" CssClass="forminput">
                                            </asp:CheckBoxList>
                                            <portal:CHelpLink ID="CynHelpLink20" runat="server" HelpKey="pagesettingseditroleshelp" />
                                        </p>
                                    </div>
                                    <h3 id="h3DraftRoles" runat="server">
                                        <a href="#">
                                            <cy:SiteLabel ID="SiteLabel6" runat="server" ConfigKey="PageLayoutDraftEditRolesLabel"
                                                UseLabelTag="false" />
                                        </a>
                                    </h3>
                                    <div id="divDraftRoles" runat="server">
                                        <p>
                                            <asp:CheckBoxList ID="chkDraftEditRoles" runat="server">
                                            </asp:CheckBoxList>
                                            <portal:CHelpLink ID="CynHelpLink28" runat="server" HelpKey="pagesettingsdrafteditroleshelp" />
                                        </p>
                                    </div>
                                    <h3 id="h3ChildEditRoles" runat="server">
                                        <a href="#">
                                            <cy:SiteLabel ID="SiteLabel18" runat="server" ConfigKey="PageLayoutCreateChildPageRolesLabel"
                                                UseLabelTag="false" />
                                        </a>
                                    </h3>
                                    <div id="divChildEditRoles" runat="server">
                                        <p>
                                            <asp:CheckBoxList ID="chkListCreateChildPageRoles" runat="server" SkinID="Roles"
                                                CssClass="forminput">
                                            </asp:CheckBoxList>
                                            <portal:CHelpLink ID="CynHelpLink21" runat="server" HelpKey="pagesettingschildpageroleshelp" />
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div id="tabMetaSettings">
                                
                                <asp:Panel ID="pnlMetaSettings" runat="server" SkinID="plain">
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="lblKeywords" runat="server" ForControl="txtPageKeywords" CssClass="settinglabel"
                                            ConfigKey="PageLayoutMetaKeyWordsLabel"> </cy:SiteLabel>
                                        <asp:TextBox ID="txtPageKeywords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                        <portal:CHelpLink ID="CynHelpLink22" runat="server" HelpKey="pagesettingskeywordshelp" />
                                    </div>
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="lblDescription" runat="server" ForControl="txtPageDescription"
                                            CssClass="settinglabel" ConfigKey="PageLayoutMetaDescriptionLabel"> </cy:SiteLabel>
                                        <asp:TextBox ID="txtPageDescription" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                        <portal:CHelpLink ID="CynHelpLink23" runat="server" HelpKey="pagesettingsmetadescriptionhelp" />
                                    </div>
                                    <div id="divPageEncoding" runat="server" visible="false" class="settingrow">
                                        <cy:SiteLabel ID="lblEncoding" runat="server" ForControl="txtPageEncoding" CssClass="settinglabel"
                                            ConfigKey="PageLayoutMetaEncodingLabel"> </cy:SiteLabel>
                                        <asp:TextBox ID="txtPageEncoding" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                        <portal:CHelpLink ID="CynHelpLink24" runat="server" HelpKey="pagesettingsmetaencodinghelp" />
                                    </div>
                                    <asp:Panel ID="pnlMeta" runat="server">
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="PageLayoutMetaAdditionalLabel"> </cy:SiteLabel>
                                            <portal:CHelpLink ID="CynHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
                                        </div>
                                        <div class="settingrow">
                                            <asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cy:CGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                        DataKeyNames="Guid" EnableTheming="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.Resource.ContentMetaGridEditButton %>' />
                                                                    <asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                        CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveUpButton %>'
                                                                        Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                                    <asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                        CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveDownButton %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Rel") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblNameMetaRel" runat="server" ForControl="txtRel" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaRelLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
                                                                        <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
                                                                            ErrorMessage='<%# Resources.Resource.ContentMetaLinkRelRequired %>' ValidationGroup="metalink" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
                                                                            ErrorMessage='<%# Resources.Resource.ContentMetaLinkHrefRequired %>' ValidationGroup="metalink" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetHrefLangLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridUpdateButton %>'
                                                                            CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
                                                                        <asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridDeleteButton %>'
                                                                            CommandName="Delete" CausesValidation="false" />
                                                                        <asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridCancelButton %>'
                                                                            CommandName="Cancel" CausesValidation="false" />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Href") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </cy:CGridView>
                                                    <div class="settingrow">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddMetaLink" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
                                                                        <ProgressTemplate>
                                                                            <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>' alt=' ' />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="settingrow">
                                            <asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cy:CGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                        DataKeyNames="Guid" EnableTheming="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.Resource.ContentMetaGridEditButton %>' />
                                                                    <asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                        CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveUpButton %>'
                                                                        Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                                    <asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                        CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveDownButton %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("Name") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaNameLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
                                                                        <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
                                                                            ErrorMessage='<%# Resources.Resource.ContentMetaNameRequired %>' ValidationGroup="meta" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaMetaContentLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
                                                                            Text='<%# Eval("MetaContent") %>' />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                                            ErrorMessage='<%# Resources.Resource.ContentMetaContentRequired %>' ValidationGroup="meta" />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaSchemeLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaLangCodeLabel" ResourceFile="Resource" />
                                                                        <asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <cy:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
                                                                            ConfigKey="ContentMetaDirLabel" ResourceFile="Resource" />
                                                                        <asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
                                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                            <asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
                                                                            <asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="settingrow">
                                                                        <asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridUpdateButton %>'
                                                                            CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
                                                                        <asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridDeleteButton %>'
                                                                            CommandName="Delete" CausesValidation="false" />
                                                                        <asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridCancelButton %>'
                                                                            CommandName="Cancel" CausesValidation="false" />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Eval("MetaContent") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </cy:CGridView>
                                                    <div class="settingrow">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddMeta" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
                                                                        <ProgressTemplate>
                                                                            <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>' alt=' ' />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="SiteLabel20" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                    </div>
                                </asp:Panel>
                            </div>
                            <div id="tabSEO">
                                <asp:Panel ID="pnlSearchEngineOptimization" runat="server" SkinID="plain">
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="SiteLabel14" runat="server" ForControl="ddChangeFrequency" CssClass="settinglabel"
                                            ConfigKey="PageSettingsChangeFrequencyLabel"> </cy:SiteLabel>
                                        <asp:DropDownList ID="ddChangeFrequency" EnableTheming="false" runat="server" CssClass="forminput">
                                        </asp:DropDownList>
                                        <portal:CHelpLink ID="CynHelpLink26" runat="server" HelpKey="pagesettingsseochangefequencyhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="SiteLabel15" runat="server" ForControl="ddSiteMapPriority" CssClass="settinglabel"
                                            ConfigKey="PageSettingsPriorityLabel"> </cy:SiteLabel>
                                        <asp:DropDownList EnableTheming="false" ID="ddSiteMapPriority" runat="server" CssClass="forminput">
                                            <asp:ListItem Text="0.0" Value="0.0" />
                                            <asp:ListItem Text="0.1" Value="0.1" />
                                            <asp:ListItem Text="0.2" Value="0.2" />
                                            <asp:ListItem Text="0.3" Value="0.3" />
                                            <asp:ListItem Text="0.4" Value="0.4" />
                                            <asp:ListItem Text="0.5" Value="0.5" Selected="true" />
                                            <asp:ListItem Text="0.6" Value="0.6" />
                                            <asp:ListItem Text="0.7" Value="0.7" />
                                            <asp:ListItem Text="0.8" Value="0.8" />
                                            <asp:ListItem Text="0.9" Value="0.9" />
                                            <asp:ListItem Text="1.0" Value="1.0" />
                                        </asp:DropDownList>
                                        <portal:CHelpLink ID="CynHelpLink27" runat="server" HelpKey="pagesettingssitemappriorityhelp" />
                                    </div>
                                </asp:Panel>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel22" runat="server" ForControl="chkIncludeInSearchEngineSiteMap"
                                        CssClass="settinglabel" ConfigKey="PageSettingsIncludeInSearchengineSiteMap" />
                                    <asp:CheckBox ID="chkIncludeInSearchEngineSiteMap" runat="server" Checked="true"
                                        CssClass="forminput" />
                                    <portal:CHelpLink ID="CynHelpLink30" runat="server" HelpKey="pagesettings-IncludeInSearchEngineSiteMap-help" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel23" runat="server" CssClass="settinglabel" ConfigKey="PageSettingsCanonicalOverride" />
                                    <asp:TextBox ID="txtCannonicalOverride" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                    <portal:CHelpLink ID="CynHelpLink31" runat="server" HelpKey="pagesettings-CannonicalOverride-help" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="pagesettings" />
                    <asp:RequiredFieldValidator ID="reqPageName" runat="server" Display="None" ControlToValidate="txtPageName"
                        ValidationGroup="pagesettings" />
                    <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtUrl"
                        ValidationExpression="((http\://|https\://|~/){1}(\S+){0,1})" Display="None" ValidationGroup="pagesettings" />
                    <portal:CLabel ID="lblError" runat="server" CssClass="txterror" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <portal:CButton ID="applyBtn" runat="server" Text="Apply Changes" />
                    <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />
                </div>
            </fieldset>
        </div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
