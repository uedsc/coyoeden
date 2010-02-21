
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class Post : BusinessObject
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
        
        public virtual String Title
        {
            get
            {
                return ((String)(base.GetPropertyValue("Title")));
            }
            set
            {
                base.SetPropertyValue("Title", value);
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
        
        public virtual String Content
        {
            get
            {
                return ((String)(base.GetPropertyValue("Content")));
            }
            set
            {
                base.SetPropertyValue("Content", value);
            }
        }
        
        public virtual DateTime? DateCreated
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("DateCreated")));
            }
            set
            {
                base.SetPropertyValue("DateCreated", value);
            }
        }
        
        public virtual DateTime? DateModified
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("DateModified")));
            }
            set
            {
                base.SetPropertyValue("DateModified", value);
            }
        }
        
        public virtual String Author
        {
            get
            {
                return ((String)(base.GetPropertyValue("Author")));
            }
            set
            {
                base.SetPropertyValue("Author", value);
            }
        }
        
        public virtual Boolean? IsPublished
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsPublished")));
            }
            set
            {
                base.SetPropertyValue("IsPublished", value);
            }
        }
        
        public virtual Boolean? IsCommentEnabled
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsCommentEnabled")));
            }
            set
            {
                base.SetPropertyValue("IsCommentEnabled", value);
            }
        }
        
        public virtual Boolean? IsHeadline
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsHeadline")));
            }
            set
            {
                base.SetPropertyValue("IsHeadline", value);
            }
        }
        
        public virtual Int32? Raters
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("Raters")));
            }
            set
            {
                base.SetPropertyValue("Raters", value);
            }
        }
        
        public virtual Single? Rating
        {
            get
            {
                return ((Single?)(base.GetPropertyValue("Rating")));
            }
            set
            {
                base.SetPropertyValue("Rating", value);
            }
        }
        
        public virtual String Slug
        {
            get
            {
                return ((String)(base.GetPropertyValue("Slug")));
            }
            set
            {
                base.SetPropertyValue("Slug", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<PostCategory> PostCategories
        {
            get
            {
                return Relationships.GetRelatedCollection<PostCategory>("PostCategories");
            }
        }
        
        public virtual BusinessObjectCollection<PostComment> PostComments
        {
            get
            {
                return Relationships.GetRelatedCollection<PostComment>("PostComments");
            }
        }
        
        public virtual BusinessObjectCollection<PostNotify> PostNotifies
        {
            get
            {
                return Relationships.GetRelatedCollection<PostNotify>("PostNotifies");
            }
        }
        
        public virtual BusinessObjectCollection<PostTag> PostTags
        {
            get
            {
                return Relationships.GetRelatedCollection<PostTag>("PostTags");
            }
        }
        #endregion
    }
}
