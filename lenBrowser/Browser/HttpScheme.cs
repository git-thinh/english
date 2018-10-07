using CefSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lenBrowser
{
    public class HttpSchemeHandler : ISchemeHandler
    {
        static string view = File.ReadAllText("view/view.html");
        static string view_end = File.ReadAllText("view/view-end.html");
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            mimeType = "text/html";
            string htm = File.ReadAllText("view/article.html");
            byte[] bytes = Encoding.UTF8.GetBytes(htm);
            stream = new MemoryStream(bytes);
            return true;


            //string url = request.Url;
            //Debug.WriteLine("BROWSER: -> " + url);
            //byte[] buf = App.f_api_sendMessage(MSG_TYPE.URL_GET_SOURCE_FROM_CACHE, url);
            //if (buf.Length > 0)
            //{
            //    mimeType = "text/html";
            //    string body = Encoding.UTF8.GetString(buf);
            //    string htm = view + "</head><body>" + body + view_end;
            //    File.WriteAllText("view/article-test.html", htm);
            //    byte[] bytes = Encoding.UTF8.GetBytes(htm);
            //    stream = new MemoryStream(bytes);

            //    return true;
            //}

            //else
            //{
            //    string ext = url.ToLower().Substring(url.Length - 3, 3);
            //    if (ext == "jpg" || ext == "gif" || ext == "png" || ext == "peg" || ext == "svg") { }
            //    else
            //        App.f_api_sendMessage(IpcMsgType.URL_REQUEST, url);
            //}

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