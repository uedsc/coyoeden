<%@ Control Language="C#" AutoEventWireup="true"  Inherits="CoyoEden.UI.Views.CommentListView" %>
<%@ Import Namespace="SystemX.Web" %>
<% if (CurrentPost.Comments.Count > 0){ %>
<p id="comment"><%=Resources.labels.comments %></p>
<%} %>

<div id="commentlist" <%= (CurrentPost.Comments.Count == 0) ? " style=\"display:none;\"" : ""%>>
  <asp:PlaceHolder runat="server" ID="phComments" />  
</div>

<asp:PlaceHolder runat="Server" ID="phAddComment">

<div id="comment-form">

	<img src="<%=Utils.RelativeWebRoot %>pics/ajax-loader.gif" alt="Saving the comment" style="display:none" id="ajaxLoader" />  
	<span id="status"></span>

	<div class="commentForm">
	  <p id="addcomment"><%=Resources.labels.addComment %></p>

	  <% if (NestingSupported){ %>
		<asp:HiddenField runat="Server" ID="hiddenReplyTo"  />
		<p id="cancelReply" style="display:none;"><a href="javascript:void(0);" onclick="CoyoEden.cancelReply();"><%=Resources.labels.cancelReply %></a></p>
	  <%} %>
      
	  <label for="txtName"><%=Resources.labels.name %>*</label>
	  <input type="text" id="txtName" tabindex="2" name="Name" />
	  <label for="txtEmail"><%=Resources.labels.email %>*</label>
	  <input type="text" id="txtEmail" tabindex="3" name="Email" />
	  <span id="gravatarmsg">
	  <%if (BlogSettings.Instance.Avatar != "none" && BlogSettings.Instance.Avatar != "monster"){ %>
	  (<%=string.Format(Resources.labels.willShowGravatar, "<a href=\"http://www.gravatar.com\" target=\"_blank\">Gravatar</a>")%>)
	  <%} %>
	  </span>
	  <label for="txtWebsite"><%=Resources.labels.website %></label>
	  <input type="text" id="txtWebsite" tabindex="4" name="Website" />
	  
	  <% if(BlogSettings.Instance.EnableCountryInComments){ %>
	  <label for="ddlCountry"><%=Resources.labels.country %></label>
	  <%=RenderDropDownList("ddlCountry","Country","Your country","TwoLetterISORegionName","EnglishName",SystemX.Infrastructure.CountryData.Countries.Cast<object>()) %>
	  <img alt="Country tag" id="imgFlag" width="16" height="11" /><br /><br />
	  <%} %>

	  <span class="bbcode<%= !BlogSettings.Instance.ShowLivePreview ? " bbcodeNoLivePreview" : "" %>" title="BBCode tags"><%=BBCodes() %></span>
	  <% if (BlogSettings.Instance.ShowLivePreview) { %>  
	  <ul id="commentMenu">
		<li id="compose" class="selected" onclick="CoyoEden.composeComment()"><%=Resources.labels.comment%></li>
		<li id="preview" onclick="CoyoEden.showCommentPreview()"><%=Resources.labels.livePreview%></li>
	  </ul>
	  <% } %>
	  <div id="commentCompose">
			<label for="txtContent" style="display:none"><%=Resources.labels.comment%></label>
			<textarea id="txtContent" name="Content" rows="10" cols="50" tabindex="6"></textarea>
	  </div>
	  <div id="commentPreview">
		<img src="<%=Utils.RelativeWebRoot %>pics/ajax-loader.gif" alt="Loading" />  
	  </div>
	  
	  <br />
	  <input type="checkbox" id="cbNotify" style="width: auto" tabindex="7" />
	  <label for="cbNotify" style="width:auto;float:none;display:inline"><%=Resources.labels.notifyOnNewComments %></label><br /><br />
	 
	  <input type="button" id="btnSaveAjax" value="<%=Resources.labels.saveComment %>" onclick="if(Page_ClientValidate('AddComment')){CoyoEden.addComment()}" tabindex="7" />    
	  <asp:HiddenField runat="server" ID="hfCaptcha" />
	</div>

</div>

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
	CoyoEden.comments.replyToId = CoyoEden.$("<%=hiddenReplyTo.ClientID %>"); 
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