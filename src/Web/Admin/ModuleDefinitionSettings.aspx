<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ModuleDefinitionSettings.aspx.cs" Inherits="Cynthia.Web.AdminUI.ModuleDefinitionSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlDefs" runat="server" CssClass="panelwrapper admin moduledefinitionsettings">
<div class="modulecontent">
<fieldset id="pnlExistingSettings" runat="server" class="moduledefinitionsettings">
   <legend>
   <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
    <asp:HyperLink ID="lnkModuleAdmin" runat="server" NavigateUrl="~/Admin/ModuleAdmin.aspx" />&nbsp;&gt;
    <asp:HyperLink ID="lnkModuleDefinition" runat="server"  />&nbsp;&gt;
    <cy:SiteLabel id="lbl1" runat="server" ConfigKey="ModuleDefinitionsSettingsLabel"> </cy:SiteLabel>
   </legend>
   <cy:CGridView ID="grdSettings" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="true"
     DataKeyNames="DefSettingID"
     GridLines="None"
     ShowHeader="false">
     <Columns>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" 
                    CommandName="Edit"
                    AlternateText='<%# GetEditImageAltText()%>'
                    ToolTip='<%# GetEditImageAltText()%>'
                    ImageUrl='<%# GetEditImageUrl()%>' />
            </ItemTemplate>
            <EditItemTemplate>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# Eval("SettingName") %>
            </ItemTemplate>
            <EditItemTemplate>
                <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel9" runat="server" ForControl="txtResourceFile" ConfigKey="ModuleDefinitionsResourceFileLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtResourceFile" Text='<%# Bind("ResourceFile")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
                 </div>
                 <div class="settingrow">
                    <cy:SiteLabel id="lbl1" runat="server" ForControl="txtSettingName" ConfigKey="ModuleDefinitionsSettingNameLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtSettingName" Text='<%# Bind("SettingName")%>' runat="server" MaxLength="50" Columns="60" CssClass="forminput" />
                 </div>
                 <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel2" runat="server" ForControl="ddControlType" ConfigKey="ModuleDefinitionsSettingControlTypeLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:DropDownList ID="ddControlType" runat="server" SelectedValue='<%# Bind("ControlType") %>' CssClass="forminput">
                        <asp:ListItem Value="TextBox" Text="TextBox" />
                        <asp:ListItem Value="CheckBox" Text="CheckBox" />
                        <asp:ListItem Value="ISettingControl" Text="ISettingControl" />
                    </asp:DropDownList>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel3" runat="server" ForControl="txtControlSrc" ConfigKey="ModuleDefinitionsSettingControlSrcLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtControlSrc" runat="server" Text='<%# Bind("ControlSrc")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
                <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel33" runat="server" ForControl="txtSettingValue" ConfigKey="ModuleDefinitionsSettingValueLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtSettingValue" runat="server" Text='<%# Bind("SettingValue")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel10" runat="server" ForControl="txtSortOrder" ConfigKey="ModuleDefinitionsSettingSortOrderLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Bind("SortOrder")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel12" runat="server" ForControl="txtHelpKey" ConfigKey="ModuleDefinitionsSettingHelpKeyLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtHelpKey" runat="server" Text='<%# Bind("HelpKey")%>' MaxLength="255" Columns="60" CssClass="forminput" />
               </div>
               <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel4" runat="server" ConfigKey="ModuleDefinitionsSettingRegexExpressionLabel" CssClass="settinglabel"> </cy:SiteLabel>
                    <asp:TextBox ID="txtRegexValidationExpression" runat="server"
                        Text='<%# Bind("RegexValidationExpression")%>' 
                        Rows="4" Columns="60" TextMode="MultiLine" />
               </div>
                <div class="settingrow">
                    <cy:SiteLabel id="SiteLabel38" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                   <asp:Button id="btnGridUpdate" runat="server" Text='<%# GetUpdateButtonText() %>' CommandName="Update" />
                    <asp:Button id="btnGridCancel" runat="server" Text='<%# GetCancelButtonText() %>' CommandName="Cancel" />
                    <asp:Button id="btnGridDelete" runat="server" Text='<%# GetDeleteButtonText() %>' CommandName="Delete" />
                 </div>
            </EditItemTemplate>
        </asp:TemplateField>
     </Columns>
 </cy:CGridView>

</fieldset>

<fieldset>
    <legend>
        <cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="ModuleDefinitionsAddSettingHeader"> </cy:SiteLabel>
    </legend>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel9" runat="server" ForControl="txtNewResourceFile" ConfigKey="ModuleDefinitionsResourceFileLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewResourceFile" Text='<%# Bind("ResourceFile")%>' runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel5" runat="server" ForControl="txtNewSettingName" ConfigKey="ModuleDefinitionsSettingNameLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewSettingName" runat="server" MaxLength="50" Columns="60" CssClass="forminput" />
   </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel6" runat="server" ForControl="ddNewControlType" ConfigKey="ModuleDefinitionsSettingControlTypeLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:DropDownList ID="ddNewControlType" runat="server" CssClass="forminput">
            <asp:ListItem Value="TextBox" Text="TextBox" />
            <asp:ListItem Value="CheckBox" Text="CheckBox" />
        </asp:DropDownList>
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel7" runat="server" ForControl="txtNewControlSrc" ConfigKey="ModuleDefinitionsSettingControlSrcLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewControlSrc" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
     <div class="settingrow">
        <cy:SiteLabel id="SiteLabel77" runat="server" ForControl="txtNewSettingValue" ConfigKey="ModuleDefinitionsSettingValueLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewSettingValue" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel11" runat="server" ForControl="txtNewSortOrder" ConfigKey="ModuleDefinitionsSettingSortOrderLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewSortOrder" runat="server" Text="500" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <cy:SiteLabel id="SiteLabel13" runat="server" ForControl="txtNewHelpKey" ConfigKey="ModuleDefinitionsSettingHelpKeyLabel" CssClass="settinglabel"> </cy:SiteLabel>
        <asp:TextBox ID="txtNewHelpKey" runat="server" MaxLength="255" Columns="60" CssClass="forminput" />
    </div>
     <div class="settingrow">
            <cy:SiteLabel id="SiteLabel8" runat="server" ForControl="txtNewRegexValidationExpression" ConfigKey="ModuleDefinitionsSettingRegexExpressionLabel" CssClass="settinglabel"> </cy:SiteLabel>
            <asp:TextBox ID="txtNewRegexValidationExpression" runat="server" Rows="4" Columns="60"
                TextMode="MultiLine" CssClass="forminput" />
     </div>
      <div class="settingrow">
        <cy:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <portal:CButton ID="btnCreateNewSetting" runat="server" Text='' />
            <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="moduledefinitionsettingshelp" />
       </div>
</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
