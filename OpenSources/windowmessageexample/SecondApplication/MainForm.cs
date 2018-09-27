using System;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace SecondApplication
{
  public partial class MainForm : Form
  {
    // This is where we create our message window. When this form
    // is created it will create our hidden window.
    protected MessageWindow msgWindow = new MyMessageWindow();

    public MainForm()
    {
      InitializeComponent();
    }

    private void MainForm_Closed(object sender, EventArgs e)
    {
      // This message box is here to help demonstrate that the
      // application has been closed. You will notice this
      // message is displayed when the "Close Other App" button
      // is pressed in the other application, indicating that
      // a WM_CLOSE message has been sent to this window.
      MessageBox.Show("This application's main form has closed.", "Status");
    }
  }
}