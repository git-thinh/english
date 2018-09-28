using CefSharp;
using System;
using System.IO;
using System.Text;

namespace lenBrowser
{
    public class HttpSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string url = request.Url;

            App.f_http_getSource(url);

            if (true)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(string.Empty);
                stream = new MemoryStream(bytes);
                return true;
            }
            return false;
        }
    }

    public class HttpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new HttpSchemeHandler();
        }
    }
}