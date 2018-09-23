using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace appui
{
    static class App
    {
        static App() {
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
        
        [STAThread]
        static int Main()
        {
            //string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string execDir = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath);
            execDir = Path.Combine(execDir, "bin") + @"\";

            CefRuntime.Load(execDir);

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = true;
            settings.SingleProcess = true;
            settings.LogSeverity = CefLogSeverity.Disable;
            settings.ResourcesDirPath = execDir;
            settings.RemoteDebuggingPort = 55555;
            settings.NoSandbox = true;
            //settings.BrowserSubprocessPath = Path.Combine(execDir, "cefclient.exe"),

            // Add the --enable-media-stream flag
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1");

            CefMainArgs mainArgs = new CefMainArgs(new string[] { });
            CefApp app = new CefApplication();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
            if (exitCode != -1)
                return exitCode;

            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            // register custom scheme handler
            // CefRuntime.RegisterSchemeHandlerFactory("http", DumpRequestDomain, new DemoAppSchemeHandlerFactory());
            // CefRuntime.RegisterSchemeHandlerFactory("http", DumpRequestDomain, new TestDumpRequestHandlerFactory());
            // CefRuntime.AddCrossOriginWhitelistEntry("http://localhost", "http", "", true);
            // CefRuntime.AddCrossOriginWhitelistEntry("http://localhost", "http", "_app.cross.domain", false);
            //CefRuntime.RegisterSchemeHandlerFactory("https", "", new TestDumpRequestHandlerFactory());
            //CefRuntime.RegisterSchemeHandlerFactory("https", "", new MySchemeHandlerFactory());


            if (!settings.MultiThreadedMessageLoop)
                Application.Idle += (sender, e) => CefRuntime.DoMessageLoopWork();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fBrowser());

            CefRuntime.Shutdown();
            return 0;
        }
    }
}
