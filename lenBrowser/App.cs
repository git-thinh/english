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
        static fBrowser main;
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
        static readonly string _channel = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}").ToString();
        static IIpcChannelRegistrar _registrar = new IpcChannelRegistrar(Registry.CurrentUser, EVENT_KEY_PATH);
        static IpcEventChannel HTTPS_EVENT = new IpcEventChannel(_registrar, _channel);
        static string EVENT_KEY = "UI";
        static string EVENT_NAME = "MSG";

        public static void f_http_getSource(string url)
        {
            HTTPS_EVENT.ExecutionTimeout = 1000;
            //HTTPS_EVENT.SendTo("CH1", "Message", "p1", "p2", "p3");
            //HTTPS_EVENT.SendTo(new string[] { "CH2" }, "Message", "p1", "p2", "p3");
            // HTTPS_EVENT.SendTo(1000, new string[] { "ch1", "ch2" }, "Message", "p1", "p2", "p3");
            HTTPS_EVENT.Broadcast(100, EVENT_NAME, url);
        }

        static void f_http_exit()
        {
            HTTPS_EVENT.StopListening();
            HTTPS_EVENT.StopAsyncSending(true, -1);
            HTTPS.Close();
        }

        static void f_http_init()
        {
            HTTPS_EVENT.EnableAsyncSend();
            HTTPS_EVENT.StartListening(EVENT_KEY);
            HTTPS_EVENT[EVENT_NAME].OnEvent += delegate (object o, IpcSignalEventArgs e)
            {
                if (e.EventChannel.ChannelName != EVENT_KEY)
                {
                    Console.WriteLine(string.Format("LISTENING_{0}: {1}", 1, String.Join(",", e.Arguments)));
                }
            };


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

        static void f_http_outputHandler(object sender, DataReceivedEventArgs e)
        {
            string data = e.Data;
            Console.Write(data);
        }

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

            f_http_init();

            //////try
            //////{
            //////    // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
            //////    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //////    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            //////        | (SecurityProtocolType)3072
            //////        | (SecurityProtocolType)0x00000C00
            //////        | SecurityProtocolType.Tls;
            //////}
            //////catch
            //////{
            //////    // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
            //////    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //////    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            //////        | SecurityProtocolType.Tls;
            //////}

            //string pathCache = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), "Cache");
            //if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);
            Settings settings = new Settings() { UserAgent = "Chrome7" };
            BrowserSettings browserSettings = new BrowserSettings() { PageCacheDisabled = true };

            if (!CEF.Initialize(settings, browserSettings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            //System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{
            //    return true; // **** Always accept
            //};
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => certificate.Issuer == "CN=localhost";

            CacheMemory cache = new CacheMemory();

            //CEF.RegisterScheme(CacheMemory.SCHEME, CacheMemory.HOST, new CacheSchemeHandlerFactory(cache));
            //CEF.RegisterScheme("http", "setting.local", new SettingSchemeHandlerFactory());
            //CEF.RegisterScheme("http", "test.local", new TestSchemeHandlerFactory());
            //CEF.RegisterScheme("data", new SettingSchemeHandlerFactory());
            //CEF.RegisterJsObject("___api", new apiJavascript());

            CEF.RegisterScheme("http", new HttpSchemeHandlerFactory());
            CEF.RegisterScheme("https", new HttpSchemeHandlerFactory());

            Application.EnableVisualStyles();
            main = new fBrowser(cache);
            Application.Run(main);
        }

        static void f_exit()
        {
            f_http_exit();
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
            f_http_init();
            f_exit();
        }
    }
}
