using CefSharp;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lenBrowser
{
    public class CacheSchemeHandler : ISchemeHandler
    {
        public CacheSchemeHandler() : base()
        {
        }
        
        static string view = File.ReadAllText("view/view.html");
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string path = request.Url;
            Console.WriteLine("-> REQUEST: " + path);
            if (path.Length > 8) path = path.Substring(8);

            if (File.Exists(path) && path.EndsWith(".txt"))
            {
                mimeType = "text/html";
                string body = string.Empty;

                //string view = File.ReadAllText("view/view.html");
                using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(reader, Encoding.UTF8))
                {
                    body = sr.ReadToEnd();
                    sr.Close();
                    reader.Close();
                }

                string htm = view + body + "</body></html>";
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