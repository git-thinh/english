using System;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace SecondApplication
{
  public class MyMessageWindow : MessageWindow
  {
    public MyMessageWindow()
    {
      // We set the text propert of the MessageWindow
      // so that applications can search for this window
      // by title
      Text = "MyCoolExample";
    }

    protected override void WndProc(ref Message m)
    {
      // Check for our special message type
      if (m.Msg == 1234)
      {
        // This is our special message so see what
        // value was passed to us in the LParam parameter
        switch ((int)m.LParam)
        {
          case 0:
            MessageBox.Show("The checkbox in the other program is not checked", "SecondApplication");
            break;

          case 1:
          default:
            MessageBox.Show("The checkbox in the other program is checked", "SecondApplication");
            break;
        }
      }

      base.WndProc(ref m);
    }
  }
}
