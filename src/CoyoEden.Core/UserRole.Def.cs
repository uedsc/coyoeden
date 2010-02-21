
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class UserRole : BusinessObject
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
        
        public virtual Guid? UserID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("UserID")));
            }
            set
            {
                base.SetPropertyValue("UserID", value);
            }
        }
        
        public virtual Guid? RoleID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("RoleID")));
            }
            set
            {
                base.SetPropertyValue("RoleID", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual Role Role
        {
            get
            {
                return Relationships.GetRelatedObject<Role>("Role");
            }
            set
            {
                Relationships.SetRelatedObject("Role", value);
            }
        }
        
        public virtual User User
        {
            get
            {
                return Relationships.GetRelatedObject<User>("User");
            }
            set
            {
                Relationships.SetRelatedObject("User", value);
            }
        }
        #endregion
    }
}
