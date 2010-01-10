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
    private WidgetEditBase EditTarget { get; set; }
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

			EditTarget = (WidgetEditBase)LoadControl(ViewData.EditTemplateRelativePath);
            EditTarget.BOWidget = ViewData;
            EditTarget.ID = "widget";

            phEdit.Controls.Add(EditTarget);
			if (!Page.IsPostBack) {
				cbShowTitle.Checked = ViewData.ShowTitle.Value;
				txtTitle.Text = ViewData.Title;
				txtTitle.Focus();
				btnSave.Text = Resources.labels.save;
                ShowHtmlEditor = EditTarget.HtmlEditorHost != null;
                if (ShowHtmlEditor) {
                    txtContent.Target = EditTarget.HtmlEditorHost;
                }
			}
            btnSave.Click += btnSave_Click;
		}
	}
	/// <summary>
	/// Handles the Click event of the btnSave control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void btnSave_Click(object sender, EventArgs e)
	{
        if (EditTarget != null)
        {
            EditTarget.Title = txtTitle.Text.Trim();
            EditTarget.ShowTitle = cbShowTitle.Checked;
            EditTarget.Save();
        }

		WidgetEditBase.OnSaved();
        AppMsg = new BOMessager() { Body=Resources.labels.operationDone};
	}
}
