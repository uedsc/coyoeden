<%@ Page language="c#" Codebehind="GalleryBrowse.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.GalleryUI.GalleryBrowse" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper gallerymodule">
<div class="modulecontent">
	<div class="modulepager">
			<span id="spnTopPager" runat="server"></span>
	</div>
	<div class="divgalleryimage">
    <asp:Panel id="pnlGallery" runat="server" SkinID="plain" ></asp:Panel>
    <asp:Label ID="lblCaption" Runat="server"></asp:Label>
    <asp:Label ID="lblDescription" Runat="server"></asp:Label>
    </div>
    <div class="divgalleryimagemeta">
    <asp:Panel ID="pnlImageDetails" runat="server" SkinID="plain" >
	    <asp:xml ID="xmlMeta" runat="server" />
    </asp:Panel>
    </div>	
   </div>
</asp:Panel>	
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

