<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="NewsLetterSubscribeModule.ascx.cs" Inherits="Cynthia.Web.ELetterUI.NewsLetterSubscribeModuleModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper NewsLetterSubscribeModule">
<portal:ModuleTitleControl  runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlNewsLetterSubscribeModule" runat="server" CssClass="modulecontent">
<portal:Subscribe ID="subscribe1" runat="server" />
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
