
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cynthia.Web.UI
{
    /// <summary>
    /// The only purpose of this panel is to easily have a wrapper panel that will automatically have a CSS class
    /// based on the ModuleId so that it is possible to easily make a specific module instance have a different style
    /// </summary>
    public class ModulePanel : Panel
    {

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (moduleId > -1)
            {
                if (CssClass.Length == 0)
                {
					CssClass = String.Format("module{0}", moduleId.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
					CssClass += String.Format(" module{0}", moduleId.ToString(CultureInfo.InvariantCulture));
                }
            }

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
				writer.Write(String.Format("[{0}]", this.ID));
                return;
            }
            
            if (WebConfigSettings.RenderModulePanel)
            {
                base.Render(writer);
            }
            else
            {
                base.RenderContents(writer);
            }
            
        }


    }
}
