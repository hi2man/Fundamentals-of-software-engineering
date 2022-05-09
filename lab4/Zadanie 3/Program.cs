using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

static class Program
{
    static int count = 3;

    public static void SpawnWindow ()
    {
        ApplicationBag bag = ApplicationBag.Instance;
        TextEditor editor = TextEditor.Instance;
        if (editor != null) {
            editor.OpenSelector = Actions.SelectFileToOpen;
            editor.SaveSelector = Actions.SelectFileToSave;
            editor.ShowStatsPopup = Actions.ShowStatsPopup;
            editor.ShowSearchPopup = Actions.ShowSearch;
            editor.ShowRemovedSpacesPopup = Actions.ShowProposedTextPopup;
            editor.NewDocument = Program.SpawnWindow;
            var form = new MainView(editor);
            bag.Add(form);
            form.Closed += (sender, eargs) => {
                bag.Remove(form);
            };
            form.Show();
        }
        else {
            MessageBox.Show("Can not spawn more windows.");
        }
    }

    [STAThread]
    public static void Main (string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        TextEditor.Limit = count;
        SpawnWindow();
        Application.Run();
    }
}
