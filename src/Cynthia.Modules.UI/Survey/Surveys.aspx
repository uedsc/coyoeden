<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Surveys.aspx.cs" Inherits="SurveyFeature.UI.SurveysPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="selectedcrumb"></asp:HyperLink>
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlSurveys" runat="server" CssClass="art-Post-inner panelwrapper survey">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
    <cy:CGridView ID="grdSurveys" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="SurveyGuid" 
        EnableTheming="false"
        >
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                <asp:HyperLink id="editLink" 
						    Text="<%# Resources.SurveyResources.SurveysGridEditButton %>" 
						    Tooltip="<%# Resources.SurveyResources.SurveysGridEditButtonToolTip %>"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyEdit.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
						    runat="server" />
                        <asp:ImageButton ID="btnDelete" runat="server" 
                            CommandName="delete" ToolTip='<%# Resources.SurveyResources.SurveysGridDeleteButtonToolTip %>'
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            AlternateText='<%# Resources.SurveyResources.SurveysGridDeleteButton %>'
                            ImageUrl='<%# DeleteLinkImage %>' />	
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("SurveyName") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("CreationDate") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:HyperLink id="pagesLink"
                            Text='<%# Eval("PageCount") %>'
						    Tooltip="<%# Resources.SurveyResources.SurveysGridPageCountToolTip %>"
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>' 
						    runat="server" />
						    <asp:HyperLink id="HyperLink1"
						    Text="<%# Resources.SurveyResources.AddEditGridLink %>"
						    Tooltip="<%# Resources.SurveyResources.AddEditGridLink %>"
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>' 
						    runat="server" />&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:Button ID="lnkAddRemoveFromModule" runat="server" CssClass="buttonlink" CommandArgument='<%# Eval("SurveyGuid") %>' />
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <asp:Label ID="lblResponseCount" runat="server" Text='<%# Eval("ResponseCount") %>' Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) == 0) %>'></asp:Label>
                        <asp:HyperLink
                            ID="lnkResults"
                            Text='<%# Eval("ResponseCount") %>' 
                            Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>' 
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            runat="server" 
                            ToolTip='<%# Resources.SurveyResources.SurveyGridResponseCountLinkToolTip %>'
                            NavigateUrl='<%# SiteRoot + "/Survey/Results.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>'/>
                            &nbsp;&nbsp;
                        <asp:HyperLink
                            ID="HyperLink2" 
                            Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>' 
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            runat="server" 
                            Text='<%# Resources.SurveyResources.ViewResponsesLink %>'
                            NavigateUrl='<%# SiteRoot + "/Survey/Results.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>'/>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="btnExport" runat="server" Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>'
                            CommandName="export" ToolTip='<%# Resources.SurveyResources.SurveyGridExportResponsesToolTip %>'
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            AlternateText='<%# Resources.SurveyResources.SurveyGridExportResponsesAlternateText %>'
                            ImageUrl="~/Data/SiteImages/download.gif"/>	 
                        &nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
</Columns>
 </cy:CGridView>
<div class="modulepager" >
<asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
<br class="clear" />
<asp:Label id="lblMessages" runat="server" EnableViewState="False"></asp:Label>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
