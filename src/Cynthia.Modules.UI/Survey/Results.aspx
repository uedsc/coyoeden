<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Results.aspx.cs" Inherits="SurveyFeature.UI.ResultsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkResults"></asp:HyperLink>
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlResults" runat="server" CssClass="art-Post-inner panelwrapper survey">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
    <div class="settingrow">
        <strong><cy:SiteLabel ID="lblRespondentLabel" runat="server" ConfigKey="ResponseRespondentLabel"
        ResourceFile="SurveyResources" UseLabelTag="false" /></strong>
        <asp:Label runat="server" id="lblRespondent"></asp:Label>
    </div>
    <div class="settingrow">
        <strong><cy:SiteLabel ID="lblCompletionDateLabel" runat="server" ConfigKey="ResponseCompletionDateLabel"
        ResourceFile="SurveyResources" UseLabelTag="false" /></strong>
        <asp:Label ID="lblCompletionDate" runat="server"></asp:Label>
        <asp:ImageButton ID="btnDelete" runat="server" 
        CommandName="delete" ToolTip='<%$ Resources:SurveyResources, ResultsGridDeleteResponseToolTip %>'
        AlternateText='<%$ Resources:SurveyResources, ResultsGridDeleteResponseButton %>'
        />		
    </div>
    <div class="settingrow">
        <asp:HyperLink ID="lnkPreviousResponse" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkNextResponse" runat="server"></asp:HyperLink>
    </div>
    <asp:Panel id="pnlSurveyPages" runat="server">
     <cy:CGridView ID="grdResults" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="QuestionGuid" 
        EnableTheming="false"
        >
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                 <%# Eval("QuestionText") %>&nbsp;&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                  <%# Eval("Answer") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                  <%# Eval("PageTitle") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>	
</Columns>
 </cy:CGridView>   
        <br /><br />
    </asp:Panel>
    </div>
 </portal:CPanel>
 <div class="cleared"></div>
   </asp:Panel>
   <cy:CornerRounderBottom id="cbottom1" runat="server" />
   </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
