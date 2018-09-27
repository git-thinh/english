using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using System.Runtime.InteropServices;

namespace SenderApplication
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    [DllImport("coredll.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    private readonly static int WM_CLOSE = 0x10;

    private void btnSend_Click(object sender, EventArgs e)
    {
      // The other application's main window has it's text property
      // set to "Message Test", so ask the operating system to find
      // a window with that title.
      IntPtr hwnd = FindWindow(null, "Message Test");
      if (hwnd == IntPtr.Zero)
      {
        MessageBox.Show("We could not find the main window of the other application.", "Error");
        return;
      }

      // WM_CLOSE is a message which requests a window to close. Once the
      // main widow of an application closes, the entire application will
      // exit.
      Message msg = Message.Create(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      MessageWindow.SendMessage(ref msg);
    }

    private void btnClickAnotherButton_Click(object sender, EventArgs e)
    {
      // Here we send a BM_CLICK (button click) window message to the
      // window which represents the message box button. In this case
      // we use the Handle property of the Control class to obtain
      // the nesscary window handle, so we know where to send the
      // message.
      const int BM_CLICK = 245;
      Message msg = Message.Create(btnMessageBox.Handle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
      MessageWindow.SendMessage(ref msg);
    }

    private void btnMessageBox_Click(object sender, EventArgs e)
    {
      // This method will only be called when the "message box" button
      // is pressed. Have a look at the Click event handlers connected
      // to this method, you will notice the "btnClickAnotherButton"
      // button is not hooked up to this button.
      MessageBox.Show("This is a message box", "Status");
    }

    private void btnIPC_Click(object sender, EventArgs e)
    {
      // An example of Inter Process Communication (i.e. IPC)

      // Find the hidden message window created by the
      // other application
      IntPtr hwnd = FindWindow(null, "MyCoolExample");
      if (hwnd == IntPtr.Zero)
      {
        MessageBox.Show("Could not find the other window", "Error");
        return;
      }

      // Determine the value to pass to the other window as
      // the lParam parameter in the window message. We pass
      // 0 if the checkbox is not checked, or 1 if the checkbox
      // is pressed
      IntPtr lParam = IntPtr.Zero;
      if (checkBox1.Checked)
        lParam = (IntPtr)1;

      // Send our custom message (message number 1234)
      // to the message window living within the other
      // application
      Message msg = Message.Create(hwnd, 1234, IntPtr.Zero, lParam);
      MessageWindow.PostMessage(ref msg);
    }
  }
}