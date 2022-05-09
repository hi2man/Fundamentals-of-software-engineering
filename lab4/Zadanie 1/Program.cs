using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

static class Program
{
    static int count = 3;

    [STAThread]
    public static void Main (string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        var bag = ApplicationBag.Instance;
        for (int i=0; i<count; i++) {
            var form = new Form {
                Text = "Example "+i.ToString(),
                ClientSize = new System.Drawing.Size(260, 200)
            };
            form.Controls.Add(new NumberView(NumberHolder.Instance));
            bag.Add(form);
            Console.Write("After bag.Add(form)\n");
            form.Closed += (sender, eargs) => {
                bag.Remove(form);
            };
            form.Show();
        }
        Application.Run();
    }
}
