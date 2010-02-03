#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Text;
using CoyoEden.Core;
using SystemX;

#endregion

namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Includes a reference to a JavaScript.
	/// <remarks>
	/// This control is needed in order to let the src
	/// attribute of the script tage be relative to the root.
	/// </remarks>
	/// </summary>
	public class SearchBox : Control
	{

		static SearchBox()
		{
			BlogSettings.Changed += delegate { _Html = null; };
			//Post.Saved += delegate { _Html = null; };
		}

		private static object _SyncRoot = new object();

		private static string _Html;
		/// <summary>
		/// Gets the HTML to render.
		/// </summary>
		private string Html
		{
			get
			{
				lock (_SyncRoot)
				{
					BuildHtml();
				}

				return _Html;
			}
		}

		private void BuildHtml()
		{
			string text = Context.Request.QueryString["q"] != null ? HttpUtility.HtmlEncode(Context.Request.QueryString["q"]) : BlogSettings.Instance.SearchDefaultText;
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<div id=\"searchbox\">");
			sb.Append("<label for=\"searchfield\" style=\"display:none\">Search</label>");
			sb.AppendFormat("<input type=\"text\" value=\"{0}\" id=\"searchfield\" onkeypress=\"if(event.keyCode==13) return CoyoEden.search('{1}')\" onfocus=\"CoyoEden.searchClear('{2}')\" onblur=\"CoyoEden.searchClear('{2}')\" />", text, Utils.RelativeWebRoot, text);
			sb.AppendFormat("<input type=\"button\" value=\"{0}\" id=\"searchbutton\" onclick=\"CoyoEden.search('{1}');\" onkeypress=\"CoyoEden.search('{1}');\" />", BlogSettings.Instance.SearchButtonText, Utils.RelativeWebRoot);

			if (BlogSettings.Instance.EnableCommentSearch)
			{
				string check = Context.Request.QueryString["comment"] != null ? "checked=\"checked\"" : string.Empty;
				sb.AppendFormat("<br /><input type=\"checkbox\" id=\"searchcomments\" {0} />", check);
				if (!string.IsNullOrEmpty(BlogSettings.Instance.SearchCommentLabelText))
					sb.AppendFormat("<label for=\"searchcomments\">{0}</label>", BlogSettings.Instance.SearchCommentLabelText);
			}

			sb.AppendLine("</div>");
			_Html = sb.ToString();
		}

		/// <summary>
		/// Renders the control as a script tag.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.Write(Html);
		}
	}
}