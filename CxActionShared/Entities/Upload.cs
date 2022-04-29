using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Upload Data Class
    /// </summary>
    public class Upload : IEntity
    {
        private EntityId _id;
        private string _projectName = string.Empty;
        private string _description = string.Empty;
        private int _preset = 0;
        Dictionary<int, string> _presets = null;
        private string _team = Guid.Empty.ToString();
        Dictionary<string, string> _teams = null;
        private bool _isUploading = false;

        /// <summary>
        /// Entity identifier
        /// </summary>
        public EntityId ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// Project description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets presets list
        /// </summary>
        public Dictionary<int, string> Presets
        {
            get { return _presets; }
            set { _presets = value; }
        }

        /// <summary>
        /// Selected preset
        /// </summary>
        public int Preset
        {
            get { return _preset; }
            set { _preset = value; }
        }

        /// <summary>
        /// Gets or sets teams list
        /// </summary>
        public Dictionary<string, string> Teams
        {
            get { return _teams; }
            set { _teams = value; }
        }

        /// <summary>
        /// Selected team
        /// </summary>
        public string Team
        {
            get { return _team; }
            set { _team = value; }
        }

        /// <summary>
        /// If true - upload form was validated sucessfully and user confirm to start scanning
        /// </summary>
        public bool IsUploading
        {
            get { return _isUploading; }
            set { _isUploading = value; }
        }

        public bool IsPublic
        {
            get;
            set;
        }

        public Upload() { }

        public Upload(EntityId id, string name, string description, Dictionary<int, string> presets, int preset,
                      Dictionary<string, string> teams, string team,bool isPublic)
        {
            this.ID = id;
            this.ProjectName = name;
            this.Description = description;
            this.Presets = presets;
            this.Preset = preset;
            this.Teams = teams;
            this.Team = team;
            IsPublic = isPublic;
        }
    }
}
