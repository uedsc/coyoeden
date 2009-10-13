
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class Profile : BusinessObject
    {
        
        #region Properties
        public virtual Guid? Id
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("Id")));
            }
            set
            {
                base.SetPropertyValue("Id", value);
            }
        }
        
        public virtual String UserName
        {
            get
            {
                return ((String)(base.GetPropertyValue("UserName")));
            }
            set
            {
                base.SetPropertyValue("UserName", value);
            }
        }
        
        public virtual String SettingName
        {
            get
            {
                return ((String)(base.GetPropertyValue("SettingName")));
            }
            set
            {
                base.SetPropertyValue("SettingName", value);
            }
        }
        
        public virtual String SettingValue
        {
            get
            {
                return ((String)(base.GetPropertyValue("SettingValue")));
            }
            set
            {
                base.SetPropertyValue("SettingValue", value);
            }
        }
        #endregion
    }
}
