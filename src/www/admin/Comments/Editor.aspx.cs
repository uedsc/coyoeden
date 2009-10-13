﻿using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using CoyoEden.Core;
using Vivasky.Core;
using CoyoEden.Core.Infrastructure;

public partial class admin_Comments_Editor : System.Web.UI.Page
{
    #region Private members
    private static string _id;
    private static IComment _comment;
    private static string _urlReferrer;
    private const string GRAVATAR_IMAGE = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";
    #endregion

    public IComment CurrentComment { get { return _comment; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            _id = HttpContext.Current.Request.QueryString["id"];
            _urlReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
            _comment = GetComment(_id);

            if (_urlReferrer.Contains("/Comments/Default.aspx"))
            {
                if (BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.IsCommentEnabled)
                {
                    if (BlogSettings.Instance.ModerationType == 1)
                        BtnAction.Text = "Spam"; // "Comments: Auto-Moderated";
                    else
                        BtnAction.Text = "Approve"; // "Comments: Manual Moderation";
                }
            }

            if (_urlReferrer.Contains("/Comments/Approved.aspx"))
                BtnAction.Text = "Reject";

            if (_urlReferrer.Contains("/Comments/Spam.aspx"))
                BtnAction.Text = "Restore";

            BtnAction.Visible = BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.IsCommentEnabled;

            if(_comment.Website != null)
                txtWebsite.Text = _comment.Website.ToString();

            txtEmail.Text = _comment.Email;
            txtArea.Value = _comment.Content;
        }
    }

    #region Button clicks

    protected void BtnSaveClick(object sender, EventArgs e)
    {
        bool found = false;
        // Cast ToArray so the original collection isn't modified. 
        foreach (Post p in Post.Posts.ToArray())
        {
            // Cast ToArray so the original collection isn't modified. 
            foreach (var c in p.Comments.ToArray())
            {
                if (c.Id.ToString() == _id)
                {
					var c1 = c as PostComment;
					c1.Content = txtArea.Value;
					c1.Website = txtWebsite.Text;
					c1.Email = txtEmail.Text;

					c1.Save();
                    found = true;
                    break;
                }
            }
            if (found) break;
        }
        Reload();
    }

    protected void BtnActionClick(object sender, EventArgs e)
    {
        if (BlogSettings.Instance.EnableCommentsModeration 
            && BlogSettings.Instance.IsCommentEnabled)
        {
            bool found = false;
            foreach (Post p in Post.Posts.ToArray())
            {
                foreach (var c in p.Comments.ToArray())
                {
                    if (c.Id.ToString() == _id)
                    {
                        string desc = p.Description;
                        p.Description = (desc ?? string.Empty) + " ";
                        p.Description = desc;
                        c.ModeratedBy = "Admin";

                        if (_urlReferrer.Contains("/Comments/Default.aspx"))
                        {
                            if (BlogSettings.Instance.ModerationType == 1)
                                c.IsApproved = false;
                            else
                                c.IsApproved = true;
                        }

                        if (_urlReferrer.Contains("/Comments/Approved.aspx"))
                            c.IsApproved = false;

                        if (_urlReferrer.Contains("/Comments/Spam.aspx"))
                            c.IsApproved = true;

                        p.Save();
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
        }

        Reload();
    }

    protected void BtnDeleteClick(object sender, EventArgs e)
    {
        bool found = false;
        for (int i = 0; i < Post.Posts.Count; i++)
        {
            for (int j = 0; j < Post.Posts[i].Comments.Count; j++)
            {
                if (Post.Posts[i].Comments[j].Id == _comment.Id)
                {
                    Post.Posts[i].RemoveComment(Post.Posts[i].Comments[j] as PostComment);
                    found = true;
                    break;
                }
            }
            if (found) { break; }
        }
        Reload();
    }

    #endregion

    #region Private methods

    protected IComment GetComment(string id)
    {
        foreach (Post p in Post.Posts)
        {
            foreach (var c in p.Comments)
            {
                if (c.Id.ToString() == id)
                {
                    return c;
                }
            }
        }
        return null;
    }

    protected string Gravatar(int size)
    {
        if (BlogSettings.Instance.Avatar == "none")
            return null;

        if (String.IsNullOrEmpty(_comment.Email) || !_comment.Email.Contains("@"))
        {
            if (_comment.Website != null && _comment.Website.ToString().Length > 0 && _comment.Website.ToString().Contains("http://"))
            {
                return string.Format(CultureInfo.InvariantCulture, "<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", Server.UrlEncode(_comment.Website.ToString()), _comment.Email);
            }

            return "<img src=\"" + Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg\" alt=\"" + _comment.Author + "\" />";
        }

        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(_comment.Email.ToLowerInvariant().Trim(), "MD5").ToLowerInvariant();
        string gravatar = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=" + size + "&amp;d=";

        string link = string.Empty;
        switch (BlogSettings.Instance.Avatar)
        {
            case "identicon":
                link = gravatar + "identicon";
                break;

            case "wavatar":
                link = gravatar + "wavatar";
                break;

            default:
                link = gravatar + "monsterid";
                break;
        }

        return string.Format(CultureInfo.InvariantCulture, GRAVATAR_IMAGE, link, _comment.Author);
    }

    protected void Reload()
    {
        ClientScript.RegisterClientScriptBlock(GetType(), "ClientScript",
            "<SCRIPT LANGUAGE='text/javascript'>parent.closeEditor(true);</SCRIPT>"); 
    }

    #endregion
}
