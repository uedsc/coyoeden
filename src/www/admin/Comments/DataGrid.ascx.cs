﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using CoyoEden.Core;
using System.Web.UI.WebControls;
using SystemX;
using CoyoEden.Core.Infrastructure;

public partial class admin_Comments_DataGrid : System.Web.UI.UserControl
{
    #region Private members

    static protected List<IComment> Comments;
    private const string GravatarImage = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";
    private bool _autoModerated;

    #endregion

    #region Form events

    protected void Page_Load(object sender, EventArgs e)
    {
        gridComments.RowCommand += GridCommentsRowCommand;
        gridComments.RowDataBound += gridComments_RowDataBound;

        gridComments.PageSize = (BlogSettings.Instance.CommentsPerPage > 0) ? BlogSettings.Instance.CommentsPerPage : 15;

        _autoModerated = BlogSettings.Instance.ModerationType == 1 ? true : false;

        string confirm = "return confirm('Are you sure you want to {0} selected comments?');";

        if (!BlogSettings.Instance.EnableCommentsModeration || !BlogSettings.Instance.IsCommentEnabled)
            btnApproveAll.Visible = false;

        if (_autoModerated)
            btnApproveAll.Text = "Spam";
        else
            btnApproveAll.Text = "Approve";

        if (Request.Path.ToLower().Contains("approved.aspx"))
        {
            btnApproveAll.Text = "Reject";
            btnApproveAll.OnClientClick = string.Format(confirm, "reject");
        }

        if (Request.Path.ToLower().Contains("spam.aspx"))
        {
            btnApproveAll.Text = "Restore";
            btnApproveAll.OnClientClick = string.Format(confirm, "restore");
        }

        if (!Page.IsPostBack)
        {
            BindComments();
        }
    }

