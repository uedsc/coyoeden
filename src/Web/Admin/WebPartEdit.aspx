<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="WebPartEdit.aspx.cs" Inherits="Cynthia.Web.AdminUI.WebPartEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlModules" runat="server" CssClass="panelwrapper admin webpartedit">
<div class="modulecontent">
<fieldset class="webpartedit">
    <legend>
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkWebPartAdmin" runat="server" NavigateUrl="~/Admin/WebPartAdmin.aspx" />&nbsp;&gt;
		<cy:SiteLabel id="lblEditWebPart" runat="server" ConfigKey="WebPartEditLabel"> </cy:SiteLabel>
    </legend>
    <div class="modulecontent">
    <div class="settingrow">
       <cy:SiteLabel id="Sitelabel4" runat="server" CssClass="settinglabel" ConfigKey="WebPartClassNameLabel"> </cy:SiteLabel>
       <asp:Label ID="lblClassName" runat="server" />
    </div>
    <div class="settingrow">
       <cy:SiteLabel id="Sitelabel5" runat="server" CssClass="settinglabel" ConfigKey="WebPartAssemblyNameLabel"> </cy:SiteLabel>
       <asp:Label ID="lblAssemblyName" runat="server" />
    </div>
    <div class="settingrow">
       <cy:SiteLabel id="Sitelabel6" runat="server" CssClass="settinglabel" ConfigKey="WebPartTitleLabel"> </cy:SiteLabel>
       <asp:Label ID="lblTitle" runat="server" />
     </div>
     <div class="settingrow">
        <cy:SiteLabel id="Sitelabel7" runat="server" CssClass="settinglabel" ConfigKey="WebPartDescriptionLabel"> </cy:SiteLabel>
        <asp:Label ID="lblDescription" runat="server" />
     </div>
     <div class="settingrow">
        <cy:SiteLabel id="Sitelabel1" runat="server" CssClass="settinglabel" ConfigKey="WebPartAvailableForMyPageLabel"> </cy:SiteLabel>
        <asp:CheckBox ID="chkAvailableForMyPage" runat="server"></asp:CheckBox>
     </div>
     <div class="settingrow">
        <cy:SiteLabel id="Sitelabel2" runat="server" CssClass="settinglabel" ConfigKey="WebPartAllowMultipleInstancesOnMyPageLabel"> </cy:SiteLabel>
        <asp:CheckBox ID="chkAllowMultipleInstancesOnMyPage" runat="server"></asp:CheckBox>
     </div>
     <div class="settingrow">
         <cy:SiteLabel id="Sitelabel3" runat="server" CssClass="settinglabel" ConfigKey="WebPartAvailableForContentSystemLabel"> </cy:SiteLabel>
         <asp:CheckBox ID="chkAvailableForContentSystem" runat="server"></asp:CheckBox>
     </div>
     <div  class="settingrow">
		 <cy:SiteLabel id="lblIcon" runat="server" CssClass="settinglabel" ConfigKey="WebPartIconLabel" > </cy:SiteLabel>
		 <asp:DropDownList id="ddIcons" runat="server" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
		 <img id="imgIcon" alt="" src=""  runat="server" />
	</div>
	<div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
		 <portal:CButton id="btnUpdate" runat="server" />&nbsp;
		 <portal:CButton id="btnCancel" runat="server" CausesValidation="false" />&nbsp;
	     <portal:CButton  id="btnDelete" runat="server" CausesValidation="false" />
	     <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="webpartedithelp" />		
	</div>
	</div>

</fieldset>
</div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
