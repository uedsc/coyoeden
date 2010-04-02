<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="GoogleMapModule.ascx.cs" Inherits="Cynthia.Web.MapUI.GoogleMapModule" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="art-Post-inner panelwrapper GoogleMap">
<portal:ModuleTitleControl EditText="Edit" EditUrl="~/GoogleMap/GoogleMapEdit.aspx" runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlGoogleMap" runat="server" CssClass="modulecontent">
<goog:LocationMap ID="gmap" runat="server" EnableMapType="true" EnableZoom="true" ShowInfoWindow="true" EnableLocalSearch="true"></goog:LocationMap>
<asp:Literal ID="litCaption" runat="server" /><br />
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
