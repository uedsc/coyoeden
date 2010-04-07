<%@ Control Language="c#" AutoEventWireup="True" Codebehind="HtmlFragmentInclude.ascx.cs" Inherits="Cynthia.Web.ContentUI.HtmlFragmentInclude" %>
<asp:PlaceHolder ID="ph1" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server"  CssClass="art-Post-inner panelwrapper htmlfraginclude">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent" id="<%=string.Format("module{0}",ModuleId) %>">
<asp:Literal ID="lblInclude" Runat="server" />
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:PlaceHolder>
<asp:PlaceHolder ID="ph2" runat="server" Visible="false">
<portal:ModuleTitleControl id="Title2" runat="server" />
<asp:Literal ID="lblInclude1" Runat="server" />
</asp:PlaceHolder>		