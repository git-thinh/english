using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;

namespace test
{
    public partial class FormBrowser : Form, IBrowserView
    {

        private readonly WebView ui_setting;

        public event EventHandler ShowDevToolsActivated;
        public event EventHandler CloseDevToolsActivated;
        public event EventHandler ExitActivated;
        public event Action<object, string> UrlActivated;
        public event EventHandler BackActivated;
        public event EventHandler ForwardActivated;

        public FormBrowser()
        {
            Text = "test";
            this.Width = 999;
            this.Height = 600;

            ui_setting = new WebView();
            ui_setting.Dock = DockStyle.Fill;
            this.Controls.Add(ui_setting);
            var presenter = new BrowserUI("https://google.com.vn", ui_setting, this, invoke => Invoke(invoke));


            this.Shown += (se, ev) =>
            {
                ////ui_setting.Load("https://google.com.vn");
                ////if (this.UrlActivated != null) this.UrlActivated(this, );

                //if (ui_setting.IsBrowserInitialized)
                //{
                //    var handler = this.UrlActivated;
                //    if (handler != null)
                //    {
                //        handler(null, "https://google.com.vn");
                //    }
                //}
            };

            this.FormClosing += (se, ev) =>
            {
                ExitActivated(se, ev);
            };
        }

        public void SetTitle(string title)
        {
        }

        public void SetAddress(string address)
        {
        }

        public void SetCanGoBack(bool can_go_back)
        {
        }

        public void SetCanGoForward(bool can_go_forward)
        {
        }

        public void SetIsLoading(bool is_loading)
        {
        }

        public void ExecuteScript(string script)
        {
        }

        public object EvaluateScript(string script)
        {
            return null;
        }

        public void DisplayOutput(string output)
        {
        }
    }
}
