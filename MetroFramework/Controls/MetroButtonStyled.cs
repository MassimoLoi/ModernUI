using MetroFramework.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Design;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button)), ToolboxItem(false)]
    public class MetroButtonStyled : Button, IMetroControl
    {
        #region "   Fields   "
        //status
        private bool isHotTracking;
        private bool isPressed;

        //colors
        Color tBorder;
        Color tBottomColorBegin;
        Color tBottomColorEnd;
        Color Textcol;
        Font controlFont;
        #endregion

        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Blue;
        [Category("Metro Appearance")]
        public MetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Light;
        [Category("Metro Appearance")]
        public MetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region "   Properties   "
        Boolean _useAlternateColor = false;
        [Browsable(true), Category("Appearance-Extended")]
        [DefaultValue("false")]
        public Boolean UseAlternateColor
        {
            get { return _useAlternateColor; }
            set { _useAlternateColor = value; InitColors(); Invalidate(); }
        }
        #endregion

        #region "   Constructor   "

        public MetroButtonStyled()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            this.DoubleBuffered = true;
        }

        #endregion

        #region "   Events - Overrides   "

        #region "   Mouse   "

        protected override void OnMouseLeave(EventArgs e)
        {
            ResumeNormalButton();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            PressButton();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            ReleaseButton();
            if (DisplayRectangle.Contains(mevent.Location))
            {
                HighlightButton();
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (Enabled && (mevent.Button & MouseButtons.Left) == MouseButtons.Left && !ClientRectangle.Contains(mevent.Location))
            {
                isHotTracking = false;
                ReleaseButton();
            }

            base.OnMouseMove(mevent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            HighlightButton();
            base.OnMouseEnter(e);
        }
        #endregion

        #region "   Keyboard   "
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space || kevent.KeyCode == Keys.Return)
            {
                PressButton();
            }
            base.OnKeyDown(kevent);
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space || kevent.KeyCode == Keys.Return)
            {
                ReleaseButton();
                if (IsDefault)
                {
                    HighlightButton();
                }
            }
            base.OnKeyUp(kevent);
        }
        #endregion

        #region "   Focus   "
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }
        #endregion

        #region "   Misc   "
        protected override void OnResize(EventArgs e)
        {
            SetControlSizes();
            this.Invalidate();
            base.OnResize(e);
        }
        protected override void OnCreateControl()
        {
            SuspendLayout();
            base.OnCreateControl();
            ResumeLayout();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }
        #endregion

        #region "   Supporting Subs   "
        private Rectangle buttonRect;
        private int rectCornerRadius;
        private float rectOutlineWidth;

        private void SetControlSizes()
        {
            int scalingDividend = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
            buttonRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            rectCornerRadius = Math.Max(1, scalingDividend / 10);
            rectOutlineWidth = Math.Max(1, scalingDividend / 50);
        }
        #endregion

        #endregion

        #region "   Graphical Path(s)   "
        public static GraphicsPath GetRoundedRect(RectangleF r, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.StartFigure();
            r = new RectangleF(r.Left, r.Top, r.Width, r.Height);
            if (radius <= 0.0F || radius <= 0.0F)
            {
                gp.AddRectangle(r);
            }
            else
            {
                gp.AddArc((float)r.X, (float)r.Y, radius, radius, 180, 90);
                gp.AddArc((float)r.Right - radius, (float)r.Y, radius - 1, radius, 270, 90);
                gp.AddArc((float)r.Right - radius, ((float)r.Bottom) - radius - 1, radius - 1, radius, 0, 90);
                gp.AddArc((float)r.X, ((float)r.Bottom) - radius - 1, radius - 1, radius, 90, 90);
            }
            gp.CloseFigure();
            return gp;
        }

        #endregion

        #region "   Overrides Paint Events   "
        protected override void OnPaint(PaintEventArgs e)
        {
            ButtonRenderer.DrawParentBackground(e.Graphics, e.ClipRectangle, this);
            //this.PaintTransparentBackground(e.Graphics, e.ClipRectangle);

            //now let's we begin painting
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!this.Enabled)
            {
                tBorder = this.colorTable.ButtonDisabledBorder;
                tBottomColorBegin = this.colorTable.ButtonDisabledBackgroundCenter;
                tBottomColorEnd = this.colorTable.ButtonDisabledBackgroundOuter;
                Textcol = this.colorTable.ButtonDisabledTextColor;
            }
            else
            {
                //Normal State
                tBorder = this.colorTable.ButtonNormalBorder;
                tBottomColorBegin = this.colorTable.ButtonNormalBackgroundCenter;
                tBottomColorEnd = this.colorTable.ButtonNormalBackgroundOuter;
                Textcol = this.colorTable.ButtonNormalTextColor;
                //hot tracking - mouse over
                if (isHotTracking)
                {
                    tBorder = this.colorTable.ButtonMouseOverBorder;
                    tBottomColorBegin = this.colorTable.ButtonMouseOverBackgroundCenter;
                    tBottomColorEnd = this.colorTable.ButtonMouseOverBackgroundOuter;
                    Textcol = this.colorTable.ButtonMouseOverTextColor;
                }
                //pressed
                if (isPressed)
                {
                    tBorder = this.colorTable.ButtonSelectedBorder;
                    tBottomColorBegin = this.colorTable.ButtonSelectedBackgroundCenter;
                    tBottomColorEnd = this.colorTable.ButtonSelectedBackgroundOuter;
                    Textcol = this.colorTable.ButtonSelectedTextColor;
                }
                //focused but not pressed
                if ((this.Focused) && (!isPressed))
                {
                    tBorder = this.colorTable.ButtonFocusedBorder;
                    tBottomColorBegin = this.colorTable.ButtonFocusedBackgroundCenter;
                    tBottomColorEnd = this.colorTable.ButtonFocusedBackgroundOuter;
                    Textcol = this.colorTable.ButtonFocusedTextColor;
                }
                //focused and mouse over
                if ((this.Focused) && (isHotTracking))
                {
                    tBorder = this.colorTable.ButtonMouseOverBorder;
                    tBottomColorBegin = this.colorTable.ButtonMouseOverBackgroundCenter;
                    tBottomColorEnd = this.colorTable.ButtonMouseOverBackgroundOuter;
                    Textcol = this.colorTable.ButtonMouseOverTextColor;
                }
            }

            //ClearType
            try
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            //paint background
            PaintBackground(e, g, tBorder, tBottomColorBegin, tBottomColorEnd);

            //paint Text
            DrawTextAndImage(g, Textcol);
        }
        #endregion

        #region "   Paint Background   "
        protected void PaintBackground(PaintEventArgs e, Graphics g, Color tBorder, Color tBottomColorBegin, Color tBottomColorEnd)
        {
            //Set button radius
            int _roundedRadiusX = rectCornerRadius;

            //define control rectangle
            Rectangle r = buttonRect;

            //rectangle for gradient, half upper and lower
            RectangleF halfup = new RectangleF(r.Left, r.Top, r.Width, r.Height);
            RectangleF halfdown = new RectangleF(r.Left, r.Top + (r.Height / 2) - 1, r.Width, r.Height);

            //start Background
            //for half upper, we paint using linear gradient

            using (GraphicsPath thePath = GetRoundedRect(r, _roundedRadiusX))
            {
                Blend blend = new Blend(4);
                using (LinearGradientBrush lgb = new LinearGradientBrush(halfup, tBottomColorEnd, tBottomColorBegin, 90f, true))
                {
                    blend.Positions = new float[] { 0, 0.18f, 0.35f, 1f };
                    blend.Factors = new float[] { 0f, .4f, .9f, 1f };
                    lgb.Blend = blend;
                    g.FillPath(lgb, thePath);
                }

                //for half lower, we paint using radial gradient
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddEllipse(halfdown); //make it radial
                    using (PathGradientBrush gradient = new PathGradientBrush(p))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(Convert.ToSingle(halfdown.Left + halfdown.Width / 2), Convert.ToSingle(halfdown.Bottom));
                        gradient.CenterColor = tBottomColorEnd;
                        gradient.SurroundColors = new Color[] { tBottomColorBegin };

                        blend = new Blend(4);
                        blend.Positions = new float[] { 0, 0.15f, 0.4f, 1f };
                        blend.Factors = new float[] { 0f, .3f, 1f, 1f };
                        gradient.Blend = blend;

                        g.FillPath(gradient, thePath);
                    }
                }
                //end Background

                //Begin Borders
                using (GraphicsPath gborderDark = thePath)
                {
                    using (Pen p = new Pen(tBorder, 1))
                    {
                        g.DrawPath(p, gborderDark);
                    }
                    //has focus?
                    if (isHotTracking)
                    {
                        using (Pen p = new Pen(colorTable.ButtonMouseOverBorder, 1))
                        {
                            g.DrawPath(p, gborderDark);
                        }
                    }
                }

                //End Borders

            }
        }

        #endregion

        #region "   Draw Text And Image  "
        private Point locPoint;
        protected void DrawTextAndImage(Graphics g, Color textColor)
        {
            #region "   Text init   "
            //Begin Drawing Text
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            #endregion

            #region "   Text Top   "
            if (this.TextAlign == ContentAlignment.TopLeft)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.TopCenter)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.TopRight)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Far;
            }
            #endregion

            #region "   Text Middle   "
            else if (this.TextAlign == ContentAlignment.MiddleLeft)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.MiddleCenter)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.MiddleRight)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Far;
            }
            #endregion

            #region "   Text Bottom  "
            else if (this.TextAlign == ContentAlignment.BottomLeft)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.BottomCenter)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.BottomRight)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Far;
            }
            #endregion

            #region "   KB Shortcut   "
            //has a KB shortcut?
            if (this.ShowKeyboardCues)
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            else
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            #endregion

            #region "   Draw Image   "
            if (this.Image != null)
            {
                Rectangle rc = new Rectangle();
                Point ImagePoint = new Point(6, 4);
                switch (this.ImageAlign)
                {
                    case ContentAlignment.MiddleRight:
                        {
                            rc.Width = this.ClientRectangle.Width - this.Image.Width - 8;
                            rc.Height = this.ClientRectangle.Height;
                            break;
                        }
                    case ContentAlignment.TopCenter:
                        {
                            ImagePoint.Y = 2;
                            ImagePoint.X = (this.ClientRectangle.Width - this.Image.Width) / 2;
                            break;
                        }
                    case ContentAlignment.MiddleCenter:
                        { // no text in this alignment
                            ImagePoint.X = (this.ClientRectangle.Width - this.Image.Width) / 2;
                            ImagePoint.Y = (this.ClientRectangle.Height - this.Image.Height) / 2;
                            break;
                        }
                    default:
                        {
                            ImagePoint.X = 6;
                            ImagePoint.Y = this.ClientRectangle.Height / 2 - Image.Height / 2;
                            break;
                        }
                }
                ImagePoint.X += locPoint.X;
                ImagePoint.Y += locPoint.Y;

                //enabled?
                if (this.Enabled)
                    g.DrawImage(this.Image, ImagePoint.X, ImagePoint.Y);
                else
                    System.Windows.Forms.ControlPaint.DrawImageDisabled(g, this.Image, ImagePoint.X, ImagePoint.Y, this.BackColor);
            }
            #endregion

            #region "   Draw Text   "
            //Draw string
            using (Brush brs = new SolidBrush(textColor))
            {
                g.DrawString(this.Text, this.Font, brs, this.ClientRectangle, sf);
            }
            #endregion
        }

        #endregion

        #region "   Button Status Management   "
        private void PressButton()
        {
            if (Enabled)
            {
                isHotTracking = false;
                isPressed = true;
                this.Invalidate();
            }
        }
        private void ReleaseButton()
        {
            if (Enabled)
            {
                isPressed = false;
                this.Invalidate();
            }
        }
        private void ResumeNormalButton()
        {
            if (Enabled)
            {
                isHotTracking = false;
                this.Invalidate();
            }
        }
        private void HighlightButton()
        {
            if (Enabled)
            {
                isHotTracking = true;
                this.Invalidate();
            }
        }
        #endregion

        #region "   Background Image   "

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        #endregion

        #region "   Colors Management   "
        private void InitColors()
        {
            //Set Colors & Fonts
            base.Font = this.colorTable.ControlFont;
            tBorder = colorTable.BorderColor;
            tBottomColorBegin = colorTable.BackgroundColor1;
            tBottomColorEnd = colorTable.BackgroundColor2;
            Textcol = colorTable.TextColor;
            controlFont = colorTable.ControlFont;

        }
        #endregion

    }
}
