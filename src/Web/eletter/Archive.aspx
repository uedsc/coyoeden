<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Archive.aspx.cs" Inherits="Cynthia.Web.ELetterUI.ArchivePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkNewsletters" runat="server" CssClass="unselectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnlLetter" runat="server" CssClass="art-Post-inner panelwrapper letter">
<asp:Panel ID="pnlGrid" runat="server">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<cy:CGridView ID="grdLetter" runat="server"
	 CssClass=""
	 AutoGenerateColumns="false"
     DataKeyNames="LetterGuid"
     EnableTheming="false"
	 >
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <portal:GreyBoxHyperlink ID="gb1" runat="server" ClientClick="return GB_showFullScreen(this.title, this.href)" NavigateUrl='<%# SiteRoot + "/eletter/LetterView.aspx?l=" + Eval("LetterInfoGuid") + "&letter=" + Eval("LetterGuid") %>'  Text='<%# Eval("Subject") %>' ToolTip='<%# Eval("Subject") %>'  DialogCloseText='<%# Resources.Resource.DialogCloseLink %>' />
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedShortDateString(Eval("SendClickedUTC"), timeOffset)%>
            </ItemTemplate>
		</asp:TemplateField>	
</Columns>
 </cy:CGridView>
<div class="modulepager">
    <portal:CCutePager ID="pgrLetter" runat="server" />
</div>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<div class="modulecontent">
<asp:Label ID="lblMessage" runat="server"  />
</div>


</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
