using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

class TextEditor: Multiton<TextEditor>, ITextEditor
{
    const int HistorySize = 100;
    public Action NewDocument { get; set; }
    public Action Close { get; set; }
    public Func<Stream> SaveSelector { get; set; }
    public Func<Stream> OpenSelector { get; set; }
    public Action<TextStats> ShowStatsPopup { get; set; }
    public Action<string> ShowSearchPopup { get; set; }
    public Func<string, string, bool> ShowRemovedSpacesPopup { get; set; }
    public event Action TextChanged;

    private LinkedList<string> History = new LinkedList<string> { };
    private LinkedList<string> UndoneHistory = new LinkedList<string> { };

    protected TextEditor() {

    }

    public virtual string Text { 
        get {
            return this.History.Count > 0 ? 
                this.History.Last.Value : 
                String.Empty;
        } 
        set {
            if (value == this.Text) {
                return;
            }
            if (this.UndoneHistory.Count > 0 && this.UndoneHistory.Last.Value != value) {
                this.UndoneHistory.Clear();
            }
            this.History.AddLast(value);
            if (this.History.Count > HistorySize) {
                this.History.RemoveFirst();
            }
            this.TextChanged.Invoke();
        } 
    }

    public void TryUndo()
    {
        if (this.History.Count > 0) {
            this.UndoneHistory.AddLast(this.History.Last.Value);
            if (this.UndoneHistory.Count > HistorySize) {
                this.UndoneHistory.RemoveFirst();
            }
            this.History.RemoveLast();
            this.TextChanged.Invoke();
        }
    }

    public void TryRedo()
    {
        if (this.UndoneHistory.Count > 0) {
            this.Text = this.UndoneHistory.Last.Value;
            if (this.History.Count > HistorySize) {
                this.History.RemoveFirst();
            }
            this.UndoneHistory.RemoveLast();
            this.TextChanged.Invoke();
        }
    }

    public bool TryNew()
    {
        try {
            this.NewDocument();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public bool TryClose()
    {
        try {
            Delete(this);
            if (this.Close != null) this.Close();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public bool TryOpen() 
    {
        try {
            var reader = new StreamReader(this.OpenSelector());
            this.Text = reader.ReadToEnd();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public bool TrySave()
    {
        try {
            var writer = new StreamWriter(this.SaveSelector());
            writer.Write(this.Text);
            writer.Close();
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public void ShowStats()
    {
        this.ShowStatsPopup(new TextStats(this.Text, Encoding.UTF8));
    }

    public void ShowSearch()
    {
        this.ShowSearchPopup(this.Text);
    }

    public void ShowRemovedSpaces()
    {
        string newText = Utils.RemoveSpaces(this.Text);
        if (this.ShowRemovedSpacesPopup(this.Text, newText)) {
            this.Text = newText;
        }
    }
}
