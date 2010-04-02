<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuthorizeNetLogList.ascx.cs" Inherits="Cynthia.Web.UI.AuthorizeNetLogList" %>

<asp:Panel ID="pnlCheckoutLog" runat="server" CssClass="checkoutlog yui-skin-sam">
    <h2 class="heading"><asp:Literal ID="litHeading" runat="server" /></h2>
    <cy:CGridView ID="grdCheckoutLog" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        CssClass="" 
        AutoGenerateColumns="false"
        DataKeyNames="RowGuid" 
        EnableTheming="false" 
        SkinID="plain">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TransactionType") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TransactionId") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Method")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("ResponseCode") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Reason") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("AuthCode") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("CreatedUtc") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cy:CGridView>
    <div class="modulepager">
        <portal:CCutePager ID="pgrCheckoutLog" runat="server" />
    </div>
    <br class="clear" />
    
</asp:Panel>
