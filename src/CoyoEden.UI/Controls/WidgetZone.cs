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
        public string ZoneName
        {
            get { return _ZoneName; }
            set { _ZoneName = Utils.RemoveIllegalCharacters(value); }
        }

        protected override void OnInit(EventArgs e)
        {
			BOZone = loadData();
            base.OnInit(e);
        }

		private BOWidgetZone loadData()
		{
			var data = BOWidgetZone.Load(ZoneName);
			return data;
		}
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			string fileNameFormat = "{0}widgets/{1}/widget.ascx";
			string fileName = string.Empty;
			BOZone.Widgets.ForEach(x => {
				fileName = string.Format(fileNameFormat, Utils.RelativeWebRoot, x.Name);
				try
				{
					WidgetBase control = (WidgetBase)Page.LoadControl(fileName);
					control.WidgetID = x.Id.Value;
					control.ID = control.WidgetID.ToString().Replace("-", string.Empty);
					control.Title = x.Title;
					control.Zone = _ZoneName;

					if (control.IsEditable)
						control.ShowTitle = x.ShowTitle.Value;
					else
						control.ShowTitle = control.DisplayHeader;

					control.LoadWidget();
					this.Controls.Add(control);
				}
				catch (Exception ex)
				{
					Literal lit = new Literal { Text = String.Format("<p style=\"color:red\">Widget {0} not found.<p>", x.Name) };
					lit.Text += ex.Message;
					lit.Text += String.Format("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"CoyoEden.widgetAdmin.removeWidget('{0}');return false\" title=\"{1} widget\">X</a>",x.Id.Value.ToString(), Utils.Translate("delete", "delete"));

					this.Controls.Add(lit);
				}
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
			writer.Write(String.Format("<div id=\"widgetzone_{0}\" class=\"widgetzone\">", _ZoneName));

			base.Render(writer);

			writer.Write("</div>");

			if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
				string selectorId = String.Format("widgetselector_{0}", _ZoneName);
				writer.Write(String.Format("<select id=\"{0}\" class=\"widgetselector\">", selectorId));
				Widget.WidgetModels.ForEach(x => { 
					writer.Write(String.Format("<option value=\"{0}\">{0}</option>", x));
				});

				writer.Write("</select>&nbsp;&nbsp;");
				writer.Write(String.Format("<input type=\"button\" value=\"Add\" onclick=\"CoyoEden.widgetAdmin.addWidget(CoyoEden.$('{0}').value, '{1}')\" />", selectorId, _ZoneName));
				writer.Write("<div class=\"clear\" id=\"clear\">&nbsp;</div>");
			}
		}

	}
}
