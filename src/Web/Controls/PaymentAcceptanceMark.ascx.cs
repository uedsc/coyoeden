/// Author:					Joe Audette
/// Created:				2008-07-15
/// Last Modified:		    2008-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Cynthia.Web.UI
{
    public partial class PaymentAcceptanceMark : UserControl
    {
        private string gCheckoutStyle = "horizontal";
        private CommerceConfiguration commerceConfig = null;
        private bool suppressGoogleCheckout = false;

        public string GCheckoutStyle
        {
            get { return gCheckoutStyle; }
            set { gCheckoutStyle = value; }
        }

        public bool SuppressGoogleCheckout
        {
            get { return suppressGoogleCheckout; }
            set { suppressGoogleCheckout = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            //SetupMarks();

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupMarks();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetupMarks()
        {
            if (commerceConfig == null)
            {
                commerceConfig = SiteUtils.GetCommerceConfig();
            }

            if (commerceConfig == null) { return; }

            if (commerceConfig.CanProcessStandardCards)
            {
                SetupStandardCards();
            }

            // we already have a button for paypal if its enabled and this is sufficient for an acceptance mark
            if (commerceConfig.PayPalIsEnabled)
            {
                SetupPayPal();
            }

            if (commerceConfig.GoogleCheckoutIsEnabled)
            {
                SetupGoogleMark();
            }

            

        }

        private void SetupGoogleMark()
        {
            if (suppressGoogleCheckout) { return; }

            divGoogle.Visible = true;
            Literal gCheckoutCss = new Literal();
            gCheckoutCss.Text = "<link rel=\"stylesheet\" href=\"https://checkout.google.com/seller/accept/s.css\" type=\"text/css\" media=\"screen\" />";
            Page.Header.Controls.Add(gCheckoutCss);

            switch (gCheckoutStyle)
            {
                case "horizontal":
                    imgGCheckout.Width = Unit.Pixel(182);
                    imgGCheckout.Height = Unit.Pixel(44);
                    imgGCheckout.ImageUrl = "https://checkout.google.com/seller/accept/images/ht.gif";
                    litGCheckoutSetup.Text = "<script type=\"text/javascript\">showMark(2);</script>";
                    break;

                case "block":
                    imgGCheckout.Width = Unit.Pixel(92);
                    imgGCheckout.Height = Unit.Pixel(88);
                    imgGCheckout.ImageUrl = "https://checkout.google.com/seller/accept/images/st.gif";
                    litGCheckoutSetup.Text = "<script type=\"text/javascript\">showMark(1);</script>";

                    break;

                case "simple":
                default:
                    imgGCheckout.Width = Unit.Pixel(72);
                    imgGCheckout.Height = Unit.Pixel(73);
                    imgGCheckout.ImageUrl = "https://checkout.google.com/seller/accept/images/sc.gif";
                    litGCheckoutSetup.Text = "<script type=\"text/javascript\">showMark(3);</script>";
                    break;
            }

        }

        private void SetupPayPal()
        {
            if (!Request.IsAuthenticated)
            {
                // only show this if the user is not authenticated
                // because when he is authenticated we show the button
                imgPayPal.Visible = true;
            }
        }

        private void SetupStandardCards()
        {
            imgVisaCard.Visible = true;
            imgMasterCard.Visible = true;
            imgAmexCard.Visible = true;
            imgDiscover.Visible = true;

        }

        
    }
}