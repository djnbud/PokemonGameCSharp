using System;
using System.Drawing;
using System.Windows.Forms;
using PokemonGame.Controls;

namespace PokemonGame.Panels
{
    public class CharacterCreationPanel : Panel
    {
        private PokemonTextInput nameInput;
        public event EventHandler<string> NameConfirmed;

        public CharacterCreationPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(800, 450); // Adjust size as needed
            this.BackColor = Color.Black;

            Label nameLabel = new Label();
            nameLabel.Text = "Enter your name:";
            nameLabel.ForeColor = Color.White;
            nameLabel.Location = new Point(100, 100);
            nameLabel.Size = new Size(200, 30);
            this.Controls.Add(nameLabel);

            nameInput = new PokemonTextInput(10); // Max length of 10 characters
            nameInput.Location = new Point(100, 140);
            nameInput.Size = new Size(200, 30);
            nameInput.InputConfirmed += NameInput_InputConfirmed;
            this.Controls.Add(nameInput);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                nameInput.Focus();
            }
        }

        private void NameInput_InputConfirmed(object sender, string playerName)
        {
            NameConfirmed?.Invoke(this, playerName);
        }

    }
}
