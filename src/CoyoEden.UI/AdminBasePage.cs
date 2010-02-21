using System;
using CoyoEden.Core;
using SystemX;
using SystemX.Web;
using CoyoEden.Core.DataContracts;
using SystemX.Infrastructure;
using System.Collections.Generic;

namespace CoyoEden.UI
{
	public class AdminBasePage : System.Web.UI.Page
	{
		private string _Theme = BlogSettings.Instance.ThemeBackfield;
        /// <summary>
        /// indicate that the page won't apply the master page.
        /// </summary>
        protected bool NotMasterPage { get; set; }
		/// <summary>
		/// Body css Class of the page
		/// </summary>
		protected string CssClass { get; set; }
		protected override void OnPreInit(EventArgs e)
		{
            //power check
            powerCheck();

            if (!NotMasterPage)
            {
                if (Request.QueryString["theme"] != null)
                    _Theme = Request.QueryString["theme"];

                MasterPageFile = String.Format("{0}themes/{1}/site.master", SystemX.Web.Utils.RelativeWebRoot, _Theme);
            }
			base.OnPreInit(e);
		}

		private void powerCheck()
		{
            //authenticated
			if (!Page.User.Identity.IsAuthenticated)
			{
				Response.Redirect(SystemX.Web.Utils.RelativeWebRoot);
				return;
			};
            //authorization
            if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                Response.StatusCode = 403;
                Response.Clear();
                Response.End();
            }
		}

		#region member variables
		/// <summary>
		/// application tips
		/// </summary>
		protected string AppTip
		{
			get
			{
				if (AppMsg == null) return "{}";
				return AppMsg.ToJSONStr();
			}
		}
		protected BOMessager AppMsg { get; set; }
		/// <summary>
		/// query string data of current request.
        /// For the sake of json serialization,we use Dictionary{string,object} instead of the raw NameValueCollection
		/// </summary>
        protected Dictionary<string,object> QStr
		{
			get
			{
                return this.Page.GetQData("d");
			}
		}
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
			if ((QStr.GetAs<ActionTypes>("a") == ActionTypes.None || QStr.GetAs<ActionTypes>("a") == ActionTypes.Unkown) && string.IsNullOrEmpty(navTag)) return yescss;
			var tags = new List<String> { QStr.GetAs<string>("t0"), QStr.GetAs<string>("t1"), QStr.GetAs<string>("t2") };
			if (tags.Contains(navTag)) return yescss;
			return "";
		}
		#endregion
	}
}
