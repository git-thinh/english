using System;
using System.Collections.Generic;
using System.Text;
using Xilium.CefGlue;

namespace appui
{
    //public class ClickCefDomEventListener : BaseCefDomEventListener
    //{
    //    public string Value { get; private set; }
    //    public ClickCefDomEventListener() { }
    //    public ClickCefDomEventListener(string @event, string bindid) : this()
    //    {
    //        this.BindElementID = bindid;
    //        this.ListenerEvent = @event;
    //    }
    //    protected override void HandleEvent(CefDomEvent @event)
    //    {
    //        @event.Document.GetElementById("").AddEventListener("onclick") //binding click 
    //    }
    //}
    //tb.WebBrowser.Browser.GetMainFrame().VisitDom(new ClickCefDomEventListener());
    //@event.Document.GetElementById("").AddEventListener()


    internal sealed class CefApplication : CefApp
    {
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return new RenderProcessHandlerJS();
        }
    }

    internal sealed class RenderProcessHandlerJS : CefRenderProcessHandler
    {
        protected override void OnWebKitInitialized()
        {
            CefRuntime.RegisterExtension("testExtension", "var test;if (!test)test = {};(function() {test.myval = 'My Value!';})();", null);
            base.OnWebKitInitialized();
        }

        //protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        //{
        //    if (message.Name == "GetHackerNewsTitles")
        //    {
        //        CefFrame mainFrame = browser.GetMainFrame();
        //        mainFrame.VisitDom(new DemoCefDomVisitor());
        //        return true;
        //    }
        //    return false;
        //}
    }

    internal class DemoCefRenderHandler : CefRenderHandler
    {
        private readonly int _windowHeight;
        private readonly int _windowWidth;

        public DemoCefRenderHandler(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }

        protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
        {
            return GetViewRect(browser, ref rect);
        }

        protected override bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            screenX = viewX;
            screenY = viewY;
            return true;
        }

        protected override bool GetViewRect(CefBrowser browser, ref CefRectangle rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = _windowWidth;
            rect.Height = _windowHeight;
            return true;
        }

        protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
        {
            return false;
        }

        protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
        {
        }

        protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, IntPtr buffer, int width, int height)
        {
        }

        protected override void OnCursorChange(CefBrowser browser, IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
        {
        }

        protected override void OnScrollOffsetChanged(CefBrowser browser, double x, double y)
        {
        }

        protected override CefAccessibilityHandler GetAccessibilityHandler()
        {
            return null;
        }

        protected override void OnImeCompositionRangeChanged(CefBrowser browser, CefRange selectedRange, CefRectangle[] characterBounds)
        {
        }
    }

    internal class DemoCefClient : CefClient
    {
        private readonly DemoCefLoadHandler _loadHandler;
        private readonly DemoCefRenderHandler _renderHandler;

        public DemoCefClient(int windowWidth, int windowHeight)
        {
            _renderHandler = new DemoCefRenderHandler(windowWidth, windowHeight);
            _loadHandler = new DemoCefLoadHandler();
        }

        protected override CefRenderHandler GetRenderHandler()
        {
            return _renderHandler;
        }

        protected override CefLoadHandler GetLoadHandler()
        {
            return _loadHandler;
        }
    }

    internal class DemoCefLoadHandler : CefLoadHandler
    {
        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            browser.SendProcessMessage(CefProcessId.Renderer, CefProcessMessage.Create("GetHackerNewsTitles"));
        }
    }

    internal class DemoCefDomVisitor : CefDomVisitor
    {
        protected override void Visit(CefDomDocument document)
        {
            //File.WriteAllLines("HackerNewsTitles.txt", GetHackerNewsTitles(document.Root));
        }

        private IEnumerable<string> GetHackerNewsTitles(CefDomNode node)
        {
            if (IsHackerNewsTitle(node))
            {
                yield return node.FirstChild.InnerText;
            }

            CefDomNode child = node.FirstChild;
            while (child != null)
            {
                foreach (string title in GetHackerNewsTitles(child))
                {
                    yield return title;
                }
                child = child.NextSibling;
            }
        }

        private bool IsHackerNewsTitle(CefDomNode node)
        {
            return
                node.NodeType == CefDomNodeType.Element &&
                node.ElementTagName == "TD" &&
                node.HasAttribute("class") &&
                node.GetAttribute("class") == "title" &&
                node.FirstChild.NextSibling != null;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////


    public class RenderProcessHandler : CefRenderProcessHandler
    {
        #region Private/Protected Methods

        protected override void OnBrowserDestroyed(CefBrowser browser) { }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            ////////////if (message.Name == RenderProcessMessages.VisitDom)
            ////////////{
            ////////////    string visitorId = message.Arguments.GetString(0);
            ////////////    lock (DomVisitor.Visitors)
            ////////////    {
            ////////////        DomVisitor visitor = null;
            ////////////        if (DomVisitor.Visitors.TryGetValue(visitorId, out visitor))
            ////////////        {
            ////////////            browser.GetMainFrame().VisitDom(visitor);
            ////////////            return true;
            ////////////        }
            ////////////    }
            ////////////}

            //return base.OnProcessMessageReceived(browser, sourceProcess, message);

            return true;
        }

        #endregion
    }

    public static class RenderProcessMessages
    {
        #region Public/Internal Properties

        public static string VisitDom
        {
            get { return "Renderer.Messages.VisitDom"; }
        }

        #endregion
    }

    public class TestApp : CefApp
    {
        #region Private/Protected Fields and Constants

        private readonly RenderProcessHandler renderProcessHandler;

        #endregion

        #region Constructors

        public TestApp()
        {
            renderProcessHandler = new RenderProcessHandler();
        }

        #endregion

        #region Private/Protected Methods

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return renderProcessHandler;
        }

        #endregion
    }
}
