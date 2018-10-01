using Newtonsoft.Json;
using Rpc;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;

namespace System
{
    public class IpcServer
    {
        readonly List<int> LIST_UI_NOTI;
        readonly BinaryFormatter BINARY_FORMATTER;
        readonly ConcurrentDictionary<string, string> CACHE;
        readonly ConcurrentDictionary<string, string> LINK;
        readonly ConcurrentDictionary<int, int> LINK_LEVEL;
        readonly ConcurrentDictionary<int, string> ID_LINK;
        readonly ConcurrentDictionary<int, int> TIME_VIEW_LINK;
        readonly ConcurrentDictionary<string, List<int>> INDEX;
        readonly ConcurrentDictionary<string, List<int>> DOMAIN_LINK;

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
            LINK = new ConcurrentDictionary<string, string>();
            LINK_LEVEL = new ConcurrentDictionary<int, int>();
            ID_LINK = new ConcurrentDictionary<int, string>();
            INDEX = new ConcurrentDictionary<string, List<int>>();
            DOMAIN_LINK = new ConcurrentDictionary<string, List<int>>();
            TIME_VIEW_LINK = new ConcurrentDictionary<int, int>();

            SERVER_RPC = new RpcServerApi(new Guid(_CONST.RPC_IID), 100, ushort.MaxValue, allowAnonTcp: false);
            SERVER_RPC.AddProtocol(RpcProtseq.ncalrpc, _CONST.RPC_NAME, 100);
            SERVER_RPC.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE);
            SERVER_RPC.OnExecute += f_executeMessageReceiver;
            SERVER_RPC.StartListening();
        }

        private byte[] f_executeMessageReceiver(IRpcClientInfo client, byte[] input)
        {
            IpcMsgType type = (IpcMsgType)input[0];
            string text, domain;
            int value;
            oLink[] links;

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
                case IpcMsgType.URL_CACHE_FOR_SEARCH:
                    #region
                    text = Encoding.UTF8.GetString(input, 1, input.Length - 1);
                    links = JsonConvert.DeserializeObject<oLink[]>(text);
                    if (links.Length > 0)
                    {
                        var ls = links.GroupBy(x => x.Url).Select(x => x.First()).ToArray();
                        domain = Html.f_html_getDomainMainByUrl(ls[0].Url);
                        string url, title;
                        for (int i = 0; i < ls.Length; i++)
                        {
                            url = ls[i].Url;
                            title = ls[i].Text;
                            f_cacheUrl(url, title, domain, i);
                        }
                    }
                    #endregion
                    break;
                case IpcMsgType.URL_REQUEST:
                case IpcMsgType.URL_GET_SOURCE_FROM_CACHE:
                    text = Encoding.UTF8.GetString(input, 1, input.Length - 1);
                    if (CACHE.ContainsKey(text))
                        return Encoding.UTF8.GetBytes(CACHE[text]);
                    else
                    {
                        if (text.Contains('?') && text.Contains("___id=")) { } else {
                            int id = 0;
                            if (text.Contains('?')) text = text + "&___id=" + id.ToString();
                            else text = text + "?___id=" + id.ToString();
                        }
                        f_requestUrl(text);
                        return Encoding.UTF8.GetBytes(text);
                    }
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

        private void f_cacheUrl(string url, string title, string domain = "", int indexForEach = 0)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(title)) return;

            if (LINK.ContainsKey(url)) {
                int time_view = int.Parse(DateTime.Now.ToString("yMMddHHmmss")) + indexForEach;

            } else
            {
                if (domain == "") domain = Html.f_html_getDomainMainByUrl(url);

                LINK.TryAdd(url, title);

                int id = LINK.Count, level = url.Split('/').Length - 3, time_view = int.Parse(DateTime.Now.ToString("yMMddHHmmss")) + indexForEach;

                TIME_VIEW_LINK.TryAdd(id, time_view);
                ID_LINK.TryAdd(id, url);
                LINK_LEVEL.TryAdd(id, level);
                if (DOMAIN_LINK.ContainsKey(domain))
                    DOMAIN_LINK[domain].Add(id);
                else
                    DOMAIN_LINK.TryAdd(domain, new List<int>() { id });
            }
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