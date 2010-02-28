using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core.DataContracts;
using System.Web;
using System.Web.Security;

namespace CoyoEden.UI.Views
{
    public class CommentFormView:ViewBase
    {
        public CommentInputData CommentData
        {
            get
            {
                var retVal = new CommentInputData();
                HttpCookie cookie = Request.Cookies["comment"];
                try
                {
                    if (cookie != null)
                    {
                        retVal.UserName = Server.UrlDecode(cookie.Values["name"]);
                        retVal.Email = cookie.Values["email"];
                        retVal.Website = cookie.Values["url"];
                        retVal.Country = cookie.Values["country"];
                    }
                    else if (Page.User.Identity.IsAuthenticated)
                    {
                        var user = Membership.GetUser();
                        retVal.UserName = user.UserName;
                        retVal.Email = user.Email;
                        retVal.Website = Request.Url.Host;
                    }
                }
                catch (Exception)
                {
                    // Couldn't retrieve info on the visitor/user
                }
                return retVal;
            }
        }
        public int AvatarSize { get; set; }
        /// <summary>
        /// avatar picture of current user
        /// </summary>
        public string Avatar {
            get
            {
                if (AvatarSize<80||AvatarSize>180) AvatarSize=80;
                return Gravatar(CommentData.Email ?? CommentData.Website, CommentData.UserName, AvatarSize);
            }
        }
    }
}
