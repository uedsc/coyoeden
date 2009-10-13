#region Using

using System;
using System.Web.UI;
using System.Threading;
using System.Text;
using CoyoEden.Core;
using System.Collections.Specialized;
using Vivasky.Core;
using CoyoEden.Core.Infrastructure;
using CoyoEden.Core.DataContracts;
using Vivasky.Core.Infrastructure;
using Vivasky.Core.Utility;

#endregion
namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Summary description for WidgetBase
	/// </summary>
	public abstract class WidgetBase :WidgetAncestor
	{
		public WidgetBase()
		{
		}
		#region Properties
		/// <summary>
		/// Gets the name. It must be exactly the same as the folder that contains the widget.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// Gets the name of the containing WidgetZone
		/// </summary>
		public string Zone { get; set; }

		/// <summary>
		/// Gets wether or not the widget can be edited.
		/// <remarks>
		/// The only way a widget can be editable is by adding a edit.ascx file to the widget folder.
		/// </remarks>
		/// </summary>
		public abstract bool IsEditable { get; }
		/// <summary>
		/// Gets a value indicating if the header is visible. This only takes effect if the widgets isn't editable.
		/// </summary>
		/// <value><c>true</c> if the header is visible; otherwise, <c>false</c>.</value>
		public virtual bool DisplayHeader
		{
			get { return true; }
		}

		#endregion
		/// <summary>
		/// This method works as a substitute for Page_Load. You should use this method for
		/// data binding etc. instead of Page_Load.
		/// </summary>
		public abstract void LoadWidget();

		/// <summary>
		/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> 
		/// object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(Name))
				throw new NullReferenceException("Name must be set on a widget");

			StringBuilder sb = new StringBuilder();

			sb.Append("<div class=\"widget " + this.Name.Replace(" ", string.Empty).ToLowerInvariant() + "\" id=\"widget" + WidgetID + "\">");

			if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
			{

				sb.Append("<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"CoyoEden.widgetAdmin.removeWidget('" + WidgetID + "');return false\" title=\"" + Utils.Translate("delete", "delete") + " widget\">X</a>");
				//if (IsEditable)
				sb.Append("<a class=\"edit\" href=\"javascript:void(0)\" onclick=\"CoyoEden.widgetAdmin.editWidget('" + Name + "', '" + WidgetID + "');return false\" title=\"" + Utils.Translate("edit", "edit") + " widget\">" + Utils.Translate("edit", "edit") + "</a>");
				sb.Append("<a class=\"move\" href=\"javascript:void(0)\" onclick=\"CoyoEden.widgetAdmin.initiateMoveWidget('" + WidgetID + "');return false\" title=\"" + Utils.Translate("move", "move") + " widget\">" + Utils.Translate("move", "move") + "</a>");
			}

			if (ShowTitle)
				sb.Append(String.Format("<h4>{0}</h4>", Title));
			else
				sb.Append("<br />");

			sb.Append("<div class=\"content\">");

			writer.Write(sb.ToString());
			base.Render(writer);
			writer.Write("</div>");
			writer.Write("</div>");
		}

	}
}
