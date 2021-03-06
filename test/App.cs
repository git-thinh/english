﻿using CefSharp;
using Fleck;
using Newtonsoft.Json;
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
   

    class App : IApp
    {
        IForm _fomMain = null;
        readonly List<IForm> CLIENTS;

        public void f_form_openByKey(string view)
        {
            var v = new fLocal(this, view);
            v.Shown += (vse, vev) =>
            {
                v.Left = _fomMain.f_getInfo().Width + _fomMain.f_getInfo().Left + 5;
                v.Top = _fomMain.f_getInfo().Top;
                v.Height = _fomMain.f_getInfo().Height;
                v.Width = 600;
            };
            v.Show();
        }
        public void f_form_unRegister(IForm form)
        {
            if (form != null)
            {
                lock (CLIENTS)
                {
                    int pos = CLIENTS.IndexOf(form);
                    if (pos != -1) CLIENTS.RemoveAt(pos);
                }
            }
        }
        public void f_form_Register(IForm form)
        {
            if (form != null)
            {
                lock (CLIENTS)
                {
                    int pos = CLIENTS.IndexOf(form);
                    if (pos == -1) CLIENTS.Add(form);
                }
            }
        }

        public App()
        {
            if (Directory.Exists("cache")) DOMAIN_LIST = Directory.GetDirectories("cache").Select(x => x.Substring(6)).ToList();
            else DOMAIN_LIST = new List<string>();

            ///////////////////////////////////////////////////////////////////////

            TRANSLATE = new ConcurrentDictionary<string, string>();
            CACHE = new ConcurrentDictionary<string, string>();
            LINK_TITLE = new ConcurrentDictionary<string, string>();
            LINK_LEVEL = new ConcurrentDictionary<int, int>();
            LINK_ID = new ConcurrentDictionary<string, int>();
            INDEX = new ConcurrentDictionary<string, List<int>>();
            DOMAIN_LINK = new ConcurrentDictionary<string, List<int>>();
            TIME_VIEW_LINK = new ConcurrentDictionary<int, int>();
            KEY_INDEX = new ConcurrentDictionary<string, List<int>>();

            ///////////////////////////////////////////////////////////////////////

            CLIENTS = new List<IForm>();
        }

        #region [ LINK - HTML ]

        readonly List<string> DOMAIN_LIST;

        readonly ConcurrentDictionary<string, string> CACHE;
        readonly ConcurrentDictionary<string, string> LINK_TITLE;
        readonly ConcurrentDictionary<int, int> LINK_LEVEL;
        readonly ConcurrentDictionary<string, int> LINK_ID;
        readonly ConcurrentDictionary<int, int> TIME_VIEW_LINK;
        readonly ConcurrentDictionary<string, List<int>> INDEX;
        readonly ConcurrentDictionary<string, List<int>> DOMAIN_LINK;
        readonly ConcurrentDictionary<string, List<int>> KEY_INDEX;
        readonly ConcurrentDictionary<string, string> TRANSLATE;

        public oLinkResult f_link_getLinkPaging(oLinkRequest linkRequest) {
            oLinkResult rs = new oLinkResult() { };
            return rs;
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
                    {
                        return "<script> location.href='" + urlDirect + "'; </script>";
                    }
                    else
                        return html;
                }
                else
                {
                    Console.WriteLine(" ??????????????????????????????????????????? ERROR: " + url);
                }

                Console.WriteLine(" -> Fail: " + url);

                return null;
            }

            Console.WriteLine(" -> Ok: " + url);

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
            int id = LINK_TITLE.Count + 1, time_view = int.Parse(DateTime.Now.ToString("1ddHHmmss")) + indexForEach;

            if (!LINK_TITLE.ContainsKey(url))
            {
                LINK_TITLE.TryAdd(url, title);
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

        #region [ MESSAGE FROM JS ]

        void f_process_messageTo_MAIN(oMsgSocket msg)
        {
            try
            {
                string msgRequest = msg.MsgRequestJson, text = string.Empty;
                if (string.IsNullOrEmpty(msgRequest)) return;

                msgRequest = msgRequest.Trim();
                Console.WriteLine(string.Format("{0} -> {1}", msg.From, msgRequest));
                switch (msg.MsgType)
                {
                    case MSG_TYPE.EN_TRANSLATE_GOOGLE_REQUEST:
                        #region

                        var otran = JsonConvert.DeserializeObject<oEN_TRANSLATE_GOOGLE_MESSAGE>(msgRequest);

                        text = otran.text.Trim().Replace(':', '-');
                        if (TRANSLATE.ContainsKey(text))
                        {
                            otran.mean_vi = TRANSLATE[text];
                            otran.success = true;
                            Console.WriteLine("-> TRANSLATE.CACHE: {0} = {1}", text, otran.mean_vi);

                            msg.Ok = true;
                            msg.MsgResponse = JsonConvert.SerializeObject(otran);
                            msg.MsgType = MSG_TYPE.EN_TRANSLATE_GOOGLE_RESPONSE;
                            string _msgResponse = JsonConvert.SerializeObject(msg);
                            f_sendToBrowser(_msgResponse);
                        }
                        else
                        {
                            GooTranslateService_v1.TranslateAsync(otran, "en", "vi", string.Empty, (_otran) =>
                            {
                                if (_otran.mean_vi.Contains(':'))
                                {
                                    f_process_messageTo_MAIN(msg);
                                    return;
                                }
                                //Console.WriteLine("\r\n -> V1: " + text + " (" + _otran.type + "): " + _otran.mean_vi);
                                if (!TRANSLATE.ContainsKey(text)) TRANSLATE.TryAdd(text, _otran.mean_vi);
                                Console.WriteLine("-> TRANSLATE.ONLINE: {0} = {1}", text, otran.mean_vi);

                                if (_otran.success)
                                {
                                    msg.Ok = true;
                                    msg.MsgType = MSG_TYPE.EN_TRANSLATE_GOOGLE_RESPONSE;
                                    msg.MsgResponse = JsonConvert.SerializeObject(otran);
                                    string _msgWs = JsonConvert.SerializeObject(msg);
                                    f_sendToBrowser(_msgWs);
                                }
                                else
                                {
                                    msg.Ok = false;
                                    msg.MsgType = MSG_TYPE.EN_TRANSLATE_GOOGLE_REQUEST;
                                    msg.MsgResponse = _otran.mean_vi;
                                    string _msgWs = JsonConvert.SerializeObject(msg);
                                    f_sendToBrowser(_msgWs);
                                }
                            });
                        }

                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                msg.Ok = false;
                msg.MsgType = MSG_TYPE.EN_TRANSLATE_GOOGLE_REQUEST;
                msg.MsgResponse = ex.Message;
                //socket.Send(JsonConvert.SerializeObject(msg));
            }
        }

        public void f_app_callFromJs(string message)
        {
            try
            {
                oMsgSocket m = JsonConvert.DeserializeObject<oMsgSocket>(message);
                switch (m.To)
                {
                    case _NAME_UI.MAIN:
                        f_process_messageTo_MAIN(m);
                        break;
                        //case _NAME_UI.ALL:
                        //    lock (CLIENTS) { CLIENTS.ForEach((ws) => { if (ws.IsAvailable) ws.Send(message); }); }
                        //    break;
                        //case _NAME_UI.BOX_ENGLISH:
                        //    if (CLIENT_BOX_ENGLISH.IsAvailable) CLIENT_BOX_ENGLISH.Send(message);
                        //    break;
                        //case _NAME_UI.SEARCH:
                        //    if (CLIENT_SEARCH.IsAvailable) CLIENT_SEARCH.Send(message);
                        //    break;
                        //case _NAME_UI.SETTING:
                        //    if (CLIENT_SETTING.IsAvailable) CLIENT_SETTING.Send(message);
                        //    break;
                        //case _NAME_UI.PLAYER:
                        //    if (CLIENT_PLAYER.IsAvailable) CLIENT_PLAYER.Send(message);
                        //    break;
                }
            }
            catch
            {
                //socket.Send(JsonConvert.SerializeObject(new oMsgSocket(false, MSG_TYPE.EN_TRANSLATE_GOOGLE_REQUEST, 0, message, ex.Message)));
            }
        }

        #endregion

        void f_sendToBrowser(string msg)
        {
            _fomMain.f_sendToBrowser(msg);

            lock (CLIENTS)
            {
                CLIENTS.ForEach(fom =>
                {
                    fom.f_sendToBrowser(msg);
                });
            }
        }

        #region [ APP ]

        [STAThread]
        static void Main(string[] args) => new App().f_app_Run();

        public void f_app_Run()
        {
            new Thread(() => GooTranslateService_v1.TranslateAsync(new oEN_TRANSLATE_GOOGLE_MESSAGE() { text = "hello" }, "en", "vi", string.Empty, (_otran) => { })).Start();

            //List<string> args = new List<string>();
            //args.Add("--disable-web-security");
            ////cefApp_ = CefApp.getInstance(args.ToArray());
            
            //BrowserSettings
            Settings settings = new Settings() { };
            if (!CEF.Initialize(settings)) return;
            CEF.RegisterScheme("http", "api", false, new ApiHandlerFactory(this));

            //CEF.RegisterScheme("local", new LocalSchemeHandlerFactory(this));
            //CEF.RegisterJsObject("API", new API(this, main));
            // BrowserSettings.WebSecurityDisabled
            //browser.BrowserSettings.WebSecurity = CefState.Disabled; // allow cross domain
            //browser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;

            Application.ApplicationExit += (se, ev) => f_app_Exit();
            var main = new fMain(this);
            main.Shown += (se, ev) =>
            {
                //f_form_openByKey("link");
            };
            _fomMain = main;
            Application.Run(main);
            f_app_Exit();
        }

        void f_app_Exit()
        {
            CLIENTS.Clear();

            CACHE.Clear();
            LINK_TITLE.Clear();
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
