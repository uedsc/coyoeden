#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Xml;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CoyoEden.Core;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_Categories_widget : WidgetBase
{

	public override string Name
	{
		get { return "Category list"; }
	}

	public override bool IsEditable
	{
		get { return true; }
	}

	public override void LoadWidget()
	{
		bool showRssIcon = true;
		bool showPostCount = true;
		if (CurrentSettings.ContainsKey("showrssicon"))
		{
			bool.TryParse(CurrentSettings["showrssicon"], out showRssIcon);
			bool.TryParse(CurrentSettings["showpostcount"], out showPostCount);
		}

		uxCategoryList.ShowRssIcon = showRssIcon;
		uxCategoryList.ShowPostCount = showPostCount;
	}
}
