
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class XFolder : BusinessObject
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
        
        public virtual Int32? DisplayOrder
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("DisplayOrder")));
            }
            set
            {
                base.SetPropertyValue("DisplayOrder", value);
            }
        }
        
        public virtual DateTime? CreatedOn
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("CreatedOn")));
            }
            set
            {
                base.SetPropertyValue("CreatedOn", value);
            }
        }
        
        public virtual String CreatedBy
        {
            get
            {
                return ((String)(base.GetPropertyValue("CreatedBy")));
            }
            set
            {
                base.SetPropertyValue("CreatedBy", value);
            }
        }
        
        public virtual DateTime? ModifiedOn
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("ModifiedOn")));
            }
            set
            {
                base.SetPropertyValue("ModifiedOn", value);
            }
        }
        
        public virtual String ModifiedBy
        {
            get
            {
                return ((String)(base.GetPropertyValue("ModifiedBy")));
            }
            set
            {
                base.SetPropertyValue("ModifiedBy", value);
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
        
        public virtual Int32? Hits
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("Hits")));
            }
            set
            {
                base.SetPropertyValue("Hits", value);
            }
        }
        
        public virtual Int32? XType
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("XType")));
            }
            set
            {
                base.SetPropertyValue("XType", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<Product> Products
        {
            get
            {
                return Relationships.GetRelatedCollection<Product>("Products");
            }
        }
        
        public virtual BusinessObjectCollection<XFile> XFiles
        {
            get
            {
                return Relationships.GetRelatedCollection<XFile>("XFiles");
            }
        }
        
        public virtual BusinessObjectCollection<XFolderTag> XFolderTags
        {
            get
            {
                return Relationships.GetRelatedCollection<XFolderTag>("XFolderTags");
            }
        }
        #endregion
    }
}
