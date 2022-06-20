using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using System.Diagnostics;
//using CxViewerAction2022.Helpers;

namespace Common
{

    public class Logger
    {
        private PatternLayout _layout = new PatternLayout();
        private const string LOG_PATTERN = "%d [%t] %-5p --- %m%n";
        private static string theDirectory;
        private static string studioVersion = "";
        private static string FileName = "CxVsPlugin.log";
        public static string LogPath
        {
            get { return Logger.theDirectory; }
            set { Logger.theDirectory = value; }
        }

        public string DefaultPattern
        {
            get { return LOG_PATTERN; }
        }

        public static string StudioVersion
        {
            get { return Logger.studioVersion; }
            set { Logger.studioVersion = value; }
        }
        public Logger()
        {
            _layout.ConversionPattern = DefaultPattern;
            _layout.ActivateOptions();
        }

        public PatternLayout DefaultLayout
        {
            get { return _layout; }
        }

        public void AddAppender(IAppender appender)
        {
            Hierarchy hierarchy =
                    (Hierarchy)LogManager.GetRepository();

            hierarchy.Root.AddAppender(appender);
        }

        static Logger()
        {
        }

        public static void SetConfig()
        {
            string file;
            try
            {
                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                TraceAppender tracer = new TraceAppender();
                PatternLayout patternLayout = new PatternLayout();

                patternLayout.ConversionPattern = LOG_PATTERN;
                patternLayout.ActivateOptions();

                tracer.Layout = patternLayout;
                tracer.ActivateOptions();
                hierarchy.Root.AddAppender(tracer);

                RollingFileAppender roller = new RollingFileAppender();
                roller.Layout = patternLayout;
                roller.ImmediateFlush = false;
                roller.AppendToFile = true;
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.MaxSizeRollBackups = 4;
                roller.MaximumFileSize = "10MB";
                roller.StaticLogFileName = true;

                if (String.IsNullOrEmpty(theDirectory))
                {
                    //string fullPath = System.Reflection.Assembly.GetAssembly(typeof(Logger)).Location;

                    //get the folder that's in
                    //theDirectory = Path.GetDirectoryName(fullPath);
                }
                string studioVersion = Process.GetProcessesByName("DevEnv")[0].Modules[0].FileVersionInfo.FileDescription;
                studioVersion = studioVersion.Remove(0, 10);
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format(@"{0}\Settings", studioVersion));
                theDirectory = Path.GetDirectoryName(path);
                if (!theDirectory.Contains("Settings"))
                {
                    theDirectory = Path.Combine(theDirectory, string.Format("Settings"));
                }
                roller.File = theDirectory + @"\CxVsPlugin.log";                
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);

                hierarchy.Root.Level = Level.All;
                hierarchy.Configured = true;
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        public static ILog Create()
        {
            if (String.IsNullOrEmpty(theDirectory))
            {
                SetConfig();
            }
            
            return LogManager.GetLogger("CxVsPlugin");            
        }
        
    }
}
