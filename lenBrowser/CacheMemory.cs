using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace lenBrowser
{
    public class CacheMemory : ICache
    {
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

        public void Set(string key, string data)
        {
            if (!store.ContainsKey(key))
                store.TryAdd(key, data);
        }

        ~CacheMemory()
        {
            store.Clear();
        }
    }
}
