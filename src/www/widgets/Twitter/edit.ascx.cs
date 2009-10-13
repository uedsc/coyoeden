#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using CoyoEden.UI.Controls;
#endregion

public partial class widgets_Twitter_edit : WidgetEditBase
{
    private const string TWITTER_SETTINGS_CACHE_KEY = "twitter-CurrentSettings";  // same key used in widget.ascx.cs.

	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        { 
		    if (CurrentSettings.ContainsKey("feedurl"))
		    {
			    txtUrl.Text = CurrentSettings["feedurl"];
			    txtAccountUrl.Text = CurrentSettings["accounturl"];
			    txtTwits.Text = CurrentSettings["maxitems"];
                txtPolling.Text = CurrentSettings["pollinginterval"];
                txtFollowMe.Text = CurrentSettings["followmetext"];
		    }
        }
	}

	public override void Save()
	{
		CurrentSettings["feedurl"] = txtUrl.Text;
		CurrentSettings["accounturl"] = txtAccountUrl.Text;
		CurrentSettings["maxitems"] = txtTwits.Text;
        CurrentSettings["pollinginterval"] = txtPolling.Text;
        CurrentSettings["followmetext"] = txtFollowMe.Text;
		SaveSettings(CurrentSettings);

        // Don't need to clear Feed out of cache because when the Settings are cleared,
        // the last modified date (i.e. TwitterSettings.LastModified) will reset to
        // DateTime.MinValue and Twitter will be re-queried.
        HttpRuntime.Cache.Remove(TWITTER_SETTINGS_CACHE_KEY);
	}
}
