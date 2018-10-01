﻿using Rpc;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System
{
    public class IpcServer
    {
        readonly List<int> LIST_UI_NOTI;
        readonly BinaryFormatter BINARY_FORMATTER;
        readonly ConcurrentDictionary<string, string> CACHE;
        readonly RpcServerApi SERVER_RPC;

        public void Stop()
        {
            CACHE.Clear();
            SERVER_RPC.StopListening();
        }

        ~IpcServer()
        {
            Stop();
        }

        public IpcServer()
        {
            LIST_UI_NOTI = new List<int>();
            BINARY_FORMATTER = new BinaryFormatter();
            CACHE = new ConcurrentDictionary<string, string>();

            SERVER_RPC = new RpcServerApi(new Guid(_CONST.RPC_IID), 100, ushort.MaxValue, allowAnonTcp: false);
            SERVER_RPC.AddProtocol(RpcProtseq.ncalrpc, _CONST.RPC_NAME, 100);
            SERVER_RPC.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE);
            SERVER_RPC.OnExecute += f_executeMessageReceiver;
            SERVER_RPC.StartListening();
        }

        private byte[] f_executeMessageReceiver(IRpcClientInfo client, byte[] input)
        {
            IpcMsgType type = (IpcMsgType)input[0];
            string url;
            int value;

            switch (type)
            {
                case IpcMsgType.NOTIFICATION_REG_HANDLE:
                    value = BitConverter.ToInt32(input, 1);
                    if (value > 0 && LIST_UI_NOTI.FindIndex(x => x == value) == -1)
                        LIST_UI_NOTI.Add(value);
                    break;
                case IpcMsgType.NOTIFICATION_REMOVE_HANDLE:
                    value = BitConverter.ToInt32(input, 1);
                    if (value > 0 && LIST_UI_NOTI.FindIndex(x => x == value) != -1)
                        LIST_UI_NOTI.Remove(value);
                    break;
                case IpcMsgType.URL_REQUEST: 
                case IpcMsgType.GET_HTML_SOURCE:
                    url = Encoding.UTF8.GetString(input, 1, input.Length - 1);
                    if (CACHE.ContainsKey(url))
                        return Encoding.UTF8.GetBytes(CACHE[url]);
                    else
                        f_requestUrl(url);
                    break;
                default:
                    //using (var memoryStream = new MemoryStream(input, 1, input.Length - 1, false, false))
                    //obj = BINARY_FORMATTER.Deserialize(memoryStream);
                    break;
            }
            //using (var memoryStream = new MemoryStream())
            //{
            //    BINARY_FORMATTER.Serialize(memoryStream, obj);
            //    return memoryStream.ToArray();
            //} 
            return new byte[0];
        }

        private void f_requestUrl(string url)
        {
            string ext = url.ToLower().Substring(url.Length - 3, 3);
            if (ext == "jpg" || ext == "gif" || ext == "png" || ext == "peg" || ext == "svg")
                return;

            Html.f_html_getSourceByUrl(url, (_url, err) =>
            {
                f_sendNotification(IpcMsgType.URL_REQUEST_FAIL, _url);
            }, (_url, _page) =>
            {
                CACHE.TryAdd(_url, _page.Source);
                f_sendNotification(IpcMsgType.URL_REQUEST_SUCCESS, _url);
            });
        }

        public void f_sendNotification(IpcMsgType type, string message)
        {
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(message);
            byte _type = (byte)type;
            foreach (int id in LIST_UI_NOTI)
                MessageHelper.f_sendMessage(id, type, message);
        }
    }
}