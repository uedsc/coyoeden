
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class XPropertySetting : BusinessObject
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
        
        public virtual Boolean? IsDeleted
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsDeleted")));
            }
            set
            {
                base.SetPropertyValue("IsDeleted", value);
            }
        }
        
        public virtual Guid? FKXProperty
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKXProperty")));
            }
            set
            {
                base.SetPropertyValue("FKXProperty", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<ProductExt> ProductExts
        {
            get
            {
                return Relationships.GetRelatedCollection<ProductExt>("ProductExts");
            }
        }
        
        public virtual XProperty XProperty
        {
            get
            {
                return Relationships.GetRelatedObject<XProperty>("XProperty");
            }
            set
            {
                Relationships.SetRelatedObject("XProperty", value);
            }
        }
        #endregion
    }
}
