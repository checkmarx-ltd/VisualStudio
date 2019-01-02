using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ColorButton
{
    [Description("Color Button Control")]
    [ToolboxBitmap(typeof(Button))]
    [Designer(typeof(ColorButtonDesigner))]
    public class ColorButton : System.Windows.Forms.Control
    {
        private enum _States
        {
            Normal,
            MouseOver,
            Clicked,
            Selected
        }

        public enum SmoothingQualities
        {
            None,
            HighSpeed,
            AntiAlias,
            HighQuality
        }

        public enum ButtonStyles
        {
            Rectangle,
            Ellipse
        }

        public enum GradientStyles
        {
            Horizontal,
            Vertical,
            ForwardDiagonal,
            BackwardDiagonal
        }

        // default values
        private bool _Active = true;
        private _States _State = _States.Normal;

        private GradientStyles _GradientStyle = GradientStyles.Vertical;
        private ButtonStyles _ButtonStyle = ButtonStyles.Rectangle;
        private SmoothingQualities _SmoothingQuality = SmoothingQualities.AntiAlias;

        private Color _NormalBorderColor = Color.Gray;
        private Color _NormalColorA = Color.Gray;
        private Color _NormalColorB = Color.Gray;

        private Color _HoverBorderColor = Color.DarkGray;
        private Color _HoverColorA = Color.MediumVioletRed;
        private Color _HoverColorB = Color.White;

        public ColorButton()
        {
            // initiate button size and font
            base.Size = new Size(200, 28);
            base.Font = new Font("Verdana", 8, FontStyle.Bold);
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer |
                ControlStyles.Selectable |
                ControlStyles.UserMouse,
                true
                );
        }

        public void SetNormalState()
        {
            _State = _States.Normal;
        }

        [Description("Smoothing Quality"), Category("ColorButton")]
        public SmoothingQualities SmoothingQuality
        {
            get
            {
                return _SmoothingQuality;
            }
            set
            {
                _SmoothingQuality = value;
                this.Invalidate();
            }
        }

        [Description("Gradient Style"), Category("ColorButton")]
        public GradientStyles GradientStyle
        {
            get
            {
                return _GradientStyle;
            }
            set
            {
                _GradientStyle = value;
                this.Invalidate();
            }

        }

        [Description("Button Style"), Category("ColorButton")]
        public ButtonStyles ButtonStyle
        {
            get
            {
                return _ButtonStyle;
            }
            set
            {
                _ButtonStyle = value;
                this.Invalidate();
            }
        }

        [Description("The normal border color"), Category("ColorButton")]
        public Color NormalBorderColor
        {
            get
            {
                return _NormalBorderColor;
            }
            set
            {
                _NormalBorderColor = value;
                this.Invalidate();
            }

        }

        [Description("The hover border color"), Category("ColorButton")]
        public Color HoverBorderColor
        {
            get
            {
                return _HoverBorderColor;
            }
            set
            {
                _HoverBorderColor = value;
                this.Invalidate();
            }

        }

        [Description("Normal color A"), Category("ColorButton")]
        public Color NormalColorA
        {
            get
            {
                return _NormalColorA;
            }
            set
            {
                _NormalColorA = value;
                this.Invalidate();
            }
        }

        [Description("Normal color B"), Category("ColorButton")]
        public Color NormalColorB
        {
            get
            {
                return _NormalColorB;
            }
            set
            {
                _NormalColorB = value;
                this.Invalidate();
            }
        }

        [Description("Hover color A"), Category("ColorButton")]
        public Color HoverColorA
        {
            get
            {
                return _HoverColorA;
            }
            set
            {
                _HoverColorA = value;
                this.Invalidate();
            }
        }

        [Description("Hover color B"), Category("ColorButton")]
        public Color HoverColorB
        {
            get
            {
                return _HoverColorB;
            }
            set
            {
                _HoverColorB = value;
                this.Invalidate();
            }
        }

        [Description("Enable the button?"), Category("ColorButton")]
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
                this.Invalidate();
            }
        }

        // to make sure the control is invalidated(repainted) when the text is changed
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            //
            // set SmoothingMode
            //
            switch (_SmoothingQuality)
            {
                case SmoothingQualities.None:
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                    break;
                case SmoothingQualities.HighSpeed:
                    e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    break;
                case SmoothingQualities.AntiAlias:
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    break;
                case SmoothingQualities.HighQuality:
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    break;
            }

            SizeF textSize = e.Graphics.MeasureString(this.Text, base.Font);
            int textX = (int)(base.Size.Width / 2) - (int)(textSize.Width / 2);
            int textY = (int)(base.Size.Height / 2) - (int)(textSize.Height / 2);

            if (IsSelected)
            {
                _State = _States.Selected;
                DrawActiveButtonByState(e.Graphics, _State, ClientRectangle.X, ClientRectangle.Y);
                e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX + 1, textY + 1);
            }
            else
            {
                if (_Active)
                {
                    DrawActiveButtonByState(e.Graphics, _State, ClientRectangle.X, ClientRectangle.Y);
                    e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX + 1, textY + 1);
                }
                else
                {
                    DrawNotActiveButtonByState(e.Graphics, _State, ClientRectangle.X + 1, ClientRectangle.Y);
                    e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(_NormalColorA), textX, textY);
                }
            }
        }

        void DrawNotActiveButtonByState(Graphics g, _States state, int x, int y)
        {
            DrawActiveButtonByState(g, state, x, y);
        }

        void DrawActiveButtonByState(Graphics g, _States state, int x, int y)
        {
            MemoryStream ms = new MemoryStream();
            switch (state)
            {
                case _States.Normal:
                    CxViewerResources.PathButton.Save(ms, ImageFormat.Png);
                    break;
                case _States.MouseOver:
                    CxViewerResources.PathActiveButton.Save(ms, ImageFormat.Png);
                    break;
                case _States.Clicked:
                    CxViewerResources.PathActiveButton.Save(ms, ImageFormat.Png);
                    break;
                case _States.Selected:
                    CxViewerResources.PathSelectedButton.Save(ms, ImageFormat.Png);
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
                    g.DrawImage(image, (float)(x), (float)(y), 200, 28);
                    g.Transform = saveM;

                }
            }
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            if (_Active)
            {
                _State = _States.Normal;
                this.Invalidate();
                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            if (_Active)
            {
                _State = _States.MouseOver;
                this.Invalidate();
                base.OnMouseEnter(e);
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (_Active)
            {
                _State = _States.MouseOver;
                this.Invalidate();
                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (_Active)
            {
                _State = _States.Clicked;
                this.Invalidate();
                base.OnMouseDown(e);
            }
        }

        protected override void OnClick(System.EventArgs e)
        {
            // prevent click when button is inactive
            if (_Active)
            {
                if (_State == _States.Clicked)
                {
                    base.OnClick(e);
                }
            }
        }
    }
}
