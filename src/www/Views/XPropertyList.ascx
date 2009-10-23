<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.XPropertyAdminView"%>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="CoyoEden.Core" %>
<%@ Import Namespace="Vivasky.Core" %>
<% XPropertyList.ForEach(x=>{ %>
<div class="item-block">
	<div class="item-block-details">
		<h3 class="item-block-title"><a href="#" title="<%=x.Name %>"><%=x.Name %></a></h3>
		<p class="item-block-subtitle"><%=x.Description.TailStr(20,"...") %></p>
	</div>
</div>
<%}); %>