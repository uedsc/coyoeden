using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.Core.DataContracts
{
    public class WidgetAddingData
    {
        /// <summary>
        /// widget name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// widget zone
        /// </summary>
        public Guid Zone { get; set; }
    }
}
