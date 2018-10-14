using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using test.Properties;

namespace test
{
    public class fView: Form
    {
        readonly WebView ui_browser;
        readonly IApp _app;
        readonly string URL = "about:blank";

        public fView(IApp app, string key) {
            URL = "http://local/view/" + key + ".html";

            this._app = app;
            this.FormBorderStyle = FormBorderStyle.None;

            this.Icon = Resources.icon;

            ui_browser = new WebView() { Dock = DockStyle.Fill };
            this.Controls.Add(ui_browser);
            ui_browser.PropertyChanged += (se, ev) => { switch (ev.PropertyName) { case "IsBrowserInitialized": f_browser_Go(URL); break; case "Title": f_browser_loadTitleReady(ui_browser.Title); break; case "IsLoading": f_browser_loadDomReady(); break; } };
            ui_browser.RequestHandler = new BrowserRequestHandler(app);

            ContextMenu cm = new ContextMenu(f_build_contextMenu());
            ui_browser.ContextMenu = cm;
            ui_browser.MenuHandler = new BrowserMenuHandel();
        }

        private void f_browser_loadDomReady()
        {
        }

        private MenuItem[] f_build_contextMenu()
        {
            return new MenuItem[] {
                new MenuItem("Reload Page", f_browser_menuContextItemClick) { Tag = "reload" },
                new MenuItem("Show DevTool", f_browser_menuContextItemClick){ Tag = "devtool_open" },
                new MenuItem("View Source", f_browser_menuContextItemClick){ Tag = "view_source" },
                new MenuItem("-"){ Tag = "" },
                new MenuItem("Exit Application", f_browser_menuContextItemClick){ Tag = "exit" },
            };
        }

        private void f_browser_menuContextItemClick(object sender, EventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            string key = menu.Tag as string;
            switch (key)
            {
                case "reload":
                    if (ui_browser.IsLoading) ui_browser.Stop();
                    ui_browser.Reload();
                    break;
                case "devtool_open":
                    ui_browser.ShowDevTools();
                    break;
                case "view_source":
                    #region
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
                    #endregion
                    break;
                case "exit":
                    this.Close();
                    break;
            }
        }

        private void f_browser_loadTitleReady(string title)
        {
        }

        private void f_browser_Go(string url)
        {
            ui_browser.Load(url);
        }
    }
}
