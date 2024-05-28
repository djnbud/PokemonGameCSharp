using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PokemonGame.Controls
{
    public class PokemonTextInput : UserControl
    {
        private List<char> enteredText;
        private int maxLength;
        private int cursorPosition;
        private Timer cursorBlinkTimer;
        private bool isCursorVisible;

        public PokemonTextInput(int maxLength = 10)
        {
            this.maxLength = maxLength;
            this.enteredText = new List<char>();
            this.cursorPosition = 0;
            this.isCursorVisible = true;

            this.Width = 200;  // Adjust as needed
            this.Height = 50;  // Adjust as needed
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Font = new Font("Courier New", 14);
            this.DoubleBuffered = true;

            this.cursorBlinkTimer = new Timer();
            this.cursorBlinkTimer.Interval = 500; // Blink every 500ms
            this.cursorBlinkTimer.Tick += CursorBlinkTimer_Tick;
            this.cursorBlinkTimer.Start();

            this.KeyPress += PokemonTextInput_KeyPress;
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }

        private void CursorBlinkTimer_Tick(object sender, EventArgs e)
        {
            isCursorVisible = !isCursorVisible;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush textBrush = new SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(new string(enteredText.ToArray()), this.Font, textBrush, 5, 5);
            }

            if (isCursorVisible && this.Focused)
            {
                int cursorX = (int)e.Graphics.MeasureString(new string(enteredText.Take(cursorPosition).ToArray()), this.Font).Width;
                using (Brush cursorBrush = new SolidBrush(this.ForeColor))
                {
                    e.Graphics.FillRectangle(cursorBrush, cursorX + 5, 5, 2, this.Font.Height);
                }
            }
        }

        private void PokemonTextInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') // Handle backspace
            {
                if (enteredText.Count > 0 && cursorPosition > 0)
                {
                    enteredText.RemoveAt(cursorPosition - 1);
                    cursorPosition--;
                    this.Invalidate();
                }
            }
            else if (e.KeyChar == (char)Keys.Enter) // Handle enter key (confirm input)
            {
                // You can raise an event or handle the input confirmation here
                OnInputConfirmed(new string(enteredText.ToArray()));
            }
            else if (char.IsLetterOrDigit(e.KeyChar) && enteredText.Count < maxLength) // Handle alphanumeric input
            {
                enteredText.Insert(cursorPosition, e.KeyChar);
                cursorPosition++;
                this.Invalidate();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Left && cursorPosition > 0)
            {
                cursorPosition--;
                this.Invalidate();
            }
            else if (e.KeyCode == Keys.Right && cursorPosition < enteredText.Count)
            {
                cursorPosition++;
                this.Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            cursorBlinkTimer.Start();
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            cursorBlinkTimer.Stop();
            this.Invalidate();
        }

        public event EventHandler<string> InputConfirmed;

        protected virtual void OnInputConfirmed(string input)
        {
            InputConfirmed?.Invoke(this, input);
        }
    }
}
