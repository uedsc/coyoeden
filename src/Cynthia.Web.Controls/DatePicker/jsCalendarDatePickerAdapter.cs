using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.Controls;

namespace Cynthia.Web.Controls.DatePicker
{
    /// <summary>
    /// Author:		        Joe Audette
    /// Created:            2007-11-07
    /// Last Modified:      2007-11-07
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class jsCalendarDatePickerAdapter : IDatePicker
    {
        private jsCalendarDatePicker control;

        #region Constructors

        public jsCalendarDatePickerAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        
           

        

        public string ControlID
        {
            get
            {
                return control.ID;
            }
            set
            {
                control.ID = value;
            }
        }

        public string Text
        {
            get
            {
                return control.Text;
            }
            set
            {
                control.Text = value;
            }
        }

        public string ButtonImageUrl
        {
            get
            {

                return control.ButtonImageUrl;
            }
            set
            {

                control.ButtonImageUrl = value;
            }
        }

        public Unit Width
        {
            get
            {
                return control.Width;
            }
            set
            {
                control.Width = value;
            }
        }

        public bool ShowTime
        {
            get
            {
                return control.ShowTime;
            }
            set
            {
                control.ShowTime = value;
            }
        }

        public string ClockHours
        {
            get
            {
                return control.ClockHours;
            }
            set
            {
                control.ClockHours = value;
            }
        }

        

        private void InitializeAdapter()
        {
            control
            = new jsCalendarDatePicker();

        }

        #region Public Methods

        public Control GetControl()
        {
            return control;
        }



        #endregion
    }
}
