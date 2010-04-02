// Author:				Tom Opgenorth	
// Created:				2008-04-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
using System;

using Cynthia.Business;

namespace Cynthia.Web.Services.Metaweblog.Transmorgifiers
{
    /// <summary>
    /// An abstract class for <see cref="ITransmorgifier{TFrom,TTo} "/>s that will
    /// convert objects to and from Cynthia objects.
    /// </summary>
    /// <typeparam name="TFrom">The type of the object to transmorgify.</typeparam>
    /// <typeparam name="TTo">The type of the object to transmorgify into.</typeparam>
    public abstract class TransmorgifierBase<TFrom, TTo> : ITransmorgifier<TFrom, TTo>
    {
        private readonly SiteSettings _siteSettings;

        protected TransmorgifierBase(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
        }

        protected SiteSettings SiteSettings
        {
            get { return _siteSettings; }
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public abstract TTo Transmorgify(TFrom sourceObject);
    }
}