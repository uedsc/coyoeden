<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatsControl.ascx.cs" Inherits="Cynthia.Web.BlogUI.StatsControl" %>
<h3><cy:sitelabel id="Sitelabel3" runat="server" ConfigKey="BlogStatisticsLabel" ResourceFile="BlogResources" UseLabelTag="false"> </cy:sitelabel></h3>
<ul class="blognav">
<li><asp:Literal id="litEntryCount" Runat="server" /></li>
<li id="liComments" runat="server"><asp:Literal id="litCommentCount" Runat="server" /></li>
</ul>
