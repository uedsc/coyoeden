using System;
using System.Collections;
using System.IO;
using System.Web.UI;

namespace Cynthia.Web
{
    public class CynHtmlTextWriter : HtmlTextWriter
    {
        private string formAction;
        private const string actionAttribute = "action";

        public CynHtmlTextWriter(TextWriter writer)
            : base(writer)
        {}

        public CynHtmlTextWriter(TextWriter writer, string action) : base(writer)
		{
			formAction = action;
		}

		public override void RenderBeginTag(string tagName)
		{
			base.RenderBeginTag(tagName);
		}

		public override void WriteAttribute(string name, string value, bool fEncode)
		{
            // this is to support url re-writing

            
            // 0.0033
            //if (string.Compare(name, "action", true) == 0)
            //if (string.Equals(name, "action", StringComparison.InvariantCultureIgnoreCase))

            // 0.0017
            //if (name == actionAttribute)

            // Antz Profiler showed this to be most performant
            if (string.Equals(name, actionAttribute))
            {
                base.WriteAttribute(name, formAction, fEncode);
            }
            else
            {
                base.WriteAttribute(name, value, fEncode);
            }
		}

    }
}
