using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

public static class Extensions
{
    public static void Invoke<TControlType>(this TControlType control, Action<TControlType> del)
        where TControlType : Control
    {
        if (control.InvokeRequired)
            control.Invoke(new Action(() => del(control)));
        else
            del(control);
    }
}
