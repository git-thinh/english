using CefSharp;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace test
{
    //public class RequestHandler : IRequestHandler
    //{
    //    public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ApiHandler : ISchemeHandler //, CefSharp.IRequestResponse
    {
        readonly IApp _app;
        public ApiHandler(IApp app) : base() => this._app = app;

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string url = request.Url, input = string.Empty;
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            Console.WriteLine("API -> " + path);
            byte[] bytes;
            int len = 0;
            mimeType = "application/json";

            switch (path)
            {
                case "/":
                    mimeType = "text/plain";
                    bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                    stream = new MemoryStream(bytes);
                    return true;
                case "/link/test":
                    var headers = request.GetHeaders();

                    bytes = new byte[1024 * 5];
                    len = stream.Read(bytes, 0, bytes.Length);
                    input = Encoding.UTF8.GetString(bytes, 0, len);
                    oLinkRequest lr = JsonConvert.DeserializeObject<oLinkRequest>(input);
                    oLinkResult rs = _app.f_link_getLinkPaging(lr);
                    string s = JsonConvert.SerializeObject(rs);
                    bytes = Encoding.UTF8.GetBytes(s);
                    stream = new MemoryStream(bytes);
                    return true;
                case "/link/list":
                    mimeType = "application/json";

                    bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                    stream = new MemoryStream(bytes);
                    return true;
                default:
                    break;
            }

            return false;
        }
    }

    public class ApiHandlerFactory : ISchemeHandlerFactory
    {
        readonly IApp _app;
        public ApiHandlerFactory(IApp app) : base() => this._app = app;

        public ISchemeHandler Create()
        {
            return new ApiHandler(this._app);
        }
    }
}