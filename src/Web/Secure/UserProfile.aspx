<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="UserProfile.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.UserProfile" %>
<%@ Register Src="~/Controls/UserCommerceHistory.ascx" TagPrefix="portal" TagName="PurchaseHistory" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <cy:YUIPanel ID="pnlUserProfile" runat="server" DefaultButton="btnUpdate" CssClass="panelwrapper userprofile yui-skin-sam">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <cy:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="UserProfileMyProfileLabel">
                    </cy:SiteLabel>
                </legend>
                <br />
                <div id="divtabs" class="yui-navset">
                    <ul class="yui-nav">
                        <li id="liSecurity" runat="server"><a href="#tabSecurity"><em>
                            <asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
                        <li  id="liNewsletters" runat="server" visible="false"><a href="#tabNewsletter"
                            id="lnkNewsletter" runat="server"><em>
                                <asp:Literal ID="litNewsletterTab" runat="server" /></em></a></li>
                        <li id="liProfile" runat="server"><a href="#tabProfile" id="lnkProfile" runat="server">
                            <em>
                                <asp:Literal ID="litProfileTab" runat="server" /></em></a></li>
                        <li id="liOrderHistory" runat="server"><a href="#tabOrderHistory" id="lnkOrderHistory" runat="server">
                            <em>
                                <asp:Literal ID="litOrderHistoryTab" runat="server" /></em></a></li>
                    </ul>
                    <div class="yui-content">
                        <div id="tabSecurity" runat="server">
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblUserName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                    ConfigKey="ManageUsersUserNameLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtName" runat="server" TabIndex="10" MaxLength="100" CssClass="widetextbox forminput"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="userfullnamehelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SitelabelLoginName" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersLoginNameLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblLoginName" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel"
                                    ConfigKey="ManageUsersEmailLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="10" Columns="45" CssClass="widetextbox forminput"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="useremailhelp" />
                            </div>
                            <div id="divOpenID" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel4" runat="server" ForControl="OpenIdLogin1" CssClass="settinglabel"
                                    ConfigKey="ManageUsersOpenIDURILabel"> </cy:SiteLabel>
                                    <div class="forminput">
                                <asp:Label ID="lblOpenID" runat="server" />
                                <asp:HyperLink ID="lnkOpenIDUpdate" runat="server" />
                                <portal:OpenIdRpxNowLink ID="rpxLink" runat="server" Embed="false" UseOverlay="true" Visible="false" />
                                </div>
                            </div>
                            <asp:Panel ID="pnlSecurityQuestion" runat="server">
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtPasswordQuestion" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityQuestionLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtPasswordQuestion" runat="server" TabIndex="10" MaxLength="255" CssClass="widetextbox forminput"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="usersecurityquestionhelp" />
                            </div>
                            <div class="settingrow">
                                <cy:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtPasswordAnswer" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityAnswerLabel"> </cy:SiteLabel>
                                <asp:TextBox ID="txtPasswordAnswer" runat="server" TabIndex="10" MaxLength="255" CssClass="widetextbox forminput"></asp:TextBox>
                                <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="usersecurityanswerhelp" />
                            </div>
                            </asp:Panel>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabNewsletters" runat="server">
                            <portal:SubscriberPreferences ID="newsLetterPrefs" runat="server" Visible="false">
                            </portal:SubscriberPreferences>
                        </div>
                        <div id="tabProfile" runat="server">
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel">
                                </cy:SiteLabel>
                                <asp:Label ID="lblCreatedDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divSkin" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblSkin" runat="server" ForControl="ddSkins" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsSiteSkinLabel"> </cy:SiteLabel>
                                <asp:DropDownList ID="ddSkins" runat="server" EnableTheming="false" CssClass="forminput" TabIndex="10" DataValueField="Name"
                                    DataTextField="Name">
                                </asp:DropDownList>
                                <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="userskinhelp" />
                            </div>
                            <div id="divAvatar" runat="server" class="settingrow">
                                <cy:SiteLabel ID="lblAvatar" runat="server" CssClass="settinglabel"
                                    ConfigKey="UserProfileAvatarLabel"> </cy:SiteLabel>
                                    <div class="forminput">
                                <cy:Gravatar ID="gravatar1" runat="server" />
                                <img alt="" id="imgAvatar" src="" runat="server" />
                                <portal:GreyBoxHyperlink ID="lnkAvatarUpload" runat="server" />
                                <portal:CHelpLink ID="avatarHelp" runat="server" HelpKey="useravatarhelp" />
                                </div>
                            </div>
                            <div id="divLiveMessenger" runat="server" class="settingrow">
                                <cy:SiteLabel ID="SiteLabel14" runat="server" ForControl="chkEnableLiveMessengerOnProfile" CssClass="settinglabel"
                                    ConfigKey="EnableLiveMessengerLabel"> </cy:SiteLabel>
                                    <div class="forminput">
                                <asp:CheckBox ID="chkEnableLiveMessengerOnProfile" runat="server" />
                                    <asp:HyperLink ID="lnkAllowLiveMessenger" runat="server" Text="Enable Live Messenger" />
                                <portal:CHelpLink ID="CynHelpLink6" runat="server" HelpKey="livemessenger-user-help" />
                                </div>
                            </div>
                            <asp:Panel ID="pnlProfileProperties" runat="server">
                            </asp:Panel>
                            
                            <div class="settingrow">
                                <cy:SiteLabel ID="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel">
                                </cy:SiteLabel>
                                <div class="forminput">
                                <asp:Label ID="lblTotalPosts" runat="server"></asp:Label>
                                <portal:ForumUserThreadLink ID="lnkUserPosts" runat="server" />
                                </div>
                            </div>
                            <div class="settingrow">
                                <portal:GreyBoxHyperlink ID="lnkPublicProfile" runat="server" ClientClick="return GB_showFullScreen(this.title, this.href)"  />
                            </div>
                            
                        </div>
                        <div id="tabOrderHistory" runat="server">
                            <portal:PurchaseHistory id="purchaseHx" runat="server"></portal:PurchaseHistory>
                        </div>
                    </div>
                </div>
                <div class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror" ValidationGroup="profile">
                    </asp:ValidationSummary>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="profile"
                        Display="none" ErrorMessage="" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexEmail" runat="server" ValidationGroup="profile"
                        ErrorMessage="" ControlToValidate="txtEmail" Display="None" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="profile"
                        ErrorMessage="" ControlToValidate="txtEmail" Display="none"></asp:RequiredFieldValidator>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <portal:CButton ID="btnUpdate" runat="server" Text="" ValidationGroup="profile" />
                    <asp:HyperLink ID="lnkChangePassword" runat="server" CssClass="passwordrecovery"></asp:HyperLink>
                    <portal:CHelpLink ID="CynHelpLink7" runat="server" HelpKey="userchangepasswordhelp" />
                    <br />
                    <portal:CLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
                </div>
            </fieldset>
        </div>
    </cy:YUIPanel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
