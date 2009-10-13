
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class Product : BusinessObject
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
        
        public virtual Decimal? Price
        {
            get
            {
                return ((Decimal?)(base.GetPropertyValue("Price")));
            }
            set
            {
                base.SetPropertyValue("Price", value);
            }
        }
        
        public virtual Decimal? CostPrice
        {
            get
            {
                return ((Decimal?)(base.GetPropertyValue("CostPrice")));
            }
            set
            {
                base.SetPropertyValue("CostPrice", value);
            }
        }
        
        public virtual Int32? Stock
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("Stock")));
            }
            set
            {
                base.SetPropertyValue("Stock", value);
            }
        }
        
        public virtual String ExternalLink
        {
            get
            {
                return ((String)(base.GetPropertyValue("ExternalLink")));
            }
            set
            {
                base.SetPropertyValue("ExternalLink", value);
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
        
        public virtual Guid? FKFolder
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKFolder")));
            }
            set
            {
                base.SetPropertyValue("FKFolder", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual XFolder Folder
        {
            get
            {
                return Relationships.GetRelatedObject<XFolder>("Folder");
            }
            set
            {
                Relationships.SetRelatedObject("Folder", value);
            }
        }
        
        public virtual BusinessObjectCollection<ProductCategory> ProductCategories
        {
            get
            {
                return Relationships.GetRelatedCollection<ProductCategory>("ProductCategories");
            }
        }
        
        public virtual BusinessObjectCollection<ProductExt> ProductExts
        {
            get
            {
                return Relationships.GetRelatedCollection<ProductExt>("ProductExts");
            }
        }
        #endregion
    }
}
