using CefSharp;
using System;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace test
{
    public class API
    {
        readonly IApp _app;
        readonly IForm _form;
        public API(IApp app, IForm form)
        {
            this._form = form;
            this._app = app;
        }

        public bool f_main_openUrl(String url, String title)
        {
            return _app.f_main_openUrl(url, title);
        }

        public String f_link_getLinkPaging(String linkRequest)
        {
            oLinkRequest lr = JsonConvert.DeserializeObject<oLinkRequest>(linkRequest);
            oLinkResult rs = _app.f_link_getLinkPaging(lr);
            string s = JsonConvert.SerializeObject(rs);
            return s;
        }

        public void f_link_updateUrls(String jsonsUrls)
        {
            oLink[] links = JsonConvert.DeserializeObject<oLink[]>(jsonsUrls);
            //App.f_api_sendMessage(MSG_TYPE.URL_CACHE_FOR_SEARCH, jsonLinks);
            _app.f_link_updateUrls(links);
        }

        public void f_app_callFromJs(String data)
        {
            _app.f_app_callFromJs(data);
        }

        public String f_app_getInfo()
        {
            String s = JsonConvert.SerializeObject(_form.f_getInfo());
            return s;
        }

        public String f_file_Read(String file)
        {
            if (File.Exists(file)) return File.ReadAllText(file);
            return JsonConvert.SerializeObject(new { Ok = false, Message = "Cannot find the file: " + file });
        }

        public void f_file_Write(String file, String data)
        {
            if (File.Exists(file))
                File.WriteAllText(file, data);
        }
    }

    public class BrowserMenuHandel : IMenuHandler
    {
        public bool OnBeforeMenu(IWebBrowser browser) => true;
    }

    public class BrowserRequestHandler : IRequestHandler
    {
        readonly IForm _form;
        readonly IApp _app;
        string _url = string.Empty;

        static string view = string.Empty;
        static string view_end = string.Empty;

        public BrowserRequestHandler(IApp app, IForm form) : base()
        {
            this._form = form;
            this._app = app;
        }

        static BrowserRequestHandler()
        {
            if (File.Exists("view/view.html")) view = File.ReadAllText("view/view.html");
            if (File.Exists("view/view-end.html")) view_end = File.ReadAllText("view/view-end.html");
        }

        public static string buildPageHtml(string body, IForm form)
        {
            string formKey = form.f_get_formKey();
            return view + @"<script type=""text/javascript"" src=""/view/js/"+ formKey + @".js""></script><link type=""text/css"" href=""/view/css/" + formKey + @".css"" rel=""stylesheet"" />" +
                  "</head><body>" + body + view_end;
        }


        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password) => false;
        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler) => false;

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            string url = request.Url;
            if (url.StartsWith("chrome-devtools")) return false;
            _url = url;
            Console.WriteLine("|> " + url);
            return false;
        }

        bool _request_Fail(string url) { Console.WriteLine(" ?> " + url); return false; }

        void _writeFileLogURL(string url) => new Thread(() => File.WriteAllText("url.txt", url)).Start();

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            string url = request.Url;            
            if (request.Method != "GET") return _request_Fail(url);
            if (url.StartsWith("chrome-devtools")) return _request_Fail(url);
            if (url.StartsWith("http://api")) return _request_Fail(url);

            Uri uri = new Uri(url);
            bool isView = uri.Segments.Length > 1 && uri.Segments[1] == "view/";
            Console.WriteLine(" -> " + url);


            if (isView)
            {
                #region [ LOCAL IN VIEW]

                string path = uri.AbsolutePath.Replace('/', '\\').Substring(1).ToLower(), ext = path.Substring(path.Length - 3, 3);
                //Console.WriteLine("#> " + path);
                switch (ext)
                {
                    case "tml":
                        #region
                        if (!path.EndsWith(".html")) return false;
                        if (File.Exists(path))
                        {
                            //byte[] bytes = File.ReadAllBytes(path);
                            string html = File.ReadAllText(path);
                            html = buildPageHtml(html, _form);
                            byte[] bytes = Encoding.UTF8.GetBytes(html);
                            Stream resourceStream = new MemoryStream(bytes);
                            requestResponse.RespondWith(resourceStream, "text/html");
                            return false;
                        }
                        else
                        {
                            Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes("Cannot found page: " + path));
                            requestResponse.RespondWith(resourceStream, "text/html");
                            return false;
                        }
                        break;
                    #endregion
                    case ".js":
                        #region
                        if (File.Exists(path))
                        {
                            byte[] bytes = File.ReadAllBytes(path);
                            Stream resourceStream = new MemoryStream(bytes);
                            requestResponse.RespondWith(resourceStream, "text/javascript");
                            return false;
                        }
                        #endregion
                        break;
                    case "css":
                        #region 
                        if (File.Exists(path))
                        {
                            byte[] bytes = File.ReadAllBytes(path);
                            Stream resourceStream = new MemoryStream(bytes);
                            requestResponse.RespondWith(resourceStream, "text/css");
                            return false;
                        }
                        #endregion
                        break;
                    default:
                        #region 
                        //if (!path.EndsWith(".html")) return false;
                        //if (File.Exists(path))
                        //{
                        //    byte[] bytes = File.ReadAllBytes(path);
                        //    Stream resourceStream = new MemoryStream(bytes);
                        //    requestResponse.RespondWith(resourceStream, "text/html");
                        //    return true;
                        //}
                        #endregion
                        break;
                }

                #endregion
            }
            else
            {
                #region [ HTML FROM CURL ]

                if (url == _url)
                {
                    //Console.WriteLine(">> " + url);

                    string html = _app.f_link_getHtmlCache(url);
                    if (!string.IsNullOrEmpty(html))
                    {
                        html = buildPageHtml(html, _form);
                        Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                        requestResponse.RespondWith(resourceStream, "text/html");
                        _writeFileLogURL(url);
                        return false;
                    }
                    else
                    {
                        //html = "<br><br><br><br><br><center><img src='data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PHN2ZyB4bWxuczpzdmc9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjAiIHdpZHRoPSI2NHB4IiBoZWlnaHQ9IjY0cHgiIHZpZXdCb3g9IjAgMCAxMjggMTI4IiB4bWw6c3BhY2U9InByZXNlcnZlIj48Zz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjMDAwIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoNDUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoOTAgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoMTM1IDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiNiZWJlYmUiIHRyYW5zZm9ybT0icm90YXRlKDE4MCA2NCA2NCkiLz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjOTc5Nzk3IiB0cmFuc2Zvcm09InJvdGF0ZSgyMjUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iIzZlNmU2ZSIgdHJhbnNmb3JtPSJyb3RhdGUoMjcwIDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiMzYzNjM2MiIHRyYW5zZm9ybT0icm90YXRlKDMxNSA2NCA2NCkiLz48YW5pbWF0ZVRyYW5zZm9ybSBhdHRyaWJ1dGVOYW1lPSJ0cmFuc2Zvcm0iIHR5cGU9InJvdGF0ZSIgdmFsdWVzPSIwIDY0IDY0OzQ1IDY0IDY0OzkwIDY0IDY0OzEzNSA2NCA2NDsxODAgNjQgNjQ7MjI1IDY0IDY0OzI3MCA2NCA2NDszMTUgNjQgNjQiIGNhbGNNb2RlPSJkaXNjcmV0ZSIgZHVyPSI3MjBtcyIgcmVwZWF0Q291bnQ9ImluZGVmaW5pdGUiPjwvYW5pbWF0ZVRyYW5zZm9ybT48L2c+PGc+PGNpcmNsZSBmaWxsPSIjMDAwIiBjeD0iNjMuNjYiIGN5PSI2My4xNiIgcj0iMTIiLz48YW5pbWF0ZSBhdHRyaWJ1dGVOYW1lPSJvcGFjaXR5IiBkdXI9IjcyMG1zIiBiZWdpbj0iMHMiIHJlcGVhdENvdW50PSJpbmRlZmluaXRlIiBrZXlUaW1lcz0iMDswLjU7MSIgdmFsdWVzPSIxOzA7MSIvPjwvZz48L3N2Zz4='/></center>";
                        //Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                        //requestResponse.RespondWith(resourceStream, "text/html");

                        html = _app.f_link_getHtmlOnline(url);
                        if (string.IsNullOrEmpty(html))
                        {
                            html = "<h1>Cannot find: " + url + "</h1>";
                        }
                        else
                        {
                            html = buildPageHtml(html, _form);
                            _writeFileLogURL(url);
                        }

                        Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(html));
                        requestResponse.RespondWith(resourceStream, "text/html");

                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("-ERROR->" + url);
                    return false;
                }
                #endregion
            }

            //var headers = request.GetHeaders();
            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers) { }
    }
}
