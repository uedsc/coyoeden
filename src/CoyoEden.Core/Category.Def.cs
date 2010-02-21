
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class Category : BusinessObject
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
        
        public virtual String Name
        {
            get
            {
                return ((String)(base.GetPropertyValue("Name")));
            }
            set
            {
                base.SetPropertyValue("Name", value);
            }
        }
        
        public virtual String Description
        {
            get
            {
                return ((String)(base.GetPropertyValue("Description")));
            }
            set
            {
                base.SetPropertyValue("Description", value);
            }
        }
        
        public virtual String Tag
        {
            get
            {
                return ((String)(base.GetPropertyValue("Tag")));
            }
            set
            {
                base.SetPropertyValue("Tag", value);
            }
        }
        
        public virtual Guid? ParentID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("ParentID")));
            }
            set
            {
                base.SetPropertyValue("ParentID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<CategoryExt> CategoryExts
        {
            get
            {
                return Relationships.GetRelatedCollection<CategoryExt>("CategoryExts");
            }
        }
        
        public virtual Category Parent
        {
            get
            {
                return Relationships.GetRelatedObject<Category>("Parent");
            }
            set
            {
                Relationships.SetRelatedObject("Parent", value);
            }
        }
        
        public virtual BusinessObjectCollection<PostCategory> PostCategories
        {
            get
            {
                return Relationships.GetRelatedCollection<PostCategory>("PostCategories");
            }
        }
        
        public virtual BusinessObjectCollection<ProductCategory> ProductCategories
        {
            get
            {
                return Relationships.GetRelatedCollection<ProductCategory>("ProductCategories");
            }
        }
        
        public virtual BusinessObjectCollection<Category> SubCategories
        {
            get
            {
                return Relationships.GetRelatedCollection<Category>("SubCategories");
            }
        }
        #endregion
    }
}
