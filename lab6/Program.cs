using System;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

static class Program
{
    public static void SetLimits ()
    {
        TextEditor.Limit = 3;
        TextEditorReadonly.Limit = 16;
    }

    public static ITextEditor SpawnEditor ()
    {
        ApplicationBag bag = ApplicationBag.Instance;
        TextEditor editor = TextEditor.Instance;
        if (editor != null) {
            editor.OpenSelector = Actions.SelectFileToOpen;
            editor.SaveSelector = Actions.SelectFileToSave;
            editor.ShowStatsPopup = Actions.ShowStatsPopup;
            editor.ShowSearchPopup = Actions.ShowSearch;
            editor.ShowRemovedSpacesPopup = Actions.ShowProposedTextPopup;
            editor.CreateNewEditor = Program.SpawnEditor;
            editor.CreateNewReadonly = Program.SpawnReadonly;
            var form = new EditorView(editor);
            bag.Add(form);
            form.Closed += (sender, eargs) => {
                bag.Remove(form);
            };
            form.Show();
        }
        else {
            MessageBox.Show("Can not spawn more windows.");
        }
        return editor;
    }

    public static ITextEditor SpawnReadonly ()
    {
        ApplicationBag bag = ApplicationBag.Instance;
        TextEditorReadonly editor = TextEditorReadonly.Instance;
        if (editor != null) {
            editor.SaveSelector = Actions.SelectFileToSave;
            editor.ShowStatsPopup = Actions.ShowStatsPopup;
            editor.ShowSearchPopup = Actions.ShowSearch;
            var form = new ReadonlyView(editor);
            bag.Add(form);
            form.Closed += (sender, eargs) => {
                bag.Remove(form);
            };
            form.Show();
        }
        else {
            MessageBox.Show("Can not spawn more windows.");
        }
        return editor;
    }

    [STAThread]
    public static void Main (string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        SetLimits();
        SpawnEditor();
        Application.Run();
    }
}
