using System;
using System.Collections.Generic;
using System.Text;

namespace Cynthia.Business
{
    public delegate void UserRegistreredEventHandler(object sender, UserRegisteredEventArgs e);

    public class UserRegisteredEventArgs : EventArgs
    {
        private SiteUser _siteUser = null;

        public SiteUser SiteUser
        {
            get { return _siteUser; }
        }

        public UserRegisteredEventArgs(SiteUser siteUser)
        {
            _siteUser = siteUser;
        }
    }
}
