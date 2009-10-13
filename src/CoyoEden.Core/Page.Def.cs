
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class Page : BusinessObject
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
        
        public virtual String Keywords
        {
            get
            {
                return ((String)(base.GetPropertyValue("Keywords")));
            }
            set
            {
                base.SetPropertyValue("Keywords", value);
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
        
        public virtual Boolean? IsFrontPage
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("IsFrontPage")));
            }
            set
            {
                base.SetPropertyValue("IsFrontPage", value);
            }
        }
        
        public virtual Boolean? ShowInList
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("ShowInList")));
            }
            set
            {
                base.SetPropertyValue("ShowInList", value);
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
        public virtual Page Parent
        {
            get
            {
                return Relationships.GetRelatedObject<Page>("Parent");
            }
            set
            {
                Relationships.SetRelatedObject("Parent", value);
            }
        }
        
        public virtual BusinessObjectCollection<Page> SubPages
        {
            get
            {
                return Relationships.GetRelatedCollection<Page>("SubPages");
            }
        }
        #endregion
    }
}
