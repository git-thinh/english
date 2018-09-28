using CefSharp;
using System;
using System.IO;
using System.Text;

namespace lenBrowser
{
    public class CacheSchemeHandler : ISchemeHandler
    {
        readonly ICache cache;
        public CacheSchemeHandler(ICache cache) : base()
        {
            this.cache = cache;
        }

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string path = cache.getKeyByUrl(request.Url);
            Console.WriteLine("CACHE_REQUEST: " + path);

            if (cache.isExist(path))
            {
                if (path.Contains(".htm"))
                    mimeType = "text/html";
                else if (path.Contains(".css"))
                    mimeType = "text/css";
                else if (path.Contains(".js"))
                    mimeType = "text/javascript";
                else mimeType = "text/html";

                string body = cache.Get(path);
                int posH1 = body.ToLower().IndexOf("<h1");
                if (posH1 != -1) body = body.Substring(posH1, body.Length - posH1);

                string temp = File.ReadAllText("view/view.html");
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
        readonly ICache cache;
        public CacheSchemeHandlerFactory(ICache cache) : base()
        {
            this.cache = cache;
        }

        public ISchemeHandler Create()
        {
            return new CacheSchemeHandler(this.cache);
        }
    }
}