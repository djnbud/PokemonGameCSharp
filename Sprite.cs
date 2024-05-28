using System;
using System.Drawing;

namespace PokemonGame
{
    public class Sprite
    {
        protected Image spriteSheet;
        protected int frameWidth;
        protected int frameHeight;
        protected int currentFrame;
        protected int totalFrames;
        protected int currentRow;

        protected bool isAnimating;
        public Point Position { get; set; }
        protected int frameDelay; // Delay in milliseconds between frames
        protected int frameTimer; // Timer to count the delay

        public Sprite(string filePath, int frameWidth, int frameHeight, int totalFrames, int frameDelay = 100)
        {
            try
            {
                this.spriteSheet = Image.FromFile(filePath);
                Console.WriteLine("Sprite loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading sprite: {ex.Message}");
                throw; // Optionally rethrow or handle it appropriately
            }
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.totalFrames = totalFrames;
            this.currentFrame = 0;
            this.currentRow = 0;
            this.isAnimating = false;
            this.frameDelay = frameDelay;
            this.frameTimer = 0;
        }

        protected virtual void StartAnimation(int directionRow)
        {
            this.currentRow = directionRow;
            this.isAnimating = true;
        }

        protected virtual void StopAnimation()
        {
            this.isAnimating = false;
            this.currentFrame = 0; // Reset to the first frame
        }

        protected virtual void UpdateAnimation()
        {
            if (this.isAnimating)
            {
                frameTimer += 16; // Assuming the game timer ticks every 16ms (60 FPS)

                if (frameTimer >= frameDelay)
                {
                    currentFrame++;
                    if (currentFrame >= totalFrames)
                        currentFrame = 0;

                    frameTimer = 0; // Reset the timer
                }
            }
        }

        public virtual void Draw(Graphics g)
        {
            int frameWidth = spriteSheet.Width / totalFrames;
            int frameHeight = spriteSheet.Height / 4; // Assuming 4 rows for 4 directions

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
            Rectangle destRect = new Rectangle(Position.X, Position.Y, frameWidth, frameHeight);

            g.DrawImage(spriteSheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }


        public int FrameWidth => frameWidth;
        public int FrameHeight => frameHeight;
    }
}
