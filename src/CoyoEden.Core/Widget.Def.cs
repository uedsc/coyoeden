
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace CoyoEden.Core
{
    using System;
    using Habanero.BO;
    
    
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
        
        public virtual Boolean? Movable
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("Movable")));
            }
            set
            {
                base.SetPropertyValue("Movable", value);
            }
        }
        
        public virtual Boolean? Deletable
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("Deletable")));
            }
            set
            {
                base.SetPropertyValue("Deletable", value);
            }
        }
        
        public virtual Boolean? Collapsable
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("Collapsable")));
            }
            set
            {
                base.SetPropertyValue("Collapsable", value);
            }
        }
        
        public virtual Boolean? Editable
        {
            get
            {
                return ((Boolean?)(base.GetPropertyValue("Editable")));
            }
            set
            {
                base.SetPropertyValue("Editable", value);
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
