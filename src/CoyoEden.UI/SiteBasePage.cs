#region Using

using System;
using System.Globalization;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using Vivasky.Core;
using CoyoEden.Core;
using CoyoEden.Core.Web.Controls;

#endregion

namespace CoyoEden.UI
{
	/// <summary>
	/// All pages in the custom themes as well as pre-defined pages in the root
	/// must inherit from this class.
	/// </summary>
	/// <remarks>
	/// The class is responsible for assigning the theme to all
	/// derived pages as well as adding RSS, RSD, tracking script
	/// and a whole lot more.
	/// </remarks>
	public abstract class SiteBasePage : System.Web.UI.Page
	{

		private string _Theme = BlogSettings.Instance.Theme;
		/// <summary>
		/// Assignes the selected theme to the pages.
		/// </summary>
		protected override void OnPreInit(EventArgs e)
		{			
			if (Request.QueryString["theme"] != null)
				_Theme = Request.QueryString["theme"];

			MasterPageFile = String.Format("{0}themes/{1}/site.master", Utils.RelativeWebRoot, _Theme);

			base.OnPreInit(e);

			if (!Page.IsPostBack && !string.IsNullOrEmpty(Request.QueryString["deletepost"]))
			{
				if (Page.User.Identity.IsAuthenticated)
				{
					Post post = CoyoEden.Core.Post.GetPost(new Guid(Request.QueryString["deletepost"]));
					if (Page.User.IsInRole(BlogSettings.Instance.AdministratorRole) || Page.User.Identity.Name == post.Author)
					{
						post.MarkForDelete();
						post.Save();
						Response.Redirect(Utils.RelativeWebRoot);
					}
				}
			}
		}

		/// <summary>
		/// Adds links and javascript to the HTML header tag.
		/// </summary>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!Page.IsCallback)
			{
				// Links
				AddGenericLink("contents", "Archive", String.Format("{0}archive.aspx", Utils.RelativeWebRoot));
				AddGenericLink("start", BlogSettings.Instance.Name, Utils.RelativeWebRoot);
				AddGenericLink("application/rdf+xml", "meta", "SIOC", Utils.AbsoluteWebRoot + "sioc.axd");
				AddGenericLink("application/apml+xml", "meta", "APML", Utils.AbsoluteWebRoot + "apml.axd");
				AddGenericLink("application/rdf+xml", "meta", "FOAF", Utils.AbsoluteWebRoot + "foaf.axd");

				if (string.IsNullOrEmpty(BlogSettings.Instance.AlternateFeedUrl))
				{
					AddGenericLink("application/rss+xml", "alternate", String.Format("{0} (RSS)", BlogSettings.Instance.Name), String.Format("{0}?format=rss", BlogSettings.FeedUrl));
					AddGenericLink("application/atom+xml", "alternate", String.Format("{0} (ATOM)", BlogSettings.Instance.Name), BlogSettings.FeedUrl + "?format=atom");
				}
				else
				{
					AddGenericLink("application/rss+xml", "alternate", BlogSettings.Instance.Name, BlogSettings.FeedUrl);
				}				
				
				AddGenericLink("application/rsd+xml", "edituri", "RSD", Utils.AbsoluteWebRoot + "rsd.axd");

				AddMetaContentType();
				AddDefaultLanguages();

                AddLocalizationKeys();
				AddJavaScriptInclude(String.Format("{0}blog.js", Utils.RelativeWebRoot), true, true);				

				if (BlogSettings.Instance.EnableOpenSearch)
					AddGenericLink("application/opensearchdescription+xml", "search", BlogSettings.Instance.Name, Utils.AbsoluteWebRoot + "opensearch.axd");

				if (!string.IsNullOrEmpty(BlogSettings.Instance.HtmlHeader))
					AddCustomCodeToHead();

				if (!string.IsNullOrEmpty(BlogSettings.Instance.TrackingScript))
					AddTrackingScript();
			}

			if (User.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
				AddJavaScriptInclude(String.Format("{0}assets/js/widget.js", Utils.RelativeWebRoot), true, true);
			}

			if (BlogSettings.Instance.RemoveWhitespaceInStyleSheets)
				CompressCss();

			

		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Page.PreRenderComplete"></see> event after 
		/// the <see cref="M:System.Web.UI.Page.OnPreRenderComplete(System.EventArgs)"></see> event and before the page is rendered.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnPreRenderComplete(EventArgs e)
		{
			base.OnPreRenderComplete(e);
			if (BlogSettings.Instance.UseBlogNameInPageTitles)
			{
				Page.Title = (String.Format("{0} | {1}", BlogSettings.Instance.Name, Page.Title));
			}
		}
		/// <summary>
		/// Adds the localization keys to JavaScript for use globally.
		/// </summary>
		protected virtual void AddLocalizationKeys()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("function registerVariables(){");
			sb.AppendFormat("CoyoEden.webRoot='{0}';", Utils.RelativeWebRoot);
			sb.AppendFormat("CoyoEden.i18n.hasRated='{0}';", Utils.Translate("youAlreadyRated").Replace("'", "\\'"));			
			sb.AppendFormat("CoyoEden.i18n.savingTheComment='{0}';", Utils.Translate("savingTheComment").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.comments='{0}';", Utils.Translate("comments").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.commentWasSaved='{0}';", Utils.Translate("commentWasSaved").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.commentWaitingModeration='{0}';", Utils.Translate("commentWaitingModeration").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.cancel='{0}';", Utils.Translate("cancel").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.filter='{0}';", Utils.Translate("filter").Replace("'", "\\'"));
			sb.AppendFormat("CoyoEden.i18n.apmlDescription='{0}';", Utils.Translate("filterByApmlDescription").Replace("'", "\\'"));
            sb.AppendFormat("CoyoEden.i18n.beTheFirstToRate='{0}';", Utils.Translate("beTheFirstToRate").Replace("'", "\\'"));
            sb.AppendFormat("CoyoEden.i18n.currentlyRated='{0}';", Utils.Translate("currentlyRated").Replace("'", "\\'"));
            sb.AppendFormat("CoyoEden.i18n.ratingHasBeenRegistered='{0}';", Utils.Translate("ratingHasBeenRegistered").Replace("'", "\\'"));
            sb.AppendFormat("CoyoEden.i18n.rateThisXStars='{0}';", Utils.Translate("rateThisXStars").Replace("'", "\\'"));
            
			sb.Append("};");

            ClientScript.RegisterStartupScript(this.GetType(), "registerVariables", sb.ToString(), true);
		}

		/// <summary>
		/// Finds all stylesheets in the header and changes the 
		/// path so it points to css.axd which removes the whitespace.
		/// </summary>
		protected virtual void CompressCss()
		{
			if (Request.QueryString["theme"] != null)
				return;

			foreach (Control control in Page.Header.Controls)
			{
				HtmlControl c = control as HtmlControl;
				if (c != null && c.Attributes["type"] != null && c.Attributes["type"].Equals("text/css", StringComparison.OrdinalIgnoreCase))
				{
					if (!c.Attributes["href"].StartsWith("http://"))
					{
						string url = String.Format("{0}themes/{1}/css.axd?name={2}", Utils.RelativeWebRoot, BlogSettings.Instance.Theme, c.Attributes["href"]);
						c.Attributes["href"] = url.Replace(".css", String.Format("{0}.css", BlogSettings.Instance.Version()));
						c.EnableViewState = false;
					}
				}
			}
		}

		/// <summary>
		/// Adds the content-type meta tag to the header.
		/// </summary>
		protected virtual void AddMetaContentType()
		{
			HtmlMeta meta = new HtmlMeta();
			meta.HttpEquiv = "content-type";
			meta.Content = String.Format("{0}; charset={1}", Response.ContentType, Response.ContentEncoding.HeaderName);
			Page.Header.Controls.Add(meta);
		}

		/// <summary>
		/// Adds the default stylesheet language
		/// </summary>
		protected virtual void AddDefaultLanguages()
		{
			Response.AppendHeader("Content-Style-Type", "text/css");
			Response.AppendHeader("Content-Script-Type", "text/javascript");
		}

		/// <summary>
		/// Add a meta tag to the page's header.
		/// </summary>
		protected virtual void AddMetaTag(string name, string value)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
				return;

			HtmlMeta meta = new HtmlMeta();
			meta.Name = name;
			meta.Content = value;
			Page.Header.Controls.Add(meta);
		}

		/// <summary>
		/// Adds the generic link to the header.
		/// </summary>
		public virtual void AddGenericLink(string relation, string title, string href)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["rel"] = relation;
			link.Attributes["title"] = title;
			link.Attributes["href"] = href;
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Adds the generic link to the header.
		/// </summary>
		public virtual void AddGenericLink(string type, string relation, string title, string href)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["type"] = type;
			link.Attributes["rel"] = relation;
			link.Attributes["title"] = title;
			link.Attributes["href"] = href;
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Adds a JavaScript reference to the HTML head tag.
		/// </summary>
		public virtual void AddJavaScriptInclude(string url, bool placeInBottom, bool addDeferAttribute)
		{
			if (placeInBottom)
			{
				string script = String.Format("<script type=\"text/javascript\"{0} src=\"{1}\"></script>", (addDeferAttribute ? " defer=\"defer\"" : string.Empty), ResolveScriptUrl(url));
				ClientScript.RegisterStartupScript(GetType(), url.GetHashCode().ToString(), script);
			}
			else
			{
				HtmlGenericControl script = new HtmlGenericControl("script");
				script.Attributes["type"] = "text/javascript";
				script.Attributes["src"] = ResolveScriptUrl(url);
				if (addDeferAttribute)
				{
					script.Attributes["defer"] = "defer";
				}

				Page.Header.Controls.Add(script);
			}
		}

		/// <summary>
		/// Adds a Stylesheet reference to the HTML head tag.
		/// </summary>
		/// <param name="url">The relative URL.</param>
		public virtual void AddStylesheetInclude(string url)
		{
			HtmlLink link = new HtmlLink();
			link.Attributes["type"] = "text/css";
			link.Attributes["href"] = url;
			link.Attributes["rel"] = "stylesheet";
			Page.Header.Controls.Add(link);
		}

		/// <summary>
		/// Resolves the script URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public virtual string ResolveScriptUrl(string url)
		{
			return String.Format("{0}js.axd?path={1}&amp;v={2}", Utils.RelativeWebRoot, HttpUtility.UrlEncode(url), BlogSettings.Instance.Version());
		}

		/// <summary>
		/// Adds a JavaScript to the bottom of the page at runtime.    
		/// </summary>
		/// <remarks>
		/// You must add the script tags to the BlogSettings.Instance.TrackingScript.
		/// </remarks>
		protected virtual void AddTrackingScript()
		{
			ClientScript.RegisterStartupScript(this.GetType(), "tracking", String.Format("\n{0}", BlogSettings.Instance.TrackingScript), false);
		}

		/// <summary>
		/// Adds code to the HTML head section.
		/// </summary>
		protected virtual void AddCustomCodeToHead()
		{
			string code = string.Format(CultureInfo.InvariantCulture, "{0}<!-- Start custom code -->{0}{1}{0}<!-- End custom code -->{0}", Environment.NewLine, BlogSettings.Instance.HtmlHeader);
			LiteralControl control = new LiteralControl(code);
			Page.Header.Controls.Add(control);
		}
		/// <summary>
		/// Current theme
		/// </summary>
		public virtual string CurrentTheme
		{
			get
			{
				string theme = Request.QueryString["theme"];
				theme = string.IsNullOrEmpty(theme) ? BlogSettings.Instance.Theme : theme;
				return theme;
			}
		}
		/// <summary>
		/// Url root of current theme
		/// </summary>
		public virtual string ThemeRoot
		{
			get
			{
				return string.Format("{0}themes/{1}/", Utils.AbsoluteWebRoot, CurrentTheme);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.TemplateControl.Error"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnError(EventArgs e)
		{
			HttpContext ctx = HttpContext.Current;
			Exception exception = ctx.Server.GetLastError();

			if (exception != null && exception.Message.Contains("callback"))
			{
				// This is a robot spam attack so we send it a 404 status to make it go away.
				ctx.Response.StatusCode = 404;
				ctx.Server.ClearError();
				PostComment.OnSpamAttack();
			}

			base.OnError(e);
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter"></see> object and calls on the child
		/// controls of the <see cref="T:System.Web.UI.Page"></see> to render.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> that receives the page content.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			//if (BlogSettings.Instance.RemoveWhitespaceInPages)
			//{
			//  using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
			//  {
			//    base.Render(new RewriteFormHtmlTextWriter(htmlwriter));
			//    string html = htmlwriter.InnerWriter.ToString();
			//    html = Utils.RemoveHtmlWhitespace(html);
			//    writer.Write(html);
			//  }
			//}
			//else
			//{
				base.Render(new RewriteFormHtmlTextWriter(writer));
			//}
		}
		protected bool UserIsAdmin {
			get
			{
				return Page.User.IsInRole(BlogSettings.Instance.AdministratorRole);
			}
		}
	}
}