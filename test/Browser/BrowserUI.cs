using CefSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace test
{
    public class BrowserUI : IRequestHandler, ICookieVisitor
    {
        public static void Init()
        {
            Settings settings = new Settings();
            if (CEF.Initialize(settings))
            {
                //CEF.RegisterScheme("test", new SchemeHandlerFactory());
                //CEF.RegisterJsObject("bound", new BoundObject());
            }
        }

        public static void Exit()
        {
            //model.Dispose();
            CEF.Shutdown();
            //System.Environment.Exit(0);
        }

        private readonly IWebBrowser model;
        private readonly IBrowserView view;
        private readonly Action<Action> gui_invoke;
        readonly string _URL = "about:blank";

        public BrowserUI(string url, IWebBrowser model, IBrowserView view, Action<Action> gui_invoke)
        {
            this._URL = url;
            this.model = model;
            this.view = view;
            this.gui_invoke = gui_invoke;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", CEF.ChromiumVersion, CEF.CefVersion, CEF.CefSharpVersion);
            view.DisplayOutput(version);

            model.RequestHandler = this;
            model.PropertyChanged += model_PropertyChanged;
            model.ConsoleMessage += model_ConsoleMessage;

            // file
            view.ShowDevToolsActivated += view_ShowDevToolsActivated;
            view.CloseDevToolsActivated += view_CloseDevToolsActivated;
            view.ExitActivated += view_ExitActivated;

            // navigation
            view.UrlActivated += view_UrlActivated;
            view.ForwardActivated += view_ForwardActivated;
            view.BackActivated += view_BackActivated;
        }

        private void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string @string = null;
            bool @bool = false;

            Debug.WriteLine(" = " + e.PropertyName);

            switch (e.PropertyName)
            {
                case "IsBrowserInitialized":
                    if (model.IsBrowserInitialized)
                    {
                        model.Load(_URL);
                    }
                    break;
                case "Title":
                    @string = model.Title;
                    gui_invoke(() => view.SetTitle(@string));
                    break;
                case "Address":
                    @string = model.Address;
                    gui_invoke(() => view.SetAddress(@string));
                    break;
                case "CanGoBack":
                    @bool = model.CanGoBack;
                    gui_invoke(() => view.SetCanGoBack(@bool));
                    break;
                case "CanGoForward":
                    @bool = model.CanGoForward;
                    gui_invoke(() => view.SetCanGoForward(@bool));
                    break;
                case "IsLoading":
                    @bool = model.IsLoading;
                    gui_invoke(() => view.SetIsLoading(@bool));
                    break;
            }
        }

        private void model_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            gui_invoke(() => view.DisplayOutput(e.Message));
        }

        private void view_ShowDevToolsActivated(object sender, EventArgs e)
        {
            model.ShowDevTools();
        }

        private void view_CloseDevToolsActivated(object sender, EventArgs e)
        {
            model.CloseDevTools();
        }

        private void view_ExitActivated(object sender, EventArgs e)
        {
            Exit();
        }
        
        private void view_UrlActivated(object sender, string url)
        {
            model.Load(url);
        }

        private void view_BackActivated(object sender, EventArgs e)
        {
            model.Back();
        }

        private void view_ForwardActivated(object sender, EventArgs e)
        {
            model.Forward();
        }

        #region IRequestHandler Members

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            return false;
        }

        bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith("XXXX"))
            {
                Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                requestResponse.RespondWith(resourceStream, "text/html");
            }

            return false;
        }

        void IRequestHandler.OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers)
        {

        }

        bool IRequestHandler.GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
        {
            //handler = new DownloadHandler(fileName);
            return true;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        #endregion

        #region ICookieVisitor Members

        bool ICookieVisitor.Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            Console.WriteLine("Cookie #{0}: {1}", count, cookie.Name);
            return true;
        }

        #endregion
    }
}
