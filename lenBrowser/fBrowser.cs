using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;

namespace lenBrowser
{
    public class fBrowser: Form, IBeforeResourceLoad
    {
        //const string URL = "about:blank";
        //const string URL = "https://vnexpress.net";
        const string URL = "https://google.com.vn";
        readonly CefWebBrowser ui_browser;
        readonly Panel ui_header;
        readonly Panel ui_footer;
        readonly Label ui_urlLabel;
        readonly TextBox ui_urlTextBox;

        public fBrowser()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Browser";
            this.Icon = Resources.icon;

            ui_browser = new CefWebBrowser(URL);
            ui_browser.Dock = DockStyle.Fill;
            ui_browser.PropertyChanged += f_browserPropertyChanged;
            ui_browser.ConsoleMessage += f_browserConsoleMessage;
            ui_browser.BeforeResourceLoadHandler = this;
            this.Controls.Add(ui_browser);
            this.Shown += (se, ev) => {
                this.WindowState = FormWindowState.Maximized;
            };

            ui_header = new Panel() {
                Dock = DockStyle.Top,
                BackColor = SystemColors.ControlLight,
                Height = 15,
            };
            this.Controls.Add(ui_header);
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
                //BackColor = Color.Red,
                Text = URL,
            };

            ui_urlTextBox = new TextBox()
            {
                Dock = DockStyle.Fill,
                //BackColor = Color.Blue,
                Text = URL,
                Visible = false,
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.ControlLight,
            };

            ui_urlLabel.Click += (se, ev) => {
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
                lblMin, lblExit
            });
        }
        
        #region [ BROWSER ]

        public void HandleBeforeResourceLoad(CefWebBrowser browserControl, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            string url = request.Url.ToLower();
            //if (url.Contains(".js") || url.Contains(".png") || url.Contains(".jpeg") || url.Contains(".jpg") || url.Contains(".gif")) 
            //requestResponse.Cancel();
        }

        private void f_browserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }


        private void f_browserPropertyChangeUpdate(string propertyName) {
            switch (propertyName)
            {
                case "Title":
                    this.Text = ui_browser.Title;
                    //ui_urlLabel.Text = ui_browser.get
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
                    if (!ui_browser.IsLoading)
                    {
                        //_browserControl.RunScript(" alert ", "about:blank", 0);
                        //string url = f_browser_runScript("alert('123')");
                        ui_browser.Load("javascript:alert(123); return false;");
                    }
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
    }
}
