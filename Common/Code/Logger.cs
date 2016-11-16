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


namespace Common
{

    public class Logger
    {
        private PatternLayout _layout = new PatternLayout();
        private const string LOG_PATTERN = "%d [%t] %-5p --- %m%n";
        private static string theDirectory;

        public static string LogPath
        {
            get { return Logger.theDirectory; }
            set { Logger.theDirectory = value; }
        }

        public string DefaultPattern
        {
            get { return LOG_PATTERN; }
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
                roller.AppendToFile = true;
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.MaxSizeRollBackups = 4;
                roller.MaximumFileSize = "100KB";
                roller.StaticLogFileName = true;

                if (String.IsNullOrEmpty(theDirectory))
                {
                    string fullPath = System.Reflection.Assembly.GetAssembly(typeof(Logger)).Location;

                    //get the folder that's in
                    theDirectory = Path.GetDirectoryName(fullPath);

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
            if(String.IsNullOrEmpty(theDirectory))
            {
                SetConfig();
            }
            return LogManager.GetLogger("CxVsPlugin");
        }
    }
}
