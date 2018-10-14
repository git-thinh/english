using System;
using System.Windows.Forms;
using System.Drawing;
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;
using test.Properties;
using CefSharp.WinForms;
using CefSharp;
using System.Collections.Generic;

namespace test
{
    public interface IFormMain {
        void f_browser_Go(string url);
        void f_browser_updateInfoPage(string url, string title);
    }

    public class fMain : Form, IFormMain
    {
        #region [ VAR ]
        readonly IApp _app;

        //// This is where we create our message window. When this form is created it will create our hidden window.
        //readonly MessageListener MSG_WINDOW;
        //readonly oAppInfo APP_INFO;

        //////☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //////🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆

        //const int SETTING_WIDTH = 299;
        //const string URL_SETTING = "about:blank";
        //const string URL_SETTING = "local://view/setting.html";
        //const string URL = "https://vnexpress.net";
        //const string URL = "https://dictionary.cambridge.org/grammar/british-grammar/above-or-over";
        const string URL = "https://dictionary.cambridge.org/grammar/british-grammar/before";
        //const string URL_GOOGLE = "https://google.com.vn";
        //const string URL = "http://w2ui.com/web/demos/#!layout/layout-1";
        //const string URL = "about:blank";
        //const string URL = "local://view/ws.html";
        //const string URL = "local://view/bc1.html";
        //const string URL = "local://view/article.html";
        //const string URL = "http://test.local/demo.html";
        //const string URL = "https://vuejs.org/v2/guide/";
        //const string URL = "https://msdn.microsoft.com/en-us/library/ff361664(v=vs.110).aspx";
        //const string URL = "https://developer.mozilla.org/en-US/docs/Web";
        //const string URL = "https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/rest_parameters";
        //const string URL = "https://www.myenglishpages.com/site_php_files/grammar-lesson-tenses.php";
        //const string URL = "https://learnenglish.britishcouncil.org/en/english-grammar/pronouns";

        //const string URL = "https://translate.google.com/#en/vi/hello";

        readonly WebView ui_browser;

        private Panel ui_header;
        private Panel ui_footer;
        private Label ui_urlLabel;
        private TextBox ui_urlTextBox;
        private Label ui_statusLabel;
        private Label ui_backLabel;
        private Label ui_nextLabel;

        private Label ui_resize;
        private bool m_resizing = false;
        const bool m_hook_MouseMove = true;

        #endregion

        #region [ MAIN ]

        public IWebBrowser f_getBrowser()
        {
            return ui_browser;
        }

        public fMain(IApp app)
        {
            this._app = app;

            #region [ MAIN ]

            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Browser";
            this.Icon = Resources.icon;

            ui_browser = new WebView() { Dock = DockStyle.Fill };
            this.Controls.Add(ui_browser);
            ui_browser.PropertyChanged += (se, ev) => { switch (ev.PropertyName) { case "IsBrowserInitialized": f_browser_Go(URL); break; case "Title": f_browser_loadTitleReady(ui_browser.Title); break; case "IsLoading": f_browser_loadDomReady(); break; } };
            ui_browser.RequestHandler = new BrowserRequestHandler(app);

            var listMenuContext = new List<MenuItem>() {
                new MenuItem("Reload Page", f_browser_menuContextItemClick) { Tag = "reload" },
                new MenuItem("Bookmark Page", f_browser_menuContextItemClick){ Tag = "bookmark" },
                new MenuItem("-"){ Tag = "" },
                new MenuItem("Links", f_browser_menuContextItemClick){ Tag = "link" },
                new MenuItem("Search", f_browser_menuContextItemClick){ Tag = "search" },
                new MenuItem("-"){ Tag = "" },
                new MenuItem("Setting", f_browser_menuContextItemClick){ Tag = "setting" },
                new MenuItem("Show DevTool", f_browser_menuContextItemClick){ Tag = "devtool_open" },
                new MenuItem("View Source", f_browser_menuContextItemClick){ Tag = "view_source" },
                new MenuItem("Print", f_browser_menuContextItemClick){ Tag = "print" },
                new MenuItem("-"){ Tag = "" },
                new MenuItem("Exit Application", f_browser_menuContextItemClick){ Tag = "exit" },
            };
            ContextMenu cm = new ContextMenu(listMenuContext.ToArray());
            ui_browser.ContextMenu = cm;
            ui_browser.MenuHandler = new BrowserMenuHandel(app);




            this.Shown += (se, ev) =>
            {
                //this.WindowState = FormWindowState.Maximized;
                this.Width = 800;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height - 200;
                this.Top = 100;
                this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;

                f_main_Init();
            };

            this.FormClosing += (se, ev) =>
            {
                ui_browser.Dispose();
            };

            #endregion
        }

