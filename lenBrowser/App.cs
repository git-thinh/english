using CefSharp;
using Microsoft.Win32;
using Rpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
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
        static int m_msgHandleID;
        static fBrowser m_main;

        #region [ API ]

        /* https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output */
        static Process m_api_process;
        static RpcClientApi m_api;

        public static void f_api_Register(int handleID)
        {
            m_api.f_sendApi(MSG_TYPE.NOTIFICATION_REG_HANDLE, handleID);
        }
        public static void f_api_UnRegister(int handleID)
        {
            m_api.f_sendApi(MSG_TYPE.NOTIFICATION_REMOVE_HANDLE, handleID);
        }
        
        public static byte[] f_api_sendMessage(MSG_TYPE type, string data = "")
        {
            return m_api.f_sendApi(type, data);
        }

        static void f_api_Exit()
        {
            m_api.Dispose();
            m_api_process.Close();
        }

        static void f_api_Init()
        {
            m_api_process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "https.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            m_api = new RpcClientApi(new Guid(_CONST.RPC_IID), RpcProtseq.ncalrpc, null, _CONST.RPC_NAME);
            m_api.AuthenticateAs(RpcClientApi.Self);

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

        #endregion

        #region [ UI ]

        public static void f_ui_browserGoUrl(string url, string title = "")
        {
            if (m_main != null)
                m_main.f_browserGoPage(url, title);
        }

        public static void f_ui_browserReload()
        {
            if (m_main != null)
                m_main.f_browserReload();
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
            Console.Title = "BROWSER";

            f_api_Init();

            //string pathCache = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), "Cache");
            //if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);
            Settings settings = new Settings() { UserAgent = "Chrome7" };
            BrowserSettings browserSettings = new BrowserSettings() { PageCacheDisabled = true };

            if (!CEF.Initialize(settings, browserSettings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            CEF.RegisterScheme("local", new LocalSchemeHandlerFactory());
            CEF.RegisterScheme("http", new HttpSchemeHandlerFactory());
            CEF.RegisterScheme("https", new HttpSchemeHandlerFactory());

            CEF.RegisterJsObject("API", new ApiJavascript());

            m_main = new fBrowser();
            m_main.Shown += (se, ev) =>
            {
                m_msgHandleID = m_main.f_main_msgHandleID();
                f_api_Register(m_msgHandleID);
            };

            Application.ApplicationExit += (se, ev) => {
                f_api_UnRegister(m_msgHandleID);
                f_app_Exit();
            };
            Application.EnableVisualStyles();
            Application.Run(m_main);
        }

        static void f_app_Exit()
        {
            f_api_Exit();
            //Console.WriteLine("Enter to EXIT...");
            //Console.ReadLine();

            CEF.Shutdown();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        static void f_app_Run()
        {
            f_app_Init();
            f_api_Init();
            f_app_Exit();
        }

        public static oAppInfo f_app_getInfo() {
            if (m_main != null) return m_main.f_main_getAppInfo();
            return null;
        }

        #endregion
    }
}
