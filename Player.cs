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
        public Player() : base("Assets/player/player_spritesheet.png", 48, 68, 4, 100) { }
        public void Update()
        {
            Point newPosition = Position;

            if (MovingUp) newPosition.Y -= Speed;
            if (MovingDown) newPosition.Y += Speed;
            if (MovingLeft) newPosition.X -= Speed;
            if (MovingRight) newPosition.X += Speed;

            Position = newPosition;

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

        public override void Draw(Graphics g)
        {
            int frameWidth = spriteSheet.Width / totalFrames;
            int frameHeight = spriteSheet.Height / 4; // Assuming 4 rows for 4 directions

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, (int)Direction * frameHeight, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle(Position.X, Position.Y, frameWidth, frameHeight);

            g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }

    }
}