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
using SystemX.WebControls;

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
		public virtual string ViewPath(string viewName) {
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
                image.Append(String.Format("&amp;size={0}", size));
                image.Append("&amp;default=");
                image.Append(Server.UrlEncode(String.Format("{0}img/nopic_user.gif",ThemeRoot)));
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
        /// <summary>
        /// render a dropdownlist control
        /// </summary>
        /// <returns></returns>
        protected virtual string RenderDropDownList(string id,string name,string cssClass,string selecttip,string key,string desc,IEnumerable<object> datasource) {
            var ddl = new SiteDDSelect();
            ddl.ID = id;
            ddl.Name = name;
            ddl.KeyField = key;
            ddl.DescField = desc;
            ddl.Tip = selecttip;
            ddl.DataSource = datasource;
            ddl.CssClass = cssClass;

            var html=new ViewManager<SiteDDSelect>(ddl, false).Render();
            return html;
        }
        /// <summary>
        /// render a view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewName"></param>
        /// <returns></returns>
        protected virtual string RenderView<T>(string viewName) where T:Control 
        {
            var vm = ViewPath(viewName);
            var html = new ViewManager<T>(vm).Render();
            return html;
        }
	}
}
