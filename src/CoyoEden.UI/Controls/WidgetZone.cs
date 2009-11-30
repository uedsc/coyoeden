#region Using

using System;
using System.Web.UI.WebControls;
using System.Threading;
using System.Xml;
using CoyoEden.Core;
using System.IO;
using Vivasky.Core;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core.DataContracts;
using Vivasky.Core.Infrastructure;
using BOWidgetZone=CoyoEden.Core.WidgetZone;
#endregion

namespace CoyoEden.UI.Controls
{
	public class WidgetZone : PlaceHolder
	{
		protected BOWidgetZone BOZone { get; private set; }
		public WidgetZone()
		{
			WidgetEditBase.Saved += (sender, e) => BOZone = loadData();
		}
        

        // For backwards compatibility or if a ZoneName is omitted, provide a default ZoneName.
        private string _ZoneName = "cy_WidgetZone0";
        /// <summary>
        /// Gets or sets the name of the data-container used by this instance
        /// </summary>
        public string Name
        {
            get { return _ZoneName; }
            set { _ZoneName = Utils.RemoveIllegalCharacters(value); }
        }
		public HtmlTags HtmlTag { get; set; }
		public enum HtmlTags
		{
			Ul,
			Div
		}
        protected override void OnInit(EventArgs e)
        {
			BOZone = loadData();
            base.OnInit(e);
        }

		private BOWidgetZone loadData()
		{
			var data = BOWidgetZone.Load(Name);
			return data;
		}
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			var widgetTemplate = BlogSettings.GetFileOfCurTheme("widgetLayout.ascx", "~/views/widgetLayout.ascx");

			BOZone.Widgets.ForEach(x => {
				loadWidget(x, widgetTemplate);
			});
		}

		/// <summary>
		/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> 
		/// object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object 
		/// that receives the server control content.
		/// </param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			var adminClass = Thread.CurrentPrincipal.IsInRole("admin") ? " wzone_admin" : "";//TODO:
			writer.Write(string.Format("<{0} id=\"widgetzone_{1}\" class=\"widgetzone{2}\">", HtmlTag, Name, adminClass));

			base.Render(writer);

			writer.Write(string.Format("</{0}>", HtmlTag));
		}
		/// <summary>
		/// load a widget
		/// </summary>
		/// <param name="boWidget"></param>
		/// <returns></returns>
		private void loadWidget(Widget boWidget,string widgetTemplate)
		{
			var control = default(WidgetBase);
			string fileNameFormat = "{0}widgets/{1}/widget.ascx";
			string fileName = string.Empty;
			fileName = string.Format(fileNameFormat, Utils.RelativeWebRoot, boWidget.Name);
			
			try
			{
				//get widget
				control = (WidgetBase)Page.LoadControl(fileName);
				control.BOWidget = boWidget;
				control.WidgetID = boWidget.Id.Value;
				control.ID = control.WidgetID.ToString().Replace("-", string.Empty);
				control.Title = boWidget.Title;
				control.Zone = _ZoneName;

				if (control.IsEditable)
					control.ShowTitle = boWidget.ShowTitle.Value;
				else
					control.ShowTitle = control.DisplayHeader;

				control.LoadWidget();

				//get widget template
				var template = (CoyoEden.UI.Views.WidgetLayoutViewM)Page.LoadControl(widgetTemplate);
				template.WidgetCore = control;
				template.ViewData = control.BOWidget;

				this.Controls.Add(template);
			}
			catch (Exception ex)
			{
				Literal lit = new Literal { Text = String.Format("<p style=\"color:red\">Widget {0} not found.<p>", boWidget.Name) };
				lit.Text += ex.Message;
				lit.Text += String.Format("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"CoyoEden.widgetAdmin.removeWidget('{0}');return false\" title=\"{1} widget\">X</a>", boWidget.Id.Value.ToString(), Utils.Translate("delete", "delete"));

				this.Controls.Add(lit);
			}
		}

	}
}
