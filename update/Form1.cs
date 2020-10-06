using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooSoft;

namespace update
{
    public partial class Form1 : Form
    {
        bool called = true;

        private string tempDownloadFolder = "";
        private string processToEnd = "";
        private string downloadFile = "";
        private string URL = "";
        private string destinationFolder = "";
        private string updateFolder = Application.StartupPath + @"\updates\";
        private string postProcessFile = "";
        private string postProcessCommand = "";

        delegate void SetLabelCallback(Label label, string text);

        public void SetLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetLabel);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
                label.Refresh();
                Invalidate();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();

            if (called)
            {
                WindowState = FormWindowState.Normal;

                if (this.ClientRectangle.Height<progressBar1.Location.Y + progressBar1.Size.Height)
                {
                    this.Height = 21 + progressBar1.Location.Y + progressBar1.Size.Height * 2;
                }

                Show();

                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork -= new DoWorkEventHandler(backgroundWorker);
                bw.DoWork += new DoWorkEventHandler(backgroundWorker);
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerAsync();
            }
        }

        private void backgroundWorker(object sender, DoWorkEventArgs e)
        {
            preDownload();

            if (called)
            {
                WindowState = FormWindowState.Normal;
                Show();

                SetLabel(line1, "Stopping " + processToEnd);
                Thread.Sleep(1000);

                try
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processToEnd)
                        {
                            process.Kill();
                        }
                    }
                }
                catch (Exception)
                {

                }

                webdata.bytesDownloaded += Bytesdownloaded;
                webdata.downloadFromWeb(URL, downloadFile, tempDownloadFolder);

                SetLabel(line1, "Unzippingpackage...");
                Thread.Sleep(1000);
                unZip(tempDownloadFolder + downloadFile, tempDownloadFolder);
                SetLabel(line1, "Moving files...");
                Thread.Sleep(1000);
                moveFiles();
                SetLabel(line1, "Wrapping up...");
                wrapUp();
                if (postProcessFile != "")
                {
                    postDownload();
                }
            }

            Close();
        }

        private void unpackCommandline()
        {
            string cmdLn = "";
            
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                cmdLn += arg;
            }

            if (cmdLn.IndexOf('|') == -1)
            {
                called = false;
                info info = new info();
                info.ShowDialog();
                Close();
            }

            string[] tmpCmd = cmdLn.Split('|');

            for (int i=1; i<tmpCmd.GetLength(0); i++)
            {
                if (tmpCmd[i] == "downloadFile") downloadFile = tmpCmd[i + 1];
                if (tmpCmd[i] == "URL") URL = tmpCmd[i + 1];
                if (tmpCmd[i] == "destinationFolder") destinationFolder = tmpCmd[i + 1];
                if (tmpCmd[i] == "processToEnd") processToEnd = tmpCmd[i + 1];
                if (tmpCmd[i] == "postProcess") postProcessFile = tmpCmd[i + 1];
                if (tmpCmd[i] == "command") postProcessCommand += @" /" + tmpCmd[i + 1];
                i++;
            }
        }

        private void unZip(string file, string unZipTo)
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

            }
        }

        private void preDownload()
        {
            if (!Directory.Exists(updateFolder))
            {
                Directory.CreateDirectory(updateFolder);
            }

            tempDownloadFolder = updateFolder + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + @"\";

            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }

            Directory.CreateDirectory(tempDownloadFolder);

            unpackCommandline();
        }

        private void postDownload()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = postProcessFile;
            startInfo.Arguments = postProcessCommand;
            Process.Start(startInfo);
        }

        private void wrapUp()
        {
            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }
        }

        private void moveFiles()
        {
            DirectoryInfo di = new DirectoryInfo(tempDownloadFolder);
            FileInfo[] files = di.GetFiles();

            foreach (FileInfo fi in files)
            {
                if (fi.Name != downloadFile)
                {
                    File.Copy(tempDownloadFolder + fi.Name, destinationFolder + fi.Name, true);
                }
            }
        }

        private void Bytesdownloaded(ByteArgs e)
        {
            progressBar1.Maximum = e.total;

            if (progressBar1.Value + e.downloaded <= progressBar1.Maximum)
            {
                progressBar1.Value += e.downloaded;
                SetLabel(line1, "DownloadingUpdate...");
            }
            else
            {
                SetLabel(line1, "Downloadcomplete.");
            }

            progressBar1.Refresh();
            Invalidate();
        }
    }
}
