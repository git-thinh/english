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
            string url = request.Url, ext = url.ToLower().Split('?')[0].Split('#')[0], path = string.Empty;
            ext = ext.Substring(ext.Length - 3, 3);
            Uri uri = new Uri(url);
            Console.WriteLine("LOCAL -> " + url);
            switch (ext)
            {
                case "tml":
                    #region
                    path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                    if (!path.EndsWith(".html")) return false;
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
                    #endregion
                    break;
                case ".js":
                    #region
                    if (url.ToLower().StartsWith("local://view/js/"))
                    {
                        path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');

                        if (File.Exists(path))
                        {
                            mimeType = "text/javascript";

                            //string css = File.ReadAllText(path.Substring(0, path.Length - 4) + ".css"),
                            //    html = File.ReadAllText(path);
                            //byte[] bytes = Encoding.UTF8.GetBytes(admin + html + @" <style>  " + css + " </style> <script> if(window['f_ready']) window['f_ready']() </script> </body></html>");

                            byte[] bytes = File.ReadAllBytes(path);
                            stream = new MemoryStream(bytes);
                            return true;
                        }
                    }
                    #endregion
                    break;
                case "css":
                    #region
                    if (url.ToLower().StartsWith("local://view/css/"))
                    {
                        path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                        if (File.Exists(path))
                        {
                            mimeType = "text/css";
                            byte[] bytes = File.ReadAllBytes(path);
                            stream = new MemoryStream(bytes);
                            return true;
                        }
                    }
                    #endregion
                    break;
                default:
                    #region
                    path = "view" + uri.LocalPath.ToLower().Replace('/', '\\');
                    if (!path.EndsWith(".html")) return false;
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
                    #endregion
                    break;
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