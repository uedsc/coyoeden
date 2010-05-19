<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BlogViewControl.ascx.cs" Inherits="Cynthia.Web.BlogUI.BlogViewControl" %>
<%@ Register TagPrefix="blog" TagName="TagList" Src="~/Blog/Controls/CategoryListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="Archives" Src="~/Blog/Controls/ArchiveListControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="FeedLinks" Src="~/Blog/Controls/FeedLinksControl.ascx" %>
<%@ Register TagPrefix="blog" TagName="StatsControl" Src="~/Blog/Controls/StatsControl.ascx" %>
<%@ Import Namespace="Resources" %>
<asp:Panel id="pnlBlog" runat="server" CssClass="module_inner blogview">
<portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<asp:Panel id="divNav" runat="server" cssclass="blognavright" EnableViewState="false" >
	<blog:FeedLinks id="Feeds" runat="server" />
	<asp:Panel ID="pnlStatistics" runat="server" EnableViewState="false">
	<blog:StatsControl id="stats" runat="server" />
	</asp:Panel>
	<asp:Panel ID="pnlCategories" Runat="server" SkinID="plain" EnableViewState="false">
	    <blog:TagList id="tags" runat="server" />
	</asp:Panel>
	<asp:Panel ID="pnlArchives" Runat="server" SkinID="plain">
	    <blog:Archives id="archive" runat="server" />
	</asp:Panel>
 </asp:Panel>	    
