using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Vivasky.Core;
using CoyoEden.Core.DataContracts;
using CoyoEden.Core;

public partial class StandardSite : CoyoEden.UI.SiteMaster
{
	protected UserInfo UserData { get; private set; }
	protected string UserName
	{
		get
		{
			var retVal = Page.User.Identity.Name;
			retVal = string.IsNullOrEmpty(retVal) ? "Guest" : retVal;
			return retVal;
		}
	}
	protected string DefaultAvatar
	{
		get
		{
			return Utils.AbsoluteWebRoot + "assets/img/avatar0.png";//TODO
		}
	}
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!UserIsAdmin) {
			cssSiteTop.Visible = false;
		}

		UserData = User.GetUserInfo(UserName);
		UserData.Avatar = UserData.Avatar ?? DefaultAvatar;
	}
}
