using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SendMessage
{
    public partial class Form1 : Form
    {
           private const int WM_COPYDATA = 0x4A;

       [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            public int lpData;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             foreach (Process clsProcess in Process.GetProcesses())
            {
                              
                Process[] pname = Process.GetProcessesByName("GetMessge");
                if (pname.Length == 0)
                {                   
                    string message = textBox1.Text+"-" + System.DateTime.Now.ToString();
                    COPYDATASTRUCT cds;
                    cds.dwData = 0;
                    cds.lpData = (int)Marshal.StringToHGlobalAnsi(message);
                    cds.cbData = message.Length;
                    SendMessage(clsProcess.MainWindowHandle, (int)WM_COPYDATA, 0, ref cds);
                }
                else
                {
                   // MessageBox.Show("run");

                } 

         
            }

        }
        }
    }

