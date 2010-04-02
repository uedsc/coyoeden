<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserCommerceHistory.ascx.cs" Inherits="Cynthia.Web.UI.UserCommerceHistory" %>
<div class="yui-skin-sam">
<asp:UpdatePanel ID="updItems" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<cy:CGridView ID="grdUserItems" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
	 AutoGenerateColumns="false"
     EnableTheming="false"
	 SkinID="plain"
	 DisableYui="true"
	 >
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
			    <%# Eval("ModuleTitle") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			    <span id="spnAdmin" runat="server" visible='<%# ShowAdminOrderLink %>'>
			    <a href='<%# SiteRoot + Eval("AdminOrderLink") %>'><%# Eval("ItemName") %></a>
			    </span>
			    <span id="Span1" runat="server" visible='<%# !ShowAdminOrderLink %>'>
			    <a href='<%# SiteRoot + Eval("UserOrderLink") %>'><%# Eval("ItemName") %></a>
			    </span>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			    <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("SubTotal"))) %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.GetTimeZoneAdjustedShortDateString(Eval("OrderDateUtc"), timeOffset)%>
            </ItemTemplate>
     </asp:TemplateField>
		
</Columns>
 </cy:CGridView>
<div class="modulepager">
    <portal:CCutePager ID="pgrItems" runat="server" />
</div>
</ContentTemplate>
</asp:UpdatePanel>

</div>