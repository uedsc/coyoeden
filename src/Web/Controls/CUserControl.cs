using System;
using System.Web.UI;
using System.Globalization;
using Cynthia.Business.WebHelpers;
using Cynthia.Business;
using System.Configuration;
using SystemX;

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
		protected string EditContentImage { get; set; }
		protected string EditImageUrl { get; set; }
		protected CBasePage BasePage { get; set; }
		protected string SiteRoot { get; set; }
		protected string ImageSiteRoot { get; set; }
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

        /// <summary>
        /// convert datetime to the "xx days ago..." format
        /// </summary>
        /// <param name="pubDate"></param>
        /// <param name="formatStrHour"></param>
        /// <param name="formatStrMinutes"></param>
        /// <param name="formatStrDay"></param>
        /// <param name="formatStrSeconds"></param>
        /// <param name="rightNow"></param>
        /// <returns></returns>
        protected string GetDateStr(object pubDate,string formatStrDay,string formatStrHour,string formatStrMinutes,string formatStrSeconds,string rightNow)
        {
            var dateTemp = pubDate.As<DateTime>();
            dateTemp = dateTemp.AddHours(TimeZoneOffset);

            var tsTemp = TimeSpan.FromTicks(dateTemp.Ticks);
            var tsNow = TimeSpan.FromTicks(DateTime.Now.Ticks);
            var diff = tsNow - tsTemp;
            if (diff.Days>0)
            {
                return string.Format(formatStrDay, diff.Days);
            }
            if(diff.Hours>0)
            {
                return string.Format(formatStrHour, diff.Hours);
            }
            if(diff.Minutes>0)
            {
                return string.Format(formatStrMinutes, diff.Hours);
            }
            if(diff.Seconds>0)
            {
                return string.Format(formatStrSeconds, diff.Hours);
            }
            return rightNow;
        }

        protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			SiteSettings = CacheHelper.GetCurrentSiteSettings();
			CurPageSettings = CacheHelper.GetCurrentPage();
			TimeZoneOffset = SiteUtils.GetUserTimeOffset();
			BasePage = Page as CBasePage;
			if (null != BasePage)
			{
				SiteRoot = BasePage.SiteRoot;
				ImageSiteRoot = BasePage.ImageSiteRoot;
			}
			//Date format string
			DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
			EditContentImage = ConfigurationManager.AppSettings["EditContentImage"];
			EditImageUrl = String.Format("{0}/Data/SiteImages/{1}", ImageSiteRoot, EditContentImage);
		}
		
        protected string RenderIf<T>(T current, T shouldBe, string trueStr)
		{
			return RenderIf<T>(current, shouldBe, trueStr, "");
		}
		protected string RenderIf<T>(T current, T shouldBe, string trueStr, string falseStr)
		{
			if (current.Equals(shouldBe)) return trueStr;
			return falseStr;
		}
    }
}
