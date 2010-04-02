<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingPages.aspx.cs" 
    MasterPageFile="~/App_MasterPages/layout.Master" Inherits="Cynthia.Web.AdminUI.PendingPagesPage" %>
    
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
 <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkContentWorkFlow" runat="server"  NavigateUrl="~/Admin/ContentWorkflow.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" />    
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper admin workflow">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
   <asp:literal id="ltlIntroduction" runat="server"></asp:literal>
   <cy:CGridView ID="grdPendingPages" runat="server" AllowPaging="false" AllowSorting="false"
        AutoGenerateColumns="false"  DataKeyNames="PageGuid" EnableTheming="false">
        <Columns>            
            <asp:TemplateField>
                <ItemTemplate>
                    <a href='<%# SiteRoot + Eval("Url").ToString().Replace("~/","/")%>'><%# Eval("PageName")%></a>
                </ItemTemplate>                                  
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("WipCount")%>
                </ItemTemplate>                                
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <a href='<%# SiteRoot + "/Admin/PageSettings.aspx"%>?pageid=<%# Eval("PageID")%>'><%= Resources.Resource.PageSettingsLabel %></a>                            
                 </ItemTemplate>
            </asp:TemplateField>
                   
        </Columns>
    </cy:CGridView>
    <div class="modulepager">
        <portal:CCutePager ID="pgrPendingPages" runat="server" />
    </div>
    </div>
    </portal:CPanel>
    <div class="cleared"></div>
    </asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />
</portal:CPanel>
<portal:SessionKeepAliveControl id="ka1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

