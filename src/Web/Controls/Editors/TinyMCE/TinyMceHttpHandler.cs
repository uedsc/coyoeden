
using System;
using System.Web;
//using Moxiecode.TinyMCE.Compression;

namespace Cynthia.Web.Editor
{
    /// <summary>
    /// Handles callback for TinyMCE editor
    /// </summary>
    public class TinyMceHttpHandler : IHttpHandler
    {
       
        public TinyMceHttpHandler()
        {
        }

       
        
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            IProcessRequest requestProcessor = null;

            switch (request.QueryString["rp"])
            {
                //case "GzipModule":
                //    requestProcessor = new Moxiecode.TinyMCE.Compression.GzipModule();
                //    break;

                case "spellchecker":
                    requestProcessor = new SpellCheckerRequestProcessor();
                    break;
            }

            if (requestProcessor != null)
                requestProcessor.ProcessRequest(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
