using System;
using System.Web.UI;
using SystemX.Web;
namespace Cynthia.Web.Controls
{
    /// <summary>
    /// object extensions
    /// </summary>
    public static partial class ObjectX
    {
        /// <summary>
        /// render a partial view
        /// </summary>
        /// <returns></returns>
        public static void RenderView<T>(this IHtmlWriterControl ctr,string viewName,  Action<T> action) where T : Control
        {
            var vpath = SiteUtils.GetViewPath(viewName);
            ctr.RenderV<T>(false, false, vpath, action);
        }
        /// <summary>
        /// render a partial view
        /// </summary>
        /// <returns></returns>
        public static void RenderView<T>(this IHtmlWriterControl ctr,Action<T> action) where T : Control
        {
            var vpath = SiteUtils.GetViewPath(typeof(T).Name);
            ctr.RenderV<T>(false, false, vpath, action);
        }
        /// <summary>
        /// render a partial view
        /// </summary>
        /// <returns></returns>
        public static void RenderView<T>(this IHtmlWriterControl ctr, Action<T> action,bool pageDepended) where T : Control
        {
            var vpath = SiteUtils.GetViewPath(typeof(T).Name);
            ctr.RenderV<T>(pageDepended, false, vpath, action);
        }
    }
}