    protected void ddCommentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindComments();
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindComments();
        gridComments.PageIndex = e.NewPageIndex;
        gridComments.DataBind();
    }

    protected void gridComments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[8].Text = string.Format("Total : {0} comments", Comments.Count);
        }
    }

    #endregion

    #region Binding

    protected void BindComments()
    {
        LoadData();
        gridComments.DataSource = Comments;
        gridComments.DataBind();
    }

    #endregion

    #region Data handling

    protected void LoadData()
    {
        var  comments = new List<IComment>();

        foreach (Post p in Post.Posts)
        {
            foreach (var  c in p.Comments)
            {
                if (!BlogSettings.Instance.EnableCommentsModeration || !BlogSettings.Instance.IsCommentEnabled)
                {
                    comments.Add(c);
                }
                else
                {
                    if (Request.Path.ToLower().Contains("approved.aspx"))
                    {
                        if (c.IsApproved.Value) comments.Add(c);
                    }
                    else if (Request.Path.ToLower().Contains("spam.aspx"))
                    {
                        if (!c.IsApproved.Value) comments.Add(c);
                    }
                    else
                    {
                        // if auto-moderated, inbox has unapproved comments
                        if (_autoModerated && c.IsApproved.Value) comments.Add(c);

                        // if manual moderation inbox has unapproved comments
                        if (!_autoModerated && !c.IsApproved.Value) comments.Add(c);
                    }
                }
            }
        }
        // sort in descending order
        comments.Sort(delegate(IComment c1, IComment c2)
        { return DateTime.Compare(c2.DateCreated.Value, c1.DateCreated.Value); });
        Comments = comments;
    }

    protected void ApproveComment(IComment comment)
    {
        comment.IsApproved = true;
        comment.ModeratedBy = "Admin";
        UpdateComment(comment);
    }

    protected void RejectComment(IComment comment)
    {
        comment.IsApproved = false;
        comment.ModeratedBy = "Admin";
        UpdateComment(comment);
    }

    protected void RemoveComment(IComment comment)
    {
        bool found = false;
        for (int i = 0; i < Post.Posts.Count; i++)
        {
            for (int j = 0; j < Post.Posts[i].Comments.Count; j++)
            {
                if (Post.Posts[i].Comments[j].Id == comment.Id)
                {
                    Post.Posts[i].RemoveComment(Post.Posts[i].Comments[j] as PostComment);
                    found = true;
                    break;
                }
            }
            if (found) { break; }
        }
    }

    protected void UpdateComment(IComment comment)
    {
        bool found = false;
        // Cast ToArray so the original collection isn't modified. 
        foreach (Post p in Post.Posts.ToArray())
        {
            // Cast ToArray so the original collection isn't modified. 
            foreach (var c in p.Comments.ToArray())
            {
                if (c.Id == comment.Id)
                {
					var c1 = c as PostComment;
					c1.Content = comment.Content;
					c1.IsApproved = comment.IsApproved;
					c1.ModeratedBy = comment.ModeratedBy;

					c1.Save();
                    
                    found = true;
                    break;
                }
            }
            if (found) break;
        }
        BindComments();
    }

    #endregion

    #region Grid buttons

    void GridCommentsRowCommand(object sender, GridViewCommandEventArgs e)
    {
        //System.Web.UI.Page pg = (System.Web.UI.Page)HttpContext.Current.CurrentHandler;
        //bool x = pg.IsPostBack;

        //GridView grid = (GridView)sender;
        //int index = Convert.ToInt32(e.CommandArgument);

        //if (grid.PageIndex > 0)
        //{
        //    index = grid.PageIndex * grid.PageSize + index;
        //}

        //Comment comment;

        //if (e.CommandName == "btnInspect")
        //{
        //    comment = Comments[index];

        //    commId.InnerHtml = comment.Id.ToString();
        //    popAuthor.InnerHtml = "Author: " + comment.Author;

        //    string postTmpl = "<a rel=\"bookmark\" title=\"Post permalink\" href=\"{0}\" target=\"_new\">{1}</a>";
        //    popPost.InnerHtml = "Post: " + string.Format(postTmpl, comment.Parent.RelativeLink, comment.Parent.Title);
        //    popIp.InnerHtml = "<a href=\"http://www.domaintools.com/go/?service=whois&q=" + comment.IP + "\" target=\"_new\">" + comment.IP + "</a>";
        //    popEmail.InnerHtml = "<a href=\"mailto:" + comment.Email + "\">" + comment.Email + "</a>";
        //    txtArea.Value = comment.Content;

        //    popWebsite.InnerHtml = "";
        //    if (comment.Website != null && comment.Website.ToString().Length > 0)
        //    {
        //        popWebsite.InnerHtml = string.Format("Website: <a href=\"{0}\" target=\"_new\">{1}</a>", comment.Website, comment.Website);
        //    }
        //    if (!string.IsNullOrEmpty(comment.Country))
        //    {
        //        popCountry.InnerHtml = "Country: " + comment.Country.ToUpper();
        //    }
        //    pop1.Visible = true;
        //}

        //BindComments();
    }

    #endregion

    #region Footer buttons

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        BindComments();
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (cb != null && cb.Enabled)
            {
                cb.Checked = true;
            }
        }
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        BindComments();
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (cb != null && cb.Enabled)
            {
                cb.Checked = false;
            }
        }
    }

    protected void btnApproveAll_Click(object sender, EventArgs e)
    {
        if (Request.Path.ToLower().Contains("approved.aspx"))
            ProcessSelected("unapprove");
        else
            ProcessSelected("approve");
    }

    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        ProcessSelected("delete");
    }

    protected void ProcessSelected(string action)
    {
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cbx = (CheckBox)row.FindControl("chkSelect");
            if (cbx != null && cbx.Checked)
            {
                int index = row.RowIndex;
                if (gridComments.PageIndex > 0)
                {
                    index = gridComments.PageIndex * gridComments.PageSize + index;
                }
                var comment = Comments[index];

                if (action == "approve")
                {
                    if (!comment.IsApproved.Value)
                    {
                        ApproveComment(comment);
                    }
                }
                else if (action == "unapprove")
                {
                    comment.IsApproved = false;
                    RejectComment(comment);
                }
                if (action == "delete")
                {
                    RemoveComment(comment);
                }
            }
        }

        BindComments();
    }

    public static bool HasNoChildren(Guid comId)
    {
        foreach (Post p in Post.Posts)
        {
            // Cast ToArray so the original collection isn't modified. 
            foreach (var c in p.Comments)
            {
                if (c.ParentID== comId)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static string GetEditHtml(string id)
    {
        return string.Format("editComment('{0}');return false;", id);
    }

    #endregion

    protected string Gravatar(string email, string author)
    {
        if (BlogSettings.Instance.Avatar == "none")
            return null;

        if (String.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            return "<img src=\"" + Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg\" alt=\"" + author + "\" width=\"28\" height=\"28\" />";
        }

        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(email.ToLowerInvariant().Trim(), "MD5").ToLowerInvariant();
        string gravatar = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=28&amp;d=";

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

        return string.Format(CultureInfo.InvariantCulture, GravatarImage, link, author);
    }

    public static string GetWebsite(object website)
    {
        if (website == null) return "";

        const string templ = "<a href='{0}' target='_new' rel='{0}'>{1}</a>";

        string site = website.ToString();
        site = site.Replace("http://www.", "");
        site = site.Replace("http://", "");
        site = site.Length < 20 ? site : site.Substring(0, 17) + "...";

        return string.Format(templ, website, site);
    }
}