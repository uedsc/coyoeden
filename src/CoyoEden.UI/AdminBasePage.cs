using System;
using CoyoEden.Core;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using Vivasky.Core.Infrastructure;
using System.Collections.Generic;

namespace CoyoEden.UI
{
	public class AdminBasePage : System.Web.UI.Page
	{
		private string _Theme = BlogSettings.Instance.ThemeBackfield;
		/// <summary>
		/// Body css Class of the page
		/// </summary>
		protected string CssClass { get; set; }
		protected override void OnPreInit(EventArgs e)
		{
			if (Request.QueryString["theme"] != null)
				_Theme = Request.QueryString["theme"];

			MasterPageFile = String.Format("{0}themes/{1}/site.master", Utils.RelativeWebRoot, _Theme);
			base.OnPreInit(e);

			powerCheck();
		}

		private void powerCheck()
		{
			if (!Page.User.Identity.IsAuthenticated)
			{
				Response.Redirect(Utils.RelativeWebRoot);
				return;
			};
		}

		#region member variables
		/// <summary>
		/// query string data
		/// </summary>
		protected QStringData QStrData {
			get
			{
				var data= Request["d"];
				return QStringData.New(data);
			}
		}
		/// <summary>
		/// application tips
		/// </summary>
		protected string AppTip
		{
			get
			{
				if (AppMsg == null) return null;
				return AppMsg.ToJSONStr1();
			}
		}
		protected BOMessager AppMsg { get; set; }
		#endregion

		#region methods
		/// <summary>
		/// Check if a tag has been selected,and return the specified css class.
		/// </summary>
		/// <param name="navTag"></param>
		/// <param name="css"></param>
		/// <returns></returns>
		protected string CheckTagCss(string navTag, string yescss)
		{
			if ((QStrData.a == ActionTypes.None || QStrData.a == ActionTypes.Unkown) && string.IsNullOrEmpty(navTag)) return yescss;
			var tags = new List<String> { QStrData.t0, QStrData.t1, QStrData.t2 };
			if (tags.Contains(navTag)) return yescss;
			return "";
		}
		#endregion
	}
}
