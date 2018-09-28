using CefSharp;
using IpcChannel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace lenBrowser
{
    public class App
    {
        [STAThread]
        static void Main(string[] args) => f_app_Run();

        static fBrowser main;

        #region [ HTTPS ]

        /* https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output */
        static Process HTTPS = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "https.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            }
        };

        const string EVENT_KEY_PATH = @"English\Browser";
        static readonly string EVENT_CHANNEL = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}").ToString();
        static IIpcChannelRegistrar EVENT_REGISTRAR = new IpcChannelRegistrar(Registry.CurrentUser, EVENT_KEY_PATH);
        static IpcEventChannel HTTPS_EVENT = new IpcEventChannel(EVENT_REGISTRAR, EVENT_CHANNEL);
        static string EVENT_KEY = "UI";
        static string EVENT_KEY_HTTPS = "EVENT_HTTPS";
        static string EVENT_NAME = "MSG";

        public static void f_http_getSource(string url)
        {
            HTTPS_EVENT.ExecutionTimeout = 1000;
            //HTTPS_EVENT.SendTo("CH1", "Message", "p1", "p2", "p3");
            //HTTPS_EVENT.SendTo(new string[] { "CH2" }, "Message", "p1", "p2", "p3");
            //HTTPS_EVENT.SendTo(1000, new string[] { "ch1", "ch2" }, "Message", "p1", "p2", "p3");

            HTTPS_EVENT.SendTo(EVENT_KEY_HTTPS, EVENT_NAME, url, "1"); // [1] = "1" is write file cache

            //HTTPS_EVENT.Broadcast(100, EVENT_NAME, url);
        }

        static void f_http_Exit()
        {
            HTTPS_EVENT.StopListening();
            HTTPS_EVENT.StopAsyncSending(true, -1);
            HTTPS.Close();
        }

        static void f_http_Init()
        {
            HTTPS_EVENT.EnableAsyncSend();
            HTTPS_EVENT.StartListening(EVENT_KEY);
            HTTPS_EVENT[EVENT_NAME].OnEvent += f_http_messageReceiver;

            ////////////////HTTPS.StartInfo.Arguments = url;
            //////////////HTTPS.Start();
            ////////////////StringBuilder buf = new StringBuilder();
            ////////////////while (!HTTPS.StandardOutput.EndOfStream)
            ////////////////{
            ////////////////    string line = HTTPS.StandardOutput.ReadLine();
            ////////////////    buf.AppendLine(line);
            ////////////////}                                
            ////////////////string data = HttpUtility.HtmlDecode(buf.ToString());
            ////////////////string data = HTTPS.StandardOutput.ReadToEnd();
            //////////////HTTPS.WaitForExit();

            //////////* Set your output and error (asynchronous) handlers
            ////////HTTPS.OutputDataReceived += new DataReceivedEventHandler(f_http_outputHandler);
            //////////HTTPS.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            //////////* Start process and handlers
            ////////HTTPS.Start();
            ////////HTTPS.BeginOutputReadLine();
            //////////HTTPS.BeginErrorReadLine();
            ////////HTTPS.WaitForExit();
            ////////Thread.Sleep(1000);
        }

        private static void f_http_messageReceiver(object sender, IpcSignalEventArgs e)
        {
            if (e.EventChannel.ChannelName != EVENT_KEY)
            {
                string[] arr = e.Arguments;
                Console.WriteLine(string.Format("MSG <= {0}: {1}", 1, String.Join(",", arr)));
                if (arr.Length > 3 && arr[0] == "OK")
                    f_ui_browserGoUrl("cache://" + arr[3]);
            }
        }

        #endregion

        #region [ UI ]

        static void f_ui_browserGoUrl(string url)
        {
            if (main != null)
                main.f_browserGoPage(url);
        }

        #endregion

        #region [ APP ]

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

        static void f_app_Init()
        {
            f_http_Init();

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
            CEF.RegisterScheme("setting", new SettingSchemeHandlerFactory());
            CEF.RegisterScheme("cache", new CacheSchemeHandlerFactory());
            CEF.RegisterScheme("http", new HttpSchemeHandlerFactory());
            CEF.RegisterScheme("https", new HttpSchemeHandlerFactory());

            main = new fBrowser(cache);

            //CEF.RegisterScheme(CacheMemory.SCHEME, CacheMemory.HOST, new CacheSchemeHandlerFactory(cache));
            //CEF.RegisterScheme("http", "test.local", new TestSchemeHandlerFactory());
            //CEF.RegisterJsObject("___api", new apiJavascript());


            Application.EnableVisualStyles();
            Application.Run(main);
        }

        static void f_app_Exit()
        {
            f_http_Exit();
            //Console.WriteLine("Enter to EXIT...");
            //Console.ReadLine();

            CEF.Shutdown();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static void f_app_Run()
        {
            f_app_Init();
            f_http_Init();
            f_app_Exit();
        }

        #endregion
    }
}
