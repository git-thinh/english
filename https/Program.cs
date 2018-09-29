using IpcChannel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace https
{
    class Program
    {
        #region [ VAR ]

        static void Main(string[] args) => f_app_Run(args);
        const string EVENT_KEY_PATH = @"English\Browser";
        static readonly string EVENT_CHANNEL = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}").ToString();
        static IIpcChannelRegistrar EVENT_REGISTRAR = new IpcChannelRegistrar(Registry.CurrentUser, EVENT_KEY_PATH);
        static IpcEventChannel client = new IpcEventChannel(EVENT_REGISTRAR, EVENT_CHANNEL);
        static string EVENT_KEY = "EVENT_HTTPS";
        static string EVENT_NAME = "MSG";

        #endregion

        #region [ CONSOLE ]

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void f_console_Hide()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        static void f_console_Show()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }

        #endregion

        static void f_app_Test()
        {
            //client.SendTo(EVENT_KEY, EVENT_NAME, "https://dictionary.cambridge.org/grammar/british-grammar/", "1");
        }

        #region [ APP ]

        static void f_app_Run(string[] args)
        {
            Console.Title = "HTTPS";

            //f_console_Hide();
            f_app_checkToolRunning();

            ThreadPool.SetMaxThreads(25, 25);
            ServicePointManager.DefaultConnectionLimit = 1000;

            /* Certificate validation callback */
            ServicePointManager.ServerCertificateValidationCallback += (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) =>
            {
                /* If the certificate is a valid, signed certificate, return true. */
                if (error == System.Net.Security.SslPolicyErrors.None) return true;
                //Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'", cert.Subject, error.ToString());
                return false;
            };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

            ///////////////////////////////////////////////////////////////////////////////////////

            if (!Directory.Exists("cache")) Directory.CreateDirectory("cache");

            if (args.Length > 0) EVENT_KEY = args[0];
            client.StartListening(EVENT_KEY);
            client[EVENT_NAME].OnEvent += f_app_messageReceiver;
            f_app_Test();

            Console.ReadKey();

            client.StopListening();
        }

        static void f_app_messageReceiver(object sender, IpcSignalEventArgs e)
        {
            Console.WriteLine(string.Format("-> REQUEST: {0} => {1}", e.Arguments[0], e.Arguments[1] == "1" ? "FILE" : ""));

            if (e.Arguments.Length > 0)
            {
                string url = e.Arguments[0];
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    string file = f_html_getPathFileByUrl(url);
                    if (File.Exists(file))
                    {
                        Console.WriteLine("-> CACHE: " + url);
                        //string data = File.ReadAllText(file);
                        f_app_sendMessageToUI(true, url, file);
                    }
                    else
                    {
                        oLink link = new oLink() { Url = url };
                        if (e.Arguments.Length > 1 && e.Arguments[1] == "1") link.isWriteFileCache = true;
                        f_html_getSourceByUrl(link);
                    }
                }
            }
        }

        static void f_app_sendMessageToUI(bool isSuccess, params string[] arguments)
        {
            var ls = new List<string>() { isSuccess ? "OK" : "FAIL", EVENT_KEY };
            if (arguments != null && arguments.Length > 0) ls.AddRange(arguments);
            client.SendTo("UI", EVENT_NAME, ls.ToArray());
        }

        static void f_app_checkToolRunning()
        {
            try
            {
                /// Get process current
                Process processCurrent = Process.GetCurrentProcess();

                // Get all process running
                var wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process";
                using (var searcher = new ManagementObjectSearcher(wmiQueryString))
                using (var results = searcher.Get())
                {
                    string name = processCurrent.ProcessName.ToLower();
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    int len = (from p in Process.GetProcesses()
                               join mo in results.Cast<ManagementObject>()
                               on p.Id equals (int)(uint)mo["ProcessId"]
                               select new
                               {
                                   Name = p.ProcessName,
                                   Path = (string)mo["ExecutablePath"],
                               })
                                .Where(x =>
                                    !string.IsNullOrEmpty(x.Name)
                                    && x.Name.ToLower() == name
                                    && !string.IsNullOrEmpty(x.Path)
                                    && x.Path.StartsWith(path))
                                .Count();

                    // If exist other application running
                    if (len > 1)
                    {
                        // If click open second show message exit
                        if (len == 2) MessageBox.Show(string.Format("HTTPS running at ", path), "Message");

                        ///////////////////////////////////
                        /// exit process application current
                        int pi = processCurrent.Id;
                        Process p = Process.GetProcessById(pi);
                        p.Kill();
                    }
                }
            }
            catch { }
        }

        #endregion

        #region [ HTML ]

        static void f_html_getSourceByUrl(oLink link)
        {
            try
            {
                //using (WebClient webClient = new WebClient())
                //{
                //    var stream = webClient.OpenRead(new Uri(url));
                //    using (StreamReader sr = new StreamReader(stream))
                //    {
                //        string data = url + "{&}" + sr.ReadToEnd();
                //        Console.WriteLine(data);
                //    }
                //}

                HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(link.Url));
                w.BeginGetResponse(asyncResult =>
                {
                    Tuple<HttpWebRequest, oLink> para = (Tuple<HttpWebRequest, oLink>)asyncResult.AsyncState;
                    string _url = para.Item1.RequestUri.ToString();
                    try
                    {
                        HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult);
                        if (rs.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8))
                            {
                                string data = sr.ReadToEnd();
                                Console.WriteLine("-> OK: " + para.Item2.Url);
                                data = HttpUtility.HtmlDecode(data);

                                // Fetch all url same domain in this page ...
                                string[] urls = f_html_actractUrl(link.Url, data);
                                data = f_html_Format(data);

                                int posH1 = data.ToLower().IndexOf("<h1");
                                if (posH1 != -1) data = data.Substring(posH1, data.Length - posH1);

                                data = "<!--" + _url + "-->\r\n" + data;

                                string domain = f_html_getDomainByUrl(para.Item2.Url);
                                string file = f_html_getPathFileByUrl(para.Item2.Url);
                                f_app_sendMessageToUI(true, _url, file);

                                if (para.Item2.isWriteFileCache)
                                {
                                    if (!Directory.Exists("cache/" + domain)) Directory.CreateDirectory("cache/" + domain);
                                    if (!File.Exists(file))
                                    {
                                        using (FileStream writer = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                                        {
                                            byte[] buf = Encoding.UTF8.GetBytes(data);
                                            writer.Write(buf, 0, buf.Length);
                                            writer.Close();
                                        }
                                    }
                                }
                            }
                            rs.Close();
                        }
                    }
                    catch
                    {
                        f_app_sendMessageToUI(false, _url);
                    }
                }, new Tuple<HttpWebRequest, oLink>(w, link));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                f_app_sendMessageToUI(false, link.Url, msg);
            }
        }

        static string f_html_getDomainByUrl(string url)
        {
            string domain = url.Split('/')[2].Replace(':', '-').ToLower();
            if (domain.StartsWith("www.")) domain = domain.Substring(4);
            return domain;
        }

        static string f_html_getNameFileByUrl(string url)
        {
            string domain = f_html_getDomainByUrl(url);
            string fn = url.Substring(domain.Length + 8).Replace('/', '_');

            if (fn.EndsWith(".php") || fn.EndsWith(".jsp")) fn = fn.Substring(0, fn.Length - 4);
            else if(fn.EndsWith(".aspx") || fn.EndsWith(".html")) fn = fn.Substring(0, fn.Length - 5);

            if (fn[0] == '_') fn = fn.Substring(1);
            if (fn.Length > 0 && fn[fn.Length - 1] == '_') fn = fn.Substring(0, fn.Length - 1);

            fn = Regex.Replace(fn, @"[^A-Za-z0-9-_]+", string.Empty);
            return fn;
        }

        static string f_html_getPathFileByUrl(string url)
        {
            string domain = f_html_getDomainByUrl(url),
                   fn = f_html_getNameFileByUrl(url),
                   file = "cache/" + domain + "/" + fn + ".txt";
            return file;
        }

        static string f_html_Format(string s)
        {
            string si = string.Empty;
            s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
            s = Regex.Replace(s, @"<style[^>]*>[\s\S]*?</style>", string.Empty);
            s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            s = Regex.Replace(s, @"(?s)(?<=<!--).+?(?=-->)", string.Empty).Replace("<!---->", string.Empty);

            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            //s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|meta|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            s = Regex.Replace(s, @"</?(?i:base|nav|form|input|fieldset|button|link|symbol|path|canvas|use|ins|svg|embed|object|frameset|frame|meta|noscript)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Remove attribute style="padding:10px;..."
            s = Regex.Replace(s, @"<([^>]*)(\sstyle="".+?""(\s|))(.*?)>", string.Empty);
            s = s.Replace(@">"">", ">");

            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            s = string.Join(Environment.NewLine, lines);

            int pos = s.ToLower().IndexOf("<body");
            if (pos > 0)
            {
                s = s.Substring(pos + 5);
                pos = s.IndexOf('>') + 1;
                s = s.Substring(pos, s.Length - pos).Trim();
            }

            s = s
                .Replace(@" data-src=""", @" src=""")
                .Replace(@"src=""//", @"src=""http://");

            var mts = Regex.Matches(s, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            if (mts.Count > 0)
                foreach (Match mt in mts)
                    s = s.Replace(mt.ToString(), string.Format("{0}{1}{2}", "<p class=box_img___>", mt.ToString(), "</p>"));
            s = s.Replace("</body>", string.Empty).Replace("</html>", string.Empty).Trim();

            return s;

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(s);
            //string tagName = string.Empty, tagVal = string.Empty;
            //foreach (var node in doc.DocumentNode.SelectNodes("//*"))
            //{
            //    if (node.InnerText == null || node.InnerText.Trim().Length == 0)
            //    {
            //        node.Remove();
            //        continue;
            //    }

            //    tagName = node.Name.ToUpper();
            //    if (tagName == "A")
            //        tagVal = node.GetAttributeValue("href", string.Empty);
            //    else if (tagName == "IMG")
            //        tagVal = node.GetAttributeValue("src", string.Empty);

            //    //node.Attributes.RemoveAll();
            //    node.Attributes.RemoveAll_NoRemoveClassName();

            //    if (tagVal != string.Empty)
            //    {
            //        if (tagName == "A") node.SetAttributeValue("href", tagVal);
            //        else if (tagName == "IMG") node.SetAttributeValue("src", tagVal);
            //    }
            //}

            //si = doc.DocumentNode.OuterHtml;
            ////string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Where(x => x.Trim().Length > 0).ToArray();
            //string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            //si = string.Join(Environment.NewLine, lines);
            //return si;
        }

        static string[] f_html_actractUrl(string url, string htm)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htm);

            string[] auri = url.Split('/');
            string uri_root = string.Join("/", auri.Where((x, k) => k < 3).ToArray());
            string uri_path1 = string.Join("/", auri.Where((x, k) => k < auri.Length - 2).ToArray());
            string uri_path2 = string.Join("/", auri.Where((x, k) => k < auri.Length - 3).ToArray());

            var lsURLs = doc.DocumentNode
                .SelectNodes("//a")
                .Where(p => p.InnerText != null && p.InnerText.Trim().Length > 0)
                .Select(p => p.GetAttributeValue("href", string.Empty))
                .Select(x => x.IndexOf("../../") == 0 ? uri_path2 + x.Substring(5) : x)
                .Select(x => x.IndexOf("../") == 0 ? uri_path1 + x.Substring(2) : x)
                .Where(x => x.Length > 1 && x[0] != '#')
                .Select(x => x[0] == '/' ? uri_root + x : (x[0] != 'h' ? uri_root + "/" + x : x))
                .Select(x => x.Split('#')[0])
                .ToList();

            //string[] a = htm.Split(new string[] { "http" }, StringSplitOptions.None).Where((x, k) => k != 0).Select(x => "http" + x.Split(new char[] { '"', '\'' })[0]).ToArray();
            //lsURLs.AddRange(a);

            //????????????????????????????????????????????????????????????????????????????????
            //uri_root = "https://dictionary.cambridge.org/grammar/british-grammar/";

            var u_html = lsURLs
                 .Where(x => x.IndexOf(uri_root) == 0)
                 .Select(x => HttpUtility.UrlDecode(x))
                 .GroupBy(x => x)
                 .Select(x => x.First())
                 //.Where(x =>
                 //    !x.EndsWith(".pdf")
                 //    || !x.EndsWith(".txt")

                 //    || !x.EndsWith(".ogg")
                 //    || !x.EndsWith(".mp3")
                 //    || !x.EndsWith(".m4a")

                 //    || !x.EndsWith(".gif")
                 //    || !x.EndsWith(".png")
                 //    || !x.EndsWith(".jpg")
                 //    || !x.EndsWith(".jpeg")

                 //    || !x.EndsWith(".doc")
                 //    || !x.EndsWith(".docx")
                 //    || !x.EndsWith(".ppt")
                 //    || !x.EndsWith(".pptx")
                 //    || !x.EndsWith(".xls")
                 //    || !x.EndsWith(".xlsx"))
                 .Distinct()
                 .ToArray();

            //if (!string.IsNullOrEmpty(setting_URL_CONTIANS))
            //    foreach (string key in setting_URL_CONTIANS.Split('|'))
            //        u_html = u_html.Where(x => x.Contains(key)).ToArray();

            //var u_audio = lsURLs.Where(x => x.EndsWith(".mp3")).Distinct().ToArray();
            //var u_img = lsURLs.Where(x => x.EndsWith(".gif") || x.EndsWith(".jpeg") || x.EndsWith(".jpg") || x.EndsWith(".png")).Distinct().ToArray();
            //var u_youtube = lsURLs.Where(x => x.Contains("youtube.com/")).Distinct().ToArray();

            return u_html;
        }

        #endregion
    }

    class oLink
    {
        public string Url { set; get; }
        public bool isWriteFileCache { set; get; }
        public oLink() { isWriteFileCache = false; }
        public override string ToString()
        {
            return string.Format("{0} -> {1}", Url, isWriteFileCache);
        }
    }
}
