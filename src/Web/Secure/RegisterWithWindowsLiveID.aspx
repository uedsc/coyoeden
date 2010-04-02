<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RegisterWithWindowsLiveID.aspx.cs" Inherits="Cynthia.Web.UI.Pages.RegisterWithWindowsLiveId" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server" >
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlRegister" runat="server" CssClass="panelwrapper register" >
<div class="modulecontent">
<fieldset>
    <legend>
        <cy:SiteLabel id="lblRegisterLabel" runat="server" ConfigKey="WindowsLiveIDRegistrationHeading" UseLabelTag="false"> </cy:SiteLabel>
    </legend>
    <asp:Panel ID="pnlAuthenticated" runat="server" Visible="false">
    <asp:Literal ID="litAlreadyAuthenticated" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlRegisterWrapper" runat="server">
    <asp:Panel ID="pnlWindowsLiveLogin" runat="server">
        <div id="divAgreement" runat="server" ></div>
        <div style="background:white;color:black; font-size: small; width:350px; padding: 3px 3px 3px 3px; border: dashed thin gray;">
        <div>
        <asp:Literal ID="litInstructions" runat="server" />
        </div>
        <iframe 
               id="WebAuthControl" 
               name="WebAuthControl"
               src="<%=Protocol%>login.live.com/controls/WebAuth.htm?appid=<%=WindowsLiveAppId%>&style=font-size%3A+small%3B+font-family%3A+verdana%3B+background%3A+white%3B"
               width="97px"
               height="25px"
               marginwidth="0"
               marginheight="0"
               frameborder="0"
               scrolling="no" style="display:inline; padding: 0px 0px 0px 0px;">
        </iframe><span style="vertical-align:top;"></span>
       </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlWindowsLiveRegister" runat="server">
    <div class="settingrow">
        <cy:SiteLabel id="lblLoginName" runat="server" ForControl="txtUserName" CssClass="settinglabel" ConfigKey="RegisterLoginNameLabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtUserName" runat="server" TabIndex="10" columns="30" MaxLength="50" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="lblRegisterEmail1" runat="server" ForControl="txtEmail" CssClass="settinglabel" ConfigKey="RegisterEmailLabel"> </cy:SiteLabel>
        <asp:TextBox id="txtEmail" runat="server" TabIndex="10" columns="30" MaxLength="100"></asp:TextBox>
    </div>
    
    <asp:Panel ID="pnlRequiredProfileProperties" runat="server">
        
    </asp:Panel>
    <div class="settingrow" >
    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Register" />
    
    <asp:RequiredFieldValidator 
          ControlToValidate="txtUserName" 
          ID="UserNameRequired" 
          runat="server" 
          Display="None"
          ValidationGroup="Register"></asp:RequiredFieldValidator>
          
    <asp:RequiredFieldValidator 
          ControlToValidate="txtEmail" 
          ID="EmailRequired" 
          runat="server" 
          Display="None"
          ValidationGroup="Register"></asp:RequiredFieldValidator>
          
    <asp:RegularExpressionValidator 
        ID="EmailRegex" runat="server" 
        ControlToValidate="txtEmail" 
        Display="None" 
        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
        ValidationGroup="Register"></asp:RegularExpressionValidator>
        
    </div>
    <div class="txterror">
        <asp:Literal id="ErrorMessage" runat="server" />
    </div>
    
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="EmptyLabel"> </cy:SiteLabel>
        <asp:Button ID="btnCreateUser" runat="server" />
    </div>
    <div>
        <asp:Literal ID="litInfoNeededMessage" runat="server" />
    </div>
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" CssClass="txterror" />
    </asp:Panel>
    
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
