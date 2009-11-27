using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Vivasky.Core;

public partial class StandardSite : System.Web.UI.MasterPage
{
	public virtual string CssClass { get; set; }
  protected void Page_Load(object sender, EventArgs e)
  {
	  CssClass = CssClass ?? "normal";
		//if (Page.User.Identity.IsAuthenticated)
		//{
		//    aLogin.InnerText = Resources.labels.logoff;
		//    aLogin.HRef = Utils.RelativeWebRoot + "login.aspx?logoff";
		//}
		//else
		//{
		//    aLogin.HRef = Utils.RelativeWebRoot + "login.aspx";
		//    aLogin.InnerText = Resources.labels.login;
		//}
  }

}
