using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PokemonGame.Panels
{
    public class StoryTextPanel : Panel
    {
        private Label storyLabel;
        private PictureBox storyPictureBox;
        private Queue<(string text, Image image)> storyQueue;
        private Timer typingTimer;
        private string currentSentence;
        private int charIndex;

        public event EventHandler StoryCompleted;

        public StoryTextPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(800, 450); // Adjust size as needed
            this.BackColor = Color.Black;

            storyPictureBox = new PictureBox();
            storyPictureBox.Size = new Size(100, 100);
            storyPictureBox.Location = new Point((this.Width - storyPictureBox.Width) / 2, 200);
            storyPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(storyPictureBox);

            storyLabel = new Label();
            storyLabel.ForeColor = Color.White;
            storyLabel.Location = new Point(50, 300);
            storyLabel.Size = new Size(700, 200);
            storyLabel.Font = new Font("Arial", 16);
            storyLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(storyLabel);

            typingTimer = new Timer();
            typingTimer.Interval = 50; // Adjust the speed of the typing effect
            typingTimer.Tick += TypingTimer_Tick;

            this.KeyDown += StoryTextPanel_KeyDown;
            this.PreviewKeyDown += StoryTextPanel_PreviewKeyDown;
        }

        public void StartStory(IEnumerable<(string text, Image image)> storyParts)
        {
            storyQueue = new Queue<(string text, Image image)>(storyParts);
            NextSentence();
        }

        private void NextSentence()
        {
            if (storyQueue.Count > 0)
            {
                var storyPart = storyQueue.Dequeue();
                currentSentence = storyPart.text;
                storyPictureBox.Image = storyPart.image;
                charIndex = 0;
                storyLabel.Text = "";
                typingTimer.Start();
            }
            else
            {
                StoryCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void TypingTimer_Tick(object sender, EventArgs e)
        {
            if (charIndex < currentSentence.Length)
            {
                storyLabel.Text += currentSentence[charIndex];
                charIndex++;
            }
            else
            {
                typingTimer.Stop();
            }
        }

        private void StoryTextPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = true;
            }
        }

        private void StoryTextPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !typingTimer.Enabled)
            {
                NextSentence();
            }
        }
    }
}
