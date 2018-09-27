using CefSharp;
using System;
using System.IO;
using System.Text;

namespace lenBrowser
{
    public class TestSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            Uri uri = new Uri(request.Url);
            string path = uri.LocalPath;
            if (path[0] == '/') path = path.Substring(1);

            if (path == "")
            {
                Console.Clear();
                path = "view/test/index.html";
            }
            else path = "view/test/" + path;

            Console.WriteLine(path);

            if (File.Exists(path))
            {
                string ext = path.Substring(path.Length - 3, 3);
                switch (ext)
                {
                    case "tml":
                        mimeType = "text/html";
                        break;
                    case ".js":
                        mimeType = "text/javascript";
                        break;
                    case "css":
                        mimeType = "text/css";
                        break;
                }

                string body = File.ReadAllText(path);
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

    public class TestSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new TestSchemeHandler();
        }
    }
}