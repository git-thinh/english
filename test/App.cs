using System;
using System.Windows.Forms;

namespace test
{
    class App
    {
        [STAThread]
        static void Main(string[] args)
        {
            ExamplePresenter.Init();
            Browser browser = new Browser();
            Application.Run(browser);
        }
    }
}
