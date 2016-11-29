/**
 * MetroFramework - ExtendedRendering - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2016 Angelo Cresta, http://quarztech.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using MetroFramework.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Design;
using MetroFramework.Interfaces;
using System.Diagnostics;

namespace MetroFramework.Controls
{
    [Designer(typeof(MetroButtonStyledDesigner))]
    [ToolboxBitmap(typeof(System.Windows.Forms.Button)), ToolboxItem(true)]
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

        #region "   Interface   "

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
        private MetroButtonSize metroButtonSize = MetroButtonSize.Medium;
        [Category("Metro Appearance")]
        [Browsable(false)]
        public MetroButtonSize FontSize
        {
            get { return metroButtonSize; }
            set { metroButtonSize = value; Refresh(); }
        }

        private MetroButtonWeight metroButtonWeight = MetroButtonWeight.Regular;
        [Category("Metro Appearance")]
        [Browsable(false)]
        public MetroButtonWeight FontWeight
        {
            get { return metroButtonWeight; }
            set { metroButtonWeight = value; Refresh(); }
        }

        private bool flatMetroAppearance = false;
        [Category("Metro Appearance")]
        public bool FlatAppearance
        {
            get { return flatMetroAppearance; }
            set { flatMetroAppearance = value; InitColors(); }
        }

        private bool highlight = false;
        [Category("Metro Appearance")]
        public bool Highlight
        {
            get { return highlight; }
            set { highlight = value; }
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

            //set font
            this.Font = MetroFonts.Button(metroButtonSize, metroButtonWeight);
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
            buttonRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            rectCornerRadius = Math.Max(1, scalingDividend / 10);
            rectOutlineWidth = Math.Max(1, scalingDividend / 50);

            if (flatMetroAppearance)
            {
                rectCornerRadius = 0;
                buttonRect.Width -= 1;
                buttonRect.Height -= 1;
            }
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
                try
                {
                    gp.AddArc((float)r.X, (float)r.Y, radius, radius, 180, 90);
                    gp.AddArc((float)r.Right - radius, (float)r.Y, radius - 1, radius, 270, 90);
                    gp.AddArc((float)r.Right - radius, ((float)r.Bottom) - radius - 1, radius - 1, radius, 0, 90);
                    gp.AddArc((float)r.X, ((float)r.Bottom) - radius - 1, radius - 1, radius, 90, 90);
                }
                catch (Exception ex)
                {
                    //void error report
                    gp.AddRectangle(r);
                }
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
                tBorder = MetroPaint.BorderColor.Button.Disabled(Theme); 
                tBottomColorBegin = MetroPaint.BackColor.Button.Disabled(Theme); 
                tBottomColorEnd = MetroPaint.BackColor.Button.Disabled(Theme); 
                Textcol = MetroPaint.ForeColor.Button.Disabled(Theme); 
            }
            else
            {
                //Normal State
                tBorder = highlight ? MetroPaint.GetStyleColor(Style) : MetroPaint.BorderColor.Button.Normal(Theme);
                tBottomColorBegin = MetroPaint.BackColor.Button.Normal(Theme);
                tBottomColorEnd = !flatMetroAppearance ? MetroPaint.BackColor.Button.Normal2(Theme) : MetroPaint.BackColor.Button.Normal(Theme);
                Textcol = MetroPaint.ForeColor.Button.Normal(Theme); 
                //hot tracking - mouse over
                if (isHotTracking)
                {
                    tBorder = MetroPaint.BorderColor.Button.Hover(Theme); 
                    tBottomColorBegin = MetroPaint.BackColor.Button.Hover(Theme);
                    tBottomColorEnd = !flatMetroAppearance ? MetroPaint.BackColor.Button.Hover2(Theme) : MetroPaint.BackColor.Button.Hover(Theme);
                    Textcol = MetroPaint.ForeColor.Button.Hover(Theme); 
                }
                //pressed
                if (isPressed)
                {
                    tBorder = MetroPaint.BorderColor.Button.Press(Theme); 
                    tBottomColorBegin = MetroPaint.BackColor.Button.Press(Theme);
                    tBottomColorEnd = !flatMetroAppearance ? MetroPaint.BackColor.Button.Press2(Theme) : MetroPaint.BackColor.Button.Press(Theme);
                    Textcol = MetroPaint.ForeColor.Button.Press(Theme); 
                }
                //focused but not pressed
                if ((this.Focused) && (!isPressed))
                {
                    tBorder = highlight ? MetroPaint.GetStyleColor(Style) : MetroPaint.BorderColor.Button.Normal(Theme);
                    tBottomColorBegin = MetroPaint.BackColor.Button.Normal(Theme);
                    tBottomColorEnd = !flatMetroAppearance ? MetroPaint.BackColor.Button.Normal2(Theme) : MetroPaint.BackColor.Button.Normal(Theme);
                    Textcol = MetroPaint.ForeColor.Button.Normal(Theme); 
                }
                //focused and mouse over
                if ((this.Focused) && (isHotTracking))
                {
                    tBorder = MetroPaint.BorderColor.Button.Hover(Theme); 
                    tBottomColorBegin = MetroPaint.BackColor.Button.Hover(Theme);
                    tBottomColorEnd = !flatMetroAppearance ? MetroPaint.BackColor.Button.Hover2(Theme) : MetroPaint.BackColor.Button.Hover(Theme);
                    Textcol = MetroPaint.ForeColor.Button.Hover(Theme); 
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

        #region "   Base Overridden Methods   "
        protected override Size DefaultSize
        {
            get
            {
                // Set the default size of
                // the control
                return new Size(125, 35);
            }
        }
        #endregion

        #region "   Paint Background   "
        protected void PaintBackground(PaintEventArgs e, Graphics g, Color tBorder, Color tBottomColorBegin, Color tBottomColorEnd)
        {
            //Set button radius
            int _roundedRadiusX = rectCornerRadius;

            //to avoid 0 rectangle error
            if (buttonRect.Height == 0 && buttonRect.Width == 0) SetControlSizes();

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
                int borderWidth = !highlight ? 1 : 3; //larger if Highlited
                using (GraphicsPath gborderDark = thePath)
                {
                    using (Pen p = new Pen(tBorder, borderWidth))
                    {
                        g.DrawPath(p, gborderDark);
                    }
                    //has focus?
                    if (isHotTracking)
                    {
                        using (Pen p = new Pen(MetroPaint.BorderColor.Button.Hover(Theme), borderWidth))
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
            base.Font = MetroFonts.Button(metroButtonSize, metroButtonWeight); 
            tBorder = MetroPaint.BorderColor.Button.Normal(Theme); 
            tBottomColorBegin = MetroPaint.BackColor.Button.Normal(Theme); 
            tBottomColorEnd = MetroPaint.BackColor.Button.Normal2(Theme); 
            Textcol = MetroPaint.ForeColor.Button.Normal(Theme);
            controlFont = MetroFonts.Button(metroButtonSize, metroButtonWeight);

            this.Font = MetroFonts.Button(metroButtonSize, metroButtonWeight);

            Invalidate();
            Update();
        }
        #endregion

    }
}
