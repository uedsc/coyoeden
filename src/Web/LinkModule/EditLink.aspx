<%@ Page Language="c#" CodeBehind="EditLink.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.LinksUI.EditLinks" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper linksmodule">
        <asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="updateButton">
            <fieldset class="linksedit">
                <legend>
                    <cy:SiteLabel ID="lblLinkDetails" runat="server" ConfigKey="EditLinksDetailsLabel"
                        ResourceFile="LinkResources" UseLabelTag="false"> </cy:SiteLabel>
                </legend>
                <ol class="formlist">
                <li class="settingrow">
                    <cy:SiteLabel ID="lblDescription" runat="server" ForControl="fckDescription" ConfigKey="EditLinksDescriptionLabel"
                        ResourceFile="LinkResources" CssClass="settinglabel"> </cy:SiteLabel>
                </li>
                <li class="settingrow">
                    <cye:EditorControl ID="edDescription" runat="server">
                    </cye:EditorControl>
                </li>
                <li class="settingrow">
                    <cy:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" ConfigKey="EditLinksTitleLabel"
                        ResourceFile="LinkResources" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" Columns="65" CssClass="forminput verywidetextbox"></asp:TextBox>
                </li>
                <li class="settingrow">
                    <cy:SiteLabel ID="lblUrlLabel" runat="server" ForControl="txtUrl" ConfigKey="EditLinksUrlLabel"
                        ResourceFile="LinkResources" CssClass="settinglabel"> </cy:SiteLabel>
                    <div class="forminput">
                        <asp:DropDownList ID="ddProtocol" runat="server" EnableTheming="false">
                            <asp:ListItem Text="http://" Value="http://"></asp:ListItem>
                            <asp:ListItem Text="https://" Value="https://"></asp:ListItem>
                            <asp:ListItem Text="~/" Value="~/"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" Columns="65" CssClass="verywidetextbox "></asp:TextBox>
                    </div>
                </li>
                <li class="settingrow">
                    <cy:SiteLabel ID="lblViewOrder" runat="server" ForControl="txtViewOrder" ConfigKey="EditLinksViewOrderLabel"
                        ResourceFile="LinkResources" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtViewOrder" runat="server" MaxLength="10"  Text="500"
                        CssClass="forminput smalltextbox"></asp:TextBox>
                </li>
                <li class="settingrow">
                    <cy:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkUseNewWindow" ConfigKey="LinksUseNewWindowLabel"
                        ResourceFile="LinkResources" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkUseNewWindow" runat="server" CssClass="forminput" />
                </li>
                
                
                <li class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror"></asp:ValidationSummary>
                    <portal:CLabel ID="lblMessage" runat="server" CssClass="txterror"  />
                    <asp:RequiredFieldValidator ID="reqTitle" runat="server" CssClass="txterror" ControlToValidate="txtTitle"
                        ErrorMessage="" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="reqUrl" runat="server" CssClass="txterror" ControlToValidate="txtUrl"
                        ErrorMessage="" Display="none"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="reqViewOrder" runat="server" CssClass="txterror"
                        ControlToValidate="txtViewOrder" ErrorMessage="" Display="none"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="VerifyViewOrder" runat="server" CssClass="txterror" ControlToValidate="txtViewOrder"
                        ErrorMessage="" Display="None" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                </li>
                <li class="settingrow ">
                    <div class="forminput">
                        <portal:CButton ID="updateButton" runat="server" Text="Update" />&nbsp;
                        <portal:CButton ID="deleteButton" runat="server" Text="Delete this item" CausesValidation="false" />&nbsp;
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />
                        
                    </div>
                </li>
                </ol>
            </fieldset>
        </asp:Panel>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
