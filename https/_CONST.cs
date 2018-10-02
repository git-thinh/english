using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    static class _CONST 
    {
        public const string NOTI_PIPE_NAME = "HTTPS_PIPE_NAME";
        public const string RPC_NAME = "HTTPS_RPC_NAME";
        public const string RPC_IID = "{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}";
    }

    public enum IpcMsgType
    {
        NONE = 0,
        URL_REQUEST = 10,
        URL_REQUEST_FAIL = 11,
        URL_REQUEST_SUCCESS = 12,
        URL_CACHE_FOR_SEARCH = 13,
        URL_GET_SOURCE_FROM_CACHE = 14,
        URL_GET_ALL_DOMAIN = 15,
        NOTIFICATION_REG_HANDLE = 100,
        NOTIFICATION_REMOVE_HANDLE = 101,
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

        public static byte[] f_createMessage(IpcMsgType type, string message)
        {
            byte[] a = Encoding.UTF8.GetBytes(message);
            List<byte> ls = new List<byte>(a.Length + 1);
            ls.Add((byte)type);
            ls.AddRange(a);
            return ls.ToArray();
        }

        public static byte[] f_createMessage(IpcMsgType type, int value)
        {
            byte[] a = BitConverter.GetBytes(value);
            List<byte> ls = new List<byte>(a.Length + 1);
            ls.Add((byte)type);
            ls.AddRange(a);
            return ls.ToArray();
        }

        public static byte[] f_sendApi(this Rpc.RpcClientApi client, IpcMsgType type, string message)
        {
            byte[] m = f_createMessage(type, message);
            return client.Execute(m);
        }

        public static byte[] f_sendApi(this Rpc.RpcClientApi client, IpcMsgType type, int message)
        {
            byte[] m = f_createMessage(type, message);
            return client.Execute(m);
        }

        public static void f_sendMessage(int handle, IpcMsgType type, string message)
        {
            COPYDATASTRUCT cds;
            cds.dwData = (int)type;
            cds.lpData = (int)Marshal.StringToHGlobalAnsi(message);
            cds.cbData = message.Length;
            //SendMessage(MSG_WINDOW.MainWindowHandle, (int)WM_COPYDATA, 0, ref cds);
            SendMessage((IntPtr)handle, (int)WM_COPYDATA, 0, ref cds);
        }
    }
}
