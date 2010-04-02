<%@ Page Language="c#" CodeBehind="Register.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.Register" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlRegister" runat="server" CssClass="panelwrapper register">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <cy:SiteLabel ID="lblRegisterLabel" runat="server" ConfigKey="RegisterLabel" UseLabelTag="false">
                    </cy:SiteLabel>
                </legend>
                <asp:Panel ID="pnlAuthenticated" runat="server" Visible="false">
                    <asp:Literal ID="litAlreadyAuthenticated" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlRegisterWrapper" runat="server">
                    <asp:Panel ID="pnlStandardRegister" runat="server" CssClass="floatpanel">
                        <asp:CreateUserWizard ID="RegisterUser" runat="server" NavigationStyle-HorizontalAlign="Center">
                            <WizardSteps>
                                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlRequiredProfilePropertiesUpper" runat="server">
                                        </asp:Panel>
                                        <asp:Panel ID="pnlUserName" runat="server" class="settingrow">
                                            <cy:SiteLabel ID="lblLoginName" runat="server" ForControl="UserName" CssClass="settinglabel"
                                                ConfigKey="RegisterLoginNameLabel"> </cy:SiteLabel>
                                            <asp:TextBox ID="UserName" runat="server" TabIndex="10" Columns="30" MaxLength="50" />
                                        </asp:Panel>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="lblRegisterEmail1" runat="server" ForControl="Email" CssClass="settinglabel"
                                                ConfigKey="RegisterEmailLabel"> </cy:SiteLabel>
                                            <asp:TextBox ID="Email" runat="server" TabIndex="10" Columns="30" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="lblRegisterPassword1" runat="server" ForControl="Password" CssClass="settinglabel"
                                                ConfigKey="RegisterPasswordLabel"> </cy:SiteLabel>
                                            <asp:TextBox ID="Password" runat="server" TabIndex="10" Columns="30" TextMode="Password"
                                                MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="settingrow">
                                            <cy:SiteLabel ID="lblRegisterConfirmPassword1" runat="server" ForControl="ConfirmPassword"
                                                CssClass="settinglabel" ConfigKey="RegisterConfirmPasswordLabel"> </cy:SiteLabel>
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TabIndex="10" Columns="30" TextMode="Password"
                                                MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="settingrow" id="divQuestion" runat="server">
                                            <cy:SiteLabel ID="SiteLabel2" runat="server" ForControl="Question" CssClass="settinglabel"
                                                ConfigKey="RegisterSecurityQuestion"> </cy:SiteLabel>
                                            <asp:TextBox ID="Question" runat="server" TabIndex="10" Columns="45" />
                                        </div>
                                        <div class="settingrow" id="divAnswer" runat="server">
                                            <cy:SiteLabel ID="SiteLabel1" runat="server" ForControl="Answer" CssClass="settinglabel"
                                                ConfigKey="RegisterSecurityAnswer"> </cy:SiteLabel>
                                            <asp:TextBox ID="Answer" runat="server" TabIndex="10" Columns="45" />
                                        </div>
                                        <asp:Panel ID="pnlRequiredProfileProperties" runat="server">
                                        </asp:Panel>
                                        <div class="settingrow">
                                            <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Register" />
                                            <asp:RequiredFieldValidator ControlToValidate="UserName" ID="UserNameRequired" runat="server"
                                                Display="None" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ControlToValidate="Email" ID="EmailRequired" runat="server"
                                                Display="None" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="EmailRegex" runat="server" ControlToValidate="Email"
                                                Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
                                                ValidationGroup="Register"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ControlToValidate="Password" ID="PasswordRequired" Display="None"
                                                runat="server" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="PasswordRulesValidator" runat="server" ControlToValidate="Password"
                                                Display="None" ValidationGroup="Register"></asp:CustomValidator>
                                            <asp:RegularExpressionValidator ID="PasswordRegex" runat="server" ControlToValidate="Password"
                                                Display="None" ValidationGroup="Register"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" ID="ConfirmPasswordRequired"
                                                runat="server" Display="None" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                                ID="PasswordCompare" runat="server" Display="None" ValidationGroup="Register"></asp:CompareValidator>
                                            <asp:RequiredFieldValidator ControlToValidate="Question" ID="QuestionRequired" runat="server"
                                                Display="None" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ControlToValidate="Answer" ID="AnswerRequired" runat="server"
                                                Display="None" ValidationGroup="Register"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="regerror">
                                            <portal:CLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
                                        </div>
                                        <div id="divAgreement" runat="server">
                                        </div>
                                        
                                    </ContentTemplate>
                                </asp:CreateUserWizardStep>
                                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnComplete" runat="server">
                                        </asp:Panel>
                                        <asp:Literal ID="CompleteMessage" runat="server" />
                                        <div>
                                            <asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                                                ValidationGroup="CreateUserWizard1" />
                                        </div>
                                    </ContentTemplate>
                                </asp:CompleteWizardStep>
                            </WizardSteps>
                        </asp:CreateUserWizard>
                        <asp:Literal ID="litTest" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlThirdPartyAuth" runat="server" Visible="false" CssClass="clearpanel thirdpartyauth">
                        <h2>
                            <asp:Literal ID="litThirdPartyAuthHeading" runat="server" /></h2>
                        <asp:Panel ID="pnlWindowsLiveID" runat="server" CssClass="windowslivepanel" Visible="false">
                            <asp:HyperLink ID="lnkWindowsLiveID" runat="server" NavigateUrl="~/Secure/RegisterWithWindowsLiveID.aspx" />
                            <br />
                        </asp:Panel>
                        <asp:Panel ID="divLiteralOr" runat="server" Visible="false" CssClass="clearpanel orpanel">
                            <asp:Literal ID="litOr" runat="server" /><br />
                            <br />
                        </asp:Panel>
                        <asp:Panel ID="pnlOpenID" runat="server" CssClass="openidpanel" Visible="false">
                            <asp:HyperLink ID="lnkOpenIDRegistration" runat="server" NavigateUrl="~/Secure/RegisterWithOpenID.aspx" />
                            
                            <br />
                        </asp:Panel>
                        <asp:Panel ID="pnlRpx" runat="server" CssClass="openidpanel" Visible="false">
                            <portal:OpenIdRpxNowLink ID="rpxLink" runat="server" />
                            <br />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </fieldset>
        </div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
