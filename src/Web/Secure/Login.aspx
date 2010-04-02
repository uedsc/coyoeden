<%@ Page language="c#"  Codebehind="Login.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.UI.Pages.LoginPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlLogin" runat="server" CssClass="panelwrapper login">
<div class="modulecontent">
<fieldset>
    <legend>
        <cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="SignInLabel" UseLabelTag="false"> </cy:SiteLabel>
    </legend>
    <asp:Panel ID="pnlStandardLogin" runat="server" CssClass="floatpanel">
    <portal:SiteLogin ID="LoginCtrl" runat="server" CssClass="login" >
        <LayoutTemplate>
        <asp:Panel ID="pnlLContainer" runat="server" DefaultButton="Login">
            <div class="settingrow">
                <strong><cy:SiteLabel id="lblEmail" runat="server" ForControl="UserName"  ConfigKey="SignInEmailLabel"> </cy:SiteLabel>
                <cy:SiteLabel id="lblUserID" runat="server" ForControl="UserName"  ConfigKey="ManageUsersLoginNameLabel"> </cy:SiteLabel></strong>
                <br /><asp:TextBox ID="UserName" runat="server" Columns="35" MaxLength="100" />
            </div>
            <div class="settingrow">
                <strong><cy:SiteLabel id="lblPassword" runat="server" ForControl="Password" ConfigKey="SignInPasswordLabel"> </cy:SiteLabel></strong>
                <br /><asp:TextBox ID="Password" runat="server" TextMode="password" />
            </div>
            <div class="settingrow">
                <asp:CheckBox ID="RememberMe" runat="server" />
            </div>
            <div class="settingrow">
                <portal:CButton ID="Login" CommandName="Login" runat="server" Text="Login" />
                <br /><portal:CLabel ID="FailureText" runat="server" CssClass="txterror" EnableViewState="false" />
            </div>
            <div class="settingrow">
                <asp:HyperLink ID="lnkPasswordRecovery" runat="server" />&nbsp;&nbsp;
                <asp:HyperLink ID="lnkRegisterExtraLink" runat="server" />
            </div>
            </asp:Panel>
        </LayoutTemplate> 
    </portal:SiteLogin>
    </asp:Panel>
    <div class="floatpanel">
    <asp:Panel ID="pnlWindowsLive" runat="server" Visible="false">
    <div style="padding: 0px 0px 0px 6px;">
        <portal:WindowsLiveLoginControl ID="livelogin" runat="server" />
      </div>
    </asp:Panel>
    <asp:Panel ID="divLiteralOr" runat="server" Visible="false" CssClass="clearpanel">
    <asp:Literal ID="litOr" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlOpenID" runat="server" Visible="false"></asp:Panel>
    </div>
</fieldset>
</div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />



