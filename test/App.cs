using CefSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace test
{
    public interface IApp
    {
        void f_link_AddUrls(string[] urls);
        string f_link_getHtmlCache(string url);
        string f_link_getHtmlOnline(string url);
        void f_link_updateUrls(oLink[] links);

        bool f_main_openUrl(string url, string title);
    }

    class App : IApp
    {
        private IFormMain _fomMain = null;

        #region [ LINK - HTML]

        readonly List<string> DOMAIN_LIST;

        readonly ConcurrentDictionary<string, string> CACHE;
        readonly ConcurrentDictionary<string, string> LINK;
        readonly ConcurrentDictionary<int, int> LINK_LEVEL;
        readonly ConcurrentDictionary<string, int> LINK_ID;
        readonly ConcurrentDictionary<int, int> TIME_VIEW_LINK;
        readonly ConcurrentDictionary<string, List<int>> INDEX;
        readonly ConcurrentDictionary<string, List<int>> DOMAIN_LINK;
        readonly ConcurrentDictionary<string, List<int>> KEY_INDEX;
        readonly ConcurrentDictionary<string, string> TRANSLATE;

        public App()
        {
            if (Directory.Exists("cache")) DOMAIN_LIST = Directory.GetDirectories("cache").Select(x => x.Substring(6)).ToList();
            else DOMAIN_LIST = new List<string>();

            TRANSLATE = new ConcurrentDictionary<string, string>();
            CACHE = new ConcurrentDictionary<string, string>();
            LINK = new ConcurrentDictionary<string, string>();
            LINK_LEVEL = new ConcurrentDictionary<int, int>();
            LINK_ID = new ConcurrentDictionary<string, int>();
            INDEX = new ConcurrentDictionary<string, List<int>>();
            DOMAIN_LINK = new ConcurrentDictionary<string, List<int>>();
            TIME_VIEW_LINK = new ConcurrentDictionary<int, int>();
            KEY_INDEX = new ConcurrentDictionary<string, List<int>>();
        }

        public void f_link_updateUrls(oLink[] links)
        {
            if (links.Length > 0)
            {
                var ls = links.GroupBy(x => x.Url).Select(x => x.First()).ToArray();
                string domain = Html.f_html_getDomainMainByUrl(ls[0].Url);
                string url, title;
                for (int i = 0; i < ls.Length; i++)
                {
                    url = ls[i].Url;
                    title = ls[i].Text;
                    f_cacheUrl(url, title, domain, i);
                }
            }
        }

        public bool f_main_openUrl(string url, string title)
        {
            if (_fomMain != null)
            {
                _fomMain.f_browser_Go(url);
                return true;
            }
            return false;
        }

        public void f_link_AddUrls(string[] urls)
        {
        }

        public string f_link_getHtmlCache(string url)
        {
            if (CACHE.ContainsKey(url))
            {
                Console.WriteLine("### " + url);
                return CACHE[url];
            }
            return null;
        }

        public string f_link_getHtmlOnline(string url)
        {
            /* https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output */
            Process process = new Process();
            process.StartInfo.FileName = "curl.exe";
            //process.StartInfo.Arguments = url;
            //process.StartInfo.Arguments = "--insecure " + url;
            //process.StartInfo.Arguments = "--max-time 5 -v " + url; /* -v url: handle error 302 found redirect localtion*/
            process.StartInfo.Arguments = "-m 5 -v " + url; /* -v url: handle error 302 found redirect localtion*/
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            //* Read the output (or the error)
            string html = process.StandardOutput.ReadToEnd();
            if (string.IsNullOrEmpty(html))
            {                
                string err = process.StandardError.ReadToEnd(), urlDirect = string.Empty;
                
                int pos = err.IndexOf("< Location: ");
                if (pos != -1)
                {
                    urlDirect = err.Substring(pos + 12, err.Length - (pos + 12)).Split(new char[] { '\r', '\n' })[0].Trim();
                    if (urlDirect[0] == '/')
                    {
                        Uri uri = new Uri(url);
                        urlDirect = uri.Scheme + "://" + uri.Host + urlDirect;
                    }

                    Console.WriteLine("-> Redirect: " + urlDirect);


                    html = f_link_getHtmlCache(urlDirect);
                    if (string.IsNullOrEmpty(html))
                        return f_link_getHtmlOnline(urlDirect);
                    else
                        return html;
                }
                else {
                    Console.WriteLine("??????????????????????????????????????????? ERROR: " + url);
                }

                Console.WriteLine("-> Fail: " + url);

                return null;
            }

            Console.WriteLine("-> Ok: " + url);

            string title = Html.f_html_getTitle(html);
            html = _htmlFormat(url, html);
            f_cacheUrl(url);
            CACHE.TryAdd(url, html);

            //string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (_fomMain != null) _fomMain.f_browser_updateInfoPage(url, title);

            return html;

            //////* Create your Process
            ////Process process = new Process();
            ////process.StartInfo.FileName = "curl.exe";
            ////process.StartInfo.Arguments = url;
            ////process.StartInfo.UseShellExecute = false;
            ////process.StartInfo.RedirectStandardOutput = true;
            ////process.StartInfo.RedirectStandardError = true;
            ////process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
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

        string _htmlFormat(string url, string html)
        {
            string s = HttpUtility.HtmlDecode(html), title = "";

            // Fetch all url same domain in this page ...
            //string[] urls = Html.f_html_actractUrl(url, s);
            s = Html.f_html_Format(url, s);

            int posH1 = s.ToLower().IndexOf("<h1");
            if (posH1 != -1) s = s.Substring(posH1, s.Length - posH1);

            s = "<!--" + url + @"-->" + Environment.NewLine + @"<input id=""___title"" value=""" + title + @""" type=""hidden"">" + s;

            return s;
        }

        string f_text_convert_UTF8_ACSII(string utf8)
        {
            string stFormD = utf8.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        void f_cacheUrl(string url, string title = "", string domain = "", int indexForEach = 0)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(title)) return;

            if (domain == "") domain = Html.f_html_getDomainMainByUrl(url);
            int id = LINK.Count + 1, time_view = int.Parse(DateTime.Now.ToString("1ddHHmmss")) + indexForEach;

            if (!LINK.ContainsKey(url))
            {
                LINK.TryAdd(url, title);
                LINK_ID.TryAdd(url, id);
                TIME_VIEW_LINK.TryAdd(id, time_view);
                LINK_LEVEL.TryAdd(id, url.Split('/').Length - 3);
            }

            lock (DOMAIN_LIST) if (!DOMAIN_LIST.Contains(domain)) DOMAIN_LIST.Add(domain);
            if (DOMAIN_LINK.ContainsKey(domain)) DOMAIN_LINK[domain].Add(id); else DOMAIN_LINK.TryAdd(domain, new List<int>() { id });

            if (!string.IsNullOrEmpty(title))
            {
                string s = f_text_convert_UTF8_ACSII(title).ToLower().ToLower();
                string[] words = s.Split(' ').Where(x => x.Length > 2).ToArray();
                for (int i = 0; i < words.Length; i++)
                {
                    if (KEY_INDEX.ContainsKey(words[i]))
                    {
                        if (KEY_INDEX[words[i]].IndexOf(id) == -1)
                            KEY_INDEX[words[i]].Add(id);
                    }
                    else
                        KEY_INDEX.TryAdd(words[i], new List<int>() { id });
                }
                //Console.WriteLine(string.Format(" INDEX: {0}", KEY_INDEX.Count));
            }
        }

        #endregion

        #region [ APP ]

        [STAThread]
        static void Main(string[] args) => new App().f_app_Run();

        public void f_app_Run()
        {
            Settings settings = new Settings() { };
            if (!CEF.Initialize(settings)) return;
            //CEF.RegisterScheme("local", new LocalSchemeHandlerFactory(this));
            CEF.RegisterJsObject("API", new API(this));
            Application.ApplicationExit += (se, ev) => f_app_Exit();
            var main = new fMain(this);
            _fomMain = main;
            Application.Run(main);
            f_app_Exit();
        }

        void f_app_Exit()
        {
            CACHE.Clear();
            LINK.Clear();
            LINK_LEVEL.Clear();
            LINK_ID.Clear();
            TIME_VIEW_LINK.Clear();
            INDEX.Clear();
            DOMAIN_LINK.Clear();
            KEY_INDEX.Clear();
            TRANSLATE.Clear();

            CEF.Shutdown();
        }

        #endregion
    }
}
