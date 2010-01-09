#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using CoyoEden.Core;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_TextBox_edit : WidgetEditBase
{
    protected override void OnInit(EventArgs e)
    {
        HtmlEditorHost = txtText;
        base.OnInit(e);
    }
	/// <summary>
	/// Handles the Load event of the Page control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			txtText.Text = CurrentSettings["content"];
		}
	}

	/// <summary>
	/// Saves this the basic widget CurrentSettings such as the Title.
	/// </summary>
	public override void Save()
	{
		CurrentSettings["content"] = txtText.Text;
		SaveSettings(CurrentSettings);
	}
}
