﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    static class _CONST
    {
        public const int APP_SPLITER_WIDTH = 5;
        public const string NOTI_PIPE_NAME = "HTTPS_PIPE_NAME";
        public const string RPC_NAME = "HTTPS_RPC_NAME";
        public const string RPC_IID = "{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}";
    }

    static class _WS_NAME
    {
        public const string ALL = "*";
        public const string BOX_ENGLISH = "BOX_ENGLISH";
        public const string BROWSER = "BROWSER";
        public const string SETTING = "SETTING";
        public const string PLAYER = "PLAYER";
        public const string HTTPS = "HTTPS";
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
        public int HandleID { set; get; }
        public int HandleMessageID { set; get; }

        public int Width { set; get; }
        public int Height { set; get; }
        public int Top { set; get; }
        public int Left { set; get; }

        public string Url { set; get; }
        public oAppAreaLeftInfo AreaLeft { set; get; }

        public oAppInfo() {
            AreaLeft = new oAppAreaLeftInfo();
        }
    }

    public class oAppAreaLeftInfo
    {
        public int Width { set; get; }
        public string Url { set; get; }
    }

    public class oMsgSocketReply
    {
        public bool Ok { set; get; }
        public string MsgId { set; get; }
        public string Data { set; get; }
        public string Message { set; get; }
    }

    public class oMsgSocket
    {
        public string MsgId { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        public string MsgText { set; get; }
        public MSG_TYPE MsgType { set; get; }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}: {3}", this.MsgType, this.From, this.To, this.MsgText);
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
        public string Text { set; get; }
        public string Url { set; get; }
        public override string ToString()
        {
            return string.Format("{0} -> {1}", Text, Url);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct COPYDATASTRUCT
    {
        public int dwData;
        public int cbData;
        public int lpData;
    }

    static class MessageHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);
        const int WM_COPYDATA = 0x4A;

        public static byte[] f_createMessage(MSG_TYPE type, string message)
        {
            byte[] a = Encoding.UTF8.GetBytes(message);
            List<byte> ls = new List<byte>(a.Length + 1);
            ls.Add((byte)type);
            ls.AddRange(a);
            return ls.ToArray();
        }

        public static byte[] f_createMessage(MSG_TYPE type, int value)
        {
            byte[] a = BitConverter.GetBytes(value);
            List<byte> ls = new List<byte>(a.Length + 1);
            ls.Add((byte)type);
            ls.AddRange(a);
            return ls.ToArray();
        }

        public static byte[] f_sendApi(this Rpc.RpcClientApi client, MSG_TYPE type, string message)
        {
            byte[] m = f_createMessage(type, message);
            return client.Execute(m);
        }

        public static byte[] f_sendApi(this Rpc.RpcClientApi client, MSG_TYPE type, int message)
        {
            byte[] m = f_createMessage(type, message);
            return client.Execute(m);
        }

        public static void f_sendMessage(int handle, MSG_TYPE type, string message)
        {
            COPYDATASTRUCT cds;
            cds.dwData = (int)type;
            cds.lpData = (int)Marshal.StringToHGlobalAnsi(message);
            cds.cbData = message.Length;
            //SendMessage(MSG_WINDOW.MainWindowHandle, (int)WM_COPYDATA, 0, ref cds);
            SendMessage((IntPtr)handle, (int)WM_COPYDATA, 0, ref cds);
        }
    }

    //{"id":"event_855075b-4935-baa8-685e95443949","x":283,"y":263,"text":"above "}
    public class oEN_TRANSLATE_GOOGLE_REQUEST
    {
        public string id { set; get; }

        public string text { set; get; }
        public string mean_vi { set; get; }

        public int x { set; get; }
        public int y { set; get; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", text, mean_vi);
        }
    }
}


