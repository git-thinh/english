using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace test
{
    public partial class FormBrowser : Form, IBrowserView
    {

        #region [ VAR ]

        //////☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //////🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆

        //readonly string URL_SETTING = "about:blank";
        //readonly string URL_SETTING = "local://view/setting.html";
        //readonly string URL = "https://vnexpress.net";
        //readonly string URL_GOOGLE = "https://google.com.vn";
        //readonly string URL = "http://w2ui.com/web/demos/#!layout/layout-1";
        //readonly string URL = "about:blank";
        //readonly string URL = "local://view/ws.html";
        readonly string URL = "local://view/bc1.html";
        //readonly string URL = "http://test.local/demo.html";
        //readonly string URL = "https://dictionary.cambridge.org/grammar/british-grammar/above-or-over";
        //readonly string URL = "https://vuejs.org/v2/guide/";
        //readonly string URL = "https://msdn.microsoft.com/en-us/library/ff361664(v=vs.110).aspx";
        //readonly string URL = "https://developer.mozilla.org/en-US/docs/Web";
        //readonly string URL = "https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/rest_parameters";
        //readonly string URL = "https://www.myenglishpages.com/site_php_files/grammar-lesson-tenses.php";
        //readonly string URL = "https://learnenglish.britishcouncil.org/en/english-grammar/pronouns";

        //const string URL = "https://translate.google.com/#en/vi/hello";

        #endregion

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
            ui_setting.PropertyChanged += f_setting_eventPropertyChanged;
            //var presenter = new BrowserUI("local://view/bc1.html", ui_setting, this, invoke => Invoke(invoke));


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
            };
        }

        private void f_setting_eventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string _string = null;
            bool _bool = false;

            Debug.WriteLine(" = " + e.PropertyName);

            switch (e.PropertyName)
            {
                case "IsBrowserInitialized":
                    if (ui_setting.IsBrowserInitialized)
                    {
                        ui_setting.Load(URL);
                    }
                    break;
                case "Title":
                    //_string = ui_setting.Title;
                    //gui_invoke(() => view.SetTitle(_string));
                    break;
                case "Address":
                    //_string = ui_setting.Address;
                    //gui_invoke(() => view.SetAddress(_string));
                    break;
                case "CanGoBack":
                    //_bool = ui_setting.CanGoBack;
                    //gui_invoke(() => view.SetCanGoBack(_bool));
                    break;
                case "CanGoForward":
                    //_bool = ui_setting.CanGoForward;
                    //gui_invoke(() => view.SetCanGoForward(_bool));
                    break;
                case "IsLoading":
                    //_bool = ui_setting.IsLoading;
                    //gui_invoke(() => view.SetIsLoading(_bool));
                    break;
            }
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
            if (is_loading)
            {
                //if (this.ShowDevToolsActivated != null) this.ShowDevToolsActivated(this, new EventArgs());
                //if (this.UrlActivated != null) this.UrlActivated(this, "https://vnexpress.net");
                 
            }
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
