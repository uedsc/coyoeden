using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using CoyoEden.UI.Controls;

public partial class widgets_Most_comments_edit : WidgetEditBase
{

	protected override void OnPreRender(EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtNumber.Text = "3";
			txtSize.Text = "50";
			txtDays.Text = "60";
			cbShowComments.Checked = true;

			if (CurrentSettings.ContainsKey("avatarsize"))
				txtSize.Text = CurrentSettings["avatarsize"];

			if (CurrentSettings.ContainsKey("numberofvisitors"))
				txtNumber.Text = CurrentSettings["numberofvisitors"];

			if (CurrentSettings.ContainsKey("days"))
				txtDays.Text = CurrentSettings["days"];

			if (CurrentSettings.ContainsKey("showcomments"))
				cbShowComments.Checked = CurrentSettings["showcomments"].Equals("true", StringComparison.OrdinalIgnoreCase);
		}
	}

	public override void Save()
	{
		CurrentSettings["avatarsize"] = txtSize.Text;
		CurrentSettings["numberofvisitors"] = txtNumber.Text;
		CurrentSettings["days"] = txtDays.Text;
		CurrentSettings["showcomments"] = cbShowComments.Checked.ToString();
		SaveSettings(CurrentSettings);

		Cache.Remove("most_comments");
	}
}
