using System;
using System.Web;

namespace Cynthia.Web
{
    public interface IProcessRequest
    {
        void ProcessRequest(HttpContext context);
    }
}
