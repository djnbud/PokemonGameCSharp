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
            BackgroundImage = defaultImage;
            BackgroundImageLayout = ImageLayout.Stretch; // Adjust the layout mode if necessary
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            Cursor = Cursors.Hand;
        }
        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            //if (HoverImage == null) return;
            //if (PlainImage == null) PlainImage = base.Image;
            //base.Image = HoverImage;
            BackgroundImage = HoverImage;
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            //if (HoverImage == null) return;
            //base.Image = PlainImage;
            BackgroundImage = PlainImage;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //if (PressedImage == null) return;
            //if (PlainImage == null) PlainImage = base.Image;
            //base.Image = PressedImage;
            BackgroundImage = PressedImage;
        }
    }
}
