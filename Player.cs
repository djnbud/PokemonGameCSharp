using System;
using System.Drawing;
using PokemonGame.Misc;

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
        private CollisionManager collisionManager;
        public Player(CollisionManager collisionManager) : base("Assets/player/player_spritesheet.png", 48, 68, 4, 100) 
        { 
            this.collisionManager = collisionManager;
        }
        public void Update()
        {
            Point newPosition = Position;

            if (MovingUp) newPosition.Y -= Speed;
            if (MovingDown) newPosition.Y += Speed;
            if (MovingLeft) newPosition.X -= Speed;
            if (MovingRight) newPosition.X += Speed;

            // Check for collision
            if (!collisionManager.IsCollision(newPosition, FrameWidth, FrameHeight))
            {
                Position = newPosition;
            }

            // Check for map swap
            if (collisionManager.IsOutsideInside(newPosition, FrameWidth, FrameHeight))
            {
                collisionManager.SwapMap(newPosition, !collisionManager.gameDetails.Inside);
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