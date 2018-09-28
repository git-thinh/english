using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AppRun
{
    /// <summary>
    /// Application context instance timmer exit program after 100 milliseconds
    /// </summary>
    public class RunApplicationContext : ApplicationContext
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public RunApplicationContext()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += (se, ev) =>
            {
                int pi = Process.GetCurrentProcess().Id;
                Process p = Process.GetProcessById(pi);
                p.Kill();
            };
            aTimer.Start();
        }
    }

    /// <summary>
    /// Main class program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Get handle console current
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Show or Hide window console
        /// </summary>
        /// <param name="hWnd">Handle window console</param>
        /// <param name="nCmdShow">5 show; 0 hide</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Hide console
        /// </summary>
        const int SW_HIDE = 0;
        /// <summary>
        /// Show console
        /// </summary>
        const int SW_SHOW = 5;

        /// <summary>
        /// Hide console
        /// </summary>
        public static void ConsoleHide()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        /// <summary>
        /// Show console
        /// </summary>
        public static void ConsoleShow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }

        /// <summary>
        /// Main function program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /////////////////////////////////////////
            // Hide console
            ConsoleHide();

            /////////////////////////////////////////
            // Check net frame work 4.6 installed
            string strFile = check_ValidateOpen();
            
            /////////////////////////////////////////
            // Start process Chromium\x64\DOMGenTool.exe
            string WORKINGDIRECTORY = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Chromium\" + (IntPtr.Size == 8 ? "x64" : "x86"));
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strFile)
                {
                    WorkingDirectory = WORKINGDIRECTORY, 
                };
                Process.Start(startInfo);
            }
            catch 
            {

            }

            Application.Run(new RunApplicationContext());
        }

        /// <summary>
        /// Check net frame work 4.6 installed on this computer
        /// </summary>
        /// <returns></returns>
        private static string check_ValidateOpen()
        {
            /////////////////////////////////////////
            // check net 4.6 installed on this computer
            bool netok = check_NETVersionFromRegistry("4.6.");
            if (netok == false)
            {
                string msg =
@"Please install packages

1, Visual C++ Redistributable Packages for Visual Studio 2013:
https://www.microsoft.com/en-us/download/details.aspx?id=40784 

2, Microsoft .NET Framework 4.6:
https://www.microsoft.com/en-us/download/details.aspx?id=48137";

                System.Windows.Forms.MessageBox.Show(msg, "System Requirements");
                Exit();
            }

            /////////////////////////////////////////
            // check exist file DOMGenTool.exe
            string WORKINGDIRECTORY = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Chromium\" + (IntPtr.Size == 8 ? "x64" : "x86"));
            string strFile = Path.Combine(WORKINGDIRECTORY, @"DOMGenTool.exe");
            if (Directory.Exists(WORKINGDIRECTORY) == false || File.Exists(strFile) == false)
            {
                System.Windows.Forms.MessageBox.Show(
@"Can not find path Chromuim: 

" + strFile, "Message");
                Exit();
            }

            return strFile;
        }

        /// <summary>
        /// Exit program
        /// </summary>
        private static void Exit()
        {
            int pi = Process.GetCurrentProcess().Id;
            Process p = Process.GetProcessById(pi);
            p.Kill();
        }

        /// <summary>
        /// Check net framework installed
        /// </summary>
        /// <param name="net">Version net frame work as 4.6, 4.5 ...</param>
        /// <returns></returns>
        private static bool check_NETVersionFromRegistry(string net)
        {
            StringBuilder bi = new StringBuilder();

            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            bi.Append(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                bi.Append(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                bi.Append(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    bi.Append("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    bi.Append("  " + subKeyName + "  " + name);
                                }
                            }
                        }
                    }
                }
            }// end using
            string s = bi.ToString();
            return s.Contains(net);
        }

    }
}
