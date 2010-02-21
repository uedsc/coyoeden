
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class User : BusinessObject
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
        
        public virtual String UserName
        {
            get
            {
                return ((String)(base.GetPropertyValue("UserName")));
            }
            set
            {
                base.SetPropertyValue("UserName", value);
            }
        }
        
        public virtual String Password
        {
            get
            {
                return ((String)(base.GetPropertyValue("Password")));
            }
            set
            {
                base.SetPropertyValue("Password", value);
            }
        }
        
        public virtual DateTime? LastLoginTime
        {
            get
            {
                return ((DateTime?)(base.GetPropertyValue("LastLoginTime")));
            }
            set
            {
                base.SetPropertyValue("LastLoginTime", value);
            }
        }
        
        public virtual String EmailAddress
        {
            get
            {
                return ((String)(base.GetPropertyValue("EmailAddress")));
            }
            set
            {
                base.SetPropertyValue("EmailAddress", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<UserRole> UserRoles
        {
            get
            {
                return Relationships.GetRelatedCollection<UserRole>("UserRoles");
            }
        }
        
        public virtual BusinessObjectCollection<UserTag> UserTags
        {
            get
            {
                return Relationships.GetRelatedCollection<UserTag>("UserTags");
            }
        }
        #endregion
    }
}
