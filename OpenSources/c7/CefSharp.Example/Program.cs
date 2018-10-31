using System;
using System.Windows.Forms;
using CefSharp;
using System.IO;

namespace CefSharp.Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string pathCache = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Cache");
            if (!Directory.Exists(pathCache)) Directory.CreateDirectory(pathCache);

            Settings settings = new Settings() { UserAgent = "CHROME_7", CachePath = pathCache };
            BrowserSettings browserSettings = new BrowserSettings() { PageCacheDisabled = true };

            if(!CEF.Initialize(settings, browserSettings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            //CEF.RegisterScheme("local", new LocalSchemeHandlerFactory());
            //CEF.RegisterScheme("test", new TestSchemeHandlerFactory());
            //CEF.RegisterJsObject("bound", new BoundObject());

            Browser browser = new Browser();
            Application.Run(browser);

            CEF.Shutdown();
        }

    }
}
