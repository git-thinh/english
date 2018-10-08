using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public class fBrowser: Form
    {
        readonly ChromiumWebBrowser browser;

        public fBrowser() {

            browser = new ChromiumWebBrowser("about:blank")
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(browser);
            this.Shown += (se, ev) =>
            {
                browser.Load("https://vnexpress.net/");

            };
        }

    }
}
