using System;
using System.Collections.Generic;
using System.Linq;
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
        FORM_REG_NOTIFICATION = 5,
    }
}
