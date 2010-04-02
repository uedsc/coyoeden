<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ContentManager.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentManagerPage" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlContentManager" runat="server" CssClass="panelwrapper admin contentmanager">
<div class="modulecontent">
<fieldset class="contentmanager">
   <legend>
    <asp:HyperLink ID="lnkAdminMenu" runat="server" />&nbsp;&gt;
    <asp:HyperLink ID="lnkContentManager" runat="server" />&nbsp;&gt;
	 <cy:SiteLabel id="lbl1" runat="server" ConfigKey="ContentManagerPublishContentLabel"> </cy:SiteLabel>
	 <asp:Label ID="lblModuleTitle" runat="server" />
	 <asp:HyperLink Text="Settings" id="lnkModuleSettings" Visible="False" runat="server" />
	 <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="contentmanagerpublishpagehelp" />
     <small><asp:hyperlink id="lnkEdit" cssclass="ModuleEditLink" EnableViewState="false" runat="server" SkinID="plain" />
     &nbsp;<asp:HyperLink ID="lnkBackToList" runat="server" Visible="false" cssclass="ModuleEditLink" SkinID="plain"></asp:HyperLink></small>
   </legend>
   <asp:Panel ID="pnlWarning" runat="server" Visible="false">
 <cy:SiteLabel id="SiteLabel1" runat="server" CssClass="txterror" ConfigKey="ContentManagerNoReuseWarning" UseLabelTag="false" > </cy:SiteLabel>
 </asp:Panel>
<cy:CGridView ID="grdPages" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="true"
     DataKeyNames="PageID" SkinID="plain">
     <Columns>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="left">
            <ItemTemplate>
                <%# Eval("DepthIndicator")%><%# Eval("PageName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:ImageButton ID="btnEditPublish" runat="server" 
                    CommandName="Edit" 
                    ToolTip='<%# GetIsPublishedImageAltText(Eval("IsPublished"))%>'
                    AlternateText='<%# GetIsPublishedImageAltText(Eval("IsPublished"))%>'
                    ImageUrl='<%# GetIsPublishedImageUrl(Eval("IsPublished"))%>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# WebUtils.NullToFalse(Eval("IsPublished")) %>' />
                <br />
                <asp:Button id="btnGridUpdate" runat="server" Text='<%# GetUpdateButtonText() %>' CommandName="Update" />
                <asp:Button id="btnGridCancel" runat="server" Text='<%# GetCancelButtonText() %>' CommandName="Cancel" />
            
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <%# GetPaneAlias(Eval("PaneName"))%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList  ID="ddPaneNames" runat="server" DataTextField="key" DataValueField="value" 
                DataSource='<%# PaneList() %>' SelectedValue='<%# GetPaneName(Eval("PaneName")) %>'>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <%# Eval("ModuleOrder")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtModuleOrder" Columns="4" Text='<%# GetModuleOrder(Eval("ModuleOrder")) %>' runat="server" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# Eval("PublishBeginDate")%>
            </ItemTemplate>
            <EditItemTemplate>
                <cy:DatePickerControl id="dpBeginDate" runat="server" Text='<%# GetBeginDate(Eval("PublishBeginDate")) %>' ShowTime="True"> </cy:DatePickerControl>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# Eval("PublishEndDate")%>
            </ItemTemplate>
            <EditItemTemplate>
                <cy:DatePickerControl id="dpEndDate" runat="server" Text='<%# GetEndDate(Eval("PublishEndDate")) %>' ShowTime="True"> </cy:DatePickerControl>
            </EditItemTemplate>
        </asp:TemplateField>
      
     </Columns>
 </cy:CGridView><br />
<portal:CButton  id="btnDelete" runat="server" Text="" CausesValidation="false" />
</fieldset>
</div>
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
