<%@ Page language="c#" Codebehind="EditThread.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.GroupUI.GroupThreadEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper forummodule">
<asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
<fieldset>
    <legend>
	    <cy:SiteLabel id="lblGroupThreadEditLabel" runat="server" ConfigKey="GroupThreadEditLabel" ResourceFile="GroupResources" UseLabelTag="false" />
	</legend>
    
        <div class="settingrow">
            <cy:SiteLabel id="lblTopicTitleLabel" runat="server" ForControl="txtSubject" CssClass="settinglabel" ConfigKey="GroupThreadEditSubjectLabel" ResourceFile="GroupResources"  />
            <asp:TextBox id="txtSubject" runat="server" maxlength="100" CssClass="verywidetextbox forminput" ></asp:TextBox>
        </div>
        <div class="settingrow">
            <cy:SiteLabel id="lblThreadGroupLabel" runat="server" ForControl="ddGroupList" CssClass="settinglabel" ConfigKey="GroupThreadEditGroupLabel" ResourceFile="GroupResources"  />
            <asp:DropDownList ID="ddGroupList" Runat="server" EnableTheming="false" CssClass="forminput" AutoPostBack="False" DataTextField="Title" DataValueField="ItemID"></asp:DropDownList>
        </div>
        <div class="settingrow">
            <asp:Label ID="lblError"  runat="server" CssClass="txterror"></asp:Label>
        </div>
        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
            <portal:CButton  id="btnUpdate" runat="server"  Text="Update" />&nbsp;
		    <portal:CButton id="btnDelete" runat="server" CausesValidation="false" />&nbsp;
		    <asp:HyperLink ID="lnkCancel" runat="server" />
		    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="forumthreadedithelp" />
		    </div>
        </div>
</fieldset>	
</asp:Panel>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />	
	</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />