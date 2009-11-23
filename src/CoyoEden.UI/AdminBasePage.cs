using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CoyoEden.Core;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;

namespace CoyoEden.UI
{
	public class AdminBasePage : System.Web.UI.Page
	{
		private string _Theme = BlogSettings.Instance.ThemeBackfield;
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
		#endregion
	}
}
