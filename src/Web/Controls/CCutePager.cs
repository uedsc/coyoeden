using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cynthia.Web.Controls;
using Resources;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// This control inherits form CutePager and applies localization from Resource.resx
    /// </summary>
    public class CCutePager : CutePager
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (HttpContext.Current == null) { return; }

            this.NavigateToPageText = Resource.CutePagerNavigateToPageText;
            this.GoClause = Resource.CutePagerGoClause;
            this.OfClause = Resource.CutePagerOfClause;
            this.FromClause = Resource.CutePagerFromClause;
            this.PageClause = Resource.CutePagerPageClause;
            this.ToClause = Resource.CutePagerToClause;
            this.ShowingResultClause = Resource.CutePagerShowingResultClause;
            this.ShowResultClause = Resource.CutePagerShowResultClause;
            this.BackToFirstClause = Resource.CutePagerBackToFirstClause;
            this.GoToLastClause = Resource.CutePagerGoToLastClause;
            this.BackToPageClause = Resource.CutePagerBackToPageClause;
            this.NextToPageClause = Resource.CutePagerNextToPageClause;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
                {
                    this.RTL = true;
                }
            }
            catch { }

        }
    }
}
