<%@ Control Language="C#" AutoEventWireup="true"  Inherits="CoyoEden.UI.Views.CommentListView" %>
<%@ Import Namespace="SystemX" %>
<%@ Import Namespace="CoyoEden.Core" %>
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

	  <label for="<%=txtName.ClientID %>"><%=Resources.labels.name %>*</label>
	  <% var txtNameID = "txtName" + DateTime.Now.Ticks;%>
	  <asp:TextBox runat="Server" ID="txtName" TabIndex="2" ValidationGroup="AddComment" />
	  <% txtName.ID = txtNameID;%>
	  <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="<%=txtNameID %>" ErrorMessage=" <%$Resources:labels, chooseOtherName %>" Display="dynamic" ClientValidationFunction="CoyoEden.checkAuthorName" EnableClientScript="true" ValidationGroup="AddComment" />
	  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="<%=txtNameID %>" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" /><br />

	  <label for="<%=txtEmail.ClientID %>"><%=Resources.labels.email %>*</label>
	  <asp:TextBox runat="Server" ID="txtEmail" TabIndex="3" ValidationGroup="AddComment" />
	  <span id="gravatarmsg">
	  <%if (BlogSettings.Instance.Avatar != "none" && BlogSettings.Instance.Avatar != "monster"){ %>
	  (<%=string.Format(Resources.labels.willShowGravatar, "<a href=\"http://www.gravatar.com\" target=\"_blank\">Gravatar</a>")%>)
	  <%} %>
	  </span>
	  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" />
	  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="<%$Resources:labels, enterValidEmail%>" Display="dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AddComment" /><br />

	  <label for="<%=txtWebsite.ClientID %>"><%=Resources.labels.website %></label>
	  <asp:TextBox runat="Server" ID="txtWebsite" TabIndex="4" ValidationGroup="AddComment" />
	  <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="Server" ControlToValidate="txtWebsite" ValidationExpression="(http://|https://|)([\w-]+\.)+[\w-]+(/[\w- ./?%&=;~]*)?" ErrorMessage="<%$Resources:labels, enterValidUrl %>" Display="Dynamic" ValidationGroup="AddComment" /><br />
	  
	  <% if(BlogSettings.Instance.EnableCountryInComments){ %>
	  <label for="<%=ddlCountry.ClientID %>"><%=Resources.labels.country %></label>
	  <asp:DropDownList runat="server" ID="ddlCountry" onchange="CoyoEden.setFlag(this.value)" TabIndex="5" EnableViewState="false" ValidationGroup="AddComment" />&nbsp;
	  <asp:Image runat="server" ID="imgFlag" AlternateText="Country flag" Width="16" Height="11" EnableViewState="false" /><br /><br />
	  <%} %>

	  <span class="bbcode<%= !BlogSettings.Instance.ShowLivePreview ? " bbcodeNoLivePreview" : "" %>" title="BBCode tags"><%=BBCodes() %></span>
	  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContent" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" /><br />

	  <% if (BlogSettings.Instance.ShowLivePreview) { %>  
	  <ul id="commentMenu">
		<li id="compose" class="selected" onclick="CoyoEden.composeComment()"><%=Resources.labels.comment%></li>
		<li id="preview" onclick="CoyoEden.showCommentPreview()"><%=Resources.labels.livePreview%></li>
	  </ul>
	  <% } %>
	  <div id="commentCompose">
			<label for="<%=txtContent.ClientID %>" style="display:none"><%=Resources.labels.comment%></label>
		<asp:TextBox runat="server" ID="txtContent" TextMode="multiLine" Columns="50" Rows="10" TabIndex="6" ValidationGroup="AddComment" />
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
	CoyoEden.comments.flagImage = CoyoEden.$("<%= imgFlag.ClientID %>");
	CoyoEden.comments.contentBox = CoyoEden.$("<%=txtContent.ClientID %>");
	CoyoEden.comments.moderation = <%=BlogSettings.Instance.EnableCommentsModeration.ToString().ToLowerInvariant() %>;
	CoyoEden.comments.checkName = <%=(!Page.User.Identity.IsAuthenticated).ToString().ToLowerInvariant() %>;
	CoyoEden.comments.postAuthor = "<%=CurrentPost.Author %>";
	CoyoEden.comments.nameBox = CoyoEden.$("<%=txtName.ClientID %>");
	CoyoEden.comments.emailBox = CoyoEden.$("<%=txtEmail.ClientID %>");
	CoyoEden.comments.websiteBox = CoyoEden.$("<%=txtWebsite.ClientID %>");
	CoyoEden.comments.countryDropDown = CoyoEden.$("<%=ddlCountry.ClientID %>"); 
	CoyoEden.comments.captchaField = CoyoEden.$('<%=hfCaptcha.ClientID %>');
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
     textareaID    : "<%=txtContent.UniqueID %>",
     buttonID      : "btnSaveAjax"
}
</script>
<script id="cocomment-fetchlet" src="http://www.cocomment.com/js/enabler.js" type="text/javascript">
</script>
<%} %>
</asp:PlaceHolder>

<asp:label runat="server" id="lbCommentsDisabled" visible="false"><%=Resources.labels.commentsAreClosed %></asp:label>