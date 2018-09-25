using System;
using System.Windows.Forms;

namespace CefSharp.Example
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        public InputForm(string title, string val = "")
        {
            InitializeComponent();
            this.Text = title;
            input.Text = val;
        }

        public String GetInput()
        {
            return input.Text;
        }
    }
}
