using Newtonsoft.Json;
using Rpc;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;
using System.IO;
using Fleck;

namespace System
{
    public class IpcServer
    {
        #region [VAR]

        readonly List<string> DOMAIN_LIST;
        readonly List<int> LIST_UI_NOTI;
        readonly BinaryFormatter BINARY_FORMATTER;
        readonly ConcurrentDictionary<string, string> CACHE;
        readonly ConcurrentDictionary<string, string> LINK;
        readonly ConcurrentDictionary<int, int> LINK_LEVEL;
        readonly ConcurrentDictionary<string, int> LINK_ID;
        readonly ConcurrentDictionary<int, int> TIME_VIEW_LINK;
        readonly ConcurrentDictionary<string, List<int>> INDEX;
        readonly ConcurrentDictionary<string, List<int>> DOMAIN_LINK;
        readonly ConcurrentDictionary<string, List<int>> KEY_INDEX;

        readonly RpcServerApi SERVER_RPC;
        readonly WebSocketServer NOTI;
        readonly List<IWebSocketConnection> CLIENTS;
        private IWebSocketConnection CLIENT_SETTING = null;
        private IWebSocketConnection CLIENT_PLAYER = null;
        private IWebSocketConnection CLIENT_BROWSER = null;

        #endregion

        public void Start()
        {
            SERVER_RPC.StartListening();

            NOTI.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    CLIENTS.Add(socket);
                    //socket.Send(Guid.NewGuid().ToString());
                };

                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    CLIENTS.Remove(socket);
                };

                socket.OnMessage += (msg) => f_websocket_onMessage(socket, msg);

