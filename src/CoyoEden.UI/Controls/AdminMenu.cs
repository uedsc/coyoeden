using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI.HtmlControls;
using Vivasky.Core;
using System.Web.UI;

namespace CoyoEden.UI.Controls
{
	public class AdminMenu:WebControl
	{
		private static object _SyncRoot = new object();

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
							HtmlGenericControl ul = BuildMenu();
							using (System.IO.StringWriter sw = new System.IO.StringWriter())
							{
								ul.RenderControl(new HtmlTextWriter(sw));
								_Html = sw.ToString();
							}
						}
					}
				}

				return _Html;
			}
		}
		private HtmlGenericControl BuildMenu()
		{
			var request = HttpContext.Current.Request;
			HtmlGenericControl ulMenu = new HtmlGenericControl("ul") { ID = "ulMenu" };
			SiteMapNode root = SiteMap.Providers["SecuritySiteMap"].RootNode;
			if (root != null)
			{
				foreach (SiteMapNode adminNode in root.ChildNodes)
				{
					if (adminNode.IsAccessibleToUser(HttpContext.Current))
					{
						if (!request.RawUrl.ToUpperInvariant().Contains("/ADMIN/") && (adminNode.Url.Contains("xmanager") || adminNode.Url.Contains("PingServices")))
							continue;

						HtmlAnchor a = new HtmlAnchor();
						a.HRef = adminNode.Url;

						a.InnerHtml = String.Format("<span>{0}</span>", Utils.Translate(adminNode.Title, adminNode.Title));//"<span>" + Utils.Translate(info.Name.Replace(".aspx", string.Empty)) + "</span>";
						if (request.RawUrl.IndexOf(adminNode.Url, StringComparison.OrdinalIgnoreCase) != -1)
							a.Attributes["class"] = "current";
						HtmlGenericControl li = new HtmlGenericControl("li");
						li.Controls.Add(a);
						ulMenu.Controls.Add(li);
					}
				}
			}

			if (!request.RawUrl.ToUpperInvariant().Contains("/ADMIN/"))
				AddItem(ref ulMenu,Utils.Translate("changePassword"), String.Format("{0}login.aspx", Utils.RelativeWebRoot));

			return ulMenu;
		}

		public void AddItem(ref HtmlGenericControl ulMenu, string text, string url)
		{
			HtmlAnchor a = new HtmlAnchor();
			a.InnerHtml = String.Format("<span>{0}</span>", text);
			a.HRef = url;

			HtmlGenericControl li = new HtmlGenericControl("li");
			li.Controls.Add(a);
			ulMenu.Controls.Add(li);
		}
		/// <summary>
		/// Renders the control.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.Write(Html);
			writer.Write(Environment.NewLine);
		}
	}
}
