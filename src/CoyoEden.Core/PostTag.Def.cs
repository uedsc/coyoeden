
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class PostTag : BusinessObject
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
        #endregion
        
        #region Relationships
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
