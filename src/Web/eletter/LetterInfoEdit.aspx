<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterInfoEdit.aspx.cs" Inherits="Cynthia.Web.ELetterUI.LetterInfoEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />&nbsp;&gt;</span> 
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlEdit" runat="server" CssClass="panelwrapper admin newsletterinfoedit">
<div class="modulecontent yui-skin-sam">
<fieldset>
<legend>
    <asp:Literal ID="litHeading" runat="server" />
</legend>
<br />
<div id="divtabs" class="yui-navset">
    <ul class="yui-nav">
        <li class="selected"><a href="#tabSettings"><em>
            <asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
        <li><a href="#tabDescription"><em>
            <asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
        <li><a href="#tabSecurity"><em>
            <asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
    </ul>
    <div class="yui-content">
        <div id="tabSettings">
            <div class="settingrow">
            <cy:SiteLabel id="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel" ConfigKey="LetterInfoTitleLabel" ResourceFile="Resource" />
            <asp:TextBox ID="txtTitle"   runat="server" CssClass="widetextbox" />
            <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="letterinfotitlehelp" />		 
            </div>
            <div id="divFromName" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblFromName" runat="server" ForControl="txtFromName" CssClass="settinglabel" ConfigKey="LetterInfoFromNameLabel" ResourceFile="Resource" />
            <asp:TextBox ID="txtFromName"   runat="server" CssClass="widetextbox" />
            <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="letterinfofromnamehelp" />		 
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="lblFromAddress" runat="server" ForControl="txtFromAddress" CssClass="settinglabel" ConfigKey="LetterInfoFromAddressLabel" ResourceFile="Resource" />
            <asp:TextBox ID="txtFromAddress"  runat="server" CssClass="widetextbox" />
            <portal:CHelpLink ID="CynHelpLink2" runat="server" HelpKey="letterinfofromaddresshelp" />	 	
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="lblReplyToAddress" runat="server" ForControl="txtReplyToAddress" CssClass="settinglabel" ConfigKey="LetterInfoReplyToAddressLabel" ResourceFile="Resource" />
            <asp:TextBox ID="txtReplyToAddress"  runat="server" CssClass="widetextbox" />
            <portal:CHelpLink ID="CynHelpLink3" runat="server" HelpKey="letterinforeplytoaddresshelp" />		
            </div>
            <div id="divEnabled" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblEnabled" runat="server" ForControl="chkEnabled" CssClass="settinglabel" ConfigKey="LetterInfoEnabledLabel" ResourceFile="Resource" />
            <asp:CheckBox ID="chkEnabled"  runat="server" Checked="true" />
            <portal:CHelpLink ID="CynHelpLink5" runat="server" HelpKey="letterinfoenabledhelp" />	
            </div>
            <div id="divFeedback" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblAllowUserFeedback" runat="server" ForControl="chkAllowUserFeedback" CssClass="settinglabel" ConfigKey="LetterInfoAllowUserFeedbackLabel" ResourceFile="Resource" />
            <asp:CheckBox ID="chkAllowUserFeedback"  runat="server" Checked="true" />
            <portal:CHelpLink ID="CynHelpLink6" runat="server" HelpKey="letterinfoallowuserfeedbackhelp" />		
            </div>
            <div id="divAnonymousFeedback" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblAllowAnonFeedback" runat="server" ForControl="chkAllowAnonFeedback" CssClass="settinglabel" ConfigKey="LetterInfoAllowAnonFeedbackLabel" ResourceFile="Resource" />
            <asp:CheckBox ID="chkAllowAnonFeedback"  runat="server" Checked="true" />	
            <portal:CHelpLink ID="CynHelpLink7" runat="server" HelpKey="letterinfoallowanonfeedbackhelp" />	 
            </div>
            <div id="divViewAsWebPage" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblEnableViewAsWebPage" runat="server" ForControl="chkEnableViewAsWebPage" CssClass="settinglabel" ConfigKey="LetterInfoEnableViewAsWebPageLabel" ResourceFile="Resource" />
            <asp:CheckBox ID="chkEnableViewAsWebPage"  runat="server" Checked="true" />
            <portal:CHelpLink ID="CynHelpLink8" runat="server" HelpKey="letterinfoenableviewaswebpagehelp" />	 	
            </div>
            <div id="divSendLog" runat="server" visible="false" class="settingrow">
            <cy:SiteLabel id="lblEnableSendLog" runat="server" ForControl="chkEnableSendLog" CssClass="settinglabel" ConfigKey="LetterInfoEnableSendLogLabel" ResourceFile="Resource" />
            <asp:CheckBox ID="chkEnableSendLog"  runat="server" Checked="true" />	
            <portal:CHelpLink ID="CynHelpLink9" runat="server" HelpKey="letterinfoenablesendloghelp" />
            </div>
            <div class="settingrow" id="divSendMode" runat="server" visible="false">
            <cy:SiteLabel id="lblSendMode" runat="server" ForControl="txtSendMode" CssClass="settinglabel" ConfigKey="LetterInfoSendModeLabel" ResourceFile="Resource" />
            <asp:DropDownList ID="ddSendMode" runat="server">
            <asp:ListItem Text="Immediate" Value="Immediate" ></asp:ListItem>
            <asp:ListItem Text="QueueForService" Value="QueueForService" ></asp:ListItem>
            </asp:DropDownList>
            <portal:CHelpLink ID="CynHelpLink10" runat="server" HelpKey="letterinfosendmodehelp" />	
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="SiteLabel1" runat="server" ForControl="chkAllowArchiveView" CssClass="settinglabel" ConfigKey="NewsletterAllowArchiveView" ResourceFile="Resource" />
            <asp:CheckBox ID="chkAllowArchiveView"  runat="server" Checked="true" />	
            <portal:CHelpLink ID="CynHelpLink15" runat="server" HelpKey="newsletter-AllowArchiveView-help" />
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="SiteLabel2" runat="server" ForControl="chkProfileOptIn" CssClass="settinglabel" ConfigKey="NewsletterOptInNewUsers" ResourceFile="Resource" />
            <asp:CheckBox ID="chkProfileOptIn"  runat="server" Checked="true" />	
            <portal:CHelpLink ID="CynHelpLink16" runat="server" HelpKey="newsletter-ProfileOptIn-help" />
            </div>
            <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="NewsletterSortRank"
                    ResourceFile="Resource" ForControl="txtSortRank" />
                <asp:TextBox ID="txtSortRank" runat="server" Text="500" MaxLength="20" CssClass="smalltextbox" />
                <portal:CHelpLink ID="CynHelpLink17" runat="server" HelpKey="newsletter-SortRank-help" />
            </div>
            <div class="settingrow">
            <cy:SiteLabel id="SiteLabel3" runat="server"  CssClass="settinglabel" ConfigKey="spacer" ResourceFile="Resource" />
            </div>
        </div>
        <div id="tabDescription">
            <cye:EditorControl ID="edDescription" runat="server" />
        </div>
        <div id="tabSecurity">
            <ul class="yui-nav">
                <li class="selected"><a href="#tabSubscribeRoles"><em>
                    <asp:Literal ID="litSubscribeRolesTab" runat="server" /></em></a></li>
                <li id="liEditRoles" runat="server" visible="false"><a id="lnkEditRoles" runat="server" href="#tabEditRoles"><em>
                    <asp:Literal ID="litEditRolesTab" runat="server" /></em></a></li>
                <li id="liApproveRoles" runat="server" visible="false"><a id="lnkApproveRoles" runat="server" href="#tabApproveRoles"><em>
                    <asp:Literal ID="litApproveRolesTab" runat="server" /></em></a></li>
                <li id="liSendRoles" runat="server" visible="false"><a id="lnkSendRoles" runat="server" href="#tabSendRoles"><em><asp:Literal ID="litSendRolesTab" runat="server" /></em></a></li>
            </ul>
            <div id="tpLetterInfoSecurity" class="yui-content">
                <div id="tabSubscribeRoles" runat="server" CssClass="modulecontent">
                <asp:CheckBoxList id="chkListAvailableToRoles" runat="server" SkinID="Roles"></asp:CheckBoxList>
                <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="letterinfoavailabletoroleshelp" />	 
                </div>
                <div id="tabEditRoles" runat="server" CssClass="modulecontent" Visible="false">
                <asp:CheckBoxList id="chkListEditRoles" runat="server" SkinID="Roles"></asp:CheckBoxList>
                <portal:CHelpLink ID="CynHelpLink12" runat="server" HelpKey="letterinforolesthatcanedithelp" />		 
                </div>
                <div id="tabApproveRoles" runat="server" CssClass="modulecontent" Visible="false">
                <asp:CheckBoxList id="chkListApproveRoles" runat="server" SkinID="Roles"></asp:CheckBoxList>
                <portal:CHelpLink ID="CynHelpLink13" runat="server" HelpKey="letterinforolesthatcanapprovehelp" />		
                </div>
                <div id="tabSendRoles" runat="server" CssClass="modulecontent" Visible="false">
                <asp:CheckBoxList id="chkListSendMailRoles" runat="server" SkinID="Roles"></asp:CheckBoxList>	
                <portal:CHelpLink ID="CynHelpLink14" runat="server" HelpKey="letterinforolesthatcansendhelp" />	 
                </div>
                <div class="settingrow">
                <cy:SiteLabel id="SiteLabel5" runat="server"  CssClass="settinglabel" ConfigKey="spacer" ResourceFile="Resource" />
                </div>
            </div>
        </div>
    </div>
</div>
<div class="settingrow">
<portal:CButton ID="btnSave" runat="server" />
<portal:CButton ID="btnDelete" runat="server" />
</div>
<div class="settingrow">
<cy:SiteLabel id="lblLastModUTCLabel" runat="server" ForControl="txtLastModUTC" CssClass="settinglabel" ConfigKey="LetterInfoLastModifiedLabel" ResourceFile="Resource" />
<asp:Label ID="lblLastModified" runat="server" />	
</div>
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
