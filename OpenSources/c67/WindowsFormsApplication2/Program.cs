using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Reflection;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
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
            Application.Run(new SimpleBrowserForm());

            Cef.Shutdown();
        }
    }
}
