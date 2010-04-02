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

namespace Cynthia.Web.Services.Metaweblog
{
    /// <summary>
    /// This interface is implemented by classes that will convert objects from one thing to another.
    /// </summary>
    /// <typeparam name="TFrom">The type of the source object.</typeparam>
    /// <typeparam name="TTo">The type of the destination object.</typeparam>
    public interface ITransmorgifier<TFrom, TTo>
    {
        /// <summary>
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        TTo Transmorgify(TFrom sourceObject);
    }
}