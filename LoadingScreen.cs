using System;
using System.Drawing;
using System.Windows.Forms;

public partial class LoadingScreen : UserControl
{
    private Label loadingLabel;
    private Timer timer;
    private int dotCount = 0;

    public LoadingScreen()
    {
        InitializeComponent();
        InitializeTimer();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // UserControl
        this.BackColor = Color.Black;
        this.Dock = DockStyle.Fill;

        // Loading Label
        this.loadingLabel = new Label();
        this.loadingLabel.Text = "Loading";
        this.loadingLabel.ForeColor = Color.White;
        this.loadingLabel.Font = new Font("Arial", 16, FontStyle.Bold);
        this.loadingLabel.AutoSize = true;
        this.loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.loadingLabel.Location = new Point((this.Width - this.loadingLabel.Width) / 2, (this.Height - this.loadingLabel.Height) / 2);
        this.Controls.Add(this.loadingLabel);

        this.ResumeLayout(false);
    }

    private void InitializeTimer()
    {
        timer = new Timer();
        timer.Interval = 500; // Change text every 0.5 seconds
        timer.Tick += Timer_Tick;
    }

    public void StartLoading()
    {
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Update loading text with varying dots
        dotCount = (dotCount + 1) % 4;
        string dots = new string('.', dotCount);
        loadingLabel.Text = "Loading" + dots;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        // Update label position when UserControl is resized
        loadingLabel.Location = new Point((this.Width - loadingLabel.Width) / 2, (this.Height - loadingLabel.Height) / 2);
    }

    public void StopLoading()
    {
        timer.Stop();
        loadingLabel.Text = "Loading";
    }
}
