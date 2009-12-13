<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="CoyoEden.Core.Web.Controls.PostViewBase" %>
<%@ Import Namespace="Vivasky.Core" %>
<div class="excerpt itemview clearfix" id="<%=ID %>">
	<div class="intro">
		<a href="<%=Post.RelativeLink %>" title="<%=Post.Title %>" class="icon"><img width="185" height="185" src="http://img.xiami.com/./images/upload//1259299435_2.jpg" alt="" /></a>
		<h4>
			<a href="<%=Post.RelativeLink %>"><%=Server.HtmlEncode(Post.Title) %> </a>
		</h4>
		<small><%=Post.DateCreated.Value.ToString("d. MMMM yyyy HH:mm") %> by 
			<a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Utils.RemoveIllegalCharacters(Post.Author) %>.aspx" title="Posts by <%=Post.Author %>"><%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName??Post.Author : Post.Author %></a> in 
			<%=CategoryLinks(" | ")%>
		</small>
		<p class="text">
			<%=Body %>
		</p>
	</div>
	<div class="bottom clearfix">
		<%=Rating %>
		<p class="tags">Tags: <%=TagLinks(", ") %></p>
	</div>
  <div class="footer">    
    <div class="bookmarks">
      <a rel="nofollow" title="Index <%=Index %>" target="_blank" href="http://www.dotnetkicks.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">Submit to DotNetKicks...</a>
    </div>
    <%=AdminLinks %>
    <a rel="bookmark" href="<%=Post.PermaLink %>" title="<%=Server.HtmlEncode(Post.Title) %>">Permalink</a> |
    <a rel="nofollow" href="<%=Post.RelativeLink %>#comment"><%=Resources.labels.comments %> (<%=Post.ApprovedComments.Count %>)</a>
  </div>
</div>
