<%@ Page ValidateRequest="false" Language="c#" CodeBehind="EditPost.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.BlogUI.BlogEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlBlog" runat="server" DefaultButton="btnUpdate" CssClass="panelwrapper editpage blogedit">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <cy:SiteLabel ID="lblBlogEntry" runat="server" ConfigKey="BlogEditEntryLabel" ResourceFile="BlogResources"
                        UseLabelTag="false"> </cy:SiteLabel>
                </legend>
                <cy:YUIPanel ID="pnlEntry" runat="server">
                    <div id="divtabs" class="yui-navset">
                        <ul class="yui-nav">
                            <li class="selected"><a href="#tabContent"><em>
                                <asp:Literal ID="litContentTab" runat="server" /></em></a></li>
                            <li id="liExcerpt" runat="server"><a id="lnkExcerpt" runat="server" href="#tabExcerpt">
                                <em>
                                    <asp:Literal ID="litExcerptTab" runat="server" /></em></a></li>
                            <li><a href="#tabMeta"><em>
                                <asp:Literal ID="litMetaTab" runat="server" /></em></a></li>
                        </ul>
                        <div class="yui-content">
                            <div id="tabContent">
                                <div class="settingrow">
                                    <cy:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel"
                                        ConfigKey="BlogEditTitleLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                                    </asp:TextBox>
                                </div>
                                <div class="settingrow">
                                    <cye:EditorControl ID="edContent" runat="server">
                                    </cye:EditorControl>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel5" runat="server" ForControl="txtItemUrl" CssClass="settinglabel"
                                        ConfigKey="BlogEditItemUrlLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtItemUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                                    </asp:TextBox>
                                    <span id="spnUrlWarning" runat="server" style="font-weight: normal;" class="txterror">
                                    </span>
                                    <asp:HiddenField ID="hdnTitle" runat="server" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtLocation" CssClass="settinglabel"
                                        ConfigKey="BlogEditLocationLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtLocation" runat="server" MaxLength="100" CssClass="forminput widetextbox">
                                    </asp:TextBox>
                                </div>
                                <asp:Panel ID="pnlCategories" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <div class="settingrow">
                                                <cy:SiteLabel ID="lblCat" runat="server" ForControl="txtCategory" CssClass="settinglabel"
                                                    ConfigKey="BlogEditCategoryLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                                <asp:TextBox ID="txtCategory" runat="server" CssClass="widetextbox forminput"></asp:TextBox>
                                                <portal:CButton ID="btnAddCategory" runat="server"  CssClass="forminput" />
                                            </div>
                                            <div class="settingrow blogeditcategories">
                                                <asp:CheckBoxList ID="chkCategories" runat="server" EnableTheming="false" SkinID="plain"
                                                    RepeatColumns="5" RepeatDirection="Horizontal">
                                                </asp:CheckBoxList>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:HyperLink ID="lnkEditCategories" runat="server">
                                    </asp:HyperLink>
                                    <br />
                                </asp:Panel>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkIncludeInFeed" ConfigKey="BlogEditIncludeInFeedLabel"
                                        ResourceFile="BlogResources" CssClass="settinglabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkIncludeInFeed" runat="server" CssClass="forminput"></asp:CheckBox>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel13" runat="server" ForControl="chkIsPublished" ConfigKey="IsPublishedLabel"
                                        ResourceFile="BlogResources" CssClass="settinglabel"> </cy:SiteLabel>
                                    <asp:CheckBox ID="chkIsPublished" runat="server" CssClass="forminput" Checked="true">
                                    </asp:CheckBox>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="lblStartDate" runat="server" ForControl="dpBeginDate" ConfigKey="BlogEditStartDateLabel"
                                        ResourceFile="BlogResources" CssClass="settinglabel"> </cy:SiteLabel>
                                    <cy:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" CssClass="forminput">
                                    </cy:DatePickerControl>
                                    <cy:SiteLabel ID="SiteLabel3" runat="server" ResourceFile="BlogResources" ConfigKey="BlogDraftInstructions"
                                        UseLabelTag="false" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="Sitelabel2" runat="server" ForControl="ddCommentAllowedForDays"
                                        ConfigKey="BlogEditAllowedCommentsForDaysPrefix" ResourceFile="BlogResources"
                                        CssClass="settinglabel"> </cy:SiteLabel>
                                    <asp:DropDownList ID="ddCommentAllowedForDays" EnableTheming="false" runat="server"
                                        CssClass="forminput">
                                        <asp:ListItem Value="-1" Text="<%$ Resources:BlogResources, BlogCommentsNotAllowed %>" />
                                        <asp:ListItem Value="0" Text="<%$ Resources:BlogResources, BlogCommentsUnlimited %>" />
                                        <asp:ListItem Value="1" Text="1" />
                                        <asp:ListItem Value="7" Text="7" />
                                        <asp:ListItem Value="15" Text="15" />
                                        <asp:ListItem Value="30" Text="30" />
                                        <asp:ListItem Value="45" Text="45" />
                                        <asp:ListItem Value="60" Text="60" />
                                        <asp:ListItem Value="90" Text="90" Selected="True" />
                                        <asp:ListItem Value="120" Text="120" />
                                    </asp:DropDownList>
                                    &nbsp;<asp:Literal ID="litDays" runat="server" />
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                    <div class="forminput">
                                        <portal:CButton ID="btnUpdate" runat="server" ValidationGroup="blog" />
                                        <portal:CButton ID="btnSaveAndPreview" runat="server" ValidationGroup="blog" Visible="false" />&nbsp;
                                        <portal:CButton ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
                                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                                    </div>
                                    <br />
                                    <portal:CLabel ID="lblError" runat="server" CssClass="txterror" />
                                    <asp:HiddenField ID="hdnHxToRestore" runat="server" />
                                    <asp:ImageButton ID="btnRestoreFromGreyBox" runat="server" />
                                </div>
                                <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                                    <div class="settingrow">
                                        <cy:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="VersionHistory"
                                            ResourceFile="BlogResources"> </cy:SiteLabel>
                                    </div>
                                    <div class="settingrow">
                                        <asp:UpdatePanel ID="updHx" UpdateMode="Conditional" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="grdHistory" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <cy:CGridView ID="grdHistory" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                    DataKeyNames="Guid" EnableTheming="false">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("CreatedUtc"), timeOffset)%>
                                                                <br />
                                                                <%# Eval("UserName") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("HistoryUtc"), timeOffset)%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <portal:GreyBoxHyperlink ID="gb1" runat="server" ClientClick="return GB_showFullScreen(this.title, this.href)"
                                                                    NavigateUrl='<%# SiteRoot + "/Blog/BlogCompare.aspx?pageid=" + pageId + "&mid=" + moduleId + "&ItemID=" + itemId + "&h=" + Eval("Guid") %>'
                                                                    Text='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>' ToolTip='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>'
                                                                    DialogCloseText='<%# Resources.BlogResources.DialogCloseLink %>' />
                                                                <asp:Button ID="btnRestoreToEditor" runat="server" Text='<%# Resources.BlogResources.RestoreToEditorButton %>'
                                                                    CommandName="RestoreToEditor" CommandArgument='<%# Eval("Guid") %>' />
                                                                <asp:Button ID="btnDelete" runat="server" CommandName="DeleteHistory" CommandArgument='<%# Eval("Guid") %>'
                                                                    Visible='<%# isAdmin %>' Text='<%# Resources.BlogResources.DeleteHistoryButton %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </cy:CGridView>
                                                <div class="modulepager">
                                                    <portal:CCutePager ID="pgrHistory" runat="server" />
                                                </div>
                                                <div id="divHistoryDelete" runat="server" class="settingrow">
                                                    <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                                    <portal:CButton ID="btnDeleteHistory" runat="server" Text="" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:Panel>
                                <div class="settingrow">
                                    &nbsp;</div>
                            </div>
                            <div id="tabExcerpt" runat="server">
                                <div class="settingrow">
                                    <cye:EditorControl ID="edExcerpt" runat="server">
                                    </cye:EditorControl>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                    <div class="forminput">
                                        <portal:CButton ID="btnUpdate2" runat="server" ValidationGroup="blog" />&nbsp;
                                        <portal:CButton ID="btnDelete2" runat="server" CausesValidation="false" />
                                        <asp:HyperLink ID="lnkCancel2" runat="server" CssClass="cancellink" />&nbsp;
                                    </div>
                                </div>
                                <div class="settingrow">
                                    &nbsp;</div>
                            </div>
                            <div id="tabMeta">
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel6" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel"
                                        ConfigKey="MetaDescriptionLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                                    </asp:TextBox>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel"
                                        ConfigKey="MetaKeywordsLabel" ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                                    </asp:TextBox>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="SiteLabel12" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                    <div class="forminput">
                                        <portal:CButton ID="btnUpdate3" runat="server" ValidationGroup="blog" />&nbsp;
                                        <portal:CButton ID="btnDelete3" runat="server" CausesValidation="False" />
                                        <asp:HyperLink ID="lnkCancel3" runat="server" CssClass="cancellink" />&nbsp;
                                    </div>
                                </div>
                                <div class="settingrow">
                                    <cy:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="MetaAdditionalLabel"
                                        ResourceFile="BlogResources"> </cy:SiteLabel>
                                    <portal:CHelpLink ID="CynHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
                                </div>
                                <asp:Panel ID="pnlMetaData" runat="server" CssClass="settingrow">
                                    <asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <cy:CGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                DataKeyNames="Guid" EnableTheming="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
                                                            <asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
                                                                Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                            <asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>' />
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
                                                                    ConfigKey="ContentMetaRelLabel" ResourceFile="BlogResources" />
                                                                <asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
                                                                <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
                                                                    ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkRelRequired %>' ValidationGroup="metalink" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <cy:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="BlogResources" />
                                                                <asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
                                                                    ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkHrefRequired %>' ValidationGroup="metalink" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetHrefLangLabel" ResourceFile="BlogResources" />
                                                                <asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
                                                            </div>
                                                            <div class="settingrow">
                                                                <asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
                                                                    CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
                                                                <asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>'
                                                                    CommandName="Delete" CausesValidation="false" />
                                                                <asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>'
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
                                                            <portal:CButton ID="btnAddMetaLink" runat="server" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
                                                                <ProgressTemplate>
                                                                    <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>'
                                                                        alt=' ' />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="settingrow">
                                        <asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <cy:CGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                    DataKeyNames="Guid" EnableTheming="false">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
                                                                <asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                    CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
                                                                    Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                                <asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                    CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>' />
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
                                                                        ConfigKey="ContentMetaNameLabel" ResourceFile="BlogResources" />
                                                                    <asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
                                                                    <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
                                                                        ErrorMessage='<%# Resources.BlogResources.ContentMetaNameRequired %>' ValidationGroup="meta" />
                                                                </div>
                                                                <div class="settingrow">
                                                                    <cy:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
                                                                        ConfigKey="ContentMetaMetaContentLabel" ResourceFile="BlogResources" />
                                                                    <asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
                                                                        Text='<%# Eval("MetaContent") %>' />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                                        ErrorMessage='<%# Resources.BlogResources.ContentMetaContentRequired %>' ValidationGroup="meta" />
                                                                </div>
                                                                <div class="settingrow">
                                                                    <cy:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                        ConfigKey="ContentMetaSchemeLabel" ResourceFile="BlogResources" />
                                                                    <asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
                                                                </div>
                                                                <div class="settingrow">
                                                                    <cy:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
                                                                        ConfigKey="ContentMetaLangCodeLabel" ResourceFile="BlogResources" />
                                                                    <asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
                                                                </div>
                                                                <div class="settingrow">
                                                                    <cy:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
                                                                        ConfigKey="ContentMetaDirLabel" ResourceFile="BlogResources" />
                                                                    <asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
                                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
                                                                        <asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="settingrow">
                                                                    <asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
                                                                        CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
                                                                    <asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>'
                                                                        CommandName="Delete" CausesValidation="false" />
                                                                    <asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>'
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
                                                                <portal:CButton ID="btnAddMeta" runat="server" />&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
                                                                    <ProgressTemplate>
                                                                        <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>'
                                                                            alt=' ' />
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                            </cy:SiteLabel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </cy:YUIPanel>
                <div class="blogeditor">
                    <div class="settingrow">
                        <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle"
                            Display="None" CssClass="txterror" ValidationGroup="blog">
                        </asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="dpBeginDate"
                            Display="None" CssClass="txterror" ValidationGroup="blog">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtItemUrl"
                            ValidationExpression="((~/){1}\S+)" Display="None" ValidationGroup="blog" />
                        <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror" ValidationGroup="blog">
                        </asp:ValidationSummary>
                    </div>
                </div>
            </fieldset>
        </div>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
