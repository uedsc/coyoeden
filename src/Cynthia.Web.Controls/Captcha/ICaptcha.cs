using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007-08-15
    /// Last Modified:      2007-08-15
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public interface ICaptcha
    {
        Control GetControl();
        bool IsValid { get;}
        string ControlID { get;set;}
        bool Enabled { get; set; }
    }
}
