/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */

using System;
using System.IO;
using System.Web;

namespace Cynthia.FileSystem
{
    /// <summary>
    /// Represents a user file.
    /// </summary>
    /// <remarks>
    /// One and only one of <see cref="Stream"/>, <see cref="Path"/> 
    /// or <see cref="Data"/> should be populated.
    /// </remarks>
    public class UserFile : UserFileInfo
    {
        /// <summary>
        /// Gets or sets the stream of the file.
        /// </summary>
        /// <value>
        /// The stream of the file.
        /// </value>
        public Stream Stream
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        /// <value>
        /// The path of the file.
        /// </value>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the binary data of the file.
        /// </summary>
        /// <value>
        /// The binary data of the file.
        /// </value>
        public byte[] Data
        {
            get;
            set;
        }
    }
}
