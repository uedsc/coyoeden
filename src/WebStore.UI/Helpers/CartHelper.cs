#region using statements

using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web;
using mojoPortal.Business;
using WebStore.Business;
using mojoPortal.Web.Framework;

#endregion

namespace WebStore.UI
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-03-06
    /// Last Modified:			2008-03-09
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class CartHelper
    {

        public static Cart GetCart(Guid storeGuid)
        {
            

            if (HttpContext.Current != null)
            {
                string cartKey = "cart" + storeGuid.ToString();
                if (HttpContext.Current.Items[cartKey] != null)
                {
                    return (Cart)HttpContext.Current.Items[cartKey];
                }
                else
                {
                    if (UserHasCartCookie(storeGuid))
                    {
                        string cartCookie = GetCartCookie(storeGuid);
                        if (cartCookie.Length == 36)
                        {
                            Guid cartGuid = new Guid(cartCookie);
                            Cart cart = new Cart(cartGuid);
                            if (!cart.Exists)
                            {
                                return CreateCartAndSetCookie(storeGuid);
                            }
                            HttpContext.Current.Items[cartKey] = cart;
                            return cart;

                        }
                        else
                        {
                            // cookie is invalid
                            return CreateCartAndSetCookie(storeGuid);
                        }
                    }
                    else
                    {
                        // TODO: handle use case where user adds to cart on 1 machine
                        // then comes back to site on another machine and has no cart cookie
                        // look for a cart that has the userguid, 
                        // if found set cookie for that cart

                        // new cart
                        return CreateCartAndSetCookie(storeGuid);

                    }

                }

            }

            return null;


        }

        
        private static Cart CreateCartAndSetCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return null;

            string cartKey = "cart" + storeGuid.ToString();
            Cart cart = new Cart();
            cart.StoreGuid = storeGuid;
            if (
                (HttpContext.Current != null)
                && (HttpContext.Current.Request.IsAuthenticated)
                )
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                cart.UserGuid = siteUser.UserGuid;
                cart.LoadExistingUserCartIfExists();

            }
            cart.Save();

            SetCartCookie(storeGuid, cart.CartGuid);
            HttpContext.Current.Items[cartKey] = cart;

            return cart;


        }


        public static bool UserHasCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return false;
            string cartKey = "cart" + storeGuid.ToString();

            return CookieHelper.CookieExists(cartKey);

        }

        public static void SetCartCookie(Guid storeGuid, Guid cartGuid)
        {
            if ((storeGuid != Guid.Empty) && (cartGuid != Guid.Empty))
            {
                string cartKey = "cart" + storeGuid.ToString();

                // TODO encrypt, sign?

                CookieHelper.SetPersistentCookie(cartKey, cartGuid.ToString());


            }

        }

        public static void ClearCartCookie(Guid storeGuid)
        {
            if (storeGuid != Guid.Empty) 
            {
                string cartKey = "cart" + storeGuid.ToString();

                // TODO encrypt, sign?

                CookieHelper.SetPersistentCookie(cartKey, string.Empty);


            }

        }

        public static string GetCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return string.Empty;

            string cartKey = "cart" + storeGuid.ToString();

            // TODO: decrypt and verify?

            return CookieHelper.GetCookieValue(cartKey);


        }

        public static void InitializeOrderInfo(Cart cart, SiteUser siteUser)
        {
            cart.OrderInfo.CustomerName = siteUser.Name;
            cart.OrderInfo.CustomerEmail = siteUser.Email;
            cart.OrderInfo.Save();

        }

    }
}
