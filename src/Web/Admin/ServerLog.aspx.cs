/// Author:					Joe Audette
/// Created:				2007-08-08
/// Last Modified:			2009-06-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Business.WebHelpers;
using Resources;

namespace Cynthia.Web.AdminUI
{
    public partial class ServerLog : CBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsAdmin)||(!siteSettings.IsServerAdminSite))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            SecurityHelper.DisableBrowserCache();

            PopulateLabels();
            ShowLog();

           

        }

        

        private void ShowLog()
        {
            string pathToLog = Server.MapPath("~/Data/currentlog.config");

            // file won't exist if you clear the log, it deletes the file
            // but it will be re-created on next logged event.
            if (!File.Exists(pathToLog)) return;
            
            // in case there is logging happening right now might encounter
            // an error due to file locking, try 3 times

            try
            {
                txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
            }
            catch
            {
                try
                {
                    txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
                }
                catch
                {
                    try
                    {
                        txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
                    }
                    catch 
                    {
                        txtLog.Text = Resource.CouldNotReadLogException;
                    }
                }
            }

        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLog();
            }
            catch
            {
                try
                {
                    DeleteLog();
                }
                catch
                {
                    try
                    {
                        DeleteLog();
                    }
                    catch { }
                }
            }

        }

        private void DeleteLog()
        {
            string pathToLog = Server.MapPath("~/Data/currentlog.config");
            if (File.Exists(pathToLog))
            {
                File.Delete(pathToLog);
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }

        }

        void btnDownloadLog_Click(object sender, EventArgs e)
        {
            string downloadPath = Page.Server.MapPath("~/Data/currentlog.config");

            if (File.Exists(downloadPath))
            {
                FileInfo fileInfo = new System.IO.FileInfo(downloadPath);
                Page.Response.AppendHeader("Content-Length", fileInfo.Length.ToString(CultureInfo.InvariantCulture));
            }

            Page.Response.AddHeader("Content-Disposition", "attachment; filename=Cynthia-log-" + DateTimeHelper.GetDateTimeStringForFileName() + ".txt");
            
            
            Page.Response.ContentType = "application/txt";
            Page.Response.Buffer = false;
            Page.Response.BufferOutput = false;
            Page.Response.TransmitFile(downloadPath);
            Page.Response.End();
        }

        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkServerLog.Text = Resource.AdminMenuServerLogLabel;
            lnkServerLog.ToolTip = Resource.AdminMenuServerLogLabel;
            lnkServerLog.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuServerLogLabel);
            litHeading.Text = Resource.AdminMenuServerLogLabel;

            lnkRefresh.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";
            lnkRefresh.Text = Resource.RefreshLogViewerLink;
            lnkRefresh.ToolTip = Resource.RefreshLogViewerLink;

            btnClearLog.Text = Resource.ServerLogClearLogButton;
            btnClearLog.ToolTip = Resource.ServerLogClearLogButton;

            btnDownloadLog.Text = Resource.DownloadLogButton;

        }

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnClearLog.Click += new EventHandler(btnClearLog_Click);
            btnDownloadLog.Click += new EventHandler(btnDownloadLog_Click);
            SuppressMenuSelection();
            SuppressPageMenu();
        }

        


        #endregion
    }
}
