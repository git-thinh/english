using System;
using System.Windows.Forms;

namespace test
{
    class App
    {
        [STAThread]
        static void Main(string[] args)
        {
            BrowserUI.Init();
            var f = new FormBrowser();
            Application.Run(f);
            BrowserUI.Exit();
        }
    }
}
