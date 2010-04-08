<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FlickrSlideShowModule.ascx.cs" Inherits="Cynthia.Modules.UI.FlickrSlideShowModule" %>

<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper FlickrSlideShow">
<portal:ModuleTitleControl  runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlFlickrSlideShow" runat="server" CssClass="modulecontent">
<portal:VertigoSlideshow ID="slideShow" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlNotConfigured" runat="server" Visible="false">
<asp:Label ID="lblNotConfigured" Runat="server" EnableViewState="false" CssClass="txterror"></asp:Label>
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
