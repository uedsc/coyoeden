#region using

using System;
using System.Web;
using CoyoEden.Core.Web.Controls;
using CoyoEden.Core;
using System.Net.Mail;
using System.Threading;
using CoyoEden.Core.Infrastructure;

#endregion

/// <summary>
/// Pings all the ping services specified on the 
/// PingServices admin page and send track- and pingbacks
/// </summary>
[Extension("Pings all the ping services specified on the PingServices admin page and send track- and pingbacks", "1.3", "CoyoEden.NET")]
public class SendPings
{

  /// <summary>
  /// Hooks up an event handler to the Post.Saved event.
  /// </summary>
  static SendPings()
  {
        Post.SavedX += new EventHandler<SavedEventArgs>(Post_Saved);
		Page.SavedX += new EventHandler<SavedEventArgs>(Post_Saved);
  }

  /// <summary>
  /// Sends the pings in a new thread.
  /// <remarks>
  /// It opens a new thread and executes the pings from there,
  /// because it takes some time to complete.
  /// </remarks>
  /// </summary>
  private static void Post_Saved(object sender, SavedEventArgs e)
  {
		if (e.Action == SaveAction.None || e.Action == SaveAction.Delete)
			return;

        IPublishable item = (IPublishable)sender;

		if (item.IsVisibleToPublic)
		{
			Uri url = item.AbsoluteLink;
			ThreadPool.QueueUserWorkItem(delegate { Ping(item, url); });
		}
  }

  /// <summary>
  /// Executes the pings from the new thread.
  /// </summary>
  private static void Ping(IPublishable item, Uri itemUrl)
  {
		try
		{
			System.Threading.Thread.Sleep(2000);

			// Ping the specified ping services.
			CoyoEden.Core.Ping.PingService.Send(itemUrl);

			// Send trackbacks and pingbacks.
			if (!BlogSettings.Instance.EnableTrackBackSend && !BlogSettings.Instance.EnablePingBackSend)
				return;

			if (item.Content.ToUpperInvariant().Contains("\"HTTP"))
				CoyoEden.Core.Ping.Manager.Send(item, itemUrl);
		}
		catch (Exception)
		{
		  // We need to catch this exception so the application doesn't get killed.
		}
  }

}
