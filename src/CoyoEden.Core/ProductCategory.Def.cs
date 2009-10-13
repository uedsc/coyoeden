
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class ProductCategory : BusinessObject
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
        
        public virtual Guid? FKCategory
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKCategory")));
            }
            set
            {
                base.SetPropertyValue("FKCategory", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Category Category
        {
            get
            {
                return Relationships.GetRelatedObject<Category>("Category");
            }
            set
            {
                Relationships.SetRelatedObject("Category", value);
            }
        }
        
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
        #endregion
    }
}
