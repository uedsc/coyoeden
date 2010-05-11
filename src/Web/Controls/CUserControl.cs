using System;
using System.Web.UI;
using System.Globalization;
using Cynthia.Business.WebHelpers;
using Cynthia.Business;

namespace Cynthia.Web
{
    public class CUserControl : UserControl
	{
		#region member variables
		protected string CssClass { get; set; }
		protected double TimeZoneOffset { get; set; }
		protected string DateFormat { get; set; }
		protected PageSettings CurPageSettings { get; private set; }
		protected SiteSettings SiteSettings { get; private set; }
		/// <summary>
		/// data folder url path of current site
		/// </summary>
		protected string DataFolderUrl
		{
			get
			{
				if (SiteSettings == null) return null;
				return SiteSettings.DataFolderUrl;
			}
		}
		/// <summary>
		/// skin folder url of current site 
		/// </summary>
		protected string SkinBaseUrl
		{
			get
			{
				if (SiteSettings == null) return null;
				return SiteSettings.SkinBaseUrl;
			}
		}
		#endregion

		protected string GetDateHeader(DateTime pubDate)
		{
			// adjust from GMT to user time zone
			return pubDate.AddHours(TimeZoneOffset).ToString(DateFormat);
		}
		protected string GetDateHeader(DateTime pubDate, string dateFormatOverriade)
		{
			var d = pubDate.AddHours(TimeZoneOffset);
			var retVal = d.ToString(dateFormatOverriade);
			return retVal;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			SiteSettings = CacheHelper.GetCurrentSiteSettings();
			CurPageSettings = CacheHelper.GetCurrentPage();
			TimeZoneOffset = SiteUtils.GetUserTimeOffset();
			//Date format string
			DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
		}
    }
}
