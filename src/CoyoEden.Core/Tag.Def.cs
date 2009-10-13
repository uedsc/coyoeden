
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
    public partial class Tag : BusinessObject
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
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<UserTag> UserTags
        {
            get
            {
                return Relationships.GetRelatedCollection<UserTag>("UserTags");
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
