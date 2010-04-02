using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace Cynthia.Web.Controls.ExtJs
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-11-04
    /// Last Modified:			2007-12-01
    ///		
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// Extends the TemplateField with a few custom properties.
    /// </summary>
    public class ExtTemplateField : TemplateField
    {
        private bool isClientHideable = false;
        private bool isClientSortable = false;
        private int columnWidth = -1;

        /// <summary>
        /// true if you want column to be hideable by client side javascript
        /// </summary>
        public bool IsClientHideable
        {
            get { return isClientHideable; }
            set { isClientHideable = value; }
        }

        /// <summary>
        /// true if you want the column to be sortable by javascript. 
        /// Note this only sorts the already fetched data.
        /// </summary>
        public bool IsClientSortable
        {
            get { return isClientSortable; }
            set { isClientSortable = value; }
        }

        /// <summary>
        /// If set to a positive number will be used for column width
        /// </summary>
        public int ColumnWidth
        {
            get { return columnWidth; }
            set { columnWidth = value; }
        }
    }
}
