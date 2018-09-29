using CefSharp;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lenBrowser
{
    public class HttpSchemeHandler : ISchemeHandler
    {
        static string f_html_getDomainByUrl(string url)
        {
            string domain = url.Split('/')[2].Replace(':', '-').ToLower();
            if (domain.StartsWith("www.")) domain = domain.Substring(4);
            return domain;
        }

        static string f_html_getNameFileByUrl(string url)
        {
            string domain = f_html_getDomainByUrl(url);
            string fn = url.Substring(domain.Length + 8).Replace('/', '_');

            if (fn.EndsWith(".php") || fn.EndsWith(".jsp")) fn = fn.Substring(0, fn.Length - 4);
            else if (fn.EndsWith(".aspx") || fn.EndsWith(".html")) fn = fn.Substring(0, fn.Length - 5);

            if (fn[0] == '_') fn = fn.Substring(1);
            if (fn[fn.Length - 1] == '_') fn = fn.Substring(0, fn.Length - 1);

            fn = Regex.Replace(fn, @"[^A-Za-z0-9-_]+", string.Empty);
            return fn;
        }

        static string f_html_getPathFileByUrl(string url)
        {
            string domain = f_html_getDomainByUrl(url),
                   fn = f_html_getNameFileByUrl(url),
                   file = "cache/" + domain + "/" + fn + ".txt";
            return file;
        }

        static string view = File.ReadAllText("view/view.html");

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string url = request.Url,
                path = f_html_getPathFileByUrl(url);
            Console.WriteLine("-> " + url);

            if (File.Exists(path))
            {
                mimeType = "text/html";
                //string body = string.Empty;

                //string view = File.ReadAllText("view/view.html");
                string body = File.ReadAllText(path);
                //using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //using (StreamReader sr = new StreamReader(reader, Encoding.UTF8))
                //{
                //    body = sr.ReadToEnd();
                //    sr.Close();
                //    reader.Close();
                //}

                string htm = view + body + "</body></html>";
                byte[] bytes = Encoding.UTF8.GetBytes(htm);
                stream = new MemoryStream(bytes);

                return true;
            }
            else
            {
                string ext = url.ToLower().Substring(url.Length - 3, 3);
                if (ext == "jpg" || ext == "gif" || ext == "png" || ext == "peg") {

                }
                else
                    App.f_http_getSource(url);
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