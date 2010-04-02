<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterTemplateEdit.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterTemplateEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="breadcrumbs">
  <span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span>
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
    <asp:HyperLink ID="lnkTemplateList" runat="server" CssClass="selectedcrumb" />
    </div>

<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlSettings" runat="server" CssClass="panelwrapper editpage newsletter">
<div class="modulecontent">
 <fieldset class="newsletteredit">
    <legend>
        <cy:SiteLabel id="lblPageNameLayout" runat="server" ConfigKey="NewsLetterEditTemplateHeading"> </cy:SiteLabel>
    </legend>
    

<div class="settingrow">
    <cy:SiteLabel id="lbl1" runat="server" CssClass="settinglabel" ConfigKey="LetterTemplateTitleLabel"> </cy:SiteLabel>
    <asp:Textbox id="txtTitle" runat="server" columns="70"></asp:Textbox>
</div>
<div class="settingrow">
    <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
    <portal:CButton  id="btnSave" runat="server"  Text="" CausesValidation="false" ValidationGroup="edit" />&nbsp;
    <portal:CButton  id="btnDelete" runat="server"  Text="" CausesValidation="False" ValidationGroup="edit" />&nbsp;
    <asp:ValidationSummary ID="editSummary" runat="server" ValidationGroup="edit" />
    <asp:RequiredFieldValidator ID="reqTitle" runat="server" Display="None" ControlToValidate="txtTitle" ValidationGroup="edit" />
    
</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
<portal:CButton  id="btnSendPreview" runat="server"  Text="" CausesValidation="True" ValidationGroup="preview" />&nbsp;
<asp:TextBox ID="txtPreviewAddress" runat="server" ValidationGroup="preview"></asp:TextBox>
<asp:ValidationSummary ID="previewSummary" runat="server" ValidationGroup="preview" />
<asp:RequiredFieldValidator ID="reqPreviewAddress" runat="server" Display="None" ControlToValidate="txtPreviewAddress" ValidationGroup="preview" />
<asp:RegularExpressionValidator ID="regexPreviewAddress" runat="server" Display="None" ControlToValidate="txtPreviewAddress" ValidationGroup="preview" />
</div>
<div class="settingrow">
<cye:EditorControl id="edContent" runat="server"> </cye:EditorControl>

</div>


</fieldset>
</div>
</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
