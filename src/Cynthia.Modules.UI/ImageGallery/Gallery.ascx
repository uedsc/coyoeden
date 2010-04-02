<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Gallery.ascx.cs" Inherits="Cynthia.Web.GalleryUI.GalleryControl" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper gallerymodule">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel ID="pnlSilverlight" runat="server" Visible="false" CssClass="modulecontent">
<portal:VertigoSlideshow ID="slideShow" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlMain" runat="server">
<asp:UpdatePanel ID="upGallery" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<div><portal:GreyBoxHyperlink ID="gbLauncher" runat="server" Visible="false" /></div>
<div class="modulecontent">
<portal:CCutePager ID="pager" runat="server" />
<div >
<asp:Repeater id="rptGallery" runat="server" >
	<ItemTemplate>
	    <%# GetThumnailImageLink(DataBinder.Eval(Container.DataItem, "ItemID").ToString(), DataBinder.Eval(Container.DataItem, "ThumbnailFile").ToString(), DataBinder.Eval(Container.DataItem, "WebImageFile").ToString(), DataBinder.Eval(Container.DataItem, "Caption").ToString()) %>
		<asp:ImageButton ID="btnThumb" runat="server" Visible='<%# (!UseGreyBox) %>' ImageUrl='<%# GetThumnailUrl(Eval("ThumbnailFile").ToString()) %>' CommandName="setimage" CommandArgument='<%# Eval("ItemID") %>' />
		<span class="txtmed">
				<asp:HyperLink id="editLink" runat="server" EnableViewState="false"
				    Text="<%# Resources.GalleryResources.GalleryEditImageLink %>" 
				    Tooltip="<%# Resources.GalleryResources.GalleryEditImageLink %>" 
				    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
				    NavigateUrl='<%# this.SiteRoot + "/ImageGallery/EditImage.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + this.PageId.ToString() %>' 
				    Visible="<%# IsEditable %>"
				     />
				&nbsp; </span>
	</ItemTemplate>
</asp:Repeater>

</div>
<div class="divgalleryimage">
<asp:Panel id="pnlGallery" runat="server" SkinID="plain" EnableViewState="false"></asp:Panel>
<asp:Label ID="lblCaption" Runat="server" EnableViewState="false"></asp:Label><br />
<asp:Label ID="lblDescription" Runat="server" EnableViewState="false"></asp:Label><br /><br />
</div>
<div class="divgalleryimagemeta">
<asp:Panel ID="pnlImageDetails" runat="server" SkinID="plain" EnableViewState="false" >
	<asp:xml ID="xmlMeta" runat="server" />
</asp:Panel>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>
