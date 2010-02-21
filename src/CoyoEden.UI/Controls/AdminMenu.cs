using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI.HtmlControls;
using SystemX.Web;
using System.Web.UI;
using SystemX.Services;

namespace CoyoEden.UI.Controls
{
	public class AdminMenu:WebControl,ICacheable
	{
		#region member variables
		public string SelectedCssClass { get; set; }
		public int DisplayNum { get; set; }
		/// <summary>
		/// Whether append 'clearfix' css class
		/// </summary>
		public bool Clearfix { get; set; }
		/// <summary>
		/// put the selected item at the first place
		/// </summary>
		public bool TopActive { get; set; }
		/// <summary>
		/// index of selected menu
		/// </summary>
		protected int SelectedIndex
		{
			get;
			private set;
		}
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
			var rawUrl=HttpContext.Current.Request.RawUrl;
			var yes=rawUrl.IndexOf(node.Url, StringComparison.OrdinalIgnoreCase) != -1;
			//sub pages check
			if (rawUrl.ToUpper().IndexOf("/PAGES/") < 0)
			{
				rawUrl = rawUrl.Substring(0, rawUrl.LastIndexOf("/"));
				yes = yes || (rawUrl.IndexOf(node.Url.Substring(0, node.Url.LastIndexOf("/"))) != -1);
			}
			return yes;
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
			//sort
			sortMenus(MenuItems);
			DisplayNum = DisplayNum < 1 ? 5 : DisplayNum;
			HtmlGenericControl ulMenu = new HtmlGenericControl("ul") { ID = "ulMenu"};
			addCssClass(ref ulMenu);
			if (null != MenuItems) {
				//add top links
				var topLinks = MenuItems.Take(DisplayNum).ToList();
				for (int i = 0; i <topLinks.Count; i++)
				{
					var cssClass = i == SelectedIndex ? SelectedCssClass : null;
					AddMenuItem(ref ulMenu, Utils.Translate(topLinks[i].Title, topLinks[i].Title), topLinks[i].Url, cssClass);
				}
				//more links
				AddMoreLinks(ref ulMenu, MenuItems.Skip(DisplayNum).ToList());
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
		private void sortMenus(List<SiteMapNode> items) {
			var menu = items.SingleOrDefault(x => isSelected(x));
			SelectedIndex = items.IndexOf(menu);
			if (!TopActive) return;

			SelectedIndex = 0;
			if (menu != null) {
				items.Remove(menu);
				items.Insert(0, menu);
			}
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
		private void AddMoreLinks(ref HtmlGenericControl ulMenu, List<SiteMapNode> items)
		{
			if (items.Count == 0) return;
			var tempSelectedIndex =SelectedIndex-DisplayNum;
			var cssTemp = SelectedIndex >= DisplayNum ? String.Format(" {0}", SelectedCssClass) : "";
			//more links
			var sbInnerHtml = new StringBuilder();
			using (var liMore=new HtmlGenericControl("li"))
			{
				liMore.Attributes["class"] = "showmore";
				sbInnerHtml.AppendFormat("<a class=\"more{0}\" title=\"\" href=\"#\"><span>{1}</span></a>",
					cssTemp,
					Utils.Translate("more","更多")
					);
				//showmore_menu
				sbInnerHtml.Append("<div class=\"showmore_menu\" style=\"display:none;\">");
				sbInnerHtml.Append("<dl>");
				for (int i = 0; i < items.Count; i++)
				{
					sbInnerHtml.Append("<dd>");
					if (i == tempSelectedIndex) {
						sbInnerHtml.AppendFormat("<a title=\"\" href=\"{0}\" class=\"active_sub\">{1}</a>",items[i].Url,items[i].Title);

					} else {
						sbInnerHtml.AppendFormat("<a title=\"\" href=\"{0}\">{1}</a>", items[i].Url, items[i].Title);
					}
					
					sbInnerHtml.Append("</dd>");
				}
				sbInnerHtml.Append("</dl>");
				sbInnerHtml.Append("</div>");
				liMore.InnerHtml = sbInnerHtml.ToString();
				ulMenu.Controls.Add(liMore);
			}//endof using
		}
		private void addCssClass(ref HtmlGenericControl ulMenu)
		{
			var css = "";
			if (Clearfix) {
				css = string.IsNullOrEmpty(CssClass) ? "clearfix" : string.Format("{0} {1}", CssClass, "clearfix");
			}
			if (!string.IsNullOrEmpty(css)) {
				ulMenu.Attributes["class"] = css;
			};
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
