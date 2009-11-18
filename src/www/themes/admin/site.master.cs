using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using CoyoEden.Core;

public partial class admin_site : System.Web.UI.MasterPage
{
	protected UserInfo UserData { get; private set; }
	protected string UserName {
		get
		{
			var retVal = Page.User.Identity.Name;
			retVal = string.IsNullOrEmpty(retVal) ? "Guest" : retVal;
			return retVal;
		}
	}
	protected string DefaultAvatar {
		get
		{
			return Utils.AbsoluteWebRoot+"assets/img/avatar0.png";//TODO
		}
	}
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
      Response.Redirect(Utils.RelativeWebRoot);

	UserData = User.GetUserInfo(UserName);
	UserData.Avatar = UserData.Avatar ?? DefaultAvatar;
  }
}
