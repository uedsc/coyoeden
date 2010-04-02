<%@ Page Language="C#" ClassName="BlogView.aspx" Inherits="mojoPortal.Web.BlogUI.BlogView" MasterPageFile="~/App_MasterPages/layout.Master"  %>
<%@ Register TagPrefix="blog" TagName="BlogView" Src="~/Blog/Controls/BlogViewControl.ascx" %>
<%-- 

this page is here for legacy backward compatibility support for existing installations prior to moving this feature to mojoPortal.Features projects  
if you installed mojoportal after version 2.2.7.8, then you can safely delete this file

--%>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:ModulePanel ID="pnlContainer" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server"  />
<blog:BlogView id="BlogView1" runat="server" />
<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:ModulePanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />



