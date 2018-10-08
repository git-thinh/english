// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.WinForms.Internals;
using CefSharp.WinForms;
using CefSharp;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    public partial class SimpleBrowserForm : Form, IRequestHandler
    {
        private ChromiumWebBrowser browser;
         
        public SimpleBrowserForm()
        {
            InitializeComponent();

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);
            DisplayOutput(version);

            //Only perform layout when control has completly finished resizing
            ResizeBegin += (s, e) => SuspendLayout();
            ResizeEnd += (s, e) => ResumeLayout(true);

            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            CreateBrowser();
        }

        private void CreateBrowser()
        {
            browser = new ChromiumWebBrowser("www.google.com")
            {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.LoadingStateChanged += OnBrowserLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;

            browser.RegisterJsObject("bound", new BoundObject());

            browser.RequestHandler = this;
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnBrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            //goButton.Text = isLoading ?
            //    "Stop" :
            //    "Go";
            //goButton.Image = isLoading ?
            //    Properties.Resources.nav_plain_red :
            //    Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                browser.Load(url);
            }
        }

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            return false;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //var headers = request.Headers;
            //headers["Custom-Header"] = "My Custom Header";
            //request.Headers = headers;

            //return CefReturnValue.Continue;

            //throw new NotImplementedException(); 
            //IRequest request = requestResponse.Request;
            //if (request.Url.StartsWith("http://test/resource/load"))
            //{
            //    Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes("<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
            //    //requestResponse.RespondWith(resourceStream, "text/html");

            //}
            return CefReturnValue.Continue;

            //string url = request.Url.ToLower();
            //if (url.Contains(".js")) {
            //    Debug.WriteLine("----> CANCEL: " + url);
            //    requestResponse.Cancel();
            //}else
            //    Debug.WriteLine(url);
        }

        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //if (host.EndsWith("the-shire.org"))
            //{
            //    username = "Frodo";
            //    password = "theR1nG";
            //    return true;
            //}
            //return false;

            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return false;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
            //string url = request.Url;
            //Debug.WriteLine("----> " + url);
            //if (url == "http://www.google.com/")
            //    newUrl = "https://vnexpress.net/";
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return false;
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            string url = request.Url;
            Debug.WriteLine("----> " + url);
            if (url.Contains("www.google.com"))
            {
                response.StatusCode = 301;
                response.ResponseHeaders.Add("Location", "https://vnexpress.net/");
                
                return true;
            }

            return false;

            //throw new NotImplementedException(); 
            //IRequest request = requestResponse.Request;
            //if (request.Url.StartsWith("http://test/resource/load"))
            //{
            //    Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes("<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
            //    //requestResponse.RespondWith(resourceStream, "text/html");
            //    response.
            //    return true;
            //}

            //string url = request.Url.ToLower();
            //if (url.Contains(".js")) {
            //    Debug.WriteLine("----> CANCEL: " + url);
            //    requestResponse.Cancel();
            //}else
            //    Debug.WriteLine(url);
            
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
        }
    }

    //public class RequestHandler : IRequestHandler
    //{
    //    public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
    //    {
    //        IDictionary<string, string> headers = requestResponse.Request.GetHeaders();
    //        headers.Add("Accept-Language", "zh,zh-cn,zh-tw");
    //        requestResponse.Request.SetHeaders(headers);
    //        return false;
    //    }
    //}
}
