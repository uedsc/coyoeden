#region Using

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using CoyoEden.Core;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_Categories_edit : WidgetEditBase
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			bool showRssIcon = true;
			bool showPostCount = true;
			if (CurrentSettings.ContainsKey("showrssicon"))
			{
				bool.TryParse(CurrentSettings["showrssicon"], out showRssIcon);
				bool.TryParse(CurrentSettings["showpostcount"], out showPostCount);
			}

			cbShowRssIcon.Checked = showRssIcon;
			cbShowPostCount.Checked = showPostCount;
		}
	}

	public override void Save()
	{
		CurrentSettings["showrssicon"] = cbShowRssIcon.Checked.ToString();
		CurrentSettings["showpostcount"] = cbShowPostCount.Checked.ToString();
		SaveSettings(CurrentSettings);
	}
}
