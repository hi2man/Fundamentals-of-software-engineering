using System;

interface ITextEditor
{
    string Text { get; set; }
    bool TryNew();
    bool TrySave();
    bool TryOpen();
    void TryUndo();
    void TryRedo();
    bool TryClose();
    void ShowStats();
    void ShowSearch();
    void ShowRemovedSpaces();
    event Action TextChanged;
}
