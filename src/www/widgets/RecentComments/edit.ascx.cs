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

public partial class widgets_RecentComments_edit : WidgetEditBase
{
	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			if (CurrentSettings.ContainsKey("numberofcomments"))
				txtNumberOfPosts.Text = CurrentSettings["numberofcomments"];
			else
				txtNumberOfPosts.Text = "10";
		}
	}

	public override void Save()
	{
        AddSetting("numberofcomments", txtNumberOfPosts.Text)
            .Update();
		HttpRuntime.Cache.Remove("widget_recentcomments");
	}
}
