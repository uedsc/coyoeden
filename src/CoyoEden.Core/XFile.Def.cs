
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class XFile : BusinessObject
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
        
        public virtual String Url
        {
            get
            {
                return ((String)(base.GetPropertyValue("Url")));
            }
            set
            {
                base.SetPropertyValue("Url", value);
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
        
        public virtual String ExtName
        {
            get
            {
                return ((String)(base.GetPropertyValue("ExtName")));
            }
            set
            {
                base.SetPropertyValue("ExtName", value);
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
        #endregion
    }
}
