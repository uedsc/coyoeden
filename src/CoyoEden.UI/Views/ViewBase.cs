using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CoyoEden.Core;
using Vivasky.Core;
using System.Security.Cryptography;

namespace CoyoEden.UI.Views
{
	public class ViewBase:UserControl
	{
		public virtual string CurrentTheme {
			get
			{
				string theme = Request.QueryString["theme"];
				theme = string.IsNullOrEmpty(theme) ?BlogSettings.Instance.Theme: theme;
				return theme;
			}
		}
		public virtual string GetViewPath(string viewName) {
			return String.Format("{0}themes/{1}/{2}.ascx", Utils.RelativeWebRoot, CurrentTheme,viewName);
		}
		/// <summary>
		/// Displays the Gravatar image that matches the specified email.
		/// </summary>
		protected virtual string Gravatar(string email, string name, int size)
		{
			if (email.Contains("://"))
				return
					string.Format(
						"<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />", name,
						email);
			//http://www.artviper.net/screenshots/screener.php?&url={0}&h={1}&w={1}
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(email));

			StringBuilder hash = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
				hash.Append(result[i].ToString("x2"));

			StringBuilder image = new StringBuilder();
			image.Append("<img src=\"");
			image.Append("http://www.gravatar.com/avatar.php?");
			image.Append(String.Format("gravatar_id={0}", hash));
			image.Append("&amp;rating=G");
			image.Append("&amp;size=" + size);
			image.Append("&amp;default=");
			image.Append(Server.UrlEncode(Utils.AbsoluteWebRoot + "themes/" + BlogSettings.Instance.Theme + "/noavatar.jpg"));
			image.Append("\" alt=\"\" />");
			return image.ToString();
		}
		public string CssClass { get; set; }
		public string AbsoluteWebRoot
		{
			get
			{
				return Utils.AbsoluteWebRoot.ToString();
			}
		}

		public bool IsDebug
		{
			get
			{
				return Utils.IsWebDebug;
			}
		}
		/// <summary>
		/// Tag of current control.
		/// </summary>
		public string Tag { get; set; }
	}
}
