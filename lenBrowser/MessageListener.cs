using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace lenBrowser
{
    // NativeWindow class to listen to operating system messages.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    internal class MessageListener : NativeWindow
    {
        // Constant value was found in the "windows.h" header file.
        const int WM_ACTIVATEAPP = 0x001C;
        const int WM_COPYDATA = 0x4A;

        private fBrowser parent;

        public MessageListener(fBrowser parent)
        {
            parent.HandleCreated += new EventHandler(this.OnHandleCreated);
            parent.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
            this.parent = parent;
        }

        // Listen for the control's window creation and then hook into it.
        internal void OnHandleCreated(object sender, EventArgs e)
        {
            // Window is now created, assign handle to NativeWindow.
            AssignHandle(((fBrowser)sender).Handle);
        }

        internal void OnHandleDestroyed(object sender, EventArgs e)
        {
            // Window was destroyed, release hook.
            ReleaseHandle();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            ////// Listen for operating system messages
            ////switch (m.Msg)
            ////{
            ////    case WM_ACTIVATEAPP:
            ////        // Notify the form that this message was received.
            ////        // Application is activated or deactivated, 
            ////        // based upon the WParam parameter.
            ////        parent.f_api_messageReceiver((int)m.WParam);
            ////        break;
            ////}
            ////base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_COPYDATA:
                    COPYDATASTRUCT CD = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                    byte[] B = new byte[CD.cbData];
                    IpcMsgType type = (IpcMsgType)(new IntPtr(CD.dwData));
                    IntPtr lpData = new IntPtr(CD.lpData);
                    Marshal.Copy(lpData, B, 0, CD.cbData);
                    string strData = Encoding.Default.GetString(B);
                    parent.f_api_messageReceiver(type, strData);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
