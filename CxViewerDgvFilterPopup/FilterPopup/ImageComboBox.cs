using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace DgvFilterPopup.FilterPopup
{
    public class LegendItem
    {
        public int Index;
        public string Data;
    }

    public partial class ImageComboBox : System.Windows.Forms.ComboBox
    {
        public ImageComboBox()
        {
            InitializeComponent();

            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.DrawItem += new DrawItemEventHandler(LegendComboBox_DrawItem); 
        }

        void LegendComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {            
            e.DrawBackground();
            LegendItem currentItem = null;
            Rectangle rect = new Rectangle(2, e.Bounds.Top + 2,
                                           e.Bounds.Height, e.Bounds.Height - 4);
            try
            {
                currentItem = (LegendItem)this.Items[e.Index];
            }
            catch (InvalidCastException)
            {
                //If the item in the combo box is not of type LegendItem,
                //then we just draw the item without the legend colour.
                e.Graphics.DrawString(this.Items[e.Index].ToString(), this.Font,
                    new SolidBrush(this.ForeColor), e.Bounds);

                return;
            }

            //Draw rectangle with legend colour.
            //e.Graphics.FillRectangle(new SolidBrush(currentItem.Color), rect);
            //e.Graphics.DrawString(currentItem.Data.ToString(), this.Font,
            //   new SolidBrush(this.ForeColor),
            //   new Rectangle(e.Bounds.X + rect.Width + 2,
            //                   e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            if (currentItem.Index == 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.White), rect);
                e.Graphics.DrawString(currentItem.Data.ToString(), this.Font,
                   new SolidBrush(this.ForeColor),
                   new Rectangle(e.Bounds.X + rect.Width + 2,
                                   e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));                
            }
            else
            {
                DrawCustomItem(e.Graphics, currentItem.Index, e.Bounds.X + rect.Width + 2, e.Bounds.Y);
            }
        }

        void DrawCustomItem(Graphics g, int index, int x, int y)
        {
            MemoryStream ms = new MemoryStream();
            switch (index)
            {
                case 1:
                    ResourceImages.Enabled.Save(ms, ImageFormat.Png);
                    break;
                case 2:
                    ResourceImages.Disabled.Save(ms, ImageFormat.Png);
                    break;
            }
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

            //flip the image around its center
            using (System.Drawing.Drawing2D.Matrix m = g.Transform)
            {
                using (System.Drawing.Drawing2D.Matrix saveM = m.Clone())
                {
                    float c = (float)y;

                    using (System.Drawing.Drawing2D.Matrix m2 = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 0, 2 * c))
                        m.Multiply(m2);

                    //g.Transform = m;
                    g.DrawImage(image, new PointF((float)(x), (float)(y)));
                    g.Transform = saveM;

                }
            }
        }
    }
}
