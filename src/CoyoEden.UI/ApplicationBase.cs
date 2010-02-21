using System;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Web;
using CoyoEden.Core;
using SystemX.Web;
using CoyoEden.Core.Web.Extensions;
using CoyoEden.Core.BootStrappers;
using SystemX.Infrastructure;

namespace CoyoEden.UI
{
	public class ApplicationBase:HttpApplication
	{
		/// <summary>
		/// Hooks up the available extensions located in the App_Code folder.
		/// An extension must be decorated with the ExtensionAttribute to work.
		/// <example>
		///  <code>
		/// [Extension("Description of the SomeExtension class")]
		/// public class SomeExtension
		/// {
		///   //There must be a parameterless default constructor.
		///   public SomeExtension()
		///   {
		///     //Hook up to the CoyoEden.NET events.
		///   }
		/// }
		/// </code>
		/// </example>
		/// </summary>
		protected void Application_Start(object sender, EventArgs e)
		{
			//boot up the SystemX.Luna framework
			var app = new ApplicationBoot("CoyoEden", BlogSettings.AppVersion()) {
				//ClassDefsFileName = Server.MapPath("~/app_data/ClassDefs.xml") 
				ClassDefsXml = BOBroker.GetClassDefsXml()
			};
			app.Startup();

			//setup ioc/di
			ServicesRegister.Boot();

			ExtensionManager.LoadExtensions();

			//cache
			if (AppSettingsHelper.Instance.GetBoolean("App.EnableCache", true)){
				CoyoEden.Core.Caching.CacheManager.Init();
			}
		}
		/// <summary>
		/// Application Error handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			Exception ex = context.Server.GetLastError();
			if (ex == null || !(ex is HttpException) || (ex as HttpException).GetHttpCode() == 404)
				return;

			StringBuilder sb = new StringBuilder();

			try
			{
                sb.Append(String.Format("Url : {0}", context.Request.Url));
				sb.Append(Environment.NewLine);
				sb.Append(String.Format("Raw Url : {0}", context.Request.RawUrl));
				sb.Append(Environment.NewLine);

				while (ex != null)
				{
					sb.Append(String.Format("Message : {0}", ex.Message));
					sb.Append(Environment.NewLine);
					sb.Append(String.Format("Source : {0}", ex.Source));
					sb.Append(Environment.NewLine);
					sb.Append(String.Format("StackTrace : {0}", ex.StackTrace));
					sb.Append(Environment.NewLine);
                    sb.Append(String.Format("TargetSite : {0}", ex.TargetSite));
					sb.Append(Environment.NewLine);
					sb.Append(String.Format("Environment.StackTrace : {0}", Environment.StackTrace));
					sb.Append(Environment.NewLine);
					ex = ex.InnerException;
				}
			}
			catch (Exception ex2)
			{
				sb.Append(String.Format("Error logging error : {0}", ex2.Message));
			}

			if (BlogSettings.Instance.EnableErrorLogging)
				SystemX.Utils.Log(sb.ToString());

			context.Items["LastErrorDetails"] = sb.ToString();
			context.Response.StatusCode = 500;
			Server.ClearError();

			// Server.Transfer is prohibited during a page callback.
			System.Web.UI.Page currentPage = context.CurrentHandler as System.Web.UI.Page;
			if (currentPage != null && currentPage.IsCallback)
				return;

			context.Server.Transfer("~/error.aspx");
		}

		/// <summary>
		/// Sets the culture based on the language selection in the settings.
		/// </summary>
		protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(BlogSettings.Instance.Culture))
			{
				if (!BlogSettings.Instance.Culture.Equals("Auto"))
				{
					CultureInfo defaultCulture = BlogSettings.GetDefaultCulture();
					Thread.CurrentThread.CurrentUICulture = defaultCulture;
					Thread.CurrentThread.CurrentCulture = defaultCulture;
				}
			}
		}
 
	}
}
