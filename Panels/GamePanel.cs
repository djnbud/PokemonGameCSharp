using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using PokemonGame.Misc;

namespace PokemonGame.Panels
{
    public class GamePanel : Panel
    {
        private Player player;
        private Image backgroundImage;
        private Image foregroundImage;
        private Timer gameTimer;
        private Point screenCenter;
        private BoundaryData boundaryData;
        private CollisionData collisionData;
        private GameDetails gameDetails;
        private CollisionManager collisionManager;
        //private DateTime lastAnimationTime;

        public GamePanel(GameDetails details)
        {
            gameDetails = details;
            this.DoubleBuffered = true; // Prevent flickering
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Size = new Size(800, 450); // Adjust size as needed
            this.BackColor = Color.Black;
            LoadBackgroundAndForegroundImages();

            collisionData = LoadCollisionData(gameDetails.CollisionDataPath);

            boundaryData = CreateBoundariesArray(collisionData, gameDetails.MapWidth, gameDetails.MapHeight, 48, 48);

            collisionManager = new CollisionManager(
                boundaryData,
                gameDetails,
                this
            );

            //playerSprite = new Sprite("Assets/player/player_spritesheet.png", 48, 68, 4, 5); // Adjust as needed
            player = new Player(collisionManager);
            player.Position = new Point(gameDetails.PlayerPosition.X, gameDetails.PlayerPosition.Y);

            screenCenter = new Point((this.Width - player.FrameWidth) / 2, (this.Height - player.FrameHeight) / 2);

            gameTimer = new Timer();
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            //lastAnimationTime = DateTime.Now;

        }

        public void Reload(int currentCollisionId)
        {
            // Update background and foreground images
            LoadBackgroundAndForegroundImages();

            // Update collision arrays
            collisionData = LoadCollisionData(gameDetails.CollisionDataPath);
            

            boundaryData = CreateBoundariesArray(collisionData, gameDetails.MapWidth, gameDetails.MapHeight, 48, 48);
            collisionManager = new CollisionManager(boundaryData, gameDetails,
                this);

            player = new Player(collisionManager);
            player.Position = collisionManager.newPlayerLocation(currentCollisionId);//new Point(gameDetails.PlayerPosition.X, gameDetails.PlayerPosition.Y);


            // Reposition player if necessary
            //if (gameDetails.Inside)
            //{
                // Position player at corresponding outside tile position
                // Implement logic for corresponding outside tile position
            //}
            //else
            //{
                // Position player at corresponding inside tile position
                // Implement logic for corresponding inside tile position
            //}

            Invalidate(); // Request a redraw of the panel
        }

        private void LoadBackgroundAndForegroundImages()
        {
            if (!string.IsNullOrEmpty(gameDetails.BackgroundImagePath))
            {
                backgroundImage = Image.FromFile(gameDetails.BackgroundImagePath);
            }

            if (!string.IsNullOrEmpty(gameDetails.ForegroundImagePath))
            {
                foregroundImage = Image.FromFile(gameDetails.ForegroundImagePath);
            }
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

            using (var brush = new SolidBrush(Color.Red))
            {
                for (int x = 0; x < boundaryData.OutsideInside.GetLength(0); x++)
                {
                    for (int y = 0; y < boundaryData.OutsideInside.GetLength(1); y++)
                    {
                        if (boundaryData.OutsideInside[x, y])
                        {
                            e.Graphics.FillRectangle(brush, offsetX + x * 48, offsetY + y * 48, 48, 48);
                        }
                    }
                }
            }

            using (var brush = new SolidBrush(Color.Blue))
            {
                for (int x = 0; x < boundaryData.Upstairs.GetLength(0); x++)
                {
                    for (int y = 0; y < boundaryData.Upstairs.GetLength(1); y++)
                    {
                        if (boundaryData.Upstairs[x, y])
                        {
                            e.Graphics.FillRectangle(brush, offsetX + x * 48, offsetY + y * 48, 48, 48);
                        }
                    }
                }
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

        private BoundaryData CreateBoundariesArray(CollisionData collisionData, int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            bool[,] outsideInside = new bool[mapWidth, mapHeight];
            bool[,] boundaries = new bool[mapWidth, mapHeight];
            bool[,] upstairs = new bool[mapWidth, mapHeight];
            int[,] collisionId = new int[mapWidth, mapHeight];
            for (int i = 0; i < collisionData.collisions.Length; i++)
            {
                int x = i % mapWidth;
                int y = i / mapWidth;
                
                outsideInside[x, y] = collisionData.collisions[i] > 400 && collisionData.collisions[i] < 500;
                collisionId[x, y] = collisionData.collisions[i];
                boundaries[x, y] = collisionData.collisions[i] == 27;
                upstairs[x, y] = collisionData.collisions[i] > 800 && collisionData.collisions[i] < 900;
                
            }
            return new BoundaryData {
                OutsideInside = outsideInside,
                CollisionId = collisionId,
                Boundaries = boundaries,
                Upstairs = upstairs
                };
        }
    }
}
