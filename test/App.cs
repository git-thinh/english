using CefSharp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace test
{
    public interface IApp
    {
        void f_link_AddUrls(string[] urls);
        string f_link_getHtml(string url);
        string f_link_fetchHtmlOnline(string url);
    }

    class App : IApp
    {
        #region [ LINK - HTML]

        static ConcurrentDictionary<string, long> _link = new ConcurrentDictionary<string, long>();
        static ConcurrentDictionary<string, string> _html = new ConcurrentDictionary<string, string>();

        public void f_link_AddUrls(string[] urls)
        {
        }

        public string f_link_getHtml(string url)
        {
            if (_html.ContainsKey(url))
            {
                Console.WriteLine("##>" + url);
                return _html[url];
            }
            return null;
        }

        public string f_link_fetchHtmlOnline(string url)
        {
            Console.WriteLine("==>" + url);

            /* https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output */
            Process process = new Process();
            process.StartInfo.FileName = "curl.exe";
            process.StartInfo.Arguments = url;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string html = process.StandardOutput.ReadToEnd();

            _link.TryAdd(url, _link.Count + 1);
            _html.TryAdd(url, html);

            //Console.WriteLine(html);
            //string err = process.StandardError.ReadToEnd();
            //Console.WriteLine(err);
            process.WaitForExit();
            return html;

            //////* Create your Process
            ////Process process = new Process();
            ////process.StartInfo.FileName = "curl.exe";
            ////process.StartInfo.Arguments = url;
            ////process.StartInfo.UseShellExecute = false;
            ////process.StartInfo.RedirectStandardOutput = true;
            ////process.StartInfo.RedirectStandardError = true;
            //////* Set your output and error (asynchronous) handlers
            ////process.OutputDataReceived += (se, ev) => {
            ////    string html = ev.Data;

            ////    _link.TryAdd(url, _link.Count + 1);
            ////    _html.TryAdd(url, html);
            ////};
            //////process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            //////* Start process and handlers
            ////process.Start();
            ////process.BeginOutputReadLine();
            ////process.BeginErrorReadLine();
            ////process.WaitForExit(); 
        }


        #endregion

        #region [ APP ]

        [STAThread]
        static void Main(string[] args) => new App().f_app_Run();

        public void f_app_Run()
        {
            Settings settings = new Settings() { };
            if (!CEF.Initialize(settings)) return;
            CEF.RegisterScheme("local", new LocalSchemeHandlerFactory(this));
            Application.ApplicationExit += (se, ev) => f_app_Exit();
            Application.Run(new fMain(this));
            f_app_Exit();
        }

        void f_app_Exit()
        {
            _link.Clear();
            CEF.Shutdown();
        }

        #endregion
    }
}
