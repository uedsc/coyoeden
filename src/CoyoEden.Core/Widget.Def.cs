
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the SystemX Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using SystemX.LunaAtom;
    
    
    public partial class Widget : BusinessObject
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
        
        public virtual Boolean? ShowTitle
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("ShowTitle")));
            }
            set
            {
                base.SetPropertyValue("ShowTitle", value);
            }
        }
        
        public virtual String Template
        {
            get
            {
                return ((String)(base.GetPropertyValue("Template")));
            }
            set
            {
                base.SetPropertyValue("Template", value);
            }
        }
        
        public virtual String Tag
        {
            get
            {
                return ((String)(base.GetPropertyValue("Tag")));
            }
            set
            {
                base.SetPropertyValue("Tag", value);
            }
        }
        
        public virtual Int32? DisplayIndex
        {
            get
            {
                return ((Int32?)(base.GetPropertyValue("DisplayIndex")));
            }
            set
            {
                base.SetPropertyValue("DisplayIndex", value);
            }
        }
        
        public virtual String ExtConfig
        {
            get
            {
                return ((String)(base.GetPropertyValue("ExtConfig")));
            }
            set
            {
                base.SetPropertyValue("ExtConfig", value);
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
        
        public virtual Guid? FKZone
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("FKZone")));
            }
            set
            {
                base.SetPropertyValue("FKZone", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual WidgetZone Zone
        {
            get
            {
                return Relationships.GetRelatedObject<WidgetZone>("Zone");
            }
            set
            {
                Relationships.SetRelatedObject("Zone", value);
            }
        }
        #endregion
    }
}
