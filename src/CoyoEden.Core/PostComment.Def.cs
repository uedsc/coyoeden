
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class PostComment : BusinessObject
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
        
        public virtual String Email
        {
            get
            {
                return ((String)(base.GetPropertyValue("Email")));
            }
            set
            {
                base.SetPropertyValue("Email", value);
            }
        }
        
        public virtual String Website
        {
            get
            {
                return ((String)(base.GetPropertyValue("Website")));
            }
            set
            {
                base.SetPropertyValue("Website", value);
            }
        }
        
        public virtual String Country
        {
            get
            {
                return ((String)(base.GetPropertyValue("Country")));
            }
            set
            {
                base.SetPropertyValue("Country", value);
            }
        }
        
        public virtual String Ip
        {
            get
            {
                return ((String)(base.GetPropertyValue("Ip")));
            }
            set
            {
                base.SetPropertyValue("Ip", value);
            }
        }
        
        public virtual Boolean? IsApproved
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsApproved")));
            }
            set
            {
                base.SetPropertyValue("IsApproved", value);
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
