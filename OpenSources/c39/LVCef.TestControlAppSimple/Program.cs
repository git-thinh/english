﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Xilium.CefGlue;
using System.IO;
using System.Reflection;

namespace TestControlAppSimple
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string execDir = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath);
            execDir = Path.Combine(execDir, "bin") + @"\";

            try
            {
                CefRuntime.Load(execDir);
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            catch (CefRuntimeException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 3;
            }

            //hmm not working
            //Should maybe move to callback in render process vs can modify from browser process this way
            //Array.Resize(ref args, args.Length + 1);
            //args[args.Length - 1] = @"--log-file ""..\logs\LVCef.RenderApp.log""";
            //for (int i = 0; i < args.Length; i++)
            //    Debug.WriteLine("Render args[" + i +"]: " + args[i]);

            var mainArgs = new CefMainArgs(args);
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, null, IntPtr.Zero);
            if (exitCode != -1)
                return exitCode;

            //var settings = new CefSettings
            //{
            //    //Relative path from \cef, make sure to set Working Directory to \cef, see README.md
            //    //BrowserSubprocessPath = @"..\dotnet\LVCef.RenderApp\bin\Debug\LVCef.RenderApp.exe",
            //    BrowserSubprocessPath = @"LVCef.RenderApp.exe",
            //    SingleProcess = false,
            //    MultiThreadedMessageLoop = true,
            //    LogSeverity = CefLogSeverity.Default,
            //    LogFile = @"..\logs\TestControlAppSimple.log",
            //};

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = true;
            settings.SingleProcess = true;
            settings.LogSeverity = CefLogSeverity.Disable;
            settings.ResourcesDirPath = execDir;
            settings.RemoteDebuggingPort = 55555;
            settings.NoSandbox = true;
            //settings.BrowserSubprocessPath = Path.Combine(execDir, "cefclient.exe"),


            CefRuntime.Initialize(mainArgs, settings, null, IntPtr.Zero);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!settings.MultiThreadedMessageLoop)
            {
                Application.Idle += (sender, e) => { CefRuntime.DoMessageLoopWork(); };
            }

            Application.Run(new TestControlAppSimpleForm());

            CefRuntime.Shutdown();
            return 0;
        }
    }
}
