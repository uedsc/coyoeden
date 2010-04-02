/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */

using System;
using System.Web.Script.Serialization;

namespace Cynthia.FileSystem
{
    /// <summary>
    /// Represents a user folder.
    /// </summary>
    public class UserFolder
    {
        /// <summary>
        /// Gets or sets the full path related to the root folder of a user.
        /// </summary>
        /// <remarks>
        /// The path should be able to embed in url as a query parameter.
        /// Usually, this can be achieved by replacing directory separator and url encoding.
        /// </remarks>
        /// <value>
        /// The full path related to the root folder of a user.
        /// </value>
        public string Path { get; set; }

        public string Name { get; set; }

        public virtual object ToJson()
        {
            return new { path = Path };
        }
    }
}
