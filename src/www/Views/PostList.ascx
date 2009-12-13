<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="CoyoEden.UI.Views.PostListView" %>
<div id="posts" class="posts">
	<h3 class="Btitle">����ԭ������</h3>
	<div class="common_sec2">
	<%var ipost = 0; %>
	<%PostsToShow.ForEach(x => { %>
	<%=RenderSinglePost(x as CoyoEden.Core.Post,ipost) %>
	<%ipost++; %>
    <%}); %>
    </div>
    <span class="x_more"><a href="/post/list.aspx" title="��������">����</a></span>
</div>
<div id="postPaging">
<%if(!NothingToShow){%>
<%if (HasPreviousPosts)
  {%>
  <a id="hlPrev" style="float:left" href="<%=PreviousPageUrl %>">&lt;&lt; <%=Resources.labels.previousPosts %></a>
  <%} %>
  <%if (CurrentPage() > 0)
	{%>
  <a id="hlNext" style="float:right" href="<%=NextPageUrl%>"><%=Resources.labels.nextPosts %> &gt;&gt;</a>
  <%} %>
  <%} %>
</div>

