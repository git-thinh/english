using CefSharp;
using System;
using System.IO;
using System.Text;

namespace test
{
    public class ApiHandler : ISchemeHandler
    {
        readonly IApp _app;
        public ApiHandler(IApp app) : base() => this._app = app;

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string url = request.Url;
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            Console.WriteLine("API -> " + path);

            switch (path)
            {
                case "/":
                    #region

                    mimeType = "text/plain";
                    byte[] bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                    stream = new MemoryStream(bytes);
                    return true;

                #endregion
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