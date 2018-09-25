using System;
using System.IO;
using System.Text;
using CefSharp;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    public class TestSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            if(request.Url.EndsWith("SchemeTest.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.SchemeTest);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("BindingTest.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.BindingTest);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("modalmain.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.modalmain);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("modaldialog.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.modaldialog);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("Transparency.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.transparency);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("xmlhttprequest.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.xmlhttprequest);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("uiplugin.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.uiplugin);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("osrplugin.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.osrplugin);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("localstorage.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.localstorage);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("extensionperf.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.extensionperf);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("domaccess.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.domaccess);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
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