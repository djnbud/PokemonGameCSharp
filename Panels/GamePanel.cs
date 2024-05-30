using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace PokemonGame.Panels
{
    public class GamePanel : Panel
    {
        private Player player;
        private Image backgroundImage;
        private Image foregroundImage;
        private Timer gameTimer;
        private Point screenCenter;
        private bool[,] boundaries;
        //private DateTime lastAnimationTime;

        public GamePanel(GameDetails details)
        {
            
       
            this.DoubleBuffered = true; // Prevent flickering
            InitializeGame(details);
        }

        private void InitializeGame(GameDetails details)
        {
            this.Size = new Size(800, 450); // Adjust size as needed
            this.BackColor = Color.Green;
            backgroundImage = Image.FromFile(details.BackgroundImagePath);
            foregroundImage = Image.FromFile(details.ForegroundImagePath);

             var collisionData = LoadCollisionData(details.CollisionDataPath);
            boundaries = CreateBoundariesArray(collisionData, details.MapWidth, details.MapHeight, 48, 48);

            //playerSprite = new Sprite("Assets/player/player_spritesheet.png", 48, 68, 4, 5); // Adjust as needed
            player = new Player(boundaries);
            player.Position = new Point(details.PlayerPosition.X, details.PlayerPosition.Y);

            screenCenter = new Point((this.Width - player.FrameWidth) / 2, (this.Height - player.FrameHeight) / 2);

            gameTimer = new Timer();
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            //lastAnimationTime = DateTime.Now;

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            player.Update();
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

            // Calculate offsets for background and foreground based on player position
            int offsetX = screenCenter.X - player.Position.X;
            int offsetY = screenCenter.Y - player.Position.Y;

            Graphics g = e.Graphics;

            // Draw background
            if (backgroundImage != null)
            {
                g.DrawImage(backgroundImage, offsetX, offsetY, backgroundImage.Width, backgroundImage.Height);
            }

            // Draw the player
            player.Draw(g, screenCenter);

            // Draw foreground
            if (foregroundImage != null)
            {
                e.Graphics.DrawImage(foregroundImage, offsetX, offsetY, foregroundImage.Width, foregroundImage.Height);
            }

            // Draw other game elements here, adjusting their position relative to the player's world position
            // For example:
            // Point enemyScreenPos = new Point(enemy.WorldPosition.X - player.Position.X + screenCenter.X,
            //                                  enemy.WorldPosition.Y - player.Position.Y + screenCenter.Y);
            // enemy.Draw(g, enemyScreenPos);
        }

        private CollisionData LoadCollisionData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<CollisionData>(json);
        }

        private bool[,] CreateBoundariesArray(CollisionData collisionData, int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            bool[,] boundaries = new bool[mapWidth, mapHeight];
            for (int i = 0; i < collisionData.collisions.Length; i++)
            {
                int x = i % mapWidth;
                int y = i / mapWidth;
                boundaries[x, y] = collisionData.collisions[i] == 1025;
            }
            return boundaries;
        }
    }
}
