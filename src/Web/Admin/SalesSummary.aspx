<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SalesSummary.aspx.cs" Inherits="Cynthia.Web.AdminUI.SalesSummaryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper ">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<strong><asp:Literal ID="litAllTimeRevenue" runat="server" /></strong>
<div>
<div >
 <zgw:zedgraphweb id="zgSales" runat="server" RenderMode="ImageTag"
    Width="720" Height="300"></zgw:zedgraphweb>
</div>
<div class="floatpanel">
<div>&nbsp;</div>
<div class="floatpanel">
<cy:CGridView ID="grdSales" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
	 AutoGenerateColumns="false"
     EnableTheming="false"
	 SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <%# Eval("Y") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# Eval("M") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Sales"))) %>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
 </cy:CGridView>
<div>&nbsp;</div>
</div>

<div class="floatpanel">
<cy:CGridView ID="grdSalesByModule" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
     CssClass="yui-datatable yui-dt"
	 AutoGenerateColumns="false"
     EnableTheming="false"
	 SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <a href='<%# SiteRoot + "/Admin/SalesByModule.aspx?m=" + Eval("ModuleGuid") %>'><%# Eval("ModuleTitle") %></a>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Sales"))) %>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
 </cy:CGridView>
</div>

<div class="floatpanel">
<cy:CGridView ID="grdTopCustomers" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
	 AutoGenerateColumns="false"
     EnableTheming="false"
	 SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink Text='<%# Resources.Resource.ManageUserLink %>' id="Hyperlink2" 
						    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>' 
						    Visible="<%# IsAdmin %>" runat="server" />
                <%# Eval("Name") %><br />
                <%# Eval("LoginName") %> - <%# Eval("Email") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<a href='<%# SiteRoot + "/Admin/SalesCustomerDetail.aspx?u=" + Eval("UserGuid") %>'>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Sales"))) %></a>
            </ItemTemplate>
		</asp:TemplateField>
		
</Columns>
 </cy:CGridView>
<div><asp:HyperLink ID="lnkAllItems" runat="server" /></div>
<div>&nbsp;</div>
</div>

</div>

</div>
</div>
</portal:CPanel>
<div class="cleared"></div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
