using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace lenBrowser
{
    public class CacheMemory : ICache
    {
        public const string SCHEME = "http";
        public const string HOST = "cache.local";

        readonly ConcurrentDictionary<string, string> store;
        public CacheMemory()
        {
            store = new ConcurrentDictionary<string, string>();
        }

        public string Get(string key)
        {
            string val;
            store.TryGetValue(key, out val);
            return val;
        }

        public string Set(string key, string data)
        {
            if (!store.ContainsKey(key))
            {
                if (store.TryAdd(key, data))
                    return getUrl(key);
            }
            return null;
        }

        public string getUrl(string key)
        {
            string url = string.Format("{0}://{1}?key={2}", SCHEME, HOST, key);
            return url;
        }

        public string getKeyByUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            if (url.Length <= 5) return url;
            int pos = url.IndexOf("?key=");
            if (pos == -1) return url;
            return url.Substring(pos + 5, url.Length - (pos + 5));
        }

        public bool isExist(string key)
        {
            return store.ContainsKey(key);
        }

        ~CacheMemory()
        {
            store.Clear();
        }
    }
}
