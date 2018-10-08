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

namespace Browser
{
    class App
    {
        static App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];

                ////string resourceName = @"DLL\" + comName + ".dll";
                ////var assembly = Assembly.GetExecutingAssembly();
                ////resourceName = typeof(App).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                ////using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (Stream stream = File.OpenRead("bin/" + comName + ".dll"))
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


        static void Main(string[] args)
        {
            string pathCache = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Cache");
            if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);

            CefSettings settings = new CefSettings() { CachePath = pathCache };
            BrowserSettings browserSettings = new BrowserSettings() { };

            if (!Cef.Initialize(settings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            //CEF.RegisterScheme("test", new TestSchemeHandlerFactory());
            //CEF.RegisterJsObject("bound", new BoundObject());

            //Application.Run(new TabulationDemoForm());
            Application.Run(new fBrowser());

            Cef.Shutdown();
        }
    }
}
