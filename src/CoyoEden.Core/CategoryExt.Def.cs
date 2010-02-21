
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class CategoryExt : BusinessObject
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
