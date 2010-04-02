<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterEdit.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span> 
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;
    <asp:HyperLink ID="lnkDraftList" runat="server" CssClass="selectedcrumb" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlSettings" runat="server" CssClass="panelwrapper editpage newsletter">
<div class="modulecontent yui-skin-sam">
 <fieldset class="newsletteredit">
    <legend>
        <cy:SiteLabel id="lblPageNameLayout" runat="server" ConfigKey="NewsLetterEditLetterHeading"> </cy:SiteLabel>
        <asp:Literal ID="litHeading" runat="server" />
    </legend>
<div class="settingrow">
    <cy:SiteLabel id="lbl1" runat="server" CssClass="settinglabel" ConfigKey="NewsLetterSubjectLabel"> </cy:SiteLabel>
    <asp:Textbox id="txtSubject" runat="server" columns="70" CssClass="forminput"></asp:Textbox>
</div>
<div class="settingrow">
    <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
    <portal:CButton  id="btnSave" runat="server" />&nbsp;
    <portal:CButton  id="btnDelete" runat="server" CausesValidation="False" />&nbsp;
    <portal:CButton  id="btnSendToList" runat="server" />&nbsp;
    <portal:CLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
<portal:CButton  id="btnSendPreview" runat="server"  Text="" CausesValidation="false" />&nbsp;
<asp:TextBox ID="txtPreviewAddress" runat="server" CssClass="mediumtextbox"></asp:TextBox>
<portal:CButton ID="btnGeneratePlainText" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="NewsLetterTemplatesLabel"> </cy:SiteLabel>
<asp:DropDownList ID="ddTemplates" runat="server" DataTextField="Title" DataValueField="Guid"></asp:DropDownList>
<portal:CButton id="btnLoadTemplate" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
<portal:CButton ID="btnSaveAsTemplate" runat="server" />
<asp:TextBox ID="txtNewTemplateName" runat="server" CssClass="mediumtextbox"></asp:TextBox>
<asp:HyperLink ID="lnkManageTemplates" runat="server"></asp:HyperLink>
</div>
<div id="divtabs" class="yui-navset">
    <ul class="yui-nav">
        <li class="selected"><a href="#tabHtml"><em><asp:Literal ID="litHtmlTab" runat="server" /></em></a></li>
        <li><a href="#tabPlainText"><em><asp:Literal ID="litPlainTextTab" runat="server" /></em></a></li>
    </ul>
    <div class="yui-content">
        <div id="tabHtml">
            <cye:EditorControl id="edContent" runat="server"> </cye:EditorControl>
        </div>
        <div id="tabPlainText">
            <asp:TextBox ID="txtPlainText" runat="server" TextMode="MultiLine" Width="100%" Height="800px"></asp:TextBox>
        </div>
    </div>
</div>
</fieldset>		
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
