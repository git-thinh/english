using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;

namespace lenBrowser
{
    public class fBrowser : Form, IBeforeResourceLoad
    {
        #region
        // This is where we create our message window. When this form is created it will create our hidden window.
        readonly MessageListener MSG_WINDOW;

        readonly ICache CACHE;
        //////☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //////🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆

        const string URL_SETTING = "http://setting.local";
        //const string URL = "https://vnexpress.net";
        const string URL_GOOGLE = "https://google.com.vn";
        //const string URL = "http://w2ui.com/web/demos/#!layout/layout-1";
        //const string URL = "about:blank";
        const string URL = "http://test.local/demo.html";
        //const string URL = "https://translate.google.com/#en/vi/hello";

        readonly CefWebBrowser ui_browser;
        readonly CefWebBrowser ui_setting;

        readonly Panel ui_header;
        readonly Panel ui_footer;
        readonly Label ui_urlLabel;
        readonly TextBox ui_urlTextBox;
        readonly Label ui_statusLabel;
        readonly Label ui_backLabel;
        readonly Label ui_nextLabel;

        readonly Label ui_resize;
        private bool m_resizing = false;
        const bool m_hook_MouseMove = true;

        const int SETTING_WIDTH = 299;

        #endregion

        public fBrowser(ICache cache)
        {
            #region [ MAIN ]

            MSG_WINDOW = new MessageListener(this);
            CACHE = cache;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Browser";
            this.Icon = Resources.icon;
            this.Shown += (se, ev) =>
            {
                //this.WindowState = FormWindowState.Maximized;
                this.Width = 1024;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height - 300;
                this.Top = 100;
                this.Left = 200;// Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            };

            this.FormClosing += (se, ev) =>
            {
                ui_setting.Dispose();
                ui_browser.Dispose();
            };

            #endregion

            #region [ BROWSER ]

            ui_browser = new CefWebBrowser(URL);
            ui_browser.Dock = DockStyle.Fill;
            ui_browser.PropertyChanged += f_browserPropertyChanged;
            ui_browser.ConsoleMessage += f_browserConsoleMessage;
            ui_browser.BeforeResourceLoadHandler = this;
            this.Controls.Add(ui_browser);

            #endregion

            #region [ SETTING ]

            ui_setting = new CefWebBrowser(URL_SETTING);
            ui_setting.Width = SETTING_WIDTH;
            ui_setting.Dock = DockStyle.Left;
            ui_setting.ConsoleMessage += f_settingConsoleMessage;
            this.Controls.Add(ui_setting);

            var spliter = new Splitter()
            {
                Dock = DockStyle.Left,
                MinExtra = 0,
                MinSize = 0,
            };

            this.Controls.AddRange(new Control[] { spliter, ui_setting });

            #endregion

            #region [ HEADER ]

            ui_header = new Panel()
            {
                Dock = DockStyle.Top,
                BackColor = SystemColors.ControlLight,
                Height = 15,
            };
            this.Controls.Add(ui_header);
            ui_backLabel = new Label()
            {
                Text = " < ",
                Width = 24,
                Dock = DockStyle.Right
            };
            ui_nextLabel = new Label()
            {
                Text = " > ",
                Width = 24,
                Dock = DockStyle.Right
            };
            var lblMin = new Label()
            {
                Text = "[ - ]",
                Width = 24,
                Dock = DockStyle.Right
            };
            var lblExit = new Label()
            {
                Text = "[ x ]",
                Width = 24,
                Dock = DockStyle.Right
            };

            lblMin.Click += (se, ev) => { this.WindowState = FormWindowState.Minimized; };
            lblExit.Click += (se, ev) => { this.Close(); };

            ui_urlLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = URL,
            };
            ui_urlLabel.MouseMove += f_form_move_MouseDown;

            ui_urlTextBox = new TextBox()
            {
                Dock = DockStyle.Fill,
                //BackColor = Color.Blue,
                Text = URL,
                Visible = false,
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.ControlLight,
            };

            ui_urlLabel.Click += (se, ev) =>
            {
                ui_urlLabel.Visible = false;
                ui_urlTextBox.Visible = true;
                ui_urlTextBox.Focus();
                ui_urlTextBox.Select(ui_urlTextBox.TextLength, 0);
            };
            ui_urlTextBox.DoubleClick += (se, ev) => { ui_urlTextBox.Text = ""; };

            ui_header.Controls.AddRange(new Control[] {
                ui_urlTextBox, ui_urlLabel
            });
            ui_header.Controls.AddRange(new Control[] {
                new Label() { Text = "", Dock = DockStyle.Left, Width = 5 },
                ui_backLabel, ui_nextLabel,  lblMin, lblExit
            });

            ui_backLabel.Click += (se, ev) => f_browserBackPage();
            ui_nextLabel.Click += (se, ev) => f_browserNextPage();

            #endregion

            #region [ FOOTER ]

            ui_footer = new Panel()
            {
                Dock = DockStyle.Bottom,
                BackColor = SystemColors.ControlLight,
                Height = 14,
            };
            this.Controls.Add(ui_footer);

            var menu = new Label()
            {
                Text = "[ = ]",
                Width = 28,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.TopCenter,
            };

            ui_statusLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = string.Empty,
            };
            ui_statusLabel.MouseMove += f_form_move_MouseDown;

