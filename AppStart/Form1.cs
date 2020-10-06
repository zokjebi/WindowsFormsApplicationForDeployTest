using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppStart
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 업데이트 프로그램 자체를 업데이트할때 파일명 앞에 붙여줄 임시이름
        /// 이 프로그램 실행시마다 "M1234_update.exe" 처럼 "M1234_" 가
        /// 붙은 파일을 Prefix 빼고 덮어씌운 후 원파일 삭제 
        /// </summary>
        public const string updaterPrefix = "M1234_";
        /// <summary>
        /// 현재 대상프로그램이 실행중일때 중지할 프로세스명 (실제 실행 exe 파일명)
        /// </summary>
        private static string processToEnd = "";
        /// <summary>
        /// 업데이트가 끝나고 실행할 프로그램 경로 + 프로그램 실행파일명
        /// Application.StartupPath : "C:\\Program Files\\DeployTest" 처럼 나온다
        /// </summary>
        private static string postProcess = ""; // Application.StartupPath + @"\" + processToEnd + ".exe"
        /// <summary>
        /// 업데이트 프로그램
        /// </summary>
        public static string updater = Application.StartupPath + @"\update.exe";

        public static List<string> info = new List<string>();
        private string thisVersion = string.Empty;
        private string versionfilename = "UM.txt";
        private string downloadsurl = "";       // "http://www.fonesoft.co.kr/SoftwareUpdate/DeployTest/"

        private string downloadZipFIle = "";    // "UM.zip";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            update.updateMe(updaterPrefix, Application.StartupPath + @"\"); // Update 프로그램 자체를 업데이트한다. (이전에 받아놓은 파일의 Rename)

            info = update.populateInfoFromWeb(versionfilename, Application.StartupPath + @"\", 1);
            if (info != null)
            {
                if (info.Count == 1)    // 비정상:빈줄이면 (2번째 줄에 있는지 한번 더 체크)
                {
                    info = update.populateInfoFromWeb(versionfilename, Application.StartupPath + @"\", 2);
                }

                if (info.Count == 5)    // 정상적으로 읽은 경우
                {
                    processToEnd = info[0];
                    postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
                    downloadsurl = info[3];

                    downloadZipFIle = info[4];

                    thisVersion = info[1];
                }
                else if (info.Count == 9) // 비정상:한줄에 붙어 있는 경우
                {
                    processToEnd = info[4];
                    postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
                    downloadsurl = info[7];

                    downloadZipFIle = info[8];
                    thisVersion = info[5];
                }
                else
                {
                    StartApplication();
                    this.Close();
                    return; ;
                }
            }
            else
            {
                StartApplication();
            }
            this.Close();
        }

        private void StartApplication()
        {
            if (postProcess == "")
            {
                postProcess = "WindowsFormsApplicationForDeployTest.exe";
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = postProcess;
            startInfo.Arguments = "";
            Process.Start(startInfo);
        }

        private void checkForUpdate()
        {
            info = update.getUpdtaeInfo(downloadsurl, versionfilename, Application.StartupPath + @"\", 1);

            if (info == null)
            {
                StartApplication();
            }
            else
            {
                if (decimal.Parse(info[1]) > decimal.Parse(thisVersion))
                {
                    update.installUpdateRestart(info[3], info[4], "\"" + Application.StartupPath + "\\", processToEnd, postProcess, "updated", updater);
                }
                else
                {
                    StartApplication();
                }
            }
        }
    }
}
