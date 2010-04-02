<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="CompleteSurvey.aspx.cs" Inherits="SurveyFeature.UI.CompleteSurveyPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlSurveyQuestions" runat="server" CssClass="art-Post-inner panelwrapper survey">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2> 
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<asp:Panel ID="pnlQuestions" runat="server" >    
</asp:Panel>
      
<div class="settingrow">
    <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" UseLabelTag="false" />
    <portal:CButton ID="btnSurveyBack" runat="server"  CausesValidation="false" />
    <portal:CButton ID="btnSurveyForward" runat="server" />
</div>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
