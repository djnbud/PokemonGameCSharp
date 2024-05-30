using System;
using System.Drawing;

namespace PokemonGame
{
    public enum PlayerDirection { Down, Up, Left, Right }
    public class Player : Sprite
    {
        public int Speed { get; set; } = 5;
        public PlayerDirection Direction { get; set; }
        public bool MovingUp { get; set; } = false;
        public bool MovingDown { get; set; } = false;
        public bool MovingLeft { get; set; } = false;
        public bool MovingRight { get; set; } = false;
        public bool IsMoving { get; set; } = false;
        private bool[,] boundaries;
        public Player(bool[,] boundaries) : base("Assets/player/player_spritesheet.png", 48, 68, 4, 100) 
        { 
            this.boundaries = boundaries;
        }
        public void Update()
        {
            Point newPosition = Position;

            if (MovingUp) newPosition.Y -= Speed;
            if (MovingDown) newPosition.Y += Speed;
            if (MovingLeft) newPosition.X -= Speed;
            if (MovingRight) newPosition.X += Speed;

            // Check for collision
            if (!IsCollision(newPosition))
            {
                Position = newPosition;
            }

            if (!MovingUp && !MovingDown && !MovingLeft && !MovingRight)
            {
                StopAnimation();
            }
            else
            {
                StartAnimation(0);
            }
            UpdateAnimation();
        }

        private bool IsCollision(Point position)
        {
            int playerWidth = FrameWidth; // Full width of the player
            int playerHeight = FrameHeight / 3; // Only consider the bottom half of the player

            int tileX = (position.X + playerWidth) / 48;
            int tileY = (position.Y + playerHeight * 2) / 48;

            // Check if any part of the player's bounds intersects with a boundary tile
            for (int x = tileX - 1; x <= tileX; x++)
            {
                for (int y = tileY; y <= tileY + 1; y++)
                {
                    if (x < 0 || y < 0 || x >= boundaries.GetLength(0) || y >= boundaries.GetLength(1))
                    {
                        return true; // Out of bounds
                    }

                    if (boundaries[x, y])
                    {
                        return true; // Collision detected
                    }
                }
            }

            return false; // No collision detected
        }


        public void Draw(Graphics g, Point screenPosition)
        {
            //int frameWidth = spriteSheet.Width / totalFrames;
            //int frameHeight = spriteSheet.Height / 4; // Assuming 4 rows for 4 directions

            Rectangle sourceRect = new Rectangle(currentFrame * FrameWidth, (int)Direction * FrameHeight, FrameWidth, FrameHeight);
            Rectangle destRect = new Rectangle(screenPosition.X, screenPosition.Y, FrameWidth, FrameHeight);

            g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }

    }
}