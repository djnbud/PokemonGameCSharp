using System;
using System.Drawing;
using System.Windows.Forms;

namespace PokemonGame.Panels
{
    public class GamePanel : Panel
    {
        private Player player;
        private Timer gameTimer;
        //private DateTime lastAnimationTime;

        public GamePanel()
        {
            this.DoubleBuffered = true; // Prevent flickering
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Size = new Size(800, 600); // Adjust size as needed
            this.BackColor = Color.Green;

            //playerSprite = new Sprite("Assets/player/player_spritesheet.png", 48, 68, 4, 5); // Adjust as needed
            player = new Player();
            player.Position = new Point((this.Width - player.FrameWidth) / 2, (this.Height - player.FrameHeight) / 2);

            gameTimer = new Timer();
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            //lastAnimationTime = DateTime.Now;

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            player.Update();
            // Update animation frame
            /*if ((DateTime.Now - lastAnimationTime).TotalMilliseconds > animationSpeed)
            {
                player.UpdateAnimation();

                lastAnimationTime = DateTime.Now;
            }*/

            Invalidate(); // Request a redraw of the panel
        }

        public void HandleKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.W:
                    player.MovingUp = true;
                    player.Direction = PlayerDirection.Up;
                    break;
                case Keys.S:
                    player.MovingDown = true;
                    player.Direction = PlayerDirection.Down;
                    break;
                case Keys.A:
                    player.MovingLeft = true;
                    player.Direction = PlayerDirection.Left;
                    break;
                case Keys.D:
                    player.MovingRight = true;
                    player.Direction = PlayerDirection.Right;
                    break;
            }
        }

        public void HandleKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.W:
                    player.MovingUp = false;
                    break;
                case Keys.S:
                    player.MovingDown = false;
                    break;
                case Keys.A:
                    player.MovingLeft = false;
                    break;
                case Keys.D:
                    player.MovingRight = false;
                    break;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            player.Draw(e.Graphics);
        }
    }
}
