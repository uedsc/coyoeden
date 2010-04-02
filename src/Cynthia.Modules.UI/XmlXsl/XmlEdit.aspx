<%@ Page language="c#" CodeBehind="XmlEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.XmlUI.EditXml" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper xmlmodule">
<div class="modulecontent">
<fieldset class="xmledit">
        <legend>		
		    <cy:SiteLabel id="lblSettings" runat="server" ConfigKey="EditXmlSettingsLabel" ResourceFile="XmlResources" UseLabelTag="false"> </cy:SiteLabel>
		</legend>
	    <asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="updateButton">
	        <div class="settingrow">
	            <cy:SiteLabel id="lblXmlDataFile" runat="server" ForControl="ddXml" CssClass="settinglabel" ConfigKey="EditXmlFileLabel" ResourceFile="XmlResources" > </cy:SiteLabel>
	            <asp:DropDownList ID="ddXml" Runat="server" EnableTheming="false" CssClass="forminput" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="lblXslFile" runat="server" ForControl="ddXsl" CssClass="settinglabel" ConfigKey="EditXslFileLabel" ResourceFile="XmlResources" > </cy:SiteLabel>
	            <asp:DropDownList ID="ddXsl" Runat="server" EnableTheming="false" CssClass="forminput" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
	        </div>
	        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
        <div class="forminput">
	            <portal:CButton id="updateButton" runat="server" />&nbsp;
			    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
			    </div>
			    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="xmledithelp" />	  
	        </div>
	    </asp:Panel>
</fieldset>	
</div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />