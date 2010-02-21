#region Using

using System;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Infrastructure;

#endregion

namespace CoyoEden.UI.Controls
{
	/// <summary>
	/// Builds a category list.
	/// </summary>
	public class MonthList : Control
	{
		private const double CacheTimeoutInHours = 1;

		static MonthList()
		{
			Post.Saved1 += Post_Saved;
		}

		static void Post_Saved(object sender, SavedEventArgs e)
		{
			// invalidate cache whenever a post is modified
			lock (_SyncRoot)
			{
				HttpRuntime.Cache.Remove(CacheKey);
			}

		}

		#region Private fields

		private readonly static object _SyncRoot = new object();
		private const string CacheKey = "BE_MonthListCacheKey";
		#endregion

		private static SortedDictionary<DateTime, int> GetPostsPerMonth()
		{
			lock (_SyncRoot)
			{
				SortedDictionary<DateTime, int> months = HttpRuntime.Cache[CacheKey] as SortedDictionary<DateTime, int>;
				if (months == null)
				{
					months = new SortedDictionary<DateTime, int>();
					// let dictionary expire after 1 hour
					HttpRuntime.Cache.Insert(CacheKey, months, null, DateTime.Now.AddHours(CacheTimeoutInHours), Cache.NoSlidingExpiration);

					foreach (Post post in Post.Posts)
					{
						if (post.IsVisibleToPublic)
						{
							DateTime month = new DateTime(post.DateCreated.Value.Year, post.DateCreated.Value.Month, 1);
							int count;
							// if the date is not in the dictionary, count will be set to 0
							months.TryGetValue(month, out count);
							++count;
							months[month] = count;
						}
					}
				}
				return months;
			}
		}

		private string RenderMonths()
		{
			SortedDictionary<DateTime, int> months = GetPostsPerMonth();
			if (months.Keys.Count == 0)
				return String.Format("<p>{0}</p>", Utils.Translate("none", "none..."));

			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.Attributes.Add("id", "monthList");
			HtmlGenericControl year = null;
			HtmlGenericControl list = null;
			int current = 0;

			foreach (DateTime date in months.Keys)
			{
				if (current == 0)
					current = date.Year;

				if (date.Year > current || ul.Controls.Count == 0)
				{
					list = new HtmlGenericControl("ul");
					list.ID = String.Format("year{0}", date.Year);

					year = new HtmlGenericControl("li");
					year.Attributes.Add("class", "year");
					year.Attributes.Add("onclick", "CoyoEden.toggleMonth('year" + date.Year + "')");
					year.InnerHtml = date.Year.ToString();
					year.Controls.Add(list);

					if (date.Year == DateTime.Now.Year)
						list.Attributes.Add("class", "open");

					ul.Controls.AddAt(0, year);
				}

				HtmlGenericControl li = new HtmlGenericControl("li");

				HtmlAnchor anc = new HtmlAnchor();
				anc.HRef = Utils.RelativeWebRoot + date.Year + "/" + date.ToString("MM") + "/default" + BlogSettings.Instance.FileExtension;
				anc.InnerHtml = String.Format("{0:MMMM} ({1})", DateTime.Parse(date.Year + "-" + date.Month + "-01"), months[date]);

				li.Controls.Add(anc);
				list.Controls.AddAt(0, li);
				current = date.Year;
			}

			System.IO.StringWriter sw = new System.IO.StringWriter();
			ul.RenderControl(new HtmlTextWriter(sw));
			return sw.ToString();
		}

		private SortedDictionary<string, Guid> SortGategories(Dictionary<Guid, string> categories)
		{
			SortedDictionary<string, Guid> dic = new SortedDictionary<string, Guid>();
			foreach (Guid key in categories.Keys)
			{
				dic.Add(categories[key], key);
			}

			return dic;
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			if (Post.Posts.Count > 0)
			{
				string html = RenderMonths();
				writer.Write(html);
				writer.Write(Environment.NewLine);
			}
		}
	}
}