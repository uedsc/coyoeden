using System;
using System.Web.UI;
using System.ComponentModel;

namespace Cynthia.Web.UI
{
	public class UpdatePanelX:UpdatePanel
	{
		[Category("Appearance"),Description("The CSS class applied to the UpdatePanel rendering")]
		public string CssClass
		{
			get
			{
				string s = (string)ViewState["CssClass"];
				return s ?? String.Empty;
			}
			set
			{
				ViewState["CssClass"] = value;
			}
		}

		protected override void RenderChildren(HtmlTextWriter writer)
		{
			if (IsInPartialRendering == false)
			{
				string cssClass = CssClass;
				if (cssClass.Length > 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
				}
			}
			base.RenderChildren(writer);
		}
	}
}
