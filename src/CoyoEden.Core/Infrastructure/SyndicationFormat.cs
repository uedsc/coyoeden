
using System;

namespace CoyoEden.Core.Infrastructure
{
    /// <summary>
    /// Represents common types of syndication formats.
    /// </summary>
    public enum SyndicationFormat
    {
        /// <summary>
        /// No syndication format specified.
        /// </summary>
        None    = 0,
        /// <summary>
        ///  Indicates that a feed conforms to the Atom syndication format.
        /// </summary>
        Atom    = 1,
        /// <summary>
        /// Indicates that a feed conforms to the RSS syndication format.
        /// </summary>
        Rss     = 2
    };
}