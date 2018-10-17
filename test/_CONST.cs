//using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    public interface IForm
    {
        void f_browser_Go(string url);
        void f_browser_updateInfoPage(string url, string title);
        oAppInfo f_getInfo();
        string f_get_formKey();

        void f_sendToBrowser(string data);
    }

    public interface IApp
    {
        void f_link_AddUrls(string[] urls);
        string f_link_getHtmlCache(string url);
        string f_link_getHtmlOnline(string url);
        void f_link_updateUrls(oLink[] links);

        oLinkResult f_link_getLinkPaging(oLinkRequest linkRequest);

        bool f_main_openUrl(string url, string title);
        void f_app_callFromJs(string data);

        void f_form_openByKey(string formKey);
        void f_form_unRegister(IForm form);
        void f_form_Register(IForm form);
    }

    static class _NAME_UI
    {
        public const string MAIN = "MAIN";
        public const string ALL = "*";
        public const string BOX_ENGLISH = "BOX_ENGLISH";
        public const string SEARCH = "SEARCH";
        public const string SETTING = "SETTING";
        public const string LINK = "LINK";
        public const string PLAYER = "PLAYER";
    }

    public enum MSG_TYPE
    {
        NONE = 0,
        APP_INFO = 1,

        NOTIFICATION_REG_HANDLE = 5,
        NOTIFICATION_REMOVE_HANDLE = 6,

        URL_REQUEST = 10,
        URL_REQUEST_FAIL = 11,
        URL_REQUEST_SUCCESS = 12,

        URL_CACHE_FOR_SEARCH = 13,
        URL_GET_SOURCE_FROM_CACHE = 14,
        URL_GET_ALL_DOMAIN = 15,

        EN_TRANSLATE_GOOGLE_REQUEST = 20,
        EN_TRANSLATE_GOOGLE_RESPONSE = 21,
        EN_TRANSLATE_SAVE = 22,
        EN_TRANSLATE_REMOVE = 23,

        EN_DEFINE_WORD_REQUEST = 30,
        EN_DEFINE_WORD_RESPONSE = 31,
        EN_DEFINE_WORD_SAVE = 32,
        EN_DEFINE_WORD_REMOVE = 32,
    }

    public class oAppInfo
    {
        public int Width { set; get; }
        public int Height { set; get; }
        public int Top { set; get; }
        public int Left { set; get; }

        public string Url { set; get; }
    }
    
    public class oMsgSocket
    {
        public bool Ok { set; get; }
        public long MsgId { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        
        private string _msgRequest = string.Empty;
        public string MsgRequest
        {
            set
            {
                _msgRequest = value;
                if (_msgRequest == null) _msgRequest = string.Empty;
                _msgRequest = _msgRequest.Replace('"', '¦');
            }
            get
            {
                return _msgRequest;
            }
        }

        private string _msgResponse = string.Empty;
        public string MsgResponse
        {
            set
            {
                _msgResponse = value;
                if (_msgResponse == null) _msgResponse = string.Empty;
                _msgResponse = _msgResponse.Replace('"', '¦');
            }
            get
            {
                return _msgResponse;
            }
        }

        [JsonIgnore]
        public string MsgRequestJson
        {
            get
            {
                return _msgRequest.Replace('¦', '"');
            }
        }

        [JsonIgnore]
        public string MsgResponseJson
        {
            get
            {
                return _msgResponse.Replace('¦', '"');
            }
        }

        public MSG_TYPE MsgType { set; get; }


        public oMsgSocket(bool ok, MSG_TYPE msgType, long msgId = 0, string msgRequest = "", string msgResponse = "")
        {
            this.Ok = ok;
            this.MsgType = msgType;
            this.MsgId = msgId;

            this.MsgRequest = msgRequest;
            this.MsgResponse = msgResponse;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}: {3}", this.MsgType, this.From, this.To, this.MsgRequest);
        }
    }

    public class oPage
    {
        public string Title { set; get; }
        public string Url { set; get; }
        public string Source { set; get; }
        public string[] Urls { set; get; }
    }

    public class oLink
    {
        public int recid { set; get; }
        public string Text { set; get; }
        public string Url { set; get; }
        public override string ToString()
        {
            return string.Format("{0} -> {1}", Text, Url);
        }
    }

    public class oLinkResult
    {
        //{"cmd":"get","selected":[],"limit":100,"offset":100}
        public string status { set; get; }
        public int total { set; get; }
        public oLink[] records { set; get; }
        public oLinkResult() {
            status = "success";
            total = 0;
            records = new oLink[] { };
        }
    }

    public class oLinkRequest
    {
        //{"cmd":"get","selected":[],"limit":100,"offset":100}
        public string cmd { set; get; }
        public int limit { set; get; }
        public int offset { set; get; }
        public int[] selected { set; get; }
    }

    /////////////////////////////////////////////////

    public delegate void TranslateCallBack(oEN_TRANSLATE_GOOGLE_MESSAGE otran);

    //{"id":"event_855075b-4935-baa8-685e95443949","x":283,"y":263,"text":"above "}
    public class oEN_TRANSLATE_GOOGLE_MESSAGE
    {
        public bool success { set; get; }

        public string id { set; get; }

        public string text { set; get; }
        public string type { set; get; }
        public string mean_vi { set; get; }

        public int x { set; get; }
        public int y { set; get; }
        
        [JsonIgnore]
        public WebRequest webRequest { set; get; }

        [JsonIgnore]
        public TranslateCallBack translateCallBack { set; get; }

        public oEN_TRANSLATE_GOOGLE_MESSAGE()
        {
            this.success = false;
            //this.socket = null;
            this.webRequest = null;
            this.translateCallBack = null;

            this.mean_vi = string.Empty;
            this.type = string.Empty;
        }

        public oEN_TRANSLATE_GOOGLE_MESSAGE(WebRequest _webRequest, TranslateCallBack _translateCallBack)
        {
            this.success = false;
            this.webRequest = _webRequest;
            this.translateCallBack = _translateCallBack;

            this.mean_vi = string.Empty;
            this.type = string.Empty;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", text, mean_vi);
        }
    }
}


