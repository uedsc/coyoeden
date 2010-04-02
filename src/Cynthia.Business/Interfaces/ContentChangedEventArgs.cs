using System;
using System.Collections.Generic;
using System.Text;

namespace Cynthia.Business
{
    public delegate void ContentChangedEventHandler(object sender, ContentChangedEventArgs e);

    public class ContentChangedEventArgs : EventArgs
    {
        private bool isDeleted = false;

        public bool IsDeleted
        {
            get { return this.isDeleted; }
            set { isDeleted = value; }
        }

    }
}
