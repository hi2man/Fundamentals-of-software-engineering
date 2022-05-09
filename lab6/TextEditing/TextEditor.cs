using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class TextEditor: Multiton<TextEditor>, ITextEditor
{
    const int HistorySize = 100;

    private string fileName;

    public Func<ITextEditor> CreateNewEditor { get; set; }
    public Func<ITextEditor> CreateNewReadonly { get; set; }
    public Action Close { get; set; }
    public Func<OutFileProvider> SaveSelector { get; set; }
    public Func<InFileProvider> OpenSelector { get; set; }
    public Action<TextStats> ShowStatsPopup { get; set; }
    public Action<string> ShowSearchPopup { get; set; }
    public Func<string, string, bool> ShowRemovedSpacesPopup { get; set; }
    public event Action TextChanged;
    public event Action CaptionChanged;

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

    public string Caption {
        get {
            return String.Format("{0} - Bloknot", this.fileName);
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
            if (this.Text.Length > 0) this.CreateNewEditor();
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

    public void SetFileProvider(InFileProvider provider)
    {
        if (provider.Parser is PlainTextParser) {
            this.Text = new PlainTextBuilder(provider.Parser.Chunks).ConvertToString();
            this.fileName = provider.FileName;
            this.CaptionChanged.Invoke();
        }
    }

    public bool TryOpen() 
    {
        try {
            InFileProvider provider = this.OpenSelector();
            if (provider.Parser is PlainTextParser) {
                if (this.Text.Length < 3) {
                    this.SetFileProvider(provider);
                    return true;
                }
                else {
                    this.CreateNewEditor().SetFileProvider(provider);
                }
            }
            else {
                this.CreateNewReadonly().SetFileProvider(provider);
            }
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    public bool TrySave()
    {
        try {
            OutFileProvider provider = this.SaveSelector();
            provider.Builder.AddManyTextChunks(new PlainTextParser(this.Text).Chunks);
            provider.Write();
            this.fileName = provider.FileName;
            this.CaptionChanged.Invoke();
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
