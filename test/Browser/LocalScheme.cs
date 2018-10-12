using CefSharp;
using System;
using System.IO;
using System.Text;

namespace test
{
    public class LocalSchemeHandler : ISchemeHandler
    {
        //static string admin = File.ReadAllText("view/admin.html");
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            Uri uri = new Uri(request.Url);
            string path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
            if (!path.EndsWith(".html")) return false;

            Console.WriteLine("LOCAL -> " + request.Url);

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
            
            return false;
        }
    }

    public class LocalSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new LocalSchemeHandler();
        }
    }
}