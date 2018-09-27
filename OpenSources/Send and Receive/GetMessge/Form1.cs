using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GetMessge
{
    public partial class Form1 : Form
    {

        private string message = string.Empty;
        private string dateTime = string.Empty;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);

        private const int WM_COPYDATA = 0x4A;

        int hWnd;
        public static int NewWnd;

        [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            public int lpData;
        }


        public Form1()
        {
            InitializeComponent();
        }


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_COPYDATA:
                  
                    COPYDATASTRUCT CD = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                    byte[] B = new byte[CD.cbData];
                    IntPtr lpData = new IntPtr(CD.lpData);
                    Marshal.Copy(lpData, B, 0, CD.cbData);
                    string strData = Encoding.Default.GetString(B);
                    listBox1.Items.Add(strData);
                                     
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
