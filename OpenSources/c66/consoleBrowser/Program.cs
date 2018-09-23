using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xilium.CefGlue;

namespace consoleBrowser
{
    static class Program
    {
        static Program()
        {
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
        private static int Main(string[] args)
        {
            string execDir2 = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            execDir2 = Path.Combine(execDir2, "bin") + @"\";

            CefRuntime.Load(execDir2);

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = true;
            settings.SingleProcess = true;
            settings.LogSeverity = CefLogSeverity.Verbose;
            settings.LogFile = "cef.log";
            settings.ResourcesDirPath = execDir2;
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;
            
            // Start the secondary CEF process.
            var cefMainArgs = new CefMainArgs(new string[0]);
            var cefApp = new DemoCefApp();

            // This is where the code path divereges for child processes.
            if (CefRuntime.ExecuteProcess(cefMainArgs, cefApp) != -1)
            {
                Console.Error.WriteLine("Could not the secondary process.");
            }
            
            CefRuntime.Initialize(cefMainArgs, settings, cefApp, IntPtr.Zero);

            // Instruct CEF to not render to a window at all.
            CefWindowInfo cefWindowInfo = CefWindowInfo.Create();
            cefWindowInfo.SetAsWindowless(IntPtr.Zero, true);

            // Settings for the browser window itself (e.g. enable JavaScript?).
            var cefBrowserSettings = new CefBrowserSettings();

            // Initialize some the cust interactions with the browser process.
            // The browser window will be 1280 x 720 (pixels).
            var cefClient = new DemoCefClient(1280, 720);

            // Start up the browser instance.
            //CefBrowserHost.CreateBrowser(cefWindowInfo, cefClient, cefBrowserSettings, "http://www.reddit.com/");
            CefBrowserHost.CreateBrowser(cefWindowInfo, cefClient, cefBrowserSettings, "https://google.com.vn/");

            // Hang, to let the browser to do its work.
            Console.WriteLine("Press a key at any time to end the program.");
            Console.ReadKey();

            // Clean up CEF.
            CefRuntime.Shutdown();

            return 0;
        }
    }

}
