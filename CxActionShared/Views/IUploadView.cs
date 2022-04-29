using System;
using System.Collections.Generic;
using CxViewerAction.Entities;

namespace CxViewerAction.Views
{
    /// <summary>
    /// Represent upload view obligatory methods and properties
    /// </summary>
    public interface IUploadView : IView
    {
        /// <summary>
        /// Get or set entity identifier
        /// </summary>
        EntityId EntityId { get; set; }

        /// <summary>
        /// Get or set project name
        /// </summary>
        string ProjectName { get; set; }

        /// <summary>
        /// Get or set project description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Get or set selected preset
        /// </summary>
        int Preset { get; set; }

        /// <summary>
        /// Gets or sets presets list
        /// </summary>
        Dictionary<int, string> Presets { get; set; }

        /// <summary>
        /// Get or set selected team
        /// </summary>
        string Team { get; set; }

        /// <summary>
        /// Gets or sets teams list
        /// </summary>
        Dictionary<string, string> Teams { get; set; }

        bool IsPublic { get; }
    }
}
