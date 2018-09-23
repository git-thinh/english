using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace appui
{
    class MySchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new MySchemeHandler();
        }
    }

    // very-very-very simple stub
    class MySchemeHandler : CefResourceHandler
    {
        private static int _no;
        private bool _completed;

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            string url = request.Url;
            Debug.WriteLine(url);

            callback.Continue();
            return true;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            ////    ////var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            ////    ////headers.Add("Cache-Control", "private");
            ////    ////response.SetHeaderMap(headers);

            string url = response.Url;

            response.Status = 200;
            response.MimeType = "text/html";
            response.StatusText = "OK";
            responseLength = -1; // unknown content-length
            redirectUrl = null;  // no-redirect
        }

        protected override bool ReadResponse(System.IO.Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (_completed)
            {
                bytesRead = 0;
                return false;
            }
            else
            {
                _no++;
                bytesRead = (int)response.Length;


                ////////// very simple response with one block
                ////////var content = Encoding.UTF8.GetBytes("Response #" + _no.ToString());
                ////////if (bytesToRead < content.Length) throw new NotImplementedException(); // oops
                ////////response.Write(content, 0, content.Length);
                ////////bytesRead = content.Length;

                _completed = true;

                return true;
            }
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void Cancel()
        {
        }
    }







































    internal sealed class CefApplication : CefApp
    {
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return new RenderProcessHandlerJS();
        }
    }

    internal sealed class RenderProcessHandlerJS : CefRenderProcessHandler
    {
        static string js = File.ReadAllText("bin/api.js");
        protected override void OnWebKitInitialized()
        {

            //CefRuntime.RegisterExtension("testExtension", "var test; if (!test) test = {}; (function() { test.myval = 'My Value!'; })(); ", null);
            //CefRuntime.RegisterExtension("___api", js, new V8Handler());
            CefRuntime.RegisterExtension("___api", js, null);

            base.OnWebKitInitialized();
        }
    }

    internal sealed class TestDumpRequestHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new TestDumpRequestHandler2();
        }
    }

    internal sealed class TestDumpRequestHandler2 : CefResourceHandler
    {
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            string url = request.Url;
            Debug.WriteLine(url);
            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = -1;
            redirectUrl = null;
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            bytesRead = 0;
            return false;
        }

        protected override bool CanGetCookie(CefCookie cookie) { return false; }
        protected override bool CanSetCookie(CefCookie cookie) { return false; }
        protected override void Cancel() { }
    }

    internal sealed class TestDumpRequestHandler : CefResourceHandler
    {
        private static int _requestNo;
        private byte[] responseData;
        private int pos;


        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            string url = request.Url;
            Debug.WriteLine(url);
            callback.Continue();
            return true;

            ////var requestNo = Interlocked.Increment(ref _requestNo);
            ////var response = new StringBuilder();

            ////response.AppendFormat("<pre>\n");
            ////response.AppendFormat("Requests processed by DemoAppResourceHandler: {0}\n", requestNo);

            ////response.AppendFormat("Method: {0}\n", request.Method);
            ////response.AppendFormat("URL: {0}\n", request.Url);

            ////response.AppendLine();
            ////response.AppendLine("Headers:");
            ////var headers = request.GetHeaderMap();
            ////foreach (string key in headers)
            ////{
            ////    foreach (var value in headers.GetValues(key))
            ////    {
            ////        response.AppendFormat("{0}: {1}\n", key, value);
            ////    }
            ////}
            ////response.AppendLine();

            ////response.AppendFormat("</pre>\n");

            ////responseData = Encoding.UTF8.GetBytes(response.ToString());

            ////callback.Continue();
            ////return true;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = 0;
            redirectUrl = null;
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            bytesRead = 0;
            return false;
        }

        ////protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        ////{
        ////    ////response.MimeType = "text/html";
        ////    ////response.Status = 200;
        ////    ////response.StatusText = "OK, hello from handler!";

        ////    ////var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
        ////    ////headers.Add("Cache-Control", "private");
        ////    ////response.SetHeaderMap(headers);

        ////    //responseLength = responseData.LongLength;
        ////    //redirectUrl = null;
        ////}

        ////protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        ////{
        ////    bytesRead = 0;
        ////    return false;

        ////    ////if (bytesToRead == 0 || pos >= responseData.Length)
        ////    ////{
        ////    ////    bytesRead = 0;
        ////    ////    return false;
        ////    ////}
        ////    ////else
        ////    ////{
        ////    ////    response.Write(responseData, pos, bytesToRead);
        ////    ////    pos += bytesToRead;
        ////    ////    bytesRead = bytesToRead;
        ////    ////    return true;
        ////    ////}
        ////}

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void Cancel()
        {
        }

    }

    ////class ReaderResourceHandler : CefResourceHandler
    ////{

    ////    private bool _completed;
    ////    private string _url;
    ////    private string _resourceFolder = "res";
    ////    private long _pos, _resLength;
    ////    private byte[] _content;

    ////    protected override bool ProcessRequest(CefRequest request, CefCallback callback)
    ////    {
    ////        _url = request.Url;
    ////        callback.Continue();
    ////        return true;

    ////    }

    ////    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    ////    {
    ////        ////////response.SetHeaderMap();
    ////        //////response.Status = 200;
    ////        //////response.MimeType = getMimeType();
    ////        //////response.StatusText = "OK";
    ////        //////responseLength = -1; // unknown content-length
    ////        //////redirectUrl = null;  // no-redirect
    ////    }

    ////    protected override bool ReadResponse(System.IO.Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
    ////    {
    ////        //if (_completed)
    ////        //{
    ////        //    bytesRead = 0;
    ////        //    return false;
    ////        //}
    ////        //else
    ////        //{
    ////        //    //var content = ReadFromFile();
    ////        //    var content = Guid.NewGuid().ToString();
    ////        //    response.Write(content, 0, content.Length);
    ////        //    bytesRead = content.Length;
    ////        //    _completed = true;
    ////        //    return true;
    ////        //}


    ////        return false;
    ////    }

    ////    protected override bool CanGetCookie(CefCookie cookie)
    ////    {
    ////        return true;
    ////    }

    ////    protected override bool CanSetCookie(CefCookie cookie)
    ////    {
    ////        return true;
    ////    }

    ////    protected override void Cancel()
    ////    {

    ////    }

    ////    ////private Byte[] ReadFromFile()
    ////    ////{
    ////    ////    var _fileName = getFileName();
    ////    ////    if (!String.IsNullOrEmpty(_fileName))
    ////    ////    {
    ////    ////        var _filePath = Application.StartupPath + "\\" + _resourceFolder + "\\" + getFileName();
    ////    ////        return File.ReadAllBytes(_filePath);
    ////    ////    }
    ////    ////    else
    ////    ////    {
    ////    ////        return null;
    ////    ////    }
    ////    ////}

    ////    ////private string getFileName()
    ////    ////{
    ////    ////    var _url_splitted = _url.Split(new String[] { "http://test-local.com/" }, StringSplitOptions.None);
    ////    ////    return (_url_splitted.Length > 0) ? _url_splitted[1] : null;
    ////    ////}

    ////    //private string getMimeType()
    ////    //{
    ////    //    var _url_splitted = _url.Split('.');
    ////    //    return _objUtils.GetMimeType(_url_splitted[_url_splitted.Length - 1]);
    ////    //}
    ////}
}
