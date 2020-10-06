using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooSoft;

namespace AppStart
{
    class update
    {
        public static List<string> getUpdtaeInfo(string downloadsURL, string versionFile, string resourceDownladedFolder, int startLine)
        {
            bool updateChecked = false;

            if (!Directory.Exists(resourceDownladedFolder))
            {
                Directory.CreateDirectory(resourceDownladedFolder);
            }

            updateChecked = webdata.downloadFromWeb(downloadsURL, versionFile, resourceDownladedFolder);

            if (updateChecked)
            {
                return populateInfoFromWeb(versionFile, resourceDownladedFolder, startLine);
            }
            else
            {
                return null;
            }
        }

        public static void installUpdateNow(string downloadsURL, string filename, string downloadTo, bool unzip)
        {
            bool downloadSuccess = webdata.downloadFromWeb(downloadsURL, filename, downloadTo);

            if (unzip)
            {
                unZip(downloadTo + filename, downloadTo);
            }
        }

        public static void installUpdateRestart(string downloadURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {
            string cmdLn = "";

            cmdLn += "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess;
            cmdLn += "|command|" + @"/" + startupCommand;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updater;
            startInfo.Arguments = cmdLn;
            Process.Start(startInfo);
        }

        public static List<string> populateInfoFromWeb(string versionFile, string resourceDownladedFolder, int line)
        {
            List<string> tempList = new List<string>();
            int ln;
            int i;

            ln = 0;

            if (File.Exists(resourceDownladedFolder + versionFile))
            {
                foreach (string strline in File.ReadAllLines(resourceDownladedFolder + versionFile))
                {
                    if (ln == line)
                    {
                        string[] parts = strline.Split('|');
                        foreach (string part in parts)
                        {
                            tempList.Add(part);
                        }

                        return tempList;
                    }

                    ln++;
                }
            }

            return null;
        }

        private static bool unZip(string file, string unZipTo)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(file))
                {
                    zip.ExtractAll(unZipTo);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void updateMe(string updaterPrefix, string containingFolder)
        {
            DirectoryInfo dInfo = new DirectoryInfo(containingFolder);
            FileInfo[] updaterFiles = dInfo.GetFiles(updaterPrefix + "*");
            int fileCount = updaterFiles.Length;

            foreach (FileInfo file in updaterFiles)
            {
                string newFile = containingFolder + file.Name;
                string oriFile = containingFolder + @"\" + file.Name.Substring(updaterPrefix.Length, file.Name.Length - updaterPrefix.Length);

                if (File.Exists(oriFile))
                {
                    File.Delete(oriFile);
                }
            }
        }

        
    }
}
