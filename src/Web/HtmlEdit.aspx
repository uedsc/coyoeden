<%@ Page Language="c#" CodeBehind="HtmlEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.ContentUI.EditHtml" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper editpage htmlmodule yui-skin-sam">
        <asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
            <fieldset class="htmledit">
                <legend>
                    <cy:SiteLabel ID="lblHtmlSettings" runat="server" ConfigKey="EditHtmlSettingsLabel"
                        UseLabelTag="false"> </cy:SiteLabel>
                    <asp:Literal ID="litModuleTitle" runat="server" />
                </legend>
                <cye:EditorControl ID="edContent" runat="server">
                </cye:EditorControl>
                <div class="settingrow">
                </div>
                <div class="settingrow">
                    <portal:CButton ID="btnUpdateDraft" runat="server" Text="" Visible="false" />&nbsp;
                    <portal:CButton ID="btnUpdate" runat="server" Text="" />&nbsp;
                    <portal:CButton ID="btnDelete" runat="server" Text="" CausesValidation="False" />&nbsp;
                    <portal:GreyBoxHyperlink ID="lnkCompareDraftToLive" runat="server" Visible="false"
                        ClientClick="return GB_showFullScreen(this.title, this.href)" DialogCloseText='<%$ Resources:Resource, DialogCloseLink %>' />
                    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                    <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="htmlcontentedithelp" />
                    <asp:HiddenField ID="hdnHxToRestore" runat="server" />
                    <asp:ImageButton ID="btnRestoreFromGreyBox" runat="server" />
                </div>
                <asp:Panel ID="pnlWorkflowStatus" runat="server" Visible="false">
                    <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="ContentStatusLabel">
                        </cy:SiteLabel>
                        <asp:Literal ID="litWorkflowStatus" runat="server"></asp:Literal>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblRecentActionBy" runat="server" CssClass="settinglabel" ConfigKey="RejectedBy">
                        </cy:SiteLabel>
                        <asp:Literal ID="litRecentActionBy" runat="server"></asp:Literal>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblRecentActionOn" runat="server" CssClass="settinglabel" ConfigKey="RejectedOn">
                        </cy:SiteLabel>
                        <asp:Literal ID="litRecentActionOn" runat="server"></asp:Literal>
                    </div>
                    <div id="divRejection" runat="server">
                        <div class="settingrow">
                            <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="ContentLastEditBy">
                            </cy:SiteLabel>
                            <asp:Literal ID="litCreatedBy" runat="server"></asp:Literal>
                        </div>
                        <div class="settingrow">
                            <cy:SiteLabel ID="lblRejectionReason" runat="server" CssClass="settinglabel" ConfigKey="RejectionCommentLabel">
                            </cy:SiteLabel>
                            <asp:Literal ID="ltlRejectionReason" runat="server"></asp:Literal>
                        </div>
                    </div>
                </asp:Panel>
                <asp:UpdatePanel ID="updHx" UpdateMode="Conditional" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdHistory" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                            <h2 class="heading versionheading">
                                <asp:Literal ID="litVersionHistory" runat="server" /></h2>
                            <cy:CGridView ID="grdHistory" runat="server" CssClass="" AutoGenerateColumns="false"
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
                                                NavigateUrl='<%# SiteRoot + "/HtmlCompare.aspx?pageid=" + pageId + "&mid=" + moduleId + "&h=" + Eval("Guid") %>'
                                                Text='<%$ Resources:Resource, HtmlCompareHistoryToCurrentLink %>' ToolTip='<%$ Resources:Resource, HtmlCompareHistoryToCurrentLink %>'
                                                DialogCloseText='<%$ Resources:Resource, DialogCloseLink %>' />
                                            <asp:Button ID="btnRestoreToEditor" runat="server" Text='<%$ Resources:Resource, RestoreToEditorButton %>'
                                                CommandName="RestoreToEditor" CommandArgument='<%# Eval("Guid") %>' />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="DeleteHistory" CommandArgument='<%# Eval("Guid") %>'
                                                Visible='<%# isAdmin %>' Text='<%$ Resources:Resource, DeleteHistoryButton %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </cy:CGridView>
                            <div class="modulepager">
                                <portal:CCutePager ID="pgrHistory" runat="server" />
                            </div>
                            <div id="divHistoryDelete" runat="server" class="settingrow">
                                <portal:CButton ID="btnDeleteHistory" runat="server" Text="" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server">
</asp:Content>
