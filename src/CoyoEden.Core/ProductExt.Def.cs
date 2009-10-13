
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class ProductExt : BusinessObject
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
        
        public virtual Guid? FKProduct
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKProduct")));
            }
            set
            {
                base.SetPropertyValue("FKProduct", value);
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
        
        public virtual Guid? FKXPropertySetting
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKXPropertySetting")));
            }
            set
            {
                base.SetPropertyValue("FKXPropertySetting", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Product Product
        {
            get
            {
                return Relationships.GetRelatedObject<Product>("Product");
            }
            set
            {
                Relationships.SetRelatedObject("Product", value);
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
        
        public virtual XPropertySetting XPropertySetting
        {
            get
            {
                return Relationships.GetRelatedObject<XPropertySetting>("XPropertySetting");
            }
            set
            {
                Relationships.SetRelatedObject("XPropertySetting", value);
            }
        }
        #endregion
    }
}
