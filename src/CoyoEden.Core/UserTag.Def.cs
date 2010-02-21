
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class UserTag : BusinessObject
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
        
        public virtual Guid? FKUser
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKUser")));
            }
            set
            {
                base.SetPropertyValue("FKUser", value);
            }
        }
        
        public virtual Guid? FKTag
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKTag")));
            }
            set
            {
                base.SetPropertyValue("FKTag", value);
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
        #endregion
        
        #region Relationships
        public virtual Tag Tag
        {
            get
            {
                return Relationships.GetRelatedObject<Tag>("Tag");
            }
            set
            {
                Relationships.SetRelatedObject("Tag", value);
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