            ui_resize = new Label()
            {
                Dock = DockStyle.Right,
                Text = string.Empty,
                Width = 14,
            };
            ui_footer.Controls.AddRange(new Control[] { ui_statusLabel, menu, ui_resize });

            ui_resize.MouseDown += (se, ev) => { f_hook_mouse_Open(); m_resizing = true; };
            ui_resize.MouseUp += (se, ev) =>
            {
                m_resizing = false;
                f_hook_mouse_Close();
                //Debug.WriteLine("RESIZE: ok "); 
            };

            menu.Click += (se, ev) => f_settingToggle();

            #endregion
        }
        
        #region [ SETTING ]

        void f_settingToggle()
        {
            if (ui_setting.Width == 0)
            {
                ui_setting.Width = SETTING_WIDTH;
            }
            else
            {
                ui_setting.Width = 0;
            }
        }

        private void f_settingConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            //Console.WriteLine("SETTING.LOG = " + e.Source + ":" + e.Line + " " + e.Message);
            f_api_processMessage(e.Message);
        }

        #endregion

        #region [ BROWSER ]

        void f_browserGoPage(string url)
        {
            if (ui_browser.IsLoading)
            {
                ui_browser.Stop();
            }
            else
            {
                ui_browser.Load(url);
                ui_urlLabel.Text = url;
                ui_urlTextBox.Text = url;
            }
        }

        void f_browserBackPage()
        {
        }

        void f_browserNextPage()
        {
        }

        private void f_browserReady()
        {
            //ui_browser.Load("javascript:alert(123); return false;");
        }

        public void HandleBeforeResourceLoad(CefWebBrowser browserControl, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            string url = request.Url.ToLower();
            //if (url.Contains(".js") || url.Contains(".png") || url.Contains(".jpeg") || url.Contains(".jpg") || url.Contains(".gif"))
            //    requestResponse.Cancel();
        }

        private void f_browserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine("MAIN.LOG = " + e.Source + ":" + e.Line + " " + e.Message);
        }

        private void f_browserPropertyChangeUpdate(string propertyName)
        {
            switch (propertyName)
            {
                case "Title":
                    this.Text = ui_browser.Title;
                    ui_urlTextBox.Visible = false;
                    ui_urlLabel.Text = ui_browser.Title;
                    ui_urlLabel.Visible = true;
                    break;
                case "Address":
                    //urlTextBox.Text = ui_browser.Address;
                    break;
                case "CanGoBack":
                    //backButton.Enabled = ui_browser.CanGoBack;
                    break;
                case "CanGoForward":
                    //forwardButton.Enabled = ui_browser.CanGoForward;
                    break;
                case "IsLoading":
                    //goButton.Text = ui_browser.IsLoading ? "Stop" : "Go";
                    if (!ui_browser.IsLoading) f_browserReady();
                    break;
            }
        }

        private void f_browserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Invoke(new MethodInvoker(() => { if (!IsDisposed) f_browserPropertyChangeUpdate(e.PropertyName); }));
        }

        private string f_browser_runScript(string script, int timeout)
        {
            var result = ui_browser.RunScript("function(){ " + script + " }()", "CefSharp.Tests", 1, timeout);
            return result;
        }

        private string f_browser_runScript(string script)
        {
            return f_browser_runScript(script, Timeout.Infinite);
        }

        #endregion

        /*////////////////////////////////////////////////////////////////////////*/

        #region [ MOUSE MOVE: IN FORM, OUT FORM ]

        private void f_mouse_move_intoForm(int x, int y)
        {
            f_form_Resize(x, y, MOUSE_XY.INT);
        }

        private void f_mouse_move_outForm(int x, int y)
        {
            f_form_Resize(x, y, MOUSE_XY.OUT);
        }

        #endregion

        #region [ FORM MOVE, RESIZE ]

        enum MOUSE_XY { OUT, INT };

        void f_form_Resize(int x, int y, MOUSE_XY type)
        {
            if (m_resizing)
            {
                int max_x = this.Location.X + this.Width;
                int max_y = this.Location.Y + this.Height;
                this.Width = x - this.Location.X;
                this.Height = y - this.Location.Y;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void f_form_move_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region [ HOOK MOUSE: MOVE, WHEEL ... ]

        void f_hook_mouse_move_CallBack(MouseEventArgs e)
        {
            int max_x = this.Width + this.Location.X,
                max_y = e.Location.Y + this.Height;
            //Debug.WriteLine(this.Location.X + " " +e.X  + " " + max_x + " | " + this.Location.Y + " " +e.Y  + " " + max_y);

            if (e.X > this.Location.X && e.X < max_x
                && e.Y > this.Location.Y && e.Y < max_y)
            {
                //Debug.WriteLine("IN FORM: "+ this.Location.X + " " + e.X + " " + max_x + " | " + this.Location.Y + " " + e.Y + " " + max_y);
                f_mouse_move_intoForm(e.X, e.Y);
            }
            else
            {
                //Debug.WriteLine("OUT FORM: " + this.Location.X + " " + e.X + " " + max_x + " | " + this.Location.Y + " " + e.Y + " " + max_y);
                f_mouse_move_outForm(e.X, e.Y);
            }
        }

        void f_hook_mouse_Open()
        {
            if (m_hook_MouseMove)
                f_hook_mouse_SubscribeGlobal();
        }

        void f_hook_mouse_Close()
        {
            if (m_hook_MouseMove)
                f_hook_mouse_Unsubscribe();
        }

        /*////////////////////////////////////////////////////////////////////////*/

        private IKeyboardMouseEvents hook_events;

        private void f_hook_mouse_SubscribeApplication()
        {
            f_hook_mouse_Unsubscribe();
            f_hook_mouse_Subscribe(Hook.AppEvents());
        }

        private void f_hook_mouse_SubscribeGlobal()
        {
            f_hook_mouse_Unsubscribe();
            f_hook_mouse_Subscribe(Hook.GlobalEvents());
        }

        private void f_hook_mouse_Subscribe(IKeyboardMouseEvents events)
        {
            hook_events = events;
            //m_Events.KeyDown += OnKeyDown;
            //m_Events.KeyUp += OnKeyUp;
            //m_Events.KeyPress += HookManager_KeyPress;

            //m_Events.MouseUp += OnMouseUp;
            //m_Events.MouseClick += OnMouseClick;
            //m_Events.MouseDoubleClick += OnMouseDoubleClick;

            hook_events.MouseMove += f_hook_mouse_HookManager_MouseMove;

            //m_Events.MouseDragStarted += OnMouseDragStarted;
            //m_Events.MouseDragFinished += OnMouseDragFinished;

            //if (checkBoxSupressMouseWheel.Checked)
            //m_Events.MouseWheelExt += f_hook_mouse_HookManager_MouseWheelExt;
            //else
            ////hook_events.MouseWheel += f_hook_mouse_HookManager_MouseWheel;

            //if (checkBoxSuppressMouse.Checked)
            //m_Events.MouseDownExt += HookManager_Supress;
            //else
            //m_Events.MouseDown += OnMouseDown;
        }


        private void f_hook_mouse_Unsubscribe()
        {
            if (hook_events == null) return;
            //m_Events.KeyDown -= OnKeyDown;
            //m_Events.KeyUp -= OnKeyUp;
            //m_Events.KeyPress -= HookManager_KeyPress;

            //m_Events.MouseUp -= OnMouseUp;
            //m_Events.MouseClick -= OnMouseClick;
            //m_Events.MouseDoubleClick -= OnMouseDoubleClick;

            hook_events.MouseMove -= f_hook_mouse_HookManager_MouseMove;

            //m_Events.MouseDragStarted -= OnMouseDragStarted;
            //m_Events.MouseDragFinished -= OnMouseDragFinished;

            //if (checkBoxSupressMouseWheel.Checked)
            //m_Events.MouseWheelExt -= f_hook_mouse_HookManager_MouseWheelExt;
            //else
            //hook_events.MouseWheel -= f_hook_mouse_HookManager_MouseWheel;

            //if (checkBoxSuppressMouse.Checked)
            //m_Events.MouseDownExt -= HookManager_Supress;
            //else
            //m_Events.MouseDown -= OnMouseDown;

            hook_events.Dispose();
            hook_events = null;
        }

        private void f_hook_mouse_HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            f_hook_mouse_move_CallBack(e);
        }

        ////private void f_hook_mouse_HookManager_MouseWheel(object sender, MouseEventArgs e)
        ////{
        ////    //Debug.WriteLine(string.Format("Wheel={0:000}", e.Delta));
        ////    //f_hook_mouse_wheel_CallBack(e);
        ////}

        ////private void f_hook_mouse_HookManager_MouseWheelExt(object sender, MouseEventExtArgs e)
        ////{
        ////    //Debug.WriteLine(string.Format("Wheel={0:000}", e.Delta)); 
        ////    //Debug.WriteLine("Mouse Wheel Move Suppressed.\n");
        ////    e.Handled = true;
        ////    //e.Handled = true; // true: break event at here, stop mouse wheel at here
        ////}

        /////////////////////////////////////////////////////////////


        #endregion

        /*////////////////////////////////////////////////////////////////////////*/

        #region [ API ]

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);
        const int WM_COPYDATA = 0x4A;
        
        void f_api_sendNotification(string message)
        {
            COPYDATASTRUCT cds;
            cds.dwData = 0;
            cds.lpData = (int)Marshal.StringToHGlobalAnsi(message);
            cds.cbData = message.Length;
            //SendMessage(MSG_WINDOW.MainWindowHandle, (int)WM_COPYDATA, 0, ref cds);
            SendMessage(MSG_WINDOW.Handle, (int)WM_COPYDATA, 0, ref cds);
        }

        public void f_api_messageReceiver(string data) {
            ui_browser.Load(data);
        }
        
        void f_api_processMessage(string message)
        {
            oCmd cmd = f_api_jsonCmdParser(message);
            if (cmd != null)
            {
                switch (cmd.cmd)
                {
                    case "browser":
                        #region
                        string url = cmd.url;
                        if (!string.IsNullOrEmpty(url))
                        {
                            Console.WriteLine("GO: " + url);
                            //Invoke(new MethodInvoker(() => { if (!IsDisposed) f_browserGoPage(cmd.url); }));
                            HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
                            w.BeginGetResponse(asyncResult =>
                            {
                                string _url = ((HttpWebRequest)asyncResult.AsyncState).RequestUri.ToString();
                                string data = string.Empty;
                                bool isSuccess = false;
                                try
                                {
                                    HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult);
                                    if (rs.StatusCode == HttpStatusCode.OK)
                                    {
                                        using (StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8))
                                            data = sr.ReadToEnd();
                                        rs.Close();
                                    }

                                    if (!string.IsNullOrEmpty(data))
                                    {
                                        data = HttpUtility.HtmlDecode(data);
                                        data = format_HTML(data);
                                        File.WriteAllText("view/test/demo.HTML", data);
                                        //string[] urls = get_UrlHtml(url, data);
                                        //if (urls.Length > 0)
                                        //    StoreJob.f_url_AddRange(urls);
                                        string url_cache = CACHE.Set(url, data);
                                        isSuccess = true;
                                        f_api_sendNotification(url_cache);
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        data = "REQUEST_FAIL";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    data = ex.Message;
                                }

                                if (isSuccess)
                                {

                                }
                            }, w);
                        }
                        #endregion
                        break;
                }
            }
        }

        oCmd f_api_jsonCmdParser(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            s = s.Trim();
            if ((s[0] == '{' || s[0] == '[') && (s[s.Length - 1] == '}' || s[s.Length - 1] == ']'))
            {
                try
                {
                    return JsonConvert.DeserializeObject<oCmd>(s);
                }
                catch { }
            }
            return null;
        }

        static string format_HTML(string s)
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

        static string[] get_UrlHtml(string url, string htm)
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
            uri_root = "https://dictionary.cambridge.org/grammar/british-grammar/";

            var u_html = lsURLs
                 .Where(x => x.IndexOf(uri_root) == 0)
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
}
