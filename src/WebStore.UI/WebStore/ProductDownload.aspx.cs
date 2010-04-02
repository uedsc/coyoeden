/// Author:					Joe Audette
/// Created:				2007-02-28
/// Last Modified:			2009-07-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Cynthia.Web;
using Cynthia.Web.Framework;
using WebStore.Business;
using log4net;

namespace WebStore.UI
{
   
    public partial class ProductDownloadPage : CBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductDownloadPage));
        protected Guid ticketGuid = Guid.Empty;
        protected FullfillDownloadTicket downloadTicket;
        private SiteUser currentUser = null;
        
        protected string upLoadPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableDownloadCache();

            if (!Request.IsAuthenticated)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            //TODO: more permission checks?
            LoadSettings();

            if (
                (downloadTicket != null)
                && (downloadTicket.Guid == ticketGuid)
                && (!downloadTicket.IsExpired())
                )
            {

                DownloadFile();
            }
            else
            {
                LogRejection();
                pnlExpiredDownload.Visible = true;
            }


        }

        private void DownloadFile()
        {

            ProductFile productFile = new ProductFile(downloadTicket.ProductGuid);

            string fileType = Path.GetExtension(productFile.FileName).Replace(".", string.Empty).ToLowerInvariant();
            string mimeType = SiteUtils.GetMimeType(fileType);
            Page.Response.ContentType = mimeType;
            if (SiteUtils.IsNonAttacmentFileType(fileType))
            {
                Page.Response.AddHeader("Content-Disposition", "filename=" + productFile.FileName);
            }
            else
            {
                Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(productFile.FileName, Encoding.UTF8) + "\"");
            }

            //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());
                                                                                                                                            
            Page.Response.ContentType = "application/" + fileType;
            Page.Response.Buffer = false;
            Page.Response.BufferOutput = false;
            Page.Response.TransmitFile(upLoadPath + productFile.ServerFileName);
            downloadTicket.RecordDownloadHistory(SiteUtils.GetIP4Address());
            Page.Response.End();

        }

        private void LogRejection()
        {
            if (currentUser == null) { return; }
            
            log.Info("User " + currentUser.Email + " tried to download a product but it was either invalid or expired.");

            if (downloadTicket == null) { return; }

            log.Info("rejected download ticket Guid was " + downloadTicket.Guid.ToString());

        }

       

        private void LoadSettings()
        {
            ticketGuid = WebUtils.ParseGuidFromQueryString("ticket", ticketGuid);

            if (ticketGuid != Guid.Empty)
            {
                downloadTicket = new FullfillDownloadTicket(ticketGuid);
            }

            
            upLoadPath = Server.MapPath("~/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/webstoreproductfiles/");

            currentUser = SiteUtils.GetCurrentSiteUser();

        }


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            SuppressPageMenu();
        }

        #endregion
    }
}
