using System;
using System.Collections.Generic;
using System.Text;

namespace CxViewerAction.Entities
{
    public class ScanReportInfo
    {
        long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }        
    }
}
