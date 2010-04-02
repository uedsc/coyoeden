<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="OfferProductSelectorDialog.aspx.cs" Inherits="WebStore.UI.OfferProductSelectorDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<div  style="padding: 5px 5px 5px 5px;" class="productselector">
<asp:UpdatePanel ID="pnlPicker" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="pnlGrid" runat="server">
        <cy:CGridView ID="grdProduct" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false" 
            DataKeyNames="Guid" EnableTheming="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("ModelNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                    <asp:Button ID="btnAddProduct" runat="server" Text='<%# Resources.WebStoreResources.AddProductToOfferGridButton %>' CommandName="addProduct" 
                    CommandArgument='<%# Eval("Guid").ToString() + "|" + Eval("FulFillmentType").ToString() %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cy:CGridView>
        <div class="modulepager">
           <portal:CCutePager ID="pgrProduct" runat="server" />
        </div>
        <br class="clear" />
        <asp:Label ID="lblMessage" runat="server" CssClass="txterror" />
  </asp:Panel>
  <asp:Panel ID="pnlDownloadTerms" runat="server" Visible="false">
  <div class="settingrow">
    <cy:SiteLabel ID="lblProductGuid" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridNameHeader"
        ResourceFile="WebStoreResources" />
    <asp:Label ID="lblProduct" runat="server" />
</div>
  <div class="settingrow">
    <cy:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="OfferProductGridFullfillmentTermsHeader"
        ResourceFile="WebStoreResources" />
    <asp:DropDownList ID="ddFulfillTerms" runat="server" DataValueField="Guid" DataTextField="Name" CssClass="forminput" />
    <asp:HiddenField ID="hdnProductGuid" runat="server" />
    <asp:HiddenField ID="hdnFulfillmentType" runat="server" />
    <asp:Button ID="btnAddWithDownloadTerms" runat="server" />
</div>
  </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</div>
</asp:Content>
