
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class PostCategory : BusinessObject
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
        
        public virtual Guid? PostID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("PostID")));
            }
            set
            {
                base.SetPropertyValue("PostID", value);
            }
        }
        
        public virtual Guid? CategoryID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("CategoryID")));
            }
            set
            {
                base.SetPropertyValue("CategoryID", value);
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
        
        public virtual Post Post
        {
            get
            {
                return Relationships.GetRelatedObject<Post>("Post");
            }
            set
            {
                Relationships.SetRelatedObject("Post", value);
            }
        }
        #endregion
    }
}
