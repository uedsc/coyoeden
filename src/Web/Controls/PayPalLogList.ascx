<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayPalLogList.ascx.cs" Inherits="Cynthia.Web.UI.PayPalLogList" %>

<asp:Panel ID="pnlCheckoutLog" runat="server" CssClass="checkoutlog yui-skin-sam">
    <h2 class="heading"><asp:Literal ID="litHeading" runat="server" /></h2>
    <cy:CGridView ID="grdCheckoutLog" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        CssClass="" AutoGenerateColumns="false"
        DataKeyNames="RowGuid" EnableTheming="false" SkinID="plain" >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("RequestType") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("PaymentType") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("PaymentStatus") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TransactionId") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("CartTotal") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TaxAmt") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("PayPalAmt") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("FeeAmt") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("SettleAmt") %>
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
