using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace lenBrowser
{
    public interface ICache
    {
        bool isExist(string key);
        string Set(string key, string data);
        string Get(string key);
        string getUrl(string key);
        string getKeyByUrl(string url);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct COPYDATASTRUCT
    {
        public int dwData;
        public int cbData;
        public int lpData;
    }

    public class oCmd
    {
        public string cmd { set; get; }
        public string url { set; get; }
    }
}