<asp:Panel id="divblog" runat="server" cssclass="blogcenter-rightnav post" SkinID="plain" DefaultButton="btnPostComment">
    <h2 class="blogtitle">
    <%=Server.HtmlEncode(ThePost.Title)%>
    <%if (IsEditable){ %>
		&nbsp;
		<a href="<%=PostEditUrl %>" title="<%=BlogResources.BlogEditEntryLink %>" class="ModuleEditLink">
		<%if (!BasePage.UseTextLinksForFeatureSettings){%>
		<img alt="<%=BlogResources.EditImageAltText %>" src="<%=EditImageUrl %>" />
		<%}else{ %>
		<%=BlogResources.BlogEditEntryLink %>
		<%} %> 
		</a> 
    <%} %>
	</h2>
    <div class="postmetadata rounded">
	  <span><%=DateTimeHelper.LocalizeToCalendar(ThePost.StartDate.AddHours(TimeZoneOffset).ToString(DateFormat))%></span>
	  <%if (ShowPostAuthorSetting)
	 {%>
	  <div class="the_author">
	    <img height="20" width="20" class="avatar avatar-20 photo" src="<%= SkinBaseUrl%>img/avatar20.jpeg" alt=""/>    				    
	    <h4><a rel="external" title="" href="#"><%=String.Format(CultureInfo.CurrentCulture,BlogResources.PostAuthorFormat,BlogAuthor) %></a></h4>
	  </div>
	  <%} %>
	</div>
    <asp:Panel ID="pnlDetails" runat="server" CssClass="postdetail">
    <portal:CRating runat="server" ID="Rating" Enabled="false" />
    <cy:OdiogoItem id="odiogoPlayer" runat="server" EnableViewState="false"  />
    <div class="blogtext">
	    <%=ThePost.Description %>
    </div>
    <%if(ShowAddThisButton){ %>
    <div class="blogaddthis">
    <cy:AddThisButton ID="addThis1" runat="server" />
    </div>
    <%} %>
    <goog:LocationMap ID="gmap" runat="server" EnableViewState="false" Visible="false"></goog:LocationMap>
    <!--copyright-->
    <%if (!string.IsNullOrEmpty(BlogCopyright)){%>
    <div class="blogcopyright">
    <%=BlogCopyright%>
    </div>
    <%} %>
    <!--external comment system-->
    <%if (UseExternalCommentService){%>
    <div class="blogcommentservice">
    <portal:IntenseDebateDiscussion ID="intenseDebate" runat="server" />
    <portal:DisqusWidget ID="disqus" runat="server" RenderPoweredBy="false" />
    </div>
    <%}%>
    <!--internal comment system-->
    <%if (AllowComments && (!UseExternalCommentService)){ %>
    <div class="postcomments" id="w_comments">
		<h3><%=BlogResources.BlogFeedbackLabel%></h3>
		<asp:Repeater id="dlComments" runat="server" EnableViewState="true" OnItemCommand="dlComments_ItemCommand">
			<ItemTemplate>
				<div class="post_comment comment_odd clearfix">
				<div class="post_comment_l">
			        <a href="http://vivasky.com" title="Vivasky.Com"><img class="avatar" src="<%=SystemX.Utils.GetGravatar("cocoayoyo@gmail.com",80,"G") %>" alt="Vivasky.Com"></a>
				</div>
				<div class="post_comment_r">
					<h4>
					<asp:ImageButton id="btnDelete" runat="server"  
						AlternateText="<%# Resources.BlogResources.DeleteImageAltText %>" 
						Tooltip="<%# Resources.BlogResources.DeleteImageAltText %>"   
						ImageUrl='<%# DeleteLinkImage %>'
						CommandName="DeleteComment" 
						CommandArgument='<%# DataBinder.Eval(Container.DataItem,"BlogCommentID")%>' 
						Visible="<%# IsEditable%>" />
                    <%#RenderIf((bool)(DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0), 
					    true, 
					    string.Format("<span class=\"comment_author\">{0}</span>", Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())),
					    string.Format("<a href=\"{0}\" title=\"{1}\" class=\"comment_author\">{1}</a>", Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "URL").ToString()), Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString()))
				    )%>
					&nbsp;&nbsp;<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>
                    <span class="comment_date"><%# GetDateStr(DataBinder.Eval(Container.DataItem,"DateCreated"),BlogResources.DateDayFormatStr,BlogResources.DateHourFormatStr,BlogResources.DateMinuteFormatStr,BlogResources.DateSecondFormatStr,BlogResources.DateRightNowFormatStr) %></span>
					</h4>
					<div class="comment_text">
					<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" EnableViewState="false"  TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
					<asp:Literal ID="litComment" runat="server" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem, "Comment").ToString() %>' />
					</NeatHtml:UntrustedContent>
					</div>
				</div>
				</div>
			</ItemTemplate>
            <AlternatingItemTemplate>
            	<div class="post_comment comment_even clearfix">
				<div class="avatar post_comment_l">
                <a href="http://vivasky.com" title="Vivasky.Com"><img class="avatar" src="<%=SystemX.Utils.GetGravatar("mamboer@live.com",80,"R") %>" alt="Vivasky.Com"></a>
				</div>
				<div class="post_comment_r">
					<h4>
					<asp:ImageButton id="btnDelete" runat="server"  
						AlternateText="<%# Resources.BlogResources.DeleteImageAltText %>" 
						Tooltip="<%# Resources.BlogResources.DeleteImageAltText %>"   
						ImageUrl='<%# DeleteLinkImage %>'
						CommandName="DeleteComment" 
						CommandArgument='<%# DataBinder.Eval(Container.DataItem,"BlogCommentID")%>' 
						Visible="<%# IsEditable%>" />
					<%#RenderIf((bool)(DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0), 
					    true, 
					    string.Format("<span class=\"comment_author\">{0}</span>", Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString())),
					    string.Format("<a href=\"{0}\" title=\"{1}\" class=\"comment_author\">{1}</a>", Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "URL").ToString()), Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Name").ToString()))
				    )%>
					&nbsp;&nbsp;<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>
                    <span class="comment_date"><%# GetDateStr(DataBinder.Eval(Container.DataItem,"DateCreated"),BlogResources.DateDayFormatStr,BlogResources.DateHourFormatStr,BlogResources.DateMinuteFormatStr,BlogResources.DateSecondFormatStr,BlogResources.DateRightNowFormatStr) %></span>
					</h4>
					<div class="comment_text">
					<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" EnableViewState="false"  TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
					<asp:Literal ID="litComment" runat="server" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem, "Comment").ToString() %>' />
					</NeatHtml:UntrustedContent>
					</div>
				</div>
				</div>
            </AlternatingItemTemplate>
		</asp:Repeater>
		<asp:Panel ID="pnlCommentsClosed" runat="server" EnableViewState="false">
		<asp:Literal ID="litCommentsClosed" runat="server" EnableViewState="false" />
		</asp:Panel>
		<asp:Panel ID="pnlCommentsRequireAuthentication" runat="server" Visible="false" EnableViewState="false">
		<asp:Literal ID="litCommentsRequireAuthentication" runat="server" EnableViewState="false" />
		</asp:Panel>
    </div>
    <!--fast reply-->
	<asp:Panel ID="pnlNewComment" runat="server">
	<div class="settingrow">
		<cy:SiteLabel id="lblCommentTitle" runat="server" ForControl="txtCommentTitle" CssClass="settinglabel" ConfigKey="BlogCommentTitleLabel" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
		<asp:TextBox id="txtCommentTitle" Runat="server" Width="300" MaxLength="100" EnableViewState="false" CssClass="forminput"></asp:TextBox>
	</div>
	<div class="settingrow">
		<cy:SiteLabel id="lblCommentUserName" runat="server" ForControl="txtName" CssClass="settinglabel" ConfigKey="BlogCommentUserNameLabel" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
		<asp:TextBox id="txtName" Runat="server" Width="300" MaxLength="100" EnableViewState="false" CssClass="forminput"></asp:TextBox>
	</div>
	<div id="divCommentUrl" runat="server" class="settingrow">
		<cy:SiteLabel id="lblCommentURL" runat="server" ForControl="txtURL" CssClass="settinglabel" ConfigKey="BlogCommentUrlLabel" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
		<asp:TextBox id="txtURL" Runat="server" Width="300" MaxLength="200" EnableViewState="false" CssClass="forminput"></asp:TextBox>
	</div>
	<div class="settingrow">
		<cy:SiteLabel id="lblRememberMe" runat="server" ForControl="chkRememberMe" CssClass="settinglabel" ConfigKey="BlogCommentRemeberMeLabel" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
		 <asp:CheckBox id="chkRememberMe" Runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
	</div>
	<div class="settingrow">
		<cy:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="BlogCommentCommentLabel" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
	</div>
	<div class="settingrow">
		<cye:EditorControl id="edComment" runat="server" > </cye:EditorControl>
	</div>
	<asp:Panel ID="pnlAntiSpam" runat="server">
	<cy:CaptchaControl id="captcha" runat="server" />
	</asp:Panel>
	<div class="settingrow">
		<asp:ValidationSummary ID="vSummary" runat="server"  />
		<asp:RegularExpressionValidator ID="regexUrl" runat="server"  ControlToValidate="txtURL" Display="Dynamic" ValidationExpression="(((http(s?))\://){1}\S+)"></asp:RegularExpressionValidator>
	</div>
	<div class="modulebuttonrow">
		<portal:CButton id="btnPostComment" Runat="server" Text="Submit" />
	</div>
	</asp:Panel>
    <%} %>
	<%if (AllowComments){ %>
	<div class="comment_cnt">
		<a href="#w_comments" title="<%=BlogResources.BlogFeedbackLabel %>"><%=ThePost.CommentCount %></a> 
	</div>
	<%} %>
	</asp:Panel>
	<asp:Panel ID="pnlExcerpt" runat="server" Visible="false">
	 <div class="blogtext">
		  <%=ThePost.GetExcerpt(250,"...") %>
	 </div>
	<cy:SiteLabel id="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="MustSignInToViewFullPost" ResourceFile="BlogResources" EnableViewState="false"> </cy:SiteLabel>
	</asp:Panel>
</asp:Panel>
<%if (HasPrevious)
  { %>
  <div class="postnav postnav0 rounded">
  <a href="<%=PreviousPostUrl %>" title="<%=ThePost.PreviousPostTitle %>">
  <span class="row0"><%=Server.HtmlEncode(BlogResources.BlogPreviousPostLink) %></span>
  <br />
  &lt;&lt;
  </a>
  </div>
<%} %>
<%if (HasNext)
  { %>
  <div class="postnav postnav1 rounded">
  <a href="<%=NextPostUrl %>" title="<%=ThePost.NextPostTitle %>">
  <span class="row0"><%=Server.HtmlEncode(BlogResources.BlogNextPostLink) %></span>
  <br />
  &gt;&gt;
  </a>
  </div>
<%} %>
</portal:CPanel>
</asp:Panel> 
