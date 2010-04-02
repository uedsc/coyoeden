<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContentStyles.aspx.cs" Inherits="Cynthia.Web.AdminUI.ContentStylesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
    <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper admin ">
<h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
        <div class="modulecontent yui-skin-sam">
            <div id="divPgrTop" runat="server" class="modulepager">
                <portal:CCutePager ID="pgrTop" runat="server" />
            </div>
            <cy:CGridView ID="grdStyles" runat="server" AllowPaging="false" AllowSorting="false" EnableModelValidation="true"
                AutoGenerateColumns="false"  DataKeyNames="Guid" EnableTheming="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.TaxClassGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.ContentStyleSave %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.ContentStyleDelete %>'
                                CommandName="Delete" />
                             <asp:HyperLink ID="lnkCancel" runat="server" Text='<%# Resources.Resource.ContentStyleCancel %>' NavigateUrl='<%# Request.RawUrl %>'></asp:HyperLink>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" CssClass="mediumtextbox" Text='<%# Eval("Name") %>'  MaxLength="100" />
                            <asp:RequiredFieldValidator ID="reqName" runat="server"  ControlToValidate="txtName"  ErrorMessage='<%# Resources.Resource.StyleNameRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Element")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtElement" runat="server" CssClass="smalltextbox" Text='<%# Eval("Element") %>'  MaxLength="50" />
                            <asp:RequiredFieldValidator ID="reqElement" runat="server"  ControlToValidate="txtElement"  ErrorMessage='<%# Resources.Resource.StyleElementRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("CssClass") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCssClass" runat="server" CssClass="smalltextbox" Text='<%# Eval("CssClass") %>'  MaxLength="50" />
                            <asp:RequiredFieldValidator ID="reqCssClass" runat="server"  ControlToValidate="txtCssClass"  ErrorMessage='<%# Resources.Resource.StyleCssClassRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("IsActive") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("IsActive") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            <asp:ValidationSummary ID="valSummary" runat="server"  />
            <div class="settingrow">
                <portal:CButton ID="btnAddNew" runat="server" />
            </div>
            <div id="divPgrBottom" runat="server" class="modulepager">
                <portal:CCutePager ID="pgrBottom" runat="server" />
            </div>
            <br class="clear" />
        </div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
