#region Using

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoyoEden.Core;
using CoyoEden.UI.Controls;

#endregion

public partial class widgets_RecentPosts_edit : WidgetEditBase
{
	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			if (CurrentSettings.ContainsKey("numberofposts"))
				txtNumberOfPosts.Text = CurrentSettings["numberofposts"];
			else
				txtNumberOfPosts.Text = "10";

			if (CurrentSettings.ContainsKey("showcomments"))
				cbShowComments.Checked = CurrentSettings["showcomments"].Equals("true", StringComparison.OrdinalIgnoreCase);

			if (CurrentSettings.ContainsKey("showrating"))
				cbShowRating.Checked = CurrentSettings["showrating"].Equals("true", StringComparison.OrdinalIgnoreCase);
		}
	}

	public override void Save()
	{
        AddSetting("numberofposts", txtNumberOfPosts.Text)
            .AddSetting("showcomments", cbShowComments.Checked.ToString())
            .AddSetting("showrating", cbShowRating.Checked.ToString())
            .Update();
		HttpRuntime.Cache.Remove("widget_recentposts");
	}
}