                //{
                //};
            });

        }

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
            DOMAIN_LIST = Directory.GetDirectories("cache").Select(x => x.Substring(6)).ToList();
            LIST_UI_NOTI = new List<int>();
            BINARY_FORMATTER = new BinaryFormatter();

            CACHE = new ConcurrentDictionary<string, string>();
            LINK = new ConcurrentDictionary<string, string>();
            LINK_LEVEL = new ConcurrentDictionary<int, int>();
            LINK_ID = new ConcurrentDictionary<string, int>();
            INDEX = new ConcurrentDictionary<string, List<int>>();
            DOMAIN_LINK = new ConcurrentDictionary<string, List<int>>();
            TIME_VIEW_LINK = new ConcurrentDictionary<int, int>();
            KEY_INDEX = new ConcurrentDictionary<string, List<int>>();

            SERVER_RPC = new RpcServerApi(new Guid(_CONST.RPC_IID), 100, ushort.MaxValue, allowAnonTcp: false);
            SERVER_RPC.AddProtocol(RpcProtseq.ncalrpc, _CONST.RPC_NAME, 100);
            SERVER_RPC.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE);
            SERVER_RPC.OnExecute += f_executeMessageReceiver;

            NOTI = new WebSocketServer("ws://0.0.0.0:56789");
            CLIENTS = new List<IWebSocketConnection>();
        }

        private void f_websocket_onMessage(IWebSocketConnection socket, string message)
        {
            switch (message) {
                case "_BROWSER_":
                    CLIENT_BROWSER = socket;
                    break;
                case "_SETTING_":
                    CLIENT_SETTING = socket;
                    break;
                case "_PLAYER_":
                    CLIENT_PLAYER = socket;
                    break;
                default:
                    oMsgSocket m = JsonConvert.DeserializeObject<oMsgSocket>(message);
                    
                    break;
            }

            //Console.WriteLine("<<<: " + message);
            ////socket.Send(message);
            //CLIENTS.ForEach(s => s.Send(message));
        }

        private byte[] f_executeMessageReceiver(IRpcClientInfo client, byte[] input)
        {
            MSG_TYPE type = (MSG_TYPE)input[0];
            string text, domain;
            int value;
            oLink[] links;

            switch (type)
            {
                case MSG_TYPE.NOTIFICATION_REG_HANDLE:
                    value = BitConverter.ToInt32(input, 1);
                    lock (LIST_UI_NOTI) if (value > 0 && LIST_UI_NOTI.FindIndex(x => x == value) == -1) LIST_UI_NOTI.Add(value);
                    break;
                case MSG_TYPE.NOTIFICATION_REMOVE_HANDLE:
                    value = BitConverter.ToInt32(input, 1);
                    lock (LIST_UI_NOTI) if (value > 0 && LIST_UI_NOTI.FindIndex(x => x == value) != -1) LIST_UI_NOTI.Remove(value);
                    break;
                case MSG_TYPE.URL_CACHE_FOR_SEARCH:
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
                case MSG_TYPE.URL_REQUEST:
                case MSG_TYPE.URL_GET_SOURCE_FROM_CACHE:
                    #region

                    text = Encoding.UTF8.GetString(input, 1, input.Length - 1);

                    f_cacheUrl(text);

                    if (CACHE.ContainsKey(text))
                        return Encoding.UTF8.GetBytes(CACHE[text]);
                    else
                        f_requestUrl(text);

                    break;

                    #endregion
                case MSG_TYPE.URL_GET_ALL_DOMAIN:
                    #region

                    lock (DOMAIN_LIST)
                        text = string.Join(";", DOMAIN_LIST);
                    return Encoding.UTF8.GetBytes(text);

                #endregion
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

        private void f_cacheUrl(string url, string title = "", string domain = "", int indexForEach = 0)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(title)) return;

            if (domain == "") domain = Html.f_html_getDomainMainByUrl(url);
            int id = LINK.Count + 1, time_view = int.Parse(DateTime.Now.ToString("1ddHHmmss")) + indexForEach;

            if (!LINK.ContainsKey(url))
            {
                LINK.TryAdd(url, title);
                LINK_ID.TryAdd(url, id);
                TIME_VIEW_LINK.TryAdd(id, time_view);
                LINK_LEVEL.TryAdd(id, url.Split('/').Length - 3);
            }

            lock (DOMAIN_LIST) if (!DOMAIN_LIST.Contains(domain)) DOMAIN_LIST.Add(domain);
            if (DOMAIN_LINK.ContainsKey(domain)) DOMAIN_LINK[domain].Add(id); else DOMAIN_LINK.TryAdd(domain, new List<int>() { id });

            if (!string.IsNullOrEmpty(title))
            {
                string s = f_text_convert_UTF8_ACSII(title).ToLower().ToLower();
                string[] words = s.Split(' ').Where(x => x.Length > 2).ToArray();
                for (int i = 0; i < words.Length; i++)
                {
                    if (KEY_INDEX.ContainsKey(words[i]))
                    {
                        if (KEY_INDEX[words[i]].IndexOf(id) == -1)
                            KEY_INDEX[words[i]].Add(id);
                    }
                    else
                        KEY_INDEX.TryAdd(words[i], new List<int>() { id });
                }
                //Console.WriteLine(string.Format(" INDEX: {0}", KEY_INDEX.Count));
            }
        }

        public string f_text_convert_UTF8_ACSII(string utf8)
        {
            string stFormD = utf8.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        private void f_requestUrl(string url)
        {
            string ext = url.ToLower().Substring(url.Length - 3, 3);
            if (ext == "jpg" || ext == "gif" || ext == "png" || ext == "peg" || ext == "svg")
                return;

            Html.f_html_getSourceByUrl(url, (_url, err) =>
            {
                f_sendNotification(MSG_TYPE.URL_REQUEST_FAIL, _url);
            }, (_url, _page) =>
            {
                CACHE.TryAdd(_url, _page.Source);
                f_sendNotification(MSG_TYPE.URL_REQUEST_SUCCESS, _url);
            });
        }

        public void f_sendNotification(MSG_TYPE type, string message)
        {
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(message);
            byte _type = (byte)type;

            lock (LIST_UI_NOTI)
            {
                foreach (int id in LIST_UI_NOTI)
                    MessageHelper.f_sendMessage(id, type, message);
            }
        }
    }
}