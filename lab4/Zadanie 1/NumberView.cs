using System;
using System.Drawing;
using System.Windows.Forms;

public class NumberView: Panel
{
    private NumberHolder numberHolder;

    public NumberView (NumberHolder nholder)
    {
        this.numberHolder = nholder;
        this.InitializeView();
    }

    private void InitializeView ()
    {
        this.Dock = DockStyle.Fill;
        var outputLabel = new Label {
            Text = "",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Consolas", 18)
        };
        var randomizeButton = new Button {
            Text = "Randomize",
            Dock = DockStyle.Top
        };
        randomizeButton.Click += (sender, eargs) => this.numberHolder.Randomize();
        this.numberHolder.Changed += (value) => {
            outputLabel.Text = value.ToString();
        };

        this.Controls.Add(outputLabel);
        this.Controls.Add(randomizeButton);
    }

}
