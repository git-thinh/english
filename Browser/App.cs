using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;
using System.Diagnostics;

namespace Browser
{
    class App
    {
        //static App()
        //{
        //    AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
        //    {
        //        Assembly asm = null;
        //        string comName = ev.Name.Split(',')[0];

        //        string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //        execDir = Path.Combine(execDir, "bin");
        //        string file = Path.Combine(execDir, comName + ".dll");
        //        Debug.WriteLine("<----" + file);

        //        //string resourceName = @"DLL\" + comName + ".dll";
        //        //var assembly = Assembly.GetExecutingAssembly();
        //        //resourceName = typeof(Program).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
        //        //using (Stream stream = assembly.GetManifestResourceStream(resourceName))

        //        using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
        //        {
        //            if (stream == null)
        //            {
        //            }
        //            else
        //            {
        //                byte[] buffer = new byte[stream.Length];
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    int read;
        //                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        //                        ms.Write(buffer, 0, read);
        //                    buffer = ms.ToArray();
        //                }
        //                asm = Assembly.Load(buffer);
        //            }
        //        }
        //        return asm;
        //    };
        //}

        [STAThread]
        static void Main(string[] args)
        {
            string pathRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathCache = Path.Combine(pathRoot, "Cache");
            if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);

            //string pathBin = Path.Combine(pathRoot, "bin") + @"\";
            //CefRuntime.Load(pathBin);

            CefSettings settings = new CefSettings() {
                CachePath = pathCache,
                //LocalesDirPath = pathBin,
                //BrowserSubprocessPath = pathBin,
                //ResourcesDirPath = pathBin
            };

            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            //We're going to manually call Cef.Shutdown below, this maybe required in some complex scenarios
            CefSharpSettings.ShutdownOnExit = false;

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            var browser = new fBrowser();
            Application.Run(browser);

            //Shutdown before your application exists or it will hang.
            Cef.Shutdown();
        }
    }
}
