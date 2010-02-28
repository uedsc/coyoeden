<%@ Control Language="C#" AutoEventWireup="true"  Inherits="CoyoEden.UI.Views.CommentListView" %>
<%@ Import Namespace="SystemX.Web" %>
<% if (CurrentPost.Comments.Count > 0){ %>
<p id="comment"><%=Resources.labels.comments %></p>
<%} %>

<div id="commentlist" <%= (CurrentPost.Comments.Count == 0) ? " style=\"display:none;\"" : ""%>>
  <asp:PlaceHolder runat="server" ID="phComments" />  
</div>

<asp:PlaceHolder runat="Server" ID="phAddComment">
<%=RenderView<CoyoEden.UI.Views.CommentFormView>("CommentForm") %>
<script type="text/javascript">
<!--//
function registerCommentBox(){
	CoyoEden.comments.flagImage = CoyoEden.$("imgFlag");
	CoyoEden.comments.contentBox = CoyoEden.$("txtContent");
	CoyoEden.comments.moderation = <%=BlogSettings.Instance.EnableCommentsModeration.ToString().ToLowerInvariant() %>;
	CoyoEden.comments.checkName = <%=(!Page.User.Identity.IsAuthenticated).ToString().ToLowerInvariant() %>;
	CoyoEden.comments.postAuthor = "<%=CurrentPost.Author %>";
	CoyoEden.comments.nameBox = CoyoEden.$("txtName");
	CoyoEden.comments.emailBox = CoyoEden.$("txtEmail");
	CoyoEden.comments.websiteBox = CoyoEden.$("txtWebsite");
	CoyoEden.comments.countryDropDown = CoyoEden.$("ddlCountry"); 
	CoyoEden.comments.captchaField = CoyoEden.$('v');
	CoyoEden.comments.controlId = '<%=this.UniqueID %>';
}
//-->
</script>

<% if (BlogSettings.Instance.IsCoCommentEnabled){ %>
<script type="text/javascript">
// this ensures coComment gets the correct values
coco =
{
     tool          : "CoyoEden",
     siteurl       : "<%=Utils.AbsoluteWebRoot %>",
     sitetitle     : "<%=BlogSettings.Instance.Name %>",
     pageurl       : location.href,
     pagetitle     : "<%=this.CurrentPost.Title %>",
     author: "<%=this.CurrentPost.Title %>",
     formID        : "<%=Page.Form.ClientID %>",
     textareaID: "txtContent",
     buttonID      : "btnSaveAjax"
}
</script>
<script id="cocomment-fetchlet" src="http://www.cocomment.com/js/enabler.js" type="text/javascript">
</script>
<%} %>
</asp:PlaceHolder>

<asp:label runat="server" id="lbCommentsDisabled" visible="false"><%=Resources.labels.commentsAreClosed %></asp:label>