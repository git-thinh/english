using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace test
{
    public class HandelMenuBrowser : IMenuHandler
    {
        readonly IApp _app;
        public HandelMenuBrowser(IApp app) : base()
        {
            this._app = app;
        }

        public bool OnBeforeMenu(IWebBrowser browser)
        {
            return true;
        }
    }

    public class BrowserRequestHandler : IRequestHandler
    {
        readonly IApp _app;
        public BrowserRequestHandler(IApp app) : base()
        {
            this._app = app;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password) => false;
        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler) => false;

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect) {
            //Console.WriteLine("-> " + naigationvType.ToString() + ": " + request.Url);
            browser.TooltipText = request.Url;
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            string url = request.Url, _url = browser.TooltipText;
            if (url == _url)
            {
                Console.WriteLine("-->" + url);

                string html = _app.f_link_getHtml(url);
                if (!string.IsNullOrEmpty(html))
                {
                    Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                    requestResponse.RespondWith(resourceStream, "text/html");
                    return false;
                }
                else
                {
                    html = "<br><br><br><br><br><center><img src='data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PHN2ZyB4bWxuczpzdmc9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjAiIHdpZHRoPSI2NHB4IiBoZWlnaHQ9IjY0cHgiIHZpZXdCb3g9IjAgMCAxMjggMTI4IiB4bWw6c3BhY2U9InByZXNlcnZlIj48Zz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjMDAwIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoNDUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoOTAgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoMTM1IDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiNiZWJlYmUiIHRyYW5zZm9ybT0icm90YXRlKDE4MCA2NCA2NCkiLz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjOTc5Nzk3IiB0cmFuc2Zvcm09InJvdGF0ZSgyMjUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iIzZlNmU2ZSIgdHJhbnNmb3JtPSJyb3RhdGUoMjcwIDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiMzYzNjM2MiIHRyYW5zZm9ybT0icm90YXRlKDMxNSA2NCA2NCkiLz48YW5pbWF0ZVRyYW5zZm9ybSBhdHRyaWJ1dGVOYW1lPSJ0cmFuc2Zvcm0iIHR5cGU9InJvdGF0ZSIgdmFsdWVzPSIwIDY0IDY0OzQ1IDY0IDY0OzkwIDY0IDY0OzEzNSA2NCA2NDsxODAgNjQgNjQ7MjI1IDY0IDY0OzI3MCA2NCA2NDszMTUgNjQgNjQiIGNhbGNNb2RlPSJkaXNjcmV0ZSIgZHVyPSI3MjBtcyIgcmVwZWF0Q291bnQ9ImluZGVmaW5pdGUiPjwvYW5pbWF0ZVRyYW5zZm9ybT48L2c+PGc+PGNpcmNsZSBmaWxsPSIjMDAwIiBjeD0iNjMuNjYiIGN5PSI2My4xNiIgcj0iMTIiLz48YW5pbWF0ZSBhdHRyaWJ1dGVOYW1lPSJvcGFjaXR5IiBkdXI9IjcyMG1zIiBiZWdpbj0iMHMiIHJlcGVhdENvdW50PSJpbmRlZmluaXRlIiBrZXlUaW1lcz0iMDswLjU7MSIgdmFsdWVzPSIxOzA7MSIvPjwvZz48L3N2Zz4='/></center>";
                    Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                    requestResponse.RespondWith(resourceStream, "text/html");

                    html = _app.f_link_fetchHtmlOnline(url);
                    resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                    requestResponse.RespondWith(resourceStream, "text/html");

                    return false;
                }
            }
            else
            {
                Console.WriteLine("xx>" + url);
                return true;
            }

            //var headers = request.GetHeaders();
            //return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers) { }
    }

    public class LocalSchemeHandler : ISchemeHandler
    {
        readonly IApp _app;
        public LocalSchemeHandler(IApp app) : base()
        {
            this._app = app;
        }

        //static string admin = File.ReadAllText("view/admin.html");
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string url = request.Url, ext = url.ToLower().Split('?')[0].Split('#')[0], path = string.Empty;
            ext = ext.Substring(ext.Length - 3, 3);
            Uri uri = new Uri(url);
            Console.WriteLine("LOCAL -> " + url);
            switch (ext)
            {
                case "tml":
                    #region
                    path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                    if (!path.EndsWith(".html")) return false;
                    if (File.Exists(path))
                    {
                        mimeType = "text/html";

                        //string css = File.ReadAllText(path.Substring(0, path.Length - 4) + ".css"),
                        //    html = File.ReadAllText(path);
                        //byte[] bytes = Encoding.UTF8.GetBytes(admin + html + @" <style>  " + css + " </style> <script> if(window['f_ready']) window['f_ready']() </script> </body></html>");

                        byte[] bytes = File.ReadAllBytes(path);
                        stream = new MemoryStream(bytes);
                        return true;
                    }
                    #endregion
                    break;
                case ".js":
                    #region
                    if (url.ToLower().StartsWith("local://view/js/"))
                    {
                        path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');

                        if (File.Exists(path))
                        {
                            mimeType = "text/javascript";

                            //string css = File.ReadAllText(path.Substring(0, path.Length - 4) + ".css"),
                            //    html = File.ReadAllText(path);
                            //byte[] bytes = Encoding.UTF8.GetBytes(admin + html + @" <style>  " + css + " </style> <script> if(window['f_ready']) window['f_ready']() </script> </body></html>");

                            byte[] bytes = File.ReadAllBytes(path);
                            stream = new MemoryStream(bytes);
                            return true;
                        }
                    }
                    #endregion
                    break;
                case "css":
                    #region
                    if (url.ToLower().StartsWith("local://view/css/"))
                    {
                        path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                        if (File.Exists(path))
                        {
                            mimeType = "text/css";
                            byte[] bytes = File.ReadAllBytes(path);
                            stream = new MemoryStream(bytes);
                            return true;
                        }
                    }
                    #endregion
                    break;
                default:
                    #region
                    path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                    if (!path.EndsWith(".html")) return false;
                    if (File.Exists(path))
                    {
                        mimeType = "text/html";

                        //string css = File.ReadAllText(path.Substring(0, path.Length - 4) + ".css"),
                        //    html = File.ReadAllText(path);
                        //byte[] bytes = Encoding.UTF8.GetBytes(admin + html + @" <style>  " + css + " </style> <script> if(window['f_ready']) window['f_ready']() </script> </body></html>");

                        byte[] bytes = File.ReadAllBytes(path);
                        stream = new MemoryStream(bytes);
                        return true;
                    }
                    #endregion
                    break;
            }

            return false;
        }
    }

    public class LocalSchemeHandlerFactory : ISchemeHandlerFactory
    {
        readonly IApp _app;
        public LocalSchemeHandlerFactory(IApp app) : base()
        {
            this._app = app;
        }

        public ISchemeHandler Create()
        {
            return new LocalSchemeHandler(this._app);
        }
    }
}
