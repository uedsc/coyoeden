<%@ Page Title="" Language="C#" MasterPageFile="~/themes/Standard/site.master" Inherits="CoyoEden.UI.Views.PostDetail"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
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
<%=RenderPost("PostView") %>
<!--/Post body-->
<!--Post navigation-->
<div id="postnavigation">
<%if (null != ViewModel.Previous)
  {%>
  <%AddGenericLink("prev", ViewModel.Title, ViewModel.RelativeLink.ToString()); %>
  <span class="prev"><a href="<%=ViewModel.RelativeLink %>" title="<%=Resources.labels.previousPost %>"><%=Server.HtmlEncode(ViewModel.Previous.Title)%></a></span>
<%} %>
<%if (null != ViewModel.Next)
  {%>
  <%AddGenericLink("next", ViewModel.Title, ViewModel.RelativeLink.ToString()); %>
  <span class="next">--<a href="<%=ViewModel.RelativeLink %>" title="<%=Resources.labels.previousPost %>"><%=Server.HtmlEncode(ViewModel.Next.Title)%></a></span>
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
<div id="m300_right">

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