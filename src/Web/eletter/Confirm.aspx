<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Confirm.aspx.cs" Inherits="Cynthia.Web.ELetterUI.ConfirmPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
<asp:Panel ID="pnlConfirmed" runat="server" Visible="false">
<cy:SiteLabel id="SiteLabel1" runat="server"  ConfigKey="NewslettersSubscriptionConfirmed"> </cy:SiteLabel><br />
<asp:Literal ID="litConfrimDetails" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlNotFound" runat="server" Visible="true">
<cy:SiteLabel id="lblWarning" runat="server" CssClass="txterror" ConfigKey="NewslettersSubscriptionNotFound"> </cy:SiteLabel>
</asp:Panel>
</div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
