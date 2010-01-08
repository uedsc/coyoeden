
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class XProperty : BusinessObject
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
        
        public virtual String Icon
        {
            get
            {
                return ((String)(base.GetPropertyValue("Icon")));
            }
            set
            {
                base.SetPropertyValue("Icon", value);
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
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<CategoryExt> CategoryExts
        {
            get
            {
                return Relationships.GetRelatedCollection<CategoryExt>("CategoryExts");
            }
        }
        
        public virtual BusinessObjectCollection<ProductExt> ProductExts
        {
            get
            {
                return Relationships.GetRelatedCollection<ProductExt>("ProductExts");
            }
        }
        
        public virtual BusinessObjectCollection<XPropertySetting> XPropertySettings
        {
            get
            {
                return Relationships.GetRelatedCollection<XPropertySetting>("XPropertySettings");
            }
        }
        #endregion
    }
}
