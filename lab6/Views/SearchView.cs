using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

class SearchView : Form
{
    private string data;

    public SearchView (string text) : base()
    {
        this.data = text;

        this.Text = "Search";
        this.ClientSize = new Size(600, 500);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Padding = new Padding(5);
        this.BackColor = Styles.BackgroundColor;

        var panel = new TableLayoutPanel {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 3,
            Padding = new Padding(0)
        };  

        panel.RowStyles.Add (new RowStyle(SizeType.Percent, 50));
        panel.RowStyles.Add (new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add (new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add (new RowStyle(SizeType.Percent, 41));

        panel.ColumnStyles.Add (new ColumnStyle(SizeType.Percent, 40));
        panel.ColumnStyles.Add (new ColumnStyle(SizeType.Percent, 53));
        panel.ColumnStyles.Add (new ColumnStyle(SizeType.Percent, 7));

        var sourceText = new TextBox { 
            Text = this.data, 
            Font = Styles.MonospaceFont,
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Margin = new Padding(0)    
        };
        panel.Controls.Add (sourceText, row: 0, column: 0);
        panel.SetColumnSpan (sourceText, 3);

        var resultText = new TextBox {
            Text = "Results will be here", 
            Font = Styles.MonospaceFont,
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Margin = new Padding(0)    
        };
        panel.Controls.Add (resultText, row: 3, column: 0);
        panel.SetColumnSpan (resultText, 3);

        var knownRegexInput = new ComboBox {
            Dock = DockStyle.Top,
            Font = Styles.MonospaceFont,
            Enabled = false,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(0)  
        };
        foreach (string key in ReUtils.KnownRegex.Keys) {
            knownRegexInput.Items.Add(key);
        }
        knownRegexInput.SelectedValueChanged += (sender, args) => {
            try {
                string key = knownRegexInput.Text;
                resultText.Text = ReUtils.FindAndReport(this.data, ReUtils.KnownRegex[key]);
            }
            catch (Exception ex) {
                return;
            }
        };
        panel.Controls.Add (knownRegexInput, row: 1, column: 1);
        panel.SetColumnSpan (knownRegexInput, 2);

        var customRegexInput = new TextBox {
            Font = Styles.MonospaceFont,
            Dock = DockStyle.Top,
            Enabled = false,
            ScrollBars = ScrollBars.Vertical,
            Margin = new Padding(0)  
        };
        panel.Controls.Add (customRegexInput, row: 2, column: 1);
        var customRegexApply = new Button {
            Text = "Find",
            Enabled = false,
            Dock = DockStyle.Top,
            Margin = new Padding(0)  
        };
        customRegexApply.Click += (sender, args) => {
            resultText.Text = ReUtils.FindAndReport(this.data, customRegexInput.Text);
        };
        panel.Controls.Add (customRegexApply, row: 2, column: 2);

        var knownRegexRB = new RadioButton {
            Text = "Use Presets",
            Dock = DockStyle.Bottom
        };
        panel.Controls.Add (knownRegexRB, row: 1, column: 0);
        var customRegexRB = new RadioButton {
            Text = "Custom Regex",
            Dock = DockStyle.Top
        };
        panel.Controls.Add (customRegexRB, row: 2, column: 0);

        EventHandler regexRBChangeHandler = (sender, args) => {
            if (customRegexRB.Checked) {
                customRegexInput.Enabled = true;
                customRegexApply.Enabled = true;
                knownRegexInput.Enabled = false;
            }
            else {
                customRegexInput.Enabled = false;
                customRegexApply.Enabled = false;
                knownRegexInput.Enabled = true;
            }
        };
        customRegexRB.CheckedChanged += regexRBChangeHandler;
        knownRegexRB.CheckedChanged += regexRBChangeHandler;

        this.Controls.Add(panel);
    }
}
