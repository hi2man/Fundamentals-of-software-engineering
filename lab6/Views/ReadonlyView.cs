using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;

class ReadonlyView : Form
{
    private ITextEditor editor;

    private TextBox textArea;
    private MenuStrip menuStrip;

    public ReadonlyView(ITextEditor editor) : base()
    {
        this.editor = editor;

        this.Text = "Bloknot";
        this.ClientSize = new Size(800, 600);
        this.FormBorderStyle = FormBorderStyle.Sizable;

        this.textArea = new TextBox {
            Multiline = true,
            ReadOnly = true,
            AcceptsTab = true,
            AcceptsReturn = true,
            Dock = DockStyle.Fill,
            Font = Styles.MonospaceFont,
            BackColor = Styles.BackgroundColor,
            ScrollBars = ScrollBars.Vertical
        };
        this.textArea.TextChanged += (sender, args) => {
            this.editor.Text = this.textArea.Text;
        };
        this.editor.TextChanged += () => {
            this.textArea.Text = this.editor.Text;
        };
        this.editor.CaptionChanged += () => {
            this.Text = this.editor.Caption;
        };
        
        this.Closed += (sender, args) => {
            this.editor.TryClose();
        };

        this.menuStrip = new MenuStrip {
            Dock = DockStyle.Top
        };
        var fileItem = new ToolStripMenuItem { Text = "File" };
        fileItem.DropDownItems.Add("Save as", null, (sender, args) => {
            editor.TrySave();
        });
        var toolItem = new ToolStripMenuItem { Text = "Tools" };
        toolItem.DropDownItems.Add("Search", null, (sender, args) => {
            editor.ShowSearch();
        });
        toolItem.DropDownItems.Add("Statistics", null, (sender, args) => {
            editor.ShowStats();
        });
        this.menuStrip.Items.Add(fileItem);
        this.menuStrip.Items.Add(toolItem);

        this.Controls.Add(this.textArea);
        this.Controls.Add(this.menuStrip);
    }
}
