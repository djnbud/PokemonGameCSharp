using System;
using System.Windows.Forms;
using System.Drawing;
using PokemonGame.Misc;

namespace PokemonGame.Panels
{
    public partial class SavedGamesPanel : UserControl
    {
        public event EventHandler BackClicked;
        private ImageButton backButton;
        private float originalFormWidth;
        private float originalFormHeight;
        public SavedGamesPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.Resize += MainMenuPanel_Resize;
        }

        private void InitializeComponent()
        {
            originalFormWidth = this.ClientSize.Width;
            originalFormHeight = this.ClientSize.Height;
            // Initialize components such as buttons, labels, etc.
            backButton = new ImageButton(Image.FromFile("Assets/button.png"));
            backButton.Text = "Back";
            backButton.Size = new Size(150, 50);
            backButton.HoverImage = Image.FromFile("Assets/hoverButton.png"); // Load image from file
            backButton.PlainImage = Image.FromFile("Assets/button.png");
            backButton.PressedImage = Image.FromFile("Assets/hoverButton.png");
            backButton.Location = new System.Drawing.Point(650, 300);
            backButton.Font = new Font("Arial", 2, FontStyle.Bold);
            backButton.Click += backButton_Click;
            this.Controls.Add(backButton);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            BackClicked?.Invoke(this, EventArgs.Empty);
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
            resizeControl(125, 90, backButton);
        }
    }
}