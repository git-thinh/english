using CefSharp;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace lenBrowser
{
    
    public class ApiJavascript
    {
        // API.Go(...)
        public void Go(string url)
        {
            App.f_ui_browserGoUrl(url, string.Empty);
        }

        // API.GoTitle(...)
        public void GoTitle(string url, string title)
        {
            App.f_ui_browserGoUrl(url, title);
        }

        // API.UpdateLinks([{Text:"", Link: "http..."}])
        public void UpdateLinks(string jsonLinks)
        {
            //oLink[] links = JsonConvert.DeserializeObject<oLink[]>(jsonLinks);
            App.f_api_sendMessage(IpcMsgType.URL_CACHE_FOR_SEARCH, jsonLinks);
        }

        /*****************************************************************/

        public void windowOpen(string str)
        {
            var f = new Form();
            var _browserControl = new CefWebBrowser("test://test/modaldialog.html");
            _browserControl.Dock = DockStyle.Fill;
            f.Controls.Add(_browserControl);
            f.Show();
        }

        public void windowOpenDialog(string str)
        {
            var f = new Form();
            var _browserControl = new CefWebBrowser("test://test/modaldialog.html");
            _browserControl.Dock = DockStyle.Fill;
            f.Controls.Add(_browserControl);
            f.Show();
        }

        public string Repeat(string str, int n)
        {
            Console.WriteLine("In bound object method");

            string result = "";
            for (int i = 0; i < n; i++)
            {
                result += str;
            }
            return result;
        }

        public void EchoVoid()
        {
        }

        public Boolean EchoBoolean(Boolean arg0)
        {
            return arg0;
        }

        public Boolean? EchoNullableBoolean(Boolean? arg0)
        {
            return arg0;
        }

        public SByte EchoSByte(SByte arg0)
        {
            return arg0;
        }

        public SByte? EchoNullableSByte(SByte? arg0)
        {
            return arg0;
        }

        public Int16 EchoInt16(Int16 arg0)
        {
            return arg0;
        }

        public Int16? EchoNullableInt16(Int16? arg0)
        {
            return arg0;
        }

        public Int32 EchoInt32(Int32 arg0)
        {
            return arg0;
        }

        public Int32? EchoNullableInt32(Int32? arg0)
        {
            return arg0;
        }

        public Int64 EchoInt64(Int64 arg0)
        {
            return arg0;
        }

        public Int64? EchoNullableInt64(Int64? arg0)
        {
            return arg0;
        }

        public Byte EchoByte(Byte arg0)
        {
            return arg0;
        }

        public Byte? EchoNullableByte(Byte? arg0)
        {
            return arg0;
        }

        public UInt16 EchoUInt16(UInt16 arg0)
        {
            return arg0;
        }

        public UInt16? EchoNullableUInt16(UInt16? arg0)
        {
            return arg0;
        }

        public UInt32 EchoUInt32(UInt32 arg0)
        {
            return arg0;
        }

        public UInt32? EchoNullableUInt32(UInt32? arg0)
        {
            return arg0;
        }

        public UInt64 EchoUInt64(UInt64 arg0)
        {
            return arg0;
        }

        public UInt64? EchoNullableUInt64(UInt64? arg0)
        {
            return arg0;
        }

        public Single EchoSingle(Single arg0)
        {
            return arg0;
        }

        public Single? EchoNullableSingle(Single? arg0)
        {
            return arg0;
        }

        public Double EchoDouble(Double arg0)
        {
            return arg0;
        }

        public Double? EchoNullableDouble(Double? arg0)
        {
            return arg0;
        }

        public Char EchoChar(Char arg0)
        {
            return arg0;
        }

        public Char? EchoNullableChar(Char? arg0)
        {
            return arg0;
        }

        public DateTime EchoDateTime(DateTime arg0)
        {
            return arg0;
        }

        public DateTime? EchoNullableDateTime(DateTime? arg0)
        {
            return arg0;
        }

        public Decimal EchoDecimal(Decimal arg0)
        {
            return arg0;
        }

        public Decimal? EchoNullableDecimal(Decimal? arg0)
        {
            return arg0;
        }

        public String EchoString(String arg0)
        {
            return arg0;
        }
    }
}