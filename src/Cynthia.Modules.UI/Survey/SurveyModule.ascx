<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyModule.ascx.cs" Inherits="SurveyFeature.UI.SurveyModule" %>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server"  CssClass="art-Post-inner panelwrapper survey">
<portal:ModuleTitleControl EditText="Edit" EditUrl="~/Survey/SurveyAdmin.aspx" runat="server" id="TitleControl" />
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
    <asp:Literal runat="server" ID="litSurveyMessage"></asp:Literal>
    <asp:Label runat="server" ID="litOldResponses"></asp:Label>
    <asp:CheckBox runat="server" ID="chkUseOldResponses" Visible="false"/>
<div class="settingrow">
    <portal:CButton ID="btnStartSurvey" runat="server" CausesValidation="false" />
</div>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
