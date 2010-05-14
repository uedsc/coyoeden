using System;
using System.Web.UI;
using System.Globalization;
using Cynthia.Business.WebHelpers;
using Cynthia.Business;
using System.Configuration;

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
