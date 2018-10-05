using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace https
{
    class App
    {
        static void Main(string[] args) => f_app_Run(args);
        static IpcServer server;

        #region [ CONSOLE ]

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void f_console_Hide()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        static void f_console_Show()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }

        #endregion

        #region [ APP ]

        static void f_app_keepRunning()
        {
            string cmd = string.Empty;
            while (cmd != "exit")
            {
                cmd = Console.ReadLine();
                switch (cmd) {
                    case "cls":
                    case "clear":
                        Console.Clear();
                        break;
                }
            }
        }

        static void f_app_Run(string[] args)
        {
            Console.Title = "HTTPS";

            //f_console_Hide();
            f_app_checkToolRunning();

            ThreadPool.SetMaxThreads(25, 25);
            ServicePointManager.DefaultConnectionLimit = 1000;

            /* Certificate validation callback */
            ServicePointManager.ServerCertificateValidationCallback += (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) =>
            {
                /* If the certificate is a valid, signed certificate, return true. */
                if (error == System.Net.Security.SslPolicyErrors.None) return true;
                //Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'", cert.Subject, error.ToString());
                return false;
            };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

            ///////////////////////////////////////////////////////////////////////////////////////

            if (!Directory.Exists("cache")) Directory.CreateDirectory("cache");
            server = new IpcServer();
            server.Start();
            f_app_keepRunning();
            server.Stop();
            Process.GetCurrentProcess().Kill();
        }

        static void f_app_checkToolRunning()
        {
            try
            {
                /// Get process current
                Process processCurrent = Process.GetCurrentProcess();

                // Get all process running
                var wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process";
                using (var searcher = new ManagementObjectSearcher(wmiQueryString))
                using (var results = searcher.Get())
                {
                    string name = processCurrent.ProcessName.ToLower();
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    int len = (from p in Process.GetProcesses()
                               join mo in results.Cast<ManagementObject>()
                               on p.Id equals (int)(uint)mo["ProcessId"]
                               select new
                               {
                                   Name = p.ProcessName,
                                   Path = (string)mo["ExecutablePath"],
                               })
                                .Where(x =>
                                    !string.IsNullOrEmpty(x.Name)
                                    && x.Name.ToLower() == name
                                    && !string.IsNullOrEmpty(x.Path)
                                    && x.Path.StartsWith(path))
                                .Count();

                    // If exist other application running
                    if (len > 1)
                    {
                        // If click open second show message exit
                        if (len == 2) MessageBox.Show(string.Format("HTTPS running at ", path), "Message");

                        ///////////////////////////////////
                        /// exit process application current
                        int pi = processCurrent.Id;
                        Process p = Process.GetProcessById(pi);
                        p.Kill();
                    }
                }
            }
            catch { }
        }

        #endregion
    }
}
