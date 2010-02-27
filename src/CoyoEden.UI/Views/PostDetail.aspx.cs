using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Web.Controls;
using CoyoEden.Core.Infrastructure;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

namespace CoyoEden.UI.Views
{
    /// <summary>
    /// ViewModel for displaying a single Post
    /// </summary>
    public class PostDetail:SiteBasePage
    {
        #region base overrides
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AssertRequestedByRelativeLink();
            AssertRequestParameters();
        }


        protected override void OnLoad(EventArgs e)
        {
            LoadPost();
            base.OnLoad(e);
        }


        #endregion

        #region helper methods
        /// <summary>
        /// Render a Post
        /// </summary>
        /// <returns></returns>
        protected string RenderPost(string viewName) {
            var vm = new ViewManager<PostViewBase>(ViewPath(viewName??"PostView"));
            vm.Control.Post = ViewModel;
            vm.Control.ID = ViewModel.Id.ToString().Replace("-", string.Empty);
            vm.Control.Location = ServingLocation.SinglePost;

            AddGenericLink("application/rss+xml", "alternate", String.Format("{0} (RSS)", Server.HtmlEncode(ViewModel.Title)), String.Format("{0}?format=ATOM", vm.Control.CommentFeed));
            AddGenericLink("application/rss+xml", "alternate", String.Format("{0} (ATOM)", Server.HtmlEncode(ViewModel.Title)), String.Format("{0}?format=ATOM", vm.Control.CommentFeed));

            return vm.Render();
        }
        protected string RenderCommentsView(string viewName) {
            var vm = new ViewManager<CommentListView>(ViewPath(viewName ?? "CommentList"),null,true);
            vm.Control.CurrentPost = ViewModel;
            return vm.Render();
        }
        /// <summary>
        /// render a dropdownlist control
        /// </summary>
        /// <returns></returns>
        protected virtual string RenderRelatedPost(int showNum, bool showDesc, int descSize)
        {
            var v = new CoyoEden.UI.Controls.RelatedPosts();
            v.ShowDescription = showDesc;
            v.MaxResults = showNum;
            v.DescriptionMaxLength = descSize;
            v.Item = ViewModel;

            return new ViewManager<CoyoEden.UI.Controls.RelatedPosts>(v, false).Render();
        }
        /// <summary>
        /// assert current post was requested by the relative link
        /// </summary>
        private void AssertRequestedByRelativeLink()
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                if (Request.RawUrl.Contains("?id=") && Request.QueryString["id"].Length == 36)
                {
                    Guid id = new Guid(Request.QueryString["id"]);
                    Post post = Post.GetPost(id);
                    if (post != null)
                    {
                        Response.Clear();
                        Response.StatusCode = 301;
                        Response.AppendHeader("location", post.RelativeLink.ToString());
                        Response.End();
                    }
                }
            }
        }
        /// <summary>
        /// assert the parameters of current request is ok
        /// </summary>
        private void AssertRequestParameters()
        {
            //no id parameter...
            if (Request.QueryString["id"] == null || Request.QueryString["id"].Length != 36) {
                Response.Redirect(String.Format("{0}error404.aspx", Utils.RelativeWebRoot), true);
            }
        }

        private void LoadPost()
        {
            var id = new Guid(Request.QueryString["id"]);
            ViewModel= Post.GetPost(id);
            SystemX.Check.Require(null != ViewModel, string.Format("Page {0} not exists!",id));
            SystemX.Check.Require(ViewModel.IsVisible || Page.User.Identity.IsAuthenticated, string.Format("You are not authenticated to view this Page {0}", id));

            //TODO:Remove the CoyoEden.Core.Web to CoyoEden.UI

            Page.Title = Server.HtmlEncode(ViewModel.Title);
            AddMetaKeywords();
            AddMetaDescription();
            AddMetaTag("author", Server.HtmlEncode(ViewModel.AuthorProfile == null ? ViewModel.Author : ViewModel.AuthorProfile.FullName));

            List<Post> visiblePosts = Post.Posts.FindAll(p => p.IsVisible);
            if (visiblePosts.Count > 0)
            {
                AddGenericLink("last", visiblePosts[0].Title, visiblePosts[0].RelativeLink.ToString());
                AddGenericLink("first", visiblePosts[visiblePosts.Count - 1].Title, visiblePosts[visiblePosts.Count - 1].RelativeLink.ToString());
            }

            if (BlogSettings.Instance.EnablePingBackReceive)
                Response.AppendHeader("x-pingback", String.Format("http://{0}{1}pingback.axd", Request.Url.Authority, Utils.RelativeWebRoot));

        }
        /// <summary>
        /// Adds the post's description as the description metatag.
        /// </summary>
        private void AddMetaDescription()
        {
            base.AddMetaTag("description", Server.HtmlEncode(ViewModel.Description));
        }

        /// <summary>
        /// Adds the post's tags as meta keywords.
        /// </summary>
        private void AddMetaKeywords()
        {
            if (ViewModel.Tags.Count > 0)
            {
                string[] tags = new string[ViewModel.Tags.Count];
                for (int i = 0; i < ViewModel.Tags.Count; i++)
                {
                    tags[i] = ViewModel.Tags[i];
                }
                base.AddMetaTag("keywords", Server.HtmlEncode(string.Join(",", tags)));
            }
        }
        #endregion

        #region member variables
        /// <summary>
        /// CoyoEden.Core.Post instance.The core view model for this page
        /// </summary>
        protected Post ViewModel { get; set; }
        /// <summary>
        /// Currently request for Notification Unsubscription of comments
        /// </summary>
        protected bool NotificationUnsubscription {
            get
            {
                if (Request.QueryString["unsubscribe-email"] == null) return false ;
                if (ViewModel.NotificationEmails.Contains(Request.QueryString["unsubscribe-email"]))
                {
                    ViewModel.NotificationEmails.Remove(Request.QueryString["unsubscribe-email"]);
                    ViewModel.Save();
                    return true;
                }
                return false;
            }
        }
    
        #endregion
    }
}
