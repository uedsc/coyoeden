#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_Tag_cloud_edit : WidgetEditBase
{

	protected override void OnLoad(EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			string minimumPosts = "1";
			if (CurrentSettings.ContainsKey("minimumposts"))
				minimumPosts = CurrentSettings["minimumposts"];

			ddlNumber.SelectedValue = minimumPosts;
		}
	}

	public override void Save()
	{
        AddSetting("minimumposts",ddlNumber.SelectedValue)
            .Update();
		TagCloud.Reload();
	}
}
