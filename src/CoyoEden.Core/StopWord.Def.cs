
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class StopWord : BusinessObject
    {
        
        #region Properties
        public virtual String Word
        {
            get
            {
                return ((String)(base.GetPropertyValue("Word")));
            }
            set
            {
                base.SetPropertyValue("Word", value);
            }
        }
        #endregion
    }
}
