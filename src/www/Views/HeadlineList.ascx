<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.HeadlineListView"%>
<div class="headlines" id="headlines">
	<h3>本期精选主题</h3>
	<!--render HeadlineView.ascx-->
	<%var ipost = 0; %>
	<%HeadlinesToShow.ForEach(x =>
	{ %>
		<%= RenderSinglePost(x,ipost)%>
		<%ipost++; %>
	<%}); %>
</div>