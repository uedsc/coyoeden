﻿namespace CoyoEden.Core.Infrastructure
{
    /// <summary>
    /// An interface implemented by anti-spam 
    /// services like Waegis, Akismet etc.
    /// </summary>
    public interface ICustomFilter
    {
        /// <summary>
        /// Initializes anti-spam service
        /// </summary>
        /// <returns>True if service online and credentials validated</returns>
        bool Initialize();
        /// <summary>
        /// Check if comment is spam
        /// </summary>
        /// <param name="comment">CoyoEden comment</param>
        /// <returns>True if comment is spam</returns>
        bool Check(IComment comment);
        /// <summary>
        /// Report mistakes back to service
        /// </summary>
        /// <param name="comment">CoyoEden comment</param>
        /// <param name="isSpam">True if spam wasn't blocked</param>
        void Report(IComment comment, bool isSpam);
    }
}
