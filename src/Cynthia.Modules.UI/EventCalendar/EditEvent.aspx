<%@ Page language="c#" Codebehind="EditEvent.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.EventCalendarUI.EventCalendarEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate" CssClass="eventcalendaredit">
<div class="modulecontent">
	<fieldset>
        <legend>
	        <cy:SiteLabel id="lblEntry" runat="server" ConfigKey="EventCalendarEditEntryLabel" ResourceFile="EventCalResources" UseLabelTag="false"> </cy:SiteLabel>
	    </legend>
	    <div class="eventcal">
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel3" runat="server" ForControl="fckDescription" ConfigKey="EventCalendarEditDescriptionLabel" ResourceFile="EventCalResources" CssClass="settinglabel"  />
	        </div>
	        <div class="settingrow">
	            <cye:EditorControl id="edContent" runat="server"> </cye:EditorControl>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel" ConfigKey="EventCalendarEditTitleLabel" ResourceFile="EventCalResources" > </cy:SiteLabel>
	            <asp:TextBox id="txtTitle" runat="server"  Columns="50" maxlength="100" CssClass="forminput widetextbox"></asp:TextBox>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="lblStartDate" runat="server" ForControl="dpEventDate" CssClass="settinglabel" ConfigKey="EventCalendarEditEventDateLabel" ResourceFile="EventCalResources" > </cy:SiteLabel>			
	            <cy:DatePickerControl id="dpEventDate" runat="server" ShowTime="False" CssClass="forminput"> </cy:DatePickerControl>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel4" runat="server" ForControl="ddStartTime" CssClass="settinglabel" ConfigKey="EventCalendarEditStartTimeLabel" ResourceFile="EventCalResources" > </cy:SiteLabel>
	            <asp:DropDownList ID="ddStartTime" runat="server" EnableTheming="false" CssClass="forminput"></asp:DropDownList>
	        </div>
	        <div class="settingrow">
	            <cy:SiteLabel id="Sitelabel2" ForControl="ddEndTime" runat="server" CssClass="settinglabel" ConfigKey="EventCalendarEditEndTimeLabel" ResourceFile="EventCalResources" > </cy:SiteLabel>
	            <asp:DropDownList ID="ddEndTime" runat="server" EnableTheming="false" CssClass="forminput"></asp:DropDownList>
	        </div>
	        <div class="settingrow">
		        <cy:SiteLabel id="SiteLabel1" runat="server" ForControl="txtLocation" CssClass="settinglabel" ConfigKey="LocationLabel" ResourceFile="EventCalResources" > </cy:SiteLabel>
		        <asp:TextBox id="txtLocation" runat="server"  Columns="50" maxlength="100" CssClass="forminput widetextbox"></asp:TextBox>
		    </div>
	        <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
	            <portal:CButton id="btnUpdate" runat="server" Text="Update" />&nbsp;
			    <portal:CButton  id="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />&nbsp;
			    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;	
			    <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="eventedithelp" />
			    </div>
	        </div>
		    
		</div>
</fieldset>		
</div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>	
<cy:CornerRounderBottom id="cbottom1" runat="server" />		
<portal:SessionKeepAliveControl id="ka1" runat="server" />	
		
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