        void f_main_Init()
        {
            #region [ HEADER ]

            ui_header = new Panel()
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Height = 15,
            };
            this.Controls.Add(ui_header);
            ui_backLabel = new Label()
            {
                Text = " < ",
                Width = 24,
                ForeColor = Color.Gray,
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
                ForeColor = Color.Gray,
                Dock = DockStyle.Right
            };
            var lblExit = new Label()
            {
                Text = "[ x ]",
                Width = 24,
                ForeColor = Color.Gray,
                Dock = DockStyle.Right
            };

            lblMin.Click += (se, ev) => { this.WindowState = FormWindowState.Minimized; };
            lblExit.Click += (se, ev) => { this.Close(); };

            ui_urlLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = URL,
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.Gray,
            };
            ui_urlLabel.MouseMove += f_form_move_MouseDown;
            ui_urlLabel.DoubleClick += (se, ev) =>
            {
                if (this.Width == Screen.PrimaryScreen.WorkingArea.Width)
                {
                    this.Width = 1024;
                    this.Height = 768;
                    this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
                    this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
                }
                else
                {
                    this.Left = 0;
                    this.Top = 0;
                    this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                    this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                }

            };

            ui_urlLabel.Click += (se, ev) =>
            {
                ui_urlTextBox.Focus();
                ui_urlTextBox.Select(ui_urlTextBox.TextLength, 0);
            };

            ui_header.Controls.AddRange(new Control[] {
                ui_urlLabel,
                new Label() { Text = "", Dock = DockStyle.Left, Width = 5 },
                ui_backLabel, ui_nextLabel,   lblMin, lblExit
            });

            ui_backLabel.Click += (se, ev) => f_browser_backPage();
            ui_nextLabel.Click += (se, ev) => f_browser_nextPage();

            #endregion

            #region [ FOOTER ]

            ui_footer = new Panel()
            {
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Height = 14,
            };
            this.Controls.Add(ui_footer);

            var devTool = new Label()
            {
                Text = " devtool ",
                AutoSize = true,
                //Width = 45,
                ForeColor = Color.Gray,
                Dock = DockStyle.Left
            };
            devTool.Click += (se, ev) => { if (ui_browser.IsBrowserInitialized) ui_browser.ShowDevTools(); };
            var lblSearch = new Label()
            {
                Text = " search ",
                AutoSize = true,
                //Width = 45,
                ForeColor = Color.Gray,
                Dock = DockStyle.Left
            };
            lblSearch.Click += (se, ev) => f_browser_searchOpenWindow();
            var menu = new Label()
            {
                Text = " links ",
                //Width = 45,
                AutoSize = true,
                Dock = DockStyle.Left,
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.TopCenter,
            };

            ui_statusLabel = new Label()
            {
                Dock = DockStyle.Left,
                Text = string.Empty,
                ForeColor = Color.Gray,
                Width = 200,
            };
            ui_statusLabel.MouseMove += f_form_move_MouseDown;


