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

namespace lenBrowser
{
    public class fBrowser : Form, IBeforeResourceLoad
    {
        //////☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //////🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆
        const string URL_SETTING = "about:blank";
        //const string URL = "https://vnexpress.net";
        const string URL = "https://google.com.vn";
        readonly CefWebBrowser ui_browser;
        readonly CefWebBrowser ui_setting;
        readonly Panel ui_header;
        readonly Panel ui_footer;
        readonly Label ui_urlLabel;
        readonly TextBox ui_urlTextBox;
        readonly Label ui_statusLabel;
        readonly Label ui_backLabel;
        readonly Label ui_nextLabel;

        readonly Label m_resize;
        private bool m_resizing = false;
        const bool m_hook_MouseMove = true;

        public fBrowser()
        {
            #region [ MAIN ]

            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Browser";
            this.Icon = Resources.icon;
            this.Shown += (se, ev) =>
            {
                //this.WindowState = FormWindowState.Maximized;
                this.Width = 1024;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                this.Top = 0;
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
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
            ui_setting.Width = 0;
            ui_setting.Dock = DockStyle.Left;
            ui_setting.PropertyChanged += f_browserPropertyChanged;
            ui_setting.ConsoleMessage += f_browserConsoleMessage;
            this.Controls.Add(ui_setting);

            var spliter = new Splitter() {
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
                Text = "[ ♡ ]",
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

            m_resize = new Label()
            {
                Dock = DockStyle.Right,
                Text = string.Empty,
                Width = 14,
                BackColor = Color.DimGray,
            };
            ui_footer.Controls.AddRange(new Control[] { ui_statusLabel, menu, m_resize });

            m_resize.MouseDown += (se, ev) => { f_hook_mouse_Open(); m_resizing = true; };
            m_resize.MouseUp += (se, ev) =>
            {
                m_resizing = false;
                f_hook_mouse_Close();
                //Debug.WriteLine("RESIZE: ok "); 
            };
            #endregion

        }

        #region [ BROWSER ]

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

        #region [ API RESPONSE ]

        #endregion
    }
}
