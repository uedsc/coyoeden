<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="ManageUsers.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.AdminUI.ManageUsers" %>
<%@ Register Src="~/Controls/UserCommerceHistory.ascx" TagPrefix="portal" TagName="PurchaseHistory" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlUser" runat="server" DefaultButton="btnUpdate" CssClass="panelwrapper admin manageusers">
        <div class="modulecontent">
            <fieldset>
                <legend><span id="spnTitle" runat="server"></span></legend>
                <br />
                <div id="divtabs" class="yui-navset">
                    <ul class="yui-nav">
                        <li class="selected"><a href="#tabSecurity"><em>
                            <asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
                        <li id="liProfile" runat="server"><a href="#tabProfile" id="lnkProfile" runat="server"><em>
                            <asp:Literal ID="litProfileTab" runat="server" /></em></a></li>
                        <li id="liOrderHistory" runat="server"><a href="#tabOrderHistory" id="lnkOrderHistory" runat="server">
                            <em><asp:Literal ID="litOrderHistoryTab" runat="server" /></em></a></li>
                        <li id="liNewsletters" runat="server" visible="false"><a href="#tabNewsletter" id="lnkNewsletter"
                            runat="server"><em>
                                <asp:Literal ID="litNewsletterTab" runat="server" /></em></a></li>
                        <li id="liRoles" runat="server"><a href="#tabRoles" id="lnkRoles" runat="server"><em>
                            <asp:Literal ID="litRolesTab" runat="server" /></em></a></li>
                        <li id="liActivity" runat="server"><a href="#tabActivity" id="lnkActivity" runat="server"><em>
                            <asp:Literal ID="litActivityTab" runat="server" /></em></a></li>
                        <li id="liLocation" runat="server"><a href="#tabLocation" id="lnkLocation" runat="server"><em>
                            <asp:Literal ID="litLocationTab" runat="server" /></em></a></li>
                    </ul>
                    <div class="yui-content">
                        <div id="tabSecurity">
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblUserName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                    ConfigKey="ManageUsersUserNameLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtName" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="userfullnamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SitelabelLoginName" runat="server" ForControl="txtLoginName" CssClass="settinglabel"
                                    ConfigKey="ManageUsersLoginNameLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtLoginName" runat="server" TabIndex="10" Columns="45" MaxLength="50"
                                    CssClass="forminput mediumtextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="userloginnamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel"
                                    ConfigKey="ManageUsersEmailLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtEmail" runat="server" Columns="45" TabIndex="10" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="useremailhelp" />
                            </div>
                            <div id="divOpenID" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel12" runat="server" ForControl="txtOpenIDURI" CssClass="settinglabel"
                                    ConfigKey="ManageUsersOpenIDURILabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtOpenIDURI" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="useropenidhelp" />
                            </div>
                            <div id="divWindowsLiveID" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel13" runat="server" ForControl="txtWindowsLiveID" CssClass="settinglabel"
                                    ConfigKey="ManageUsersWindowsLiveIDLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtWindowsLiveID" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink5" runat="server" HelpKey="manageuserwindowsliveidhelp" />
                            </div>
                            <div id="divLiveMessenger" runat="server">
                            <div  class="settingrow">
                                <cy:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtLiveMessengerCID" CssClass="settinglabel"
                                    ConfigKey="LiveMessengerCIDLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtLiveMessengerCID" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink6" runat="server" HelpKey="livemessenger-cid-help" />
                            </div>
                            <div id="div1" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel15" runat="server" ForControl="chkEnableLiveMessengerOnProfile" CssClass="settinglabel"
                                    ConfigKey="EnableLiveMessengerLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkEnableLiveMessengerOnProfile" runat="server" CssClass="forminput" />
                                <portal:CHelpLink ID="CynHelpLink7" runat="server" HelpKey="livemessenger-admin-help" />
                            </div>
                            
                            </div>
                            <div id="divPassword" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblPassword" runat="server" ForControl="txtPassword" CssClass="settinglabel"
                                    ConfigKey="ManageUsersPasswordLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtPassword" runat="server" Columns="45" TabIndex="10" MaxLength="50"
                                    CssClass="forminput mediumtextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink8" runat="server" HelpKey="userpasswordhelp" />
                            </div>
                            <div class="settingrow" id="divSecurityQuestion" runat="server">
                                <cy:SiteLabel ID="SiteLabel1" runat="server" ForControl="txtPasswordQuestion" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityQuestionLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtPasswordQuestion" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink9" runat="server" HelpKey="usersecurityquestionhelp" />
                            </div>
                            <div class="settingrow" id="divSecurityAnswer" runat="server">
                                <cy:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtPasswordAnswer" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityAnswerLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtPasswordAnswer" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink10" runat="server" HelpKey="usersecurityanswerhelp" />
                            </div>
                            <div id="divProfileApproved" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblProfileApproved" runat="server" ForControl="chkProfileApproved"
                                    CssClass="settinglabel" ConfigKey="ManageUsersProfileApprovedLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkProfileApproved" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="userprofileapprovedhelp" />
                            </div>
                            <div id="divApprovedForGroups" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblApprovedForGroups" runat="server" ForControl="chkApprovedForGroups"
                                    CssClass="settinglabel" ConfigKey="ManageUsersApprovedForGroupsLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkApprovedForGroups" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:CHelpLink ID="CynHelpLink12" runat="server" HelpKey="userapprovedforforumshelp" />
                            </div>
                            <div id="divTrusted" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblTrusted" runat="server" ForControl="chkTrusted" CssClass="settinglabel"
                                    ConfigKey="ManageUsersTrustedLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkTrusted" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:CHelpLink ID="CynHelpLink13" runat="server" HelpKey="usertrustedhelp" />
                            </div>
                            <div id="divLockout" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel9" runat="server" ForControl="chkIsLockedOut" CssClass="settinglabel"
                                    ConfigKey="UserIsLockedOutLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkIsLockedOut" runat="server" Enabled="false" CssClass="forminput" />
                                <asp:Button ID="btnUnlockUser" runat="server" CausesValidation="false" />
                                <asp:Button ID="btnLockUser" runat="server" CausesValidation="false" />
                                <portal:CHelpLink ID="CynHelpLink14" runat="server" HelpKey="useradminunlockhelp" />
                            </div>
                            <div id="divEmailConfirm" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel11" runat="server" ForControl="chkEmailIsConfirmed" CssClass="settinglabel"
                                    ConfigKey="UserEmailIsConfirmedLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkEmailIsConfirmed" runat="server" Enabled="false" CssClass="forminput" />
                                <asp:Button ID="btnConfirmEmail" runat="server" CausesValidation="false" />
                                <portal:CHelpLink ID="CynHelpLink15" runat="server" HelpKey="useradminconfirmemailhelp" />
                            </div>
                            <div id="divDisplayInMemberList" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblDisplayInMemberList" runat="server" ForControl="chkDisplayInMemberList"
                                    CssClass="settinglabel" ConfigKey="ManageUsersDisplayInMemberListLabel"> </cy:SiteLabel>
                                <asp:CheckBox ID="chkDisplayInMemberList" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:CHelpLink ID="CynHelpLink16" runat="server" HelpKey="userdisplayinmemberlisthelp" />
                            </div>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabProfile" runat="server">
                            <div id="divAvatarUrl" runat="server" >
                                <div class="settingrow">
                                <cy:SiteLabel ID="lblAvatar" runat="server" ForControl="ddAvatars" CssClass="settinglabel"
                                    ConfigKey="UserProfileAvatarLabel"> </cy:SiteLabel>
                                <cy:Gravatar ID="gravatar1" runat="server" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                <img id="imgAvatar" alt="" src="" runat="server" CssClass="forminput" />
                                <portal:GreyBoxHyperlink ID="lnkAvatarUpload" runat="server" />
                                <portal:CHelpLink ID="avatarHelp" runat="server" HelpKey="useravatarhelp" />
                                </div>
                            </div>
                            <asp:Panel ID="pnlProfileProperties" runat="server">
                            </asp:Panel>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabOrderHistory" runat="server">
                            <portal:PurchaseHistory id="purchaseHx" runat="server"></portal:PurchaseHistory>
                        </div>
                        <div id="tabNewsletters" runat="server">
                            <portal:SubscriberPreferences ID="newsLetterPrefs" runat="server" Visible="false">
                            </portal:SubscriberPreferences>
                        </div>
                        <div id="tabRoles" runat="server">
                            <div id="divRoles" runat="server" class="settingrow">
                                <asp:DropDownList ID="allRoles" runat="server" DataValueField="RoleID" DataTextField="DisplayName" CssClass="forminput">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:Button ID="addExisting" runat="server" Text="<%# Resources.Resource.ManageUsersAddToRoleButton %>"
                                    CausesValidation="false" />
                                <portal:CHelpLink ID="CynHelpLink18" runat="server" HelpKey="useraddtorolehelp" />
                            </div>
                            <div id="divUserRoles" runat="server" class="settingrow">
                                <portal:CDataList ID="userRoles" runat="server" DataKeyField="RoleID" RepeatColumns="2">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" CausesValidation="false"
                                            AlternateText='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>' runat="server"
                                            ToolTip='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                                            ID="btnRemoveRole" />
                                        <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server"
                                            ID="Label1" />
                                        &nbsp;&nbsp;&nbsp;
                                    </ItemTemplate>
                                </portal:CDataList>
                            </div>
                        </div>
                        <div id="tabActivity" runat="server">
                            <div id="divCreatedDate" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblCreatedDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divTotalPosts" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblTotalPosts" runat="server" CssClass="forminput"></asp:Label>
                                <portal:GroupUserThreadLink ID="lnkUserPosts" runat="server" />
                                <asp:HyperLink ID="lnkUnsubscribeFromGroups" runat="server" />
                            </div>
                            <div id="divUserGuid" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblUserGuidLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersUserGuidLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblUserGuid" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLastActivity" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="UserLastActivityDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblLastActivityDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLastLogin" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="UserLastLoginDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblLastLoginDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divPasswordChanged" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="UserLastPasswordChangeDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblLastPasswordChangeDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLockoutDate" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="UserLastLockoutDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblLastLockoutDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divFailedPasswordAttempt" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="UserFailedPasswordAttemptCountLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblFailedPasswordAttemptCount" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divFailedPasswordAnswerAttempt" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="UserFailedPasswordAnswerAttemptCountLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblFailedPasswordAnswerAttemptCount" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divComment" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel10" runat="server" ForControl="txtComment" CssClass="settinglabel"
                                    ConfigKey="UserCommentsLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtComment" runat="server" TabIndex="10" MaxLength="255" TextMode="MultiLine"
                                    Rows="15" Columns="55" CssClass="forminput"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink19" runat="server" HelpKey="useradmincommenthelp" />
                            </div>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabLocation" runat="server">
                            <cy:CGridView ID="grdUserLocation" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                CssClass="editgrid" DataKeyNames="RowID" SkinID="plain">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("IPAddress") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Hostname") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("ISP") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Continent") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Country") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Region") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("City") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("TimeZone") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("CaptureCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("FirstCaptureUTC"), TimeOffset)%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("LastCaptureUTC"), TimeOffset)%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </cy:CGridView>
                        </div>
                    </div>
                </div>
                <div class="modulecontent">
                    <div class="settingrow">
                        <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror"></asp:ValidationSummary>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="" Display="none"
                            ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvLoginName" runat="server" ErrorMessage="" Display="none"
                            ControlToValidate="txtLoginName"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ErrorMessage="" ControlToValidate="txtEmail"
                            Display="None" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="none" ErrorMessage=""
                            ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <portal:CButton ID="btnUpdate" runat="server" TabIndex="10" />
                        <portal:CButton ID="btnDelete" runat="server" TabIndex="10" CausesValidation="false" /><br />
                        <portal:CLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
                    </div>
                </div>
            </fieldset>
        </div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
