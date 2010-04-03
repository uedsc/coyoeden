<%@ Control Language="C#" CodeBehind="XLoginLink.ascx.cs" Inherits="Cynthia.Web.Controls.XLoginLink" %>
<%@ Import Namespace="Resources" %>
<li>
<% if(IsAuthenticated){%>
<% if(AuthType=="Forms"){%>
	<a href="<%=Href %>" title="" class="sprite input-link-dark"><span class="sprite"><%=Resource.LogoutLink %></span></a>
<%} %>
<%}else{ %>
	<a href="<%=Href %>" title="" class="sprite input-link-dark"><span class="sprite"><%=Resource.LoginLink %></span></a>
<%} %>
</li>