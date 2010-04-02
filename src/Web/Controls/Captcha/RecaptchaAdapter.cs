/// Author:		        Joe Audette
/// Created:            2007-08-16
/// Last Modified:      2007-08-16
/// 
/// Licensed under the terms of the GNU Lesser General Public License:
///	http://www.opensource.org/licenses/lgpl-license.php
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Controls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;
using Recaptcha;

namespace Cynthia.Web.Controls.Captcha
{
    
    public class RecaptchaAdapter : ICaptcha
    {
        #region Constructors

        public RecaptchaAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        private Recaptcha.RecaptchaControl captchaControl = new Recaptcha.RecaptchaControl();

        //private RecaptchaControl captchaControl
        //    = new RecaptchaControl();


        //public string PrivateKey
        //{
        //    get { return captchaControl.PrivateKey; }
        //    set { captchaControl.PrivateKey = value; }
        //}

        //public string PublicKey
        //{
        //    get { return captchaControl.PublicKey; }
        //    set { captchaControl.PublicKey = value; }
        //}

        public bool IsValid
        {
            get { return captchaControl.IsValid; }
           
        }

        public bool Enabled
        {
            get { return captchaControl.Enabled; }
            set { captchaControl.Enabled = value; }

        }

        public string ControlID
        {
            get
            {
                return captchaControl.ID;
            }
            set
            {
                captchaControl.ID = value;
            }
        }

        private void InitializeAdapter()
        {
           
        }

        #region Public Methods

        public Control GetControl()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings == null)||(siteSettings.RecaptchaPrivateKey.Length == 0)||(siteSettings.RecaptchaPublicKey.Length == 0))
            {
                return new Subkismet.Captcha.CaptchaControl();
            }

           
            captchaControl.PrivateKey = siteSettings.RecaptchaPrivateKey;
            captchaControl.PublicKey = siteSettings.RecaptchaPublicKey;
            captchaControl.Theme = WebConfigSettings.RecaptchaTheme;
            

            return captchaControl;
        }



        #endregion
    }
}
