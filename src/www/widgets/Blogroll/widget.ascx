<%@ Import namespace="CoyoEden.Core"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_BlogRoll_widget" %>
 <vs:Blogroll ID="Blogroll1" runat="server" />
  <a href="<%=SystemX.Utils.AbsoluteWebRoot %>opml.axd" style="display:block;text-align:right" title="Download OPML file" >Download OPML file <img src="<%=SystemX.Utils.AbsoluteWebRoot %>pics/opml.png" alt="OPML" /></a>
