using CefSharp;
using System;
using System.IO;
using System.Text;

namespace test
{
    public class SettingSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            Uri uri = new Uri(request.Url);
            string path = uri.LocalPath;
            if (path[0] == '/') path = uri.Host;

            if (path == "")
            {
                Console.Clear();
                path = "view/setting.html";
            }
            else path = "view/" + path;

            Console.WriteLine(request.Url + " -> " + path);

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

                byte[] bytes = File.ReadAllBytes(path);
                stream = new MemoryStream(bytes);
                return true;
            }

            //if(request.Url.EndsWith("SchemeTest.html", StringComparison.OrdinalIgnoreCase))
            //{
            //    //byte[] bytes = Encoding.UTF8.GetBytes(Resources.SchemeTest);
            //    //stream = new MemoryStream(bytes);
            //    //mimeType = "text/html";
            //    //return true;
            //}

            return false;
        }
    }

    public class SettingSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new SettingSchemeHandler();
        }
    }
}