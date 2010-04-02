using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.Controls.DatePicker
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007-11-07
    /// Last Modified:      2007-11-30
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public interface IDatePicker
    {
        Control GetControl();
        string ControlID { get;set;}
        string Text { get;set;}
        bool ShowTime { get;set;}
        string ClockHours { get;set;}
        Unit Width { get; set; }
        string ButtonImageUrl { get; set; }
        

    }
}
