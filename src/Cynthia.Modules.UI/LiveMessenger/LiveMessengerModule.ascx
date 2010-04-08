<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LiveMessengerModule.ascx.cs" Inherits="Cynthia.Modules.UI.LiveMessengerModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper LiveMessenger">
<portal:ModuleTitleControl EditText="" EditUrl="" runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlLiveMessenger" runat="server" CssClass="modulecontent">
<portal:LiveMessengerControl ID="chat1" runat="server"
    Invitee=""
    Height="300"
    Width="300"
    InviteeDisplayName=""
    OverrideCulture=""
    UseTheme="true"
    ThemeName="blue"
/>
<asp:Panel ID="pnlCopyCidFromUser" runat="server" Visible="false">
<asp:Label ID="lblCopyFromProfileInstructions" runat="server" />
<asp:Button ID="btnCopyFromProfile" runat="server" />
</asp:Panel>
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
