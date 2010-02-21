#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_Tag_cloud_widget : WidgetBase
{

	public override void LoadWidget()
	{
		tagCloud1.MinimumPosts = MinimumPosts;
	}
	private int _MinimumPosts = 1;

	private int MinimumPosts
	{
		get 
		{
			if (CurrentSettings.ContainsKey("minimumposts"))
			{
				int.TryParse(CurrentSettings["minimumposts"], out _MinimumPosts);
			}

			return _MinimumPosts; 		
		}
	}
	/// <summary>
	/// Gets the name. It must be exactly the same as the folder that contains the widget.
	/// </summary>
	/// <value></value>
	public override string Name
	{
		get { return "Tag cloud"; }
	}

	/// <summary>
	/// Gets wether or not the widget can be edited.
	/// <remarks>
	/// The only way a widget can be editable is by adding a edit.ascx file to the widget folder.
	/// </remarks>
	/// </summary>
	/// <value></value>
	public override bool IsEditable
	{
		get { return true; }
	}
}
