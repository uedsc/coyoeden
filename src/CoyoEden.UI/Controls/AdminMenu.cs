using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI.HtmlControls;
using Vivasky.Core;
using System.Web.UI;
using Vivasky.Core.Services;

namespace CoyoEden.UI.Controls
{
	public class AdminMenu:WebControl,ICacheable
	{
		#region member variables
		public string SelectedCssClass { get; set; }

		private static object _SyncRoot = new object();
		private static List<SiteMapNode> _menuItems;
		private List<SiteMapNode> MenuItems
		{
			get
			{
				if (_menuItems == null)
				{
					lock (_SyncRoot)
					{
						if (_menuItems == null)
						{
							_menuItems = loadMenus();
						}
					}
				}

				return _menuItems;
			}
		}

		private static string _Html;
		/// <summary>
		/// Caches the rendered HTML in the private field and first
		/// updates it when a post has been saved (new or updated).
		/// </summary>
		private string Html
		{
			get
			{
				if (_Html == null)
				{
					lock (_SyncRoot)
					{
						if (_Html == null)
						{
							using (var menu = BuildMenu())
							{
								using (System.IO.StringWriter sw = new System.IO.StringWriter())
								{
									menu.RenderControl(new HtmlTextWriter(sw));
									_Html = sw.ToString();
								}
							};
						}
					}
				}

				return _Html;
			}
		}
		#endregion

		#region helper methods
		private bool isSelected(SiteMapNode node) {
			return HttpContext.Current.Request.RawUrl.IndexOf(node.Url, StringComparison.OrdinalIgnoreCase) != -1;
		}
		private void detectRequestUrl()
		{

			var isSame = HttpContext.Current.Request.Url.Equals(HttpContext.Current.Request.UrlReferrer);
			if (!isSame)
			{
				//clear cache
				_Html = null;
			}
		}
		private HtmlGenericControl BuildMenu()
		{
			HtmlGenericControl ulMenu = new HtmlGenericControl("ul") { ID = "ulMenu"};
			ulMenu.Attributes["class"] = "clearfix";
			if (null != MenuItems) {
				MenuItems.ForEach(x => {
					var cssClass = isSelected(x) ? SelectedCssClass : null;
					AddMenuItem(ref ulMenu, Utils.Translate(x.Title, x.Title), x.Url, cssClass);
				});
			}
			if (!HttpContext.Current.Request.RawUrl.ToUpperInvariant().Contains("/ADMIN/"))
			{
				AddMenuItem(ref ulMenu, Utils.Translate("changePassword"), String.Format("{0}login.aspx", Utils.RelativeWebRoot), "changepwd");
			}
			return ulMenu;
		}

		private List<SiteMapNode> loadMenus()
		{
			var retVal = new List<SiteMapNode>();
			var request = HttpContext.Current.Request;
			SiteMapNode root = SiteMap.Providers["SecuritySiteMap"].RootNode;
			if (root != null)
			{
				foreach (SiteMapNode adminNode in root.ChildNodes)
				{
					if (adminNode.IsAccessibleToUser(HttpContext.Current))
					{
						if (!request.RawUrl.ToUpperInvariant().Contains("/ADMIN/") && (adminNode.Url.Contains("xmanager") || adminNode.Url.Contains("PingServices")))
							continue;

						retVal.Add(adminNode);
					}
				}
			}
			return retVal;
		}
		private static void AddMenuItem(ref HtmlGenericControl ulMenu, string text, string url, string cssClass)
		{
			using (var li = new HtmlGenericControl("li"))
			{
				using (var a = new HtmlAnchor())
				{
					a.InnerHtml = String.Format("<span>{0}</span>", text);
					a.HRef = url;
					if (!string.IsNullOrEmpty(cssClass))
						a.Attributes["class"] = cssClass;

					li.Controls.Add(a);
				}
				ulMenu.Controls.Add(li);
			}
		}
		#endregion

		#region base overrides
		/// <summary>
		/// Renders the control.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.Write(Html);
			writer.Write(Environment.NewLine);
		}
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			SelectedCssClass = SelectedCssClass ?? "current";
			//set selected menu
			detectRequestUrl();
		}
		#endregion

		#region ICacheable Members

		public void ClearCache()
		{
			_menuItems = null;
			_Html = null;
		}

		#endregion
	}
}
