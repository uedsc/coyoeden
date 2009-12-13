<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="CoyoEden.Core.Web.Controls.PostViewBase" %>
<%@ Import Namespace="Vivasky.Core" %>
<div class="post itemview xfolkentry">
	<a title="<%=Server.HtmlEncode(Post.Title) %>" href="<%=Post.RelativeLink %>" class="icon">
	<img alt="<%=Server.HtmlEncode(Post.Title) %>" src="http://img.xiami.com/./images/collect/647/47/6506471260025013_1.jpg" width="100" height="100"/>
	</a>
	<div class="item_main">
		<h4><a title="" href="<%=Post.RelativeLink %>"><%=Server.HtmlEncode(Post.Title) %></a></h4>
		<small class="author">
			作者：<a href=""<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Utils.RemoveIllegalCharacters(Post.Author) %>.aspx"><%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName??Post.Author : Post.Author %></a> @<%=Post.DateCreated.Value.ToString("d. MMMM yyyy HH:mm") %></small>
		<div class="brief text"><asp:PlaceHolder ID="BodyContent" runat="server" /></div>
	</div>
	<span class="counts"><a title="" href="/post/show.aspx?id=<%=Post.Id %>#c"><%=Post.ApprovedComments.Count %>个评论<em>|</em>0人推荐</a></span>
	<div class="bottom clearfix">
    <%=Rating %>
    <p class="tags">Tags: <%=TagLinks(", ") %></p>
    <p class="categories"><%=CategoryLinks(" | ") %></p>
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
