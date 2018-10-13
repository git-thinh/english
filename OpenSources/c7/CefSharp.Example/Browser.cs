using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using System.Threading;

namespace CefSharp.Example
{
    public partial class Browser : Form, IBeforeResourceLoad
    {
        private readonly CefWebBrowser _browserControl;
        //private const string cefSharpHomeUrl = "https://google.com.vn";
        private const string cefSharpHomeUrl = "local://view/bc1.html";
        //private const string cefSharpHomeUrl = "https://vnexpress.net";

        public Browser()
        {
            InitializeComponent();
            Text = "CefSharp";
            _browserControl = new CefWebBrowser(cefSharpHomeUrl);
            
            _browserControl.Dock = DockStyle.Fill;
            _browserControl.PropertyChanged += HandleBrowserPropertyChanged;
            _browserControl.ConsoleMessage += HandleConsoleMessage;
            _browserControl.BeforeResourceLoadHandler = this; 
            toolStripContainer.ContentPanel.Controls.Add(_browserControl);
             

        }

        private string RunScript(string script, int timeout)
        {
            var result = _browserControl.RunScript("function(){" + script + "}()", "CefSharp.Tests", 1, timeout);
            return result;
        }

        private string RunScript(string script)
        {
            return RunScript(script, Timeout.Infinite);
        }

        private void HandleBrowserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("PROPERTY_CHANGED = " + e.PropertyName + "; IsLoading = " + _browserControl.IsLoading.ToString());

            Invoke(new MethodInvoker(() => { if (!IsDisposed) UpdateBrowserControls(sender, e); }));
        }

        private void UpdateBrowserControls(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title":
                    Text = _browserControl.Title;
                    break;
                case "Address":
                    urlTextBox.Text = _browserControl.Address;
                    break;
                case "CanGoBack":
                    backButton.Enabled = _browserControl.CanGoBack;
                    break;
                case "CanGoForward":
                    forwardButton.Enabled = _browserControl.CanGoForward;
                    break;
                case "IsLoading":
                    goButton.Text = _browserControl.IsLoading ? "Stop" : "Go";
                    if (!_browserControl.IsLoading) {
                        //_browserControl.RunScript(" alert ", "about:blank", 0);
                    }
                    break;
            }
        }

        private void HandleGoButtonClick(object sender, EventArgs e)
        {
            if(_browserControl.IsLoading)
            {
                _browserControl.Stop();
            }
            else
            {
                _browserControl.Load(urlTextBox.Text);    
            }            
        }

        private void HandleBackButtonClick(object sender, EventArgs e)
        {
            _browserControl.Back();
        }

        private void HandleForwardButtonClick(object sender, EventArgs e)
        {
            _browserControl.Forward();
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            int width = toolStrip1.DisplayRectangle.Width;
            foreach (ToolStripItem tsi in toolStrip1.Items)
            {
                if (tsi != urlTextBox)
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal);
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _browserControl.Load(urlTextBox.Text);
            }
        }

        public void HandleBeforeResourceLoad(CefWebBrowser browserControl, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith("http://test/resource/load"))
            {
                Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                requestResponse.RespondWith(resourceStream, "text/html");
            }

            //string url = request.Url.ToLower();
            //if (url.Contains(".js")) {
            //    Debug.WriteLine("----> CANCEL: " + url);
            //    requestResponse.Cancel();
            //}else
            //    Debug.WriteLine(url);
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TestResourceLoadToolStripMenuItemClick(object sender, EventArgs e)
        {
            _browserControl.Load("http://test/resource/load");
        }

        private void TestRunJsSynchronouslyToolStripMenuItemClick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);

            try
            {
                String result = _browserControl.RunScript(a + "+" + b, "RunJsTest", 1, 5000);

                if (result == (a + b).ToString())
                {
                    MessageBox.Show(string.Format("{0} + {1} = {2}", a, b, result), "Success");
                }
                else
                {
                    MessageBox.Show(string.Format("{0} + {1} != {2}", a, b, result), "Failure");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Failure");
            }
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void TestRunArbitraryJavaScriptToolStripMenuItemClick(object sender, EventArgs e)
        {                       
            InputForm inputForm = new InputForm();
            if(inputForm.ShowDialog() == DialogResult.OK)
            {
                string script = inputForm.GetInput();
                try
                {
                    string result = _browserControl.RunScript(script, "about:blank", 1, 5000);
                    MessageBox.Show(result, "Result");
                } 
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error");
                }
            }
        }
        private void HandleConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            //MessageBox.Show(e.Source + ":" + e.Line + " " + e.Message, "JavaScript console message");
            Debug.WriteLine("CONSOLE.LOG = " + e.Source + ":" + e.Line + " " + e.Message);
        }

        private void TestSchemeHandlerToolStripMenuItemClick(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/SchemeTest.html");
        }

        private void TestConsoleMessagesToolStripMenuItemClick(object sender, EventArgs e)
        {           
            _browserControl.Load("javascript:console.log('console log message text')");
        }


        private void TestBingClrObjectToJsToolStripMenuItemClick(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/BindingTest.html");
        }

        private void cefSharpHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load(cefSharpHomeUrl);
        }

        private void fireBugLiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("http://getfirebug.com/firebuglite");
        }

        private void popupModalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/modalmain.html");
        }

        private void aJAXXMLHttpRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/xmlhttprequest.html");
        }

        private void userInterfaceAppExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/uiplugin.html");
        }

        private void transparencyExamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/transparency.html");
        }

        private void offScreenRenderingAppExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/osrplugin.html");
        }

        private void localStorageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/localstorage.html");
        }

        private void javaScriptExtensionPerformanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/extensionperf.html");
        }

        private void menuDOM_Access_Click(object sender, EventArgs e)
        {
            _browserControl.Load("local://view/domaccess.html");
        }

        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_browserControl.IsLoading) _browserControl.Stop();
            _browserControl.Dispose();
            GC.Collect();
        }

        private void callJSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm("Call Function JS", "doAlert('Mr Thinh')");
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                string script = inputForm.GetInput() + ";return false;";
                _browserControl.Load("javascript:" + script);

                //try
                //{
                //    string result = _browserControl.RunScript(script, "about:blank", 0, 5000);
                //    MessageBox.Show(result, "Result");
                //}
                //catch (Exception err)
                //{
                //    MessageBox.Show(err.ToString(), "Error");
                //}
            }
        }
    }
}
