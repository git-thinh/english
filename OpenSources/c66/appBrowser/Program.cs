using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace appBrowser
{
    static class Program
    {
        static Program() {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];

                string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                execDir = Path.Combine(execDir, "bin");
                string file = Path.Combine(execDir, comName + ".dll");

                //string resourceName = @"DLL\" + comName + ".dll";
                //var assembly = Assembly.GetExecutingAssembly();
                //resourceName = typeof(Program).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                //using (Stream stream = assembly.GetManifestResourceStream(resourceName))

                using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (stream == null)
                    {
                        //Debug.WriteLine(resourceName);
                    }
                    else
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        {
            //////string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //////CefRuntime.Load();
            //////CefSettings settings = new CefSettings
            //////{
            //////    SingleProcess = System.Diagnostics.Debugger.IsAttached,
            //////    MultiThreadedMessageLoop = true,
            //////    LogSeverity = CefLogSeverity.Verbose,
            //////    LogFile = "cef.log",
            //////    //AutoDetectProxySettingsEnabled = true,        
            //////    ResourcesDirPath = execDir,
            //////    //BrowserSubprocessPath = Path.Combine(execDir, "cefclient.exe"),
            //////};


            string execDir1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string execDir2 = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);

            execDir2 = Path.Combine(execDir2, "bin") + @"\";

            CefRuntime.Load(execDir2);

            var settings = new CefSettings();
            //settings.MultiThreadedMessageLoop = MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;            
            settings.MultiThreadedMessageLoop = true;
            settings.SingleProcess = true;
            settings.LogSeverity = CefLogSeverity.Verbose;
            settings.LogFile = "cef.log";
            //settings.ResourcesDirPath = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            settings.ResourcesDirPath = execDir2;
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;

            CefMainArgs mainArgs = new CefMainArgs(new string[] { });
            CefApp app = new TestApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app);
            Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
            if (exitCode != -1)
                return exitCode;

            //////// guard if something wrong
            //////foreach (var arg in args) { if (arg.StartsWith("--type=")) { return -2; } }

            //////CefRuntime.Initialize(mainArgs, settings, app);
            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);


            if (!settings.MultiThreadedMessageLoop)
            {
                Application.Idle += (sender, e) => CefRuntime.DoMessageLoopWork();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fBrowser());

            CefRuntime.Shutdown();

            return 0;
        }
    }
}
