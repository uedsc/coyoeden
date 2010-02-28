<%@ Page Title="" Language="C#" Inherits="CoyoEden.UI.Views.PostDetail"%>
<%@ MasterType TypeName="CoyoEden.UI.SiteMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server">
<% this.Master.CssClass = UserIsAdmin ? "admin postdetail" : "postdetail"; %>
<vs:SiteCssX ID="csspostshow" CSSPathInTheme="post.show.css" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphChannelTab" Runat="Server">
<ul>
<%var i = 0; %>
<%ViewModel.Categories.ForEach(x =>
  { %>
  <li><a class="<%=i==0?"tab_cur":"" %>" title="" href="<%=x.RelativeLink %>"><span><%=x.Name %></span></a></li>
  <%i++; %>
<%}); %>
<li><h2>/ <%=ViewModel.Title %></h2></li>
</ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="m300_right">
<div id="post_info" class="sec_side">
<p class="icon">
<img src="<%=Icon %>" alt="<%=ViewModel.Title %>">
</p>
<h4><%=ViewModel.Title %></h4>
<div class="userinfo">
	<ul>
		<li><span>作者：</span><a href="<%=Utils.AbsoluteWebRoot %>author/<%=ViewModel.Author %>.aspx" title=""><%=ViewModel.Author %></a></li>
		<li><span>分类：</span><%=CategoryLinks("|") %></li>
		<li><span>分类：</span><%=TagLinks("|") %></li>
		<li><span>发布时间：</span><%=ViewModel.DateCreated %></li>
	</ul>
</div>	
</div>
</div>
<div id="m300_left">
<!--Notification Unsubscription for comments-->
<%if (NotificationUnsubscription)
  { %>
<div id="commentNotificationUnsubscription">
    <h1><%= Resources.labels.commentNotificationUnsubscriptionHeader%></h1>
    <div><%= Resources.labels.commentNotificationUnsubscriptionText%></div>
</div>
<%} %>
<!--Post body-->
<%=RenderPost("PostView",true) %>
<!--/Post body-->
<!--Post navigation-->
<div id="postnavigation" class="clearfix">
<%if (null != ViewModel.Previous)
  {%>
  <%AddGenericLink("prev", ViewModel.Title, ViewModel.RelativeLink.ToString()); %>
  <span class="prev"><a href="<%=ViewModel.RelativeLink %>" title="<%=Resources.labels.previousPost %>"><%=Server.HtmlEncode(ViewModel.Previous.Title)%></a></span>
<%} %>
<%if (null != ViewModel.Next)
  {%>
  <%AddGenericLink("next", ViewModel.Title, ViewModel.RelativeLink.ToString()); %>
  <span class="next"><a href="<%=ViewModel.RelativeLink %>" title="<%=Resources.labels.previousPost %>"><%=Server.HtmlEncode(ViewModel.Next.Title)%></a></span>
<%} %>
</div>
<!--BlogSettings.Instance.EnableTrackBackReceive-->
<%if (BlogSettings.Instance.EnableTrackBackReceive)
  { %>
    <!-- 
    <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:trackback="http://madskills.com/public/xml/rss/module/trackback/">
      <rdf:Description rdf:about="<%=ViewModel.AbsoluteLink %>" dc:identifier="<%=ViewModel.AbsoluteLink %>" dc:title="<%=ViewModel.Title %>" trackback:ping="<%=ViewModel.TrackbackLink %>" />
    </rdf:RDF>
    --> 
<%} %>  
<%if (BlogSettings.Instance.EnableRelatedPosts)
  { %>
  <%=RenderRelatedPost(3,true,200)%>
<%} %>
<%=RenderCommentsView("CommentList") %>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
<vs:SiteJScript ID="jsValidate" runat="server" ScriptRelativeToRoot="Assets/js/jquery/jquery.validate.min.js" />
<vs:SiteJScript ID="appjs" runat="server" ScriptRelativeToRoot="Assets/js/local/post.show.js" />
<script type="text/javascript">
    //<![CDATA[
    this$.Init({
        webRoot: WebRoot,
        admin: '<%=UserIsAdmin?1:0 %>'
    });
    //]]>
</script>
</asp:Content>