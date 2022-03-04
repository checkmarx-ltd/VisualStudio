using System;
using System.Collections.Generic;
using CxViewerAction.Entities;

namespace CxViewerAction.Views
{
    /// <summary>
    /// Represent login view obligatory properties and methods
    /// </summary>
    public interface ILoginView : IView
    {
        /// <summary>
        /// Get or set entity identifier
        /// </summary>
        EntityId EntityId { get; set; }

        /// <summary>
        /// Gets or sets to use secured connection
        /// </summary>
        bool Ssl { get; set; }

        /// <summary>
        /// Get or set server domain name (i.e. example.com)
        /// </summary>
        string ServerDomain { get; set; }
    }
}
