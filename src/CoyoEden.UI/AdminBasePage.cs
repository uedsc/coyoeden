using System;
using CoyoEden.Core;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using Vivasky.Core.Infrastructure;
using System.Collections.Generic;
using CoyoEden.Core.Web;

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

                MasterPageFile = String.Format("{0}themes/{1}/site.master", Utils.RelativeWebRoot, _Theme);
            }
			base.OnPreInit(e);
		}

		private void powerCheck()
		{
            //authenticated
			if (!Page.User.Identity.IsAuthenticated)
			{
				Response.Redirect(Utils.RelativeWebRoot);
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
            if ((QStr.GetQ<ActionTypes>("a") == ActionTypes.None || QStr.GetQ<ActionTypes>("a") == ActionTypes.Unkown) && string.IsNullOrEmpty(navTag)) return yescss;
            var tags = new List<String> { QStr.GetQ<string>("t0"), QStr.GetQ<string>("t1"), QStr.GetQ<string>("t2") };
			if (tags.Contains(navTag)) return yescss;
			return "";
		}
		#endregion
	}
}
