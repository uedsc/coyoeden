#region Using

using System;
using System.Web.UI;
using System.Web.Hosting;
using System.Xml;
using System.IO;
using System.Text;
using CoyoEden.Core;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using CoyoEden.Core.Infrastructure;
using CoyoEden.UI.Controls;
using Vivasky.Core.Infrastructure;
using CoyoEden.UI;

#endregion

public partial class admin_widgets_popshow :AdminBasePage
{
    private WidgetEditBase EditTarget { get; set; }
    protected Widget ViewData { get; set; }
    protected bool ShowHtmlEditor { get; set; }
	private ISettingsBehavior settingSrv;
	public admin_widgets_popshow()
	{
        NotMasterPage = true;
		settingSrv = new Setting();
	}
	#region Event handlers

	/// <summary>
	/// Handles the Load event of the Page control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected void Page_Init(object sender, EventArgs e)
	{
		string id = Request.QueryString["id"];
        string add = Request.QueryString["add"];
		string zone = Request.QueryString["zone"];
        
		if (!string.IsNullOrEmpty(id))
			InitEditor(id);

		if (!string.IsNullOrEmpty(add))
			AddWidget(add, zone);            
	}    

	/// <summary>
	/// Handles the Click event of the btnSave control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void btnSave_Click(object sender, EventArgs e)
	{
		EditTarget= (WidgetEditBase)FindControl("widget");

        EditTarget.Title = txtTitle.Text.Trim();
        EditTarget.ShowTitle = cbShowTitle.Checked;
        EditTarget.Save();

		WidgetEditBase.OnSaved();

        // To avoid JS errors with TextBox widget loading tinyMce scripts while
        // the edit window is closing, don't output phEdit.
        phEdit.Visible = false;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "closeWindow", "parent.App.CloseWidgetEditor();", true);
	}

	#endregion
	/// <summary>
	/// Adds a widget of the specified type.
	/// </summary>
	/// <param name="type">The type of widget.</param>
    /// <param name="zone">The zone a widget is being added to.</param>
	private void AddWidget(string type, string zone)
	{
		var widget = (WidgetBase)LoadControl(String.Format("{0}widgets/{1}/widget.ascx", Utils.RelativeWebRoot, type));
		widget.ID = widget.WidgetID.ToString().Replace("-", string.Empty);
		widget.Title = type;
        widget.Zone = zone;
		widget.ShowTitle = widget.DisplayHeader;
		widget.LoadWidget();

		Response.Clear();
		try
		{
			using (StringWriter sw = new StringWriter())
			{
				widget.RenderControl(new HtmlTextWriter(sw));

                // Using ? as a delimiter. ? is a safe delimiter because it cannot appear in a
                // zonename because ? is one of the characters removed by Utils.RemoveIllegalCharacters().
				Response.Write(zone + "?" + sw);
			}
		}
		catch (System.Web.HttpException)
		{
			Response.Write("reload");
		}

		SaveNewWidget(widget, zone);
		WidgetEditBase.OnSaved();
		Response.End();
	}

	/// <summary>
	/// Saves the new widget to the XML file.
	/// </summary>
	/// <param name="widget">The widget to add.</param>
    /// <param name="zone">The zone a widget is being added to.</param>
	private void SaveNewWidget(WidgetBase widget, string zone)
	{
		XmlDocument doc = GetXmlDocument(zone);
		XmlNode node = doc.CreateElement("widget");
		node.InnerText = widget.Name;

		XmlAttribute id = doc.CreateAttribute("id");
		id.InnerText = widget.WidgetID.ToString();
		node.Attributes.Append(id);

		XmlAttribute title = doc.CreateAttribute("title");
		title.InnerText = widget.Title;
		node.Attributes.Append(title);

		XmlAttribute show = doc.CreateAttribute("showTitle");
		show.InnerText = "True";
		node.Attributes.Append(show);

		doc.SelectSingleNode("widgets").AppendChild(node);
		SaveXmlDocument(doc, zone);
	}
	#region Helper methods

	private static readonly string FILE_NAME = HostingEnvironment.MapPath(String.Format("{0}widgetzone.xml", BlogSettings.Instance.StorageLocation));

	/// <summary>
	/// Gets the XML document.
	/// </summary>
    /// <param name="zone">The zone Xml Document to get.</param>
	/// <returns></returns>
	private XmlDocument GetXmlDocument(string zone)
	{
		XmlDocument doc;
        if (Cache[zone] == null)
		{
			doc= settingSrv.GetSettings<XmlDocument>(SettingTypes.Widget, zone);
			if (doc.SelectSingleNode("widgets") == null)
			{
				XmlNode widgets = doc.CreateElement("widgets");
				doc.AppendChild(widgets);
			}
            Cache[zone] = doc;
		}
		return (XmlDocument)Cache[zone];
	}

	/// <summary>
	/// Saves the XML document.
	/// </summary>
	/// <param name="doc">The doc.</param>
    /// <param name="zone">The zone to save the Xml Document for.</param>
	private void SaveXmlDocument(XmlDocument doc, string zone)
	{
		settingSrv.SaveSettings(SettingTypes.Widget, zone, doc.OuterXml);
		Cache[zone] = doc;
	}

	/// <summary>
	/// Inititiates the editor for widget editing.
	/// </summary>
	/// <param name="type">The type of widget to edit.</param>
	/// <param name="id">The id of the particular widget to edit.</param>
    /// <param name="zone">The zone the widget to be edited is in.</param>
	private void InitEditor(string id)
	{
        var guid = new Guid(id);
        var msg = new BOMessager();
        ViewData = Widget.Find(guid, out msg);
        AppMsg = msg;

        EditTarget = (WidgetEditBase)LoadControl(ViewData.EditTemplateRelativePath);
        EditTarget.BOWidget = ViewData;
        EditTarget.ID = "widget";

        phEdit.Controls.Add(EditTarget);
        if (!Page.IsPostBack)
        {
            cbShowTitle.Checked = ViewData.ShowTitle.Value;
            txtTitle.Text = ViewData.Title;
            txtTitle.Focus();
            btnSave.Text = Resources.labels.save;
            ShowHtmlEditor = EditTarget.HtmlEditorHost != null;
            if (ShowHtmlEditor)
            {
                txtContent.Target = EditTarget.HtmlEditorHost;
            }
        }
        btnSave.Click += btnSave_Click;
	}

	#endregion

}
