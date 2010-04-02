<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SalesItemReport.aspx.cs" Inherits="Cynthia.Web.AdminUI.SalesItemReportPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkCommerceReports" runat="server" NavigateUrl="~/Admin/SalesSummary.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkModuleReport" runat="server" NavigateUrl="~/Admin/SalesByModule.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<div class="modulecontent">
<cy:CGridView ID="grdItems" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
	 AutoGenerateColumns="false"
     EnableTheming="false"
	 SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <a href='<%# SiteRoot + "/Admin/SalesByItem.aspx?i=" + Eval("ItemGuid") %>'><%# Eval("ItemName")%></a>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# Eval("UnitsSold")%>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Revenue"))) %>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
 </cy:CGridView>
<div class="modulepager">
    <portal:CCutePager ID="pgrItems" runat="server" />
</div>

</div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
