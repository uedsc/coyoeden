using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cynthia.Web
{
    /// <summary>
    /// this is just a base class for pages not related to cms  pages like SiteSettings and other pages where the skin used should be just the site skin
    /// and not determined by the current CMS page
    /// </summary>
    public class NonCmsBasePage : CBasePage
    {
    }
}
