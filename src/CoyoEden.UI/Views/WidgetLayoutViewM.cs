using System;
using CoyoEden.Core;
using System.Web.UI.WebControls;
using System.Web.UI;
namespace CoyoEden.UI.Views
{
	public class WidgetLayoutViewM:ViewBase
	{
		public Control WidgetCore { get; set; }
		public Widget ViewData
		{
			get;
			set;
		}
		#region R Properties

		protected string Id {
			get
			{
				return ViewData.Id.ToString();
			}
		}
		protected string Name {
			get
			{
				return ViewData.Name;
			}
		}
		protected string Color {
			get
			{
				var color= ViewData.ExtConfigs["WidgetColor"];
				color = string.IsNullOrEmpty(color) ? null : color ;
				return color;
			}
		}
		protected bool Movable {
			get
			{
				return ViewData.Movable.Value ;
			}
		}
		protected bool Collapsable {
			get
			{
				return ViewData.Collapsable.Value;
			}
		}
		protected bool Deletable {
			get
			{
				return ViewData.Deletable.Value;
			}
		}
		protected bool Editable {
			get
			{
				return ViewData.Editable.Value;
			}
		}
		protected bool ShowTitle {
			get
			{
				return ViewData.ShowTitle.Value;
			}
		}
		protected bool ForceDeletable
		{
			get;
			private set;
		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			var phWidgetCore = this.FindControl("phWidgetCore");
			if (phWidgetCore != null) {
				try
				{
					phWidgetCore.Controls.Add(WidgetCore);
				}
				catch (Exception ex) {
					ForceDeletable = true;
					using (Literal lit = new Literal { Text = String.Format("<p class=\"error\">Widget {0} not loaded.</p>", Name) })
					{
						lit.Text += ex.Message;
						phWidgetCore.Controls.Add(lit);
					}
				}
			}
			base.OnLoad(e);
		}
	}
}
