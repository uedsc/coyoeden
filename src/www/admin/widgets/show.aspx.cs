using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoyoEden.UI;
using CoyoEden.Core;
using Vivasky.Core.Infrastructure;
using CoyoEden.UI.Controls;

public partial class admin_widgets_show : AdminBasePage
{
	protected Widget ViewData { get; set; }
    protected bool ShowHtmlEditor { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		loadWidget();
	}
	private void loadWidget() {
		if (string.IsNullOrEmpty(QStrData.i))
		{
			ViewData = new Widget();
		}
		else
		{
			var guid = new Guid(QStrData.i);
			var msg = new BOMessager();
			ViewData = Widget.Find(guid, out msg);
			AppMsg = msg;

			WidgetEditBase edit = (WidgetEditBase)LoadControl(ViewData.EditTemplateRelativePath);
			edit.WidgetID = guid;
			edit.Title = ViewData.Title;
			edit.ID = "widget";
			edit.ShowTitle = ViewData.ShowTitle.Value;
			edit.BOWidget = ViewData;
			phEdit.Controls.Add(edit);
			if (!Page.IsPostBack) {
				cbShowTitle.Checked = ViewData.ShowTitle.Value;
				txtTitle.Text = ViewData.Title;
				txtTitle.Focus();
				btnSave.Text = Resources.labels.save;
                ShowHtmlEditor = edit.HtmlEditorHost != null;
                if (ShowHtmlEditor) {
                    txtContent.Target = edit.HtmlEditorHost;
                }
			}
			btnSave.Click += new EventHandler(btnSave_Click);
		}
	}
	/// <summary>
	/// Handles the Click event of the btnSave control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void btnSave_Click(object sender, EventArgs e)
	{
		WidgetEditBase widget = (WidgetEditBase)FindControl("widget");

		if (widget != null)
			widget.Save();

		WidgetEditBase.OnSaved();

		// To avoid JS errors with TextBox widget loading tinyMce scripts while
		// the edit window is closing, don't output phEdit.
		phEdit.Visible = false;
	}
}
