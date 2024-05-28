using System;
using System.Drawing;
using System.Windows.Forms;

namespace PokemonGame.Misc
{
    public class ImageButton : Button
    {

        public Image HoverImage { get; set; }
        public Image PlainImage { get; set; }
        public Image PressedImage { get; set; }

        public ImageButton(Image defaultImage)
        {
            this.BackgroundImage = defaultImage;
            this.BackgroundImageLayout = ImageLayout.Stretch; // Adjust the layout mode if necessary
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;
        }
        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            //if (HoverImage == null) return;
            //if (PlainImage == null) PlainImage = base.Image;
            //base.Image = HoverImage;
            this.BackgroundImage = HoverImage;
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            //if (HoverImage == null) return;
            //base.Image = PlainImage;
            this.BackgroundImage = PlainImage;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //if (PressedImage == null) return;
            //if (PlainImage == null) PlainImage = base.Image;
            //base.Image = PressedImage;
            this.BackgroundImage = PressedImage;
        }
    }
}
