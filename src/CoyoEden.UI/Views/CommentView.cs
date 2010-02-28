using System;
using CoyoEden.Core;
using System.Text.RegularExpressions;
using CoyoEden.Core.Infrastructure;
using System.Globalization;
using SystemX.Web;
using System.Web.Security;

namespace CoyoEden.UI.Views
{
    public class CommentView:ViewBase
    {
        #region Properties

        public Post Post { get; set; }

        private PostComment _Comment;

        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        /// <value>The comment.</value>
        public PostComment Comment
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The regular expression used to parse links.
        /// </summary>
        private static readonly Regex regex = new Regex("((http://|www\\.)([A-Z0-9.-]{1,})\\.[0-9A-Z?;~&#=\\-_\\./]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string link = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";

        /// <summary>
        /// Gets the text of the comment.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                ServingEventArgs arg = new ServingEventArgs(Comment.Content, ServingLocation.SinglePost);
                PostComment.OnServing(Comment, arg);
                if (arg.Cancel)
                {
                    this.Visible = false;
                }

                string body = arg.Body.Replace("\n", "<br />");
                body = body.Replace("\t", "&nbsp;&nbsp;");
                body = body.Replace("  ", "&nbsp;&nbsp;");
                return body;
            }
        }

        /// <summary>
        /// Displays a link that lets a user reply to a specific comment
        /// </summary>
        protected string ReplyToLink
        {
            get
            {
                if (!BlogSettings.Instance.IsCommentEnabled ||
                    !BlogSettings.Instance.IsCommentNestingEnabled ||
                    !Post.IsCommentEnabled.Value ||
                    !Comment.IsApproved.Value ||
                    (BlogSettings.Instance.DaysCommentsAreEnabled > 0 && Post.DateCreated.Value.AddDays(BlogSettings.Instance.DaysCommentsAreEnabled) < DateTime.Now.Date))
                {
                    return "";
                }
                else
                {
                    return String.Format("<a href=\"javascript:void(0);\" class=\"reply-to-comment\" onclick=\"CoyoEden.replyToComment('{0}');\">{1}</a>", Comment.Id, Utils.Translate("replyToThis"));
                }
            }
        }


        /// <summary>
        /// Displays a delete link to visitors that are authenticated
        /// using the default membership provider.
        /// </summary>
        protected string AdminLinks
        {
            get
            {
                if (Page.User.IsInRole(BlogSettings.Instance.AdministratorRole) || Page.User.Identity.Name.Equals(Post.Author))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat(" | <a class=\"email\" href=\"mailto:{0}\">{0}</a>", Comment.Email);
                    sb.AppendFormat(" | <a href=\"http://www.domaintools.com/go/?service=whois&amp;q={0}/\">{0}</a>", Comment.Ip);

                    if (Comment.Comments.Count > 0)
                    {
                        string confirmDelete = string.Format(CultureInfo.InvariantCulture, Utils.Translate("areYouSure"), Utils.Translate("delete").ToLowerInvariant(), Utils.Translate("theComment"));
                        sb.AppendFormat(" | <a href=\"javascript:void(0);\" onclick=\"if (confirm('{1}?')) location.href='?deletecomment={0}'\">{2}</a>", Comment.Id, confirmDelete, Utils.Translate("deleteKeepReplies"));

                        string confirmRepliesDelete = string.Format(CultureInfo.InvariantCulture, Utils.Translate("areYouSure"), Utils.Translate("delete").ToLowerInvariant(), Utils.Translate("theComment"));
                        sb.AppendFormat(" | <a href=\"javascript:void(0);\" onclick=\"if (confirm('{1}?')) location.href='?deletecommentandchildren={0}'\">{2}</a>", Comment.Id, confirmRepliesDelete, Utils.Translate("deletePlusReplies"));
                    }
                    else
                    {
                        string confirmDelete = string.Format(CultureInfo.InvariantCulture, Utils.Translate("areYouSure"), Utils.Translate("delete").ToLowerInvariant(), Utils.Translate("theComment"));
                        sb.AppendFormat(" | <a href=\"javascript:void(0);\" onclick=\"if (confirm('{1}?')) location.href='?deletecomment={0}'\">{2}</a>", Comment.Id, confirmDelete, Utils.Translate("delete"));
                    }

                    if (!Comment.IsApproved.Value)
                    {
                        sb.AppendFormat(" | <a href=\"?approvecomment={0}\">{1}</a>", Comment.Id, Utils.Translate("approve"));

                    }
                    return sb.ToString();
                }
                return string.Empty;
            }
        }

        private const string FLAG_IMAGE = "<span class=\"adr\"><img src=\"{0}pics/flags/{1}.png\" class=\"country-name flag\" title=\"{2}\" alt=\"{2}\" /></span>";

        /// <summary>
        /// Displays the flag of the country from which the comment was written.
        /// <remarks>
        /// If the country hasn't been resolved from the authors IP address or
        /// the flag does not exist for that country, nothing is displayed.
        /// </remarks>
        /// </summary>
        protected string Flag
        {
            get
            {
                if (!string.IsNullOrEmpty(Comment.Country))
                {
                    //return "<img src=\"" + Utils.RelativeWebRoot + "pics/flags/" + Comment.Country + ".png\" class=\"country-name flag\" title=\"" + Comment.Country + "\" alt=\"" + Comment.Country + "\" />";
                    return string.Format(FLAG_IMAGE, Utils.RelativeWebRoot, Comment.Country, FindCountry(Comment.Country));
                }

                return null;
            }
        }

        string FindCountry(string isoCode)
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.Name);
                if (ri.TwoLetterISORegionName.Equals(isoCode, StringComparison.OrdinalIgnoreCase))
                {
                    return ri.DisplayName;
                }
            }

            return isoCode;
        }

        private const string GRAVATAR_IMAGE = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";

        /// <summary>
        /// Displays the Gravatar image that matches the specified email.
        /// </summary>
        protected string Gravatar(int size)
        {
            if (BlogSettings.Instance.Avatar == "none")
                return null;

            if (String.IsNullOrEmpty(Comment.Email) || !Comment.Email.Contains("@"))
            {
                if (Comment.Website != null && Comment.Website.ToString().Length > 0 && Comment.Website.ToString().Contains("http://"))
                {
                    return string.Format(CultureInfo.InvariantCulture, "<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", Server.UrlEncode(Comment.Website.ToString()), Comment.Email);
                }

                return String.Format("<img src=\"{0}nopic_user.gif\" alt=\"{1}\" />", Utils.AbsoluteWebRoot, BlogSettings.Instance.Theme, Comment.Author);
            }

            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(Comment.Email.ToLowerInvariant().Trim(), "MD5").ToLowerInvariant();
            string gravatar = String.Format("http://www.gravatar.com/avatar/{0}.jpg?s={1}&amp;d=", hash, size);

            string link = string.Empty;
            switch (BlogSettings.Instance.Avatar)
            {
                case "identicon":
                    link = String.Format("{0}identicon", gravatar);
                    break;

                case "wavatar":
                    link = String.Format("{0}wavatar", gravatar);
                    break;

                default:
                    link = String.Format("{0}monsterid", gravatar);
                    break;
            }

            return string.Format(CultureInfo.InvariantCulture, GRAVATAR_IMAGE, link, Comment.Author);
        }

        #endregion
    }
}
