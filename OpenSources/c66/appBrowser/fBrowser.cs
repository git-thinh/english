using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue.WindowsForms;

namespace appBrowser
{
    public class fBrowser: Form
    {
        private CefWebBrowser browser;
        //const string url = "https://google.com.vn";
        const string url = "https://vnexpress.net/";
        //const string url = "http://localhost";

        public fBrowser()
        {

            browser = new CefWebBrowser
            {
                Dock = DockStyle.Fill
            };
            browser.Parent = this;
            browser.BrowserCreated += (se, ev) => {
                browser.Browser.GetMainFrame().LoadUrl(url);
                //browser.Browser.GetMainFrame().LoadUrl("http://localhost:60000/index.html");
                //browser.Browser.Reload();
                //browser.Browser.GetMainFrame().LoadUrl("about:blank");
                //browser.Browser.GetMainFrame().LoadUrl("http://localhost:60000/index.html"); 
            };
            
            TextBox txt = new TextBox() {
                Text = url,
                Dock = DockStyle.Bottom,
                BorderStyle = BorderStyle.None,
                BackColor = System.Drawing.Color.LightGray,
                ForeColor = System.Drawing.Color.Gray
            };
            txt.KeyUp += (se, ev) => {
                if(ev.KeyCode == Keys.Enter)
                    browser.Browser.GetMainFrame().LoadUrl(txt.Text.Trim());
            };
            this.Controls.Add(txt);

            this.Shown += (se, ev) => {
                this.Width = 1024;
                this.Height = 600;
            };
        }
    }
}
