using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using CoyoEden.Core;
using CoyoEden.Core.DataContracts;
using SystemX.Web;
using SystemX.Infrastructure;
using System.Collections.Generic;

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
		public virtual string ThemeRoot {
			get
			{
				return string.Format("{0}themes/{1}/",AbsoluteWebRoot,CurrentTheme);
			}
		}
		public virtual string GetViewPath(string viewName) {
			var tempStr = string.Format("themes/{0}/{1}.ascx", CurrentTheme, viewName);
			var retVal = string.Format("{0}Views/{1}.ascx", Utils.RelativeWebRoot, viewName);
			if (File.Exists(Utils.ConvertToPhysicalPath(tempStr))) {
				retVal = string.Format("{0}{1}", Utils.RelativeWebRoot, tempStr);
			}
			return retVal;
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
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
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
                image.Append(Server.UrlEncode(String.Format("{0}themes/{1}/noavatar.jpg", Utils.AbsoluteWebRoot, BlogSettings.Instance.Theme)));
                image.Append("\" alt=\"\" />");
                return image.ToString();
            }
		}
		public string CssClass { get; set; }
		public virtual string AbsoluteWebRoot
		{
			get
			{
				return Utils.AbsoluteWebRoot.ToString();
			}
		}

		public virtual bool IsDebug
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

		public UserInfo UserData { get; set; }

        /// <summary>
        /// query string data of current request.
        /// For the sake of json serialization,we use Dictionary{string,object} instead of the raw NameValueCollection
        /// </summary>
        protected virtual Dictionary<string, object> QStr
        {
            get
            {
                return this.Page.GetQData("d");
            }
        }
		protected virtual bool UserIsAdmin
		{
			get
			{
				return Page.User.IsInRole(BlogSettings.Instance.AdministratorRole);
			}
		}
	}
}
