<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.ViewBase"%>
<%@ Import Namespace="SystemX" %>
<%if (Page.User.Identity.IsAuthenticated)
{ %>
<a title="" href="<%=Utils.AbsoluteWebRoot.ToString() %>login.aspx?logoff"><%=Resources.labels.logoff%></a>
<%}
else
{ %>
<a title="" href="<%=Utils.AbsoluteWebRoot.ToString() %>login.aspx"><%=Resources.labels.login%></a>
<%} %>
