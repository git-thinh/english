using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace lenBrowser
{
    class App
    {
        static fBrowser main;

        static App()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            //{
            //    Assembly asm = null;
            //    string comName = ev.Name.Split(',')[0];

            //    string resourceName = @"DLL\" + comName + ".dll";
            //    var assembly = Assembly.GetExecutingAssembly();
            //    resourceName = typeof(App).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
            //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //    //using (Stream stream = File.OpenRead("bin/" + comName + ".dll"))
            //    {
            //        if (stream == null)
            //        {
            //            //Debug.WriteLine(resourceName);
            //        }
            //        else
            //        {
            //            byte[] buffer = new byte[stream.Length];
            //            using (MemoryStream ms = new MemoryStream())
            //            {
            //                int read;
            //                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //                    ms.Write(buffer, 0, read);
            //                buffer = ms.ToArray();
            //            }
            //            asm = Assembly.Load(buffer);
            //        }
            //    }
            //    return asm;
            //};
        }

        static void f_init()
        {
            ThreadPool.SetMaxThreads(25, 25);

            ServicePointManager.DefaultConnectionLimit = 1000;
            try
            {
                // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                    | (SecurityProtocolType)3072
                    | (SecurityProtocolType)0x00000C00
                    | SecurityProtocolType.Tls;
            }
            catch
            {
                // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                    | SecurityProtocolType.Tls;
            }

            //string pathCache = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), "Cache");
            //if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);
            Settings settings = new Settings() { UserAgent = "Chrome7" };
            BrowserSettings browserSettings = new BrowserSettings() { PageCacheDisabled = true };

            if (!CEF.Initialize(settings, browserSettings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            CacheMemory cache = new CacheMemory();

            CEF.RegisterScheme("http", "cache.local", new CacheSchemeHandlerFactory(cache));
            CEF.RegisterScheme("http", "setting.local", new SettingSchemeHandlerFactory());
            //CEF.RegisterScheme("data", new SettingSchemeHandlerFactory());
            CEF.RegisterJsObject("___api", new apiJavascript());

            Application.EnableVisualStyles();
            main = new fBrowser(cache);
            Application.Run(main);
        }

        static void f_exit()
        {
            //Console.WriteLine("Enter to EXIT...");
            //Console.ReadLine();

            CEF.Shutdown();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            f_init();
            f_exit();
        }
    }
}
