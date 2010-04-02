<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ContactForm.ascx.cs" Inherits="Cynthia.Web.ContactUI.ContactForm" %>
<portal:ModulePanel ID="pnlContainer" runat="server">
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper contactform">
<div class="modulecontent">
<fieldset>
    <legend>	
        <portal:ModuleTitleControl id="Title1" runat="server" UseHeading="false" UseHorizontalRule="false" />
    </legend>
    <asp:Panel ID="pnlSend" runat="server" SkinID="plain" DefaultButton="btnSend">
    
        <asp:Panel ID="pnlToAddresses" runat="server" Visible="false" CssClass="settingrow">
            <cy:SiteLabel id="SiteLabel1" runat="server" ForControl="ddToAddresses" ConfigKey="ToLabel" ResourceFile="ContactFormResources" CssClass="settinglabel"> </cy:SiteLabel>
		    <asp:DropDownList ID="ddToAddresses" runat="server" CssClass="forminput" EnableViewState="true" EnableTheming="false"></asp:DropDownList>
        </asp:Panel>
		<div class="settingrow">
		    <cy:SiteLabel id="lblEmail" runat="server" ForControl="txtEmail" ConfigKey="ContactFormYourEmailLabel" ResourceFile="ContactFormResources" CssClass="settinglabel"> </cy:SiteLabel>
		    <asp:TextBox id="txtEmail" runat="server" CssClass="forminput NormalTextBox" Columns="50" maxlength="50"></asp:TextBox>
		</div>
		<div class="settingrow">
		    <cy:SiteLabel id="lblName" runat="server" ForControl="txtName" ConfigKey="ContactFormYourNameLabel" ResourceFile="ContactFormResources" CssClass="settinglabel"> </cy:SiteLabel>
		    <asp:TextBox id="txtName" runat="server" cssclass="forminput NormalTextBox" Columns="50" maxlength="100"></asp:TextBox>
		</div>
		<div class="settingrow">
		    <cy:SiteLabel id="lblSubject" runat="server" ForControl="txtSubject" ConfigKey="ContactFormSubjectLabel" ResourceFile="ContactFormResources" CssClass="settinglabel"> </cy:SiteLabel>
		    <asp:TextBox id="txtSubject" runat="server" cssclass="forminput NormalTextBox" Columns="50" MaxLength="50"></asp:TextBox>
		</div>
		<div class="settingrow">
		 <cy:SiteLabel id="lblMessageLabel" runat="server" ForControl="fckMessage" ConfigKey="ContactFormMessageLabel" ResourceFile="ContactFormResources" CssClass="settinglabel"> </cy:SiteLabel>
		</div>
		<div class="settingrow">
		    <cye:EditorControl id="edMessage" runat="server"> </cye:EditorControl>
		</div>
		<div class="settingrow">
		    <asp:ValidationSummary id="vSummary" runat="server" ValidationGroup="Contact" CssClass="txterror"></asp:ValidationSummary>
		    <asp:RequiredFieldValidator id="reqEmail" ValidationGroup="Contact" runat="server" CssClass="txterror" Display="none" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
		    <asp:RegularExpressionValidator ID="regexEmail" runat="server" Display="none" ValidationGroup="Contact" ControlToValidate="txtEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"></asp:RegularExpressionValidator>
		</div>
		<div class="settingrow" id="divCaptcha" runat="server">
           <cy:CaptchaControl id="captcha" runat="server" />
         </div>
		<div class="modulebuttonrow">
		    <portal:CButton ID="btnSend" Runat="server" ValidationGroup="Contact" Text="Send" CausesValidation="true" />
		</div>
    
    </asp:Panel>
    <portal:CLabel ID="lblMessage" Runat="server" CssClass="txterror" />
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
</portal:ModulePanel>