            ui_urlTextBox = new TextBox()
            {
                Dock = DockStyle.Fill,
                Text = URL,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Right,
                ForeColor = Color.Gray,
            };
            ui_urlTextBox.DoubleClick += (se, ev) => { ui_urlTextBox.Text = ""; };
            ui_urlTextBox.KeyUp += (se, ev) => { if (ev.KeyCode == Keys.Enter) f_browser_openUrlByKey(ui_urlTextBox.Text); };

            ui_resize = new Label()
            {
                Dock = DockStyle.Right,
                Text = string.Empty,
                Width = 14,
            };
            ui_footer.Controls.AddRange(new Control[] {
                ui_urlTextBox,
                ui_statusLabel,
                //devTool,
                //new Label() { Text = "|", Width = 3, Dock = DockStyle.Left, ForeColor = Color.LightGray, TextAlign = ContentAlignment.BottomCenter },
                //lblSearch,
                //new Label() { Text = "|", Width = 3, Dock = DockStyle.Left, ForeColor = Color.LightGray, TextAlign = ContentAlignment.BottomCenter },
                //menu,
                ui_resize });

            ui_resize.MouseDown += (se, ev) => { f_hook_mouse_Open(); m_resizing = true; };
            ui_resize.MouseUp += (se, ev) =>
            {
                m_resizing = false;
                f_hook_mouse_Close();
                //Debug.WriteLine("RESIZE: ok "); 
            };

            menu.Click += (se, ev) => f_browser_appOpenWindow();

            #endregion
        }

        #endregion

        #region [ BROWSER ]

        void f_browser_menuContextItemClick(object sender, EventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            string key = menu.Tag as string;
            switch (key)
            {
                case "reload":
                    if (ui_browser.IsLoading) ui_browser.Stop();
                    ui_browser.Reload();
                    break;
                case "bookmark":
                    break;
                case "link":
                    break;
                case "search":
                    break;
                case "devtool_open":
                    ui_browser.ShowDevTools();
                    break;
                case "setting":
                    break;
                case "print":
                    ui_browser.Print();
                    break;
                case "view_source":
                    string source = _app.f_link_getHtmlCache(ui_browser.Address);
                    var f = new Form()
                    {
                        FormBorderStyle = FormBorderStyle.FixedSingle,
                        Text = ui_browser.Address,
                        Icon = Resources.icon,
                        WindowState = FormWindowState.Maximized,
                        Padding = new Padding(10, 0, 0, 0),
                        BackColor = Color.Black
                    };
                    f.Controls.Add(new TextBox()
                    {
                        Text = BrowserRequestHandler.buildPageHtml(source),
                        Multiline = true,
                        BorderStyle = BorderStyle.None,
                        Dock = DockStyle.Fill,
                        ScrollBars = ScrollBars.Both,
                        BackColor = Color.Black,
                        ForeColor = Color.White,
                        Font = new Font("Lucida Console", 13, FontStyle.Regular),
                    });
                    f.Show();
                    break;
                case "exit":
                    this.Close();
                    break;
            }
        }

        public void f_browser_Go(string url)
        {
            ui_browser.Load(url);
        }

        public void f_browser_updateInfoPage(string url, string title) {
            ui_urlLabel.Invoke(t => t.Text = title);
            ui_urlTextBox.Invoke(t => t.Text = url);
        }

        void f_browser_loadTitleReady(string title)
        {
            Invoke(new MethodInvoker(() =>
            {
                if (!IsDisposed)
                {
                    this.Text = title;
                    ui_urlLabel.Text = title;
                    //ui_urlTextBox.Text = ui_browser.
                }
            }));
        }

        void f_browser_loadDomReady()
        {

        }

        void f_browser_openUrlByKey(string urlKey)
        {
            _app.f_main_openUrl(urlKey, null);
        }

        void f_browser_appOpenWindow()
        {

        }

        void f_browser_searchOpenWindow()
        {

        }

        void f_browser_backPage()
        {

        }

        void f_browser_nextPage()
        {

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

    }
}
