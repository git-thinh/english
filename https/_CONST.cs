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
        URL_REQUEST = 1,
        URL_REQUEST_FAIL = 2,
        URL_REQUEST_SUCCESS = 3,
        GET_HTML_SOURCE = 4,
        NOTIFICATION_REG_HANDLE = 5,
        NOTIFICATION_REMOVE_HANDLE = 6,
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
