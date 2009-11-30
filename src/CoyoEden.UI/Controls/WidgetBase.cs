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
			base.Render(writer);
		}

	}
}
