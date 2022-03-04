using System;
using System.IO;
using Common;

namespace CxViewerAction.Helpers
{
    public class StorageHelper
    {
        #region [Private constants]

        /// <summary>
        /// Main storage folder under VS path
        /// </summary>
        //private static readonly string _baseDir = "CxViewerActionStore";

        static string BaseDir
        {
            get
            {
                string addinTargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Checkmarx\Visual Studio Plugin\CxViewerActionStore");
                return addinTargetPath;
            }
        }

        #endregion

        #region [Static Methods]

        /// <summary>
        /// Returns contents of specific file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Read(string fileName)
        {
            string content = null;

            try
            {
                string fullFileName = GetFullPath(fileName);
                Logger.Create().Debug("Read XML File : " + fullFileName);
                FileInfo fileInfo = new FileInfo(fullFileName);

                if (fileInfo.Exists)
                {
                    using (StreamReader reader = new StreamReader(fullFileName))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex);
            }
            return content;
        }

        /// <summary>
        /// Save file
        /// </summary>
        /// <param name="fileContent">content</param>
        /// <param name="fileName">file name to save</param>
        /// <returns></returns>
        public static bool Save(byte[] fileContent, string fileName)
        {
            FileStream fs = null;
            BinaryWriter bw = null;
            bool res = false;

            try
            {
                if (!Directory.Exists(BaseDir))
                    Directory.CreateDirectory(BaseDir);

                string fullFileName = GetFullPath(fileName);
                Logger.Create().Debug("Save XML File : " + fullFileName);
                fs = new FileStream(fullFileName, FileMode.Create, FileAccess.ReadWrite);
                bw = new BinaryWriter(fs);
                
                bw.Write(fileContent);

                res = true;
            }
            catch (Exception ex)
			{
                Logger.Create().Error(ex);
                res = false;
			}
            finally
            {
                if (bw != null)
                    bw.Close();

                if (fs != null)
                    fs.Close();
            }

            return res;
        }

        public static bool Save(string fileContent, string fileName)
        {
            byte[] content = System.Text.Encoding.UTF8.GetBytes(fileContent);
            return Save(content, fileName);
        }

        public static bool FileExist(string fileName, out string fileOut)
        {
            fileOut = GetFullPath(fileName);
            FileInfo fileInfo = new FileInfo(fileOut);

            return fileInfo.Exists;
        }

        private static string GetFullPath(string file)
        {
            return BaseDir + "\\" + file;
        }

        public static void Delete(string perspective)
        {
            string fullFileName = GetFullPath(perspective);
            try
            {
                if (File.Exists(fullFileName))
                    File.Delete(fullFileName);
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex);
            }

        }

        #endregion
    }
}
