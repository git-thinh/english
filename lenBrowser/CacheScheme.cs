using CefSharp;
using System;
using System.IO;
using System.Text;

namespace lenBrowser
{
    public class CacheSchemeHandler : ISchemeHandler
    {
        public CacheSchemeHandler() : base()
        {
        }

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string path = request.Url;
            Console.WriteLine("CACHE_REQUEST: " + path);
            if(path.Length > 8) path = path.Substring(8);

            if (File.Exists(path) && path.EndsWith(".txt"))
            {                
                mimeType = "text/html";

                string temp = File.ReadAllText("view/view.html"),
                    body = File.ReadAllText(path);
                string htm = temp + body + "</body></html>";

                byte[] bytes = Encoding.UTF8.GetBytes(htm);
                stream = new MemoryStream(bytes);
                return true;
            }

            return false;
        }
    }

    public class CacheSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public CacheSchemeHandlerFactory() : base()
        {
        }
        public ISchemeHandler Create()
        {
            return new CacheSchemeHandler();
        }
    }
}