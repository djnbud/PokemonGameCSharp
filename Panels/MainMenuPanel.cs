using System;
using System.Windows.Forms;
using System.Drawing;
using PokemonGame.Misc;

namespace PokemonGame.Panels
{
    public partial class MainMenuPanel : UserControl
    {
        public event EventHandler StartClicked;
        public event EventHandler LoadClicked;
        public event EventHandler ExitClicked; // Add ExitClicked event
        public event EventHandler OptionsClicked; // Add OptionsClicked event

        private ImageButton startButton;
        private ImageButton loadButton;
        private ImageButton optionsButton;
        private ImageButton exitButton;
        private float originalFormWidth;
        private float originalFormHeight;
        public MainMenuPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.Resize += MainMenuPanel_Resize;
        }

        void button1_MouseLeave(object sender, EventArgs e)
        {
            this.startButton.BackgroundImage = Image.FromFile("Assets/button.png");
        }


        void button1_MouseEnter(object sender, EventArgs e)
        {

            this.startButton.BackgroundImage = Image.FromFile("Assets/hoverButton.png");
        }

        private void InitializeComponent()
        {
            originalFormWidth = this.ClientSize.Width;
            originalFormHeight = this.ClientSize.Height;
            startButton = new ImageButton(Image.FromFile("Assets/button.png"));
            startButton.Text = "Start New Game";
            startButton.Size = new Size(150, 50);
            startButton.HoverImage = Image.FromFile("Assets/hoverButton.png"); // Load image from file
            startButton.PlainImage = Image.FromFile("Assets/button.png");
            startButton.PressedImage = Image.FromFile("Assets/hoverButton.png");
            startButton.Location = new Point(50, 300);
            startButton.Font = new Font("Arial", 2, FontStyle.Bold);
            startButton.Click += startButton_Click;
            this.Controls.Add(startButton);

            loadButton = new ImageButton(Image.FromFile("Assets/button.png"));
            loadButton.Text = "Load Saved Game";
            loadButton.Size = new Size(150, 50);
            loadButton.HoverImage = Image.FromFile("Assets/hoverButton.png"); // Load image from file
            loadButton.PlainImage = Image.FromFile("Assets/button.png");
            loadButton.PressedImage = Image.FromFile("Assets/hoverButton.png");
            loadButton.Location = new Point(250, 300);
            loadButton.Font = new Font("Arial", 2, FontStyle.Bold);
            loadButton.Click += loadButton_Click;
            this.Controls.Add(loadButton);

            optionsButton = new ImageButton(Image.FromFile("Assets/button.png"));
            optionsButton.Text = "Options";
            optionsButton.Size = new Size(150, 50);
            optionsButton.HoverImage = Image.FromFile("Assets/hoverButton.png"); // Load image from file
            optionsButton.PlainImage = Image.FromFile("Assets/button.png");
            optionsButton.PressedImage = Image.FromFile("Assets/hoverButton.png");
            optionsButton.Location = new Point(450, 300);
            optionsButton.Font = new Font("Arial", 2, FontStyle.Bold);
            optionsButton.Click += optionsButton_Click;
            this.Controls.Add(optionsButton);

            exitButton = new ImageButton(Image.FromFile("Assets/button.png"));
            exitButton.Text = "Exit";
            exitButton.Size = new Size(150, 50);
            exitButton.HoverImage = Image.FromFile("Assets/hoverButton.png"); // Load image from file
            exitButton.PlainImage = Image.FromFile("Assets/button.png");
            exitButton.PressedImage = Image.FromFile("Assets/hoverButton.png");
            exitButton.Location = new Point(650, 300);
            exitButton.Font = new Font("Arial", 2, FontStyle.Bold);
            exitButton.Click += exitButton_Click;
            this.Controls.Add(exitButton);

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartClicked?.Invoke(this, EventArgs.Empty);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadClicked?.Invoke(this, EventArgs.Empty);
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            OptionsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            ExitClicked?.Invoke(this, EventArgs.Empty);
        }

        private void resizeControl(int origX, int origY, ImageButton button)
        {
            float xRatio = this.ClientSize.Width / (float)(originalFormWidth);
            float yRatio = this.ClientSize.Height / (float)(originalFormHeight);

            int newX = (int)(origX * xRatio);
            int newY = (int)(origY * yRatio);

            int newWidth = (int)(15 * xRatio);
            int newHeight = (int)(15 * yRatio);

            button.Location = new Point(newX, newY);
            button.Size = new Size(newWidth, newHeight);

            float scaleFactor = Math.Min(xRatio, yRatio);

            button.Font = new Font(button.Font.FontFamily, 2 * scaleFactor, button.Font.Style);
        }

        private void MainMenuPanel_Resize(object sender, EventArgs e)
        {

            resizeControl(5, 90, startButton);

            resizeControl(45, 90, loadButton);

            resizeControl(85, 90, optionsButton);

            resizeControl(125, 90, exitButton);
        }
    }
}