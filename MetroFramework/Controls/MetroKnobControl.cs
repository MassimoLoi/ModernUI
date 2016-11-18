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
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Design;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using System.Drawing.Drawing2D;

namespace MetroFramework.Controls
{

    // Delegate type for hooking up ValueChanged notifications.
    public delegate void ValueChangedEventHandler(object Sender);

    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.Timer))]
    public class MetroKnobControl : System.Windows.Forms.UserControl, IMetroControl
    {
        #region " Interface "

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
            set { metroStyleManager = value; InitColors(); }
        }

        #endregion

        #region " constructor "

        private int _Minimum = 0;
        private int _Maximum = 100;
        private int _LargeChange = 20;
        private int _SmallChange = 5;
        private int _SizeLargeScaleMarker = 6;
        private int _SizeSmallScaleMarker = 3;
        private bool _ShowSmallScale = false;
        private bool _ShowLargeScale = true;
        private bool _isFocused = false;
        private Color _KnobColor = Color.FromKnownColor(KnownColor.ControlLight);
        private Color _KnobBorderColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
        private Color _KnobBackColor = Color.FromKnownColor(KnownColor.Control);
        private int _Value;
        private bool isKnobRotating = false;
        private Rectangle rKnob;
        private Point pKnob;
        private Rectangle rScale;
        private Pen DottedPen;
        private Brush bKnob;
        private Brush bKnobPoint;

        // declare Off screen image and Offscreen graphics
        private Image OffScreenImage;
        private Graphics gOffScreen;

        // An event that clients can use to be notified whenever
        // the Value is Changed.

        // Events
        public event ValueChangedEventHandler ValueChanged;

        public MetroKnobControl()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //This call is required by the Windows Form Designer.
            InitializeComponent();

            this.ImeMode = ImeMode.On;
            this.Name = "Knob";
            this.Resize += new EventHandler(this.Knob_Resize);
            //this.ValueChanged += new ValueChangedEventHandler(this.ValueChanged);

            DottedPen = new Pen(MetroDrawingMethods.GetDarkColor(_KnobBorderColor, 40));
            DottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            DottedPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            setDimensions();

            InitColors();

        }

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // KnobControl
            // 
            this.Name = "KnobControl";
            this.Size = new System.Drawing.Size(150, 150);
            this.ResumeLayout(false);

        }
        #endregion

        #region " Public Properties "
        // Shows Small Scale marking.
        [Browsable(true), Category("Metro Appearance")]
        [Description("Shows Small Scale marking")]
        public bool ShowSmallScale
        {
            get { return _ShowSmallScale; }
            set
            {
                _ShowSmallScale = value;
                // Need to redraw
                setDimensions(); this.Invalidate();
            }
        }


        // Shows Large Scale marking
        [Browsable(true), Category("Metro Appearance")]
        [Description(" Shows Large Scale marking")]
        public bool ShowLargeScale
        {
            get { return _ShowLargeScale; }
            set
            {
                _ShowLargeScale = value;
                // Need to redraw
                setDimensions(); this.Invalidate();
            }
        }

        // Size of the Large Scale Marker
        [Browsable(true), Category("Metro Appearance")]
        [Description("Size of the large Scale Marker")]
        public int SizeLargeScaleMarker
        {
            get { return _SizeLargeScaleMarker; }
            set
            {
                _SizeLargeScaleMarker = value;
                setDimensions(); this.Invalidate();
            }
        }

        // Size of the Small Scale Marker
        [Browsable(true), Category("Metro Appearance")]
        [Description("Size of the small Scale Marker")]
        public int SizeSmallScaleMarker
        {
            get { return _SizeSmallScaleMarker; }
            set
            {
                _SizeSmallScaleMarker = value;
                setDimensions(); this.Invalidate();
            }
        }


        // Minimum Value for knob Control
        [Browsable(true), Category("Metro Appearance")]
        [Description("Minimum Value for knob Control")]
        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                setDimensions(); this.Invalidate();
            }
        }


        // Maximum value for knob control
        [Browsable(true), Category("Metro Appearance")]
        [Description("Maximum value for knob control")]
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                setDimensions(); this.Invalidate();
            }
        }


        // value set for large change
        [Browsable(true), Category("Metro Appearance")]
        [Description("value set for large change")]
        public int LargeChange
        {
            get { return _LargeChange; }
            set
            {
                _LargeChange = value;
                setDimensions(); this.Invalidate();
            }
        }


        // value set for small change.
        [Browsable(true), Category("Metro Appearance")]
        [Description("value set for small change")]
        public int SmallChange
        {
            get { return _SmallChange; }
            set
            {
                _SmallChange = value;
                setDimensions(); this.Invalidate();
            }
        }
        // Current Value of knob control
        [Browsable(true), Category("Metro Appearance")]
        [Description("Current Value of knob control")]
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                // Call delegate
                OnValueChanged(value);
                InitColors();
            }
        }

        // Set Color of knob control
        [Browsable(true), Category("Metro Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Set Color of knob control")]
        public Color KnobColor
        {
            get { return _KnobColor; }
            set
            {
                _KnobColor = value;
                //Refresh Colors
                this.Invalidate();
            }
        }

        // Set Color of the border of knob control
        [Browsable(true), Category("Metro Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Set Color of the border of knob control")]
        public Color KnobBorderColor
        {
            get { return _KnobBorderColor; }
            set
            {
                _KnobBorderColor = value;
                //Refresh Colors
                this.Invalidate();
            }
        }

        // Set Color of the back of knob control
        [Browsable(true), Category("Metro Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Set Color of the back of knob control")]
        public Color KnobBackColor
        {
            get { return _KnobBackColor; }
            set
            {
                _KnobBackColor = value;
                //Refresh Colors
                this.Invalidate();
            }
        }

        private bool useStyleColors = false;
        [Category("Metro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; InitColors(); }
        }
        #endregion

        #region " Events and sub management "
        protected object OnValueChanged(object sender)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender);
            }
            return sender;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // create LinearGradientBrush for creating knob
            bKnob = new System.Drawing.Drawing2D.LinearGradientBrush(rKnob, MetroDrawingMethods.GetLightColor(KnobColor, 55), MetroDrawingMethods.GetDarkColor(KnobColor, 55), LinearGradientMode.ForwardDiagonal);
            // create LinearGradientBrush for knobPoint
            bKnobPoint = new System.Drawing.Drawing2D.LinearGradientBrush(rKnob, MetroDrawingMethods.GetLightColor(_KnobBorderColor, 55), MetroDrawingMethods.GetDarkColor(_KnobBorderColor, 55), LinearGradientMode.ForwardDiagonal);

            // Set background color of Image...
            e.Graphics.FillRectangle(new SolidBrush(_KnobBackColor), new Rectangle(0, 0, Width, Height));
            //gOffScreen.Clear(this.BackColor);
            // Fill knob Background to give knob effect....
            gOffScreen.FillEllipse(bKnob, rKnob);
            // Set antialias effect on
            gOffScreen.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // Draw border of knob
            gOffScreen.DrawEllipse(new Pen(_KnobBorderColor), rKnob);

            //if control is focused
            if ((this._isFocused))
            {
                gOffScreen.DrawEllipse(DottedPen, rKnob);
            }
            // get current position of pointer
            Point Arrow = this.getKnobPosition();
            // Draw pointer arrow that shows knob position

            Rectangle rect = new Rectangle(Arrow.X - 3, Arrow.Y - 3, 6, 6);
            DrawInsetCircle(ref gOffScreen, ref rect, new Pen(_KnobBorderColor));

            // Draw small and large scale
            int i = Minimum;
            if ((this._ShowSmallScale))
            {
                for (i = Minimum; i <= Maximum; i = i + this._SmallChange)
                {
                    gOffScreen.DrawLine(new Pen(this.ForeColor), getMarkerPoint(0, i), getMarkerPoint(_SizeSmallScaleMarker, i));
                }
            }

            if ((this._ShowLargeScale))
            {
                for (i = Minimum; i <= Maximum; i = i + this._LargeChange)
                {
                    gOffScreen.DrawLine(new Pen(this.ForeColor), getMarkerPoint(0, i), getMarkerPoint(_SizeLargeScaleMarker, i));
                }
            }

            // Drawimage on screen
            g.DrawImage(OffScreenImage, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(new SolidBrush(_KnobBackColor), new Rectangle(0, 0, Width, Height));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((isPointinRectangle(new Point(e.X, e.Y), rKnob)))
            {
                // Start Rotation of knob
                this.isKnobRotating = true;
            }
        }

        // ----------------------------------------------------------
        // we need to override IsInputKey method to allow user to
        // use up, down, right and bottom keys other wise using me
        // keys will change focus from current object to another
        // object on the form
        // ----------------------------------------------------------
        protected override bool IsInputKey(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                    return true;
                default:
                    return false;
            }
            // return base.IsInputKey(key);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Stop rotation
            this.isKnobRotating = false;
            if ((isPointinRectangle(new Point(e.X, e.Y), rKnob)))
            {
                // get value
                this.Value = (int)this.getValueFromPosition(new PointF(e.X, e.Y));
            }
            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // --------------------------------------
            // Following Handles Knob Rotating
            // --------------------------------------
            if ((this.isKnobRotating == true))
            {
                this.Cursor = Cursors.Hand;
                PointF p = new PointF(e.X, e.Y);
                float posVal = this.getValueFromPosition(p);
                Value = (int)posVal;
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            this._isFocused = true;
            this.Refresh();
            base.OnEnter(new EventArgs());
        }

        protected override void OnLeave(EventArgs e)
        {
            this._isFocused = false;
            this.Refresh();
            base.OnLeave(new EventArgs());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // --------------------------------------------------------
            // Handles knob rotation with up,down,left and right keys
            // --------------------------------------------------------
            if ((e.KeyCode == Keys.Up | e.KeyCode == Keys.Right))
            {
                if ((_Value < Maximum))
                {
                    Value = _Value + 1;
                    this.Invalidate();
                }
            }
            else if ((e.KeyCode == Keys.Down | e.KeyCode == Keys.Left))
            {
                if ((_Value > Minimum))
                {
                    Value = _Value - 1;
                    this.Invalidate();
                }
            }
        }

        private void setDimensions()
        {
            // get smaller from height and width
            int size = this.Width;
            if ((this.Width > this.Height))
            {
                size = this.Height;
            }
            // allow 10% gap on all side to determine size of knob
            this.rKnob = new Rectangle((int)Math.Round((double)(size * 0.1)), (int)Math.Round((double)(size * 0.1)), (int)Math.Round((double)(size * 0.8)), (int)Math.Round((double)(size * 0.8)));
            this.rScale = new Rectangle(2, 2, size - 4, size - 4);
            this.pKnob = new Point((int)Math.Round((double)(this.rKnob.X + (((double)this.rKnob.Width) / 2.0))), (int)Math.Round((double)(this.rKnob.Y + (((double)this.rKnob.Height) / 2.0))));
            // create offscreen image
            this.OffScreenImage = new Bitmap(this.Width, this.Height);
            // create offscreen graphics
            this.gOffScreen = Graphics.FromImage(OffScreenImage);
        }

        private void Knob_Resize(object sender, System.EventArgs e)
        {
            setDimensions();
            Invalidate();
        }


        // gets knob position that is to be drawn on control.
        private Point getKnobPosition()
        {
            double degree = 270 * this.Value / (this.Maximum - this.Minimum);
            degree = (degree + 135) * Math.PI / 180;

            Point Pos = (new Point(0, 0));
            Pos.X = (int)Math.Round((double)(((Math.Cos(degree) * ((((double)this.rKnob.Width) / 2.0) - 10.0)) + this.rKnob.X) + (((double)this.rKnob.Width) / 2.0)));
            Pos.Y = (int)Math.Round((double)(((Math.Sin(degree) * ((((double)this.rKnob.Width) / 2.0) - 10.0)) + this.rKnob.Y) + (((double)this.rKnob.Height) / 2.0)));
            return Pos;
        }

        // gets marker point required to draw scale marker.
        // <param name="length">distance from center</param>
        // <param name="Value">value that is to be marked</param>
        // <returns>Point that describes marker position</returns>
        private Point getMarkerPoint(int length, int Value)
        {
            double degree = 270 * Value / (this.Maximum - this.Minimum);
            degree = (degree + 135) * Math.PI / 180;

            Point Pos = new Point(0, 0);
            Pos.X = (int)Math.Round((double)(((Math.Cos(degree) * (((((double)this.rKnob.Width) / 2.0) - length) + 7.0)) + this.rKnob.X) + (((double)this.rKnob.Width) / 2.0)));
            Pos.Y = (int)Math.Round((double)(((Math.Sin(degree) * (((((double)this.rKnob.Width) / 2.0) - length) + 7.0)) + this.rKnob.Y) + (((double)this.rKnob.Height) / 2.0)));

            return Pos;
        }

        // converts geomatrical position in to value..
        // <param name="p">Point that is to be converted</param>
        // <returns>Value derived from position</returns>
        public virtual float getValueFromPosition(PointF position)
        {
            float degree = 0.0F;
            float v = 0.0F;

            PointF center = this.pKnob;

            if (position.X <= center.X)
            {
                degree = (center.Y - position.Y) / (center.X - position.X);
                degree = (float)Math.Atan(degree);
                degree = (float)((degree) * (180F / Math.PI) + 45F);
                v = (degree * (this.Maximum - this.Minimum) / 270F);
            }
            else
            {
                if (position.X > center.X)
                {
                    degree = (position.Y - center.Y) / (position.X - center.X);
                    degree = (float)Math.Atan(degree);
                    degree = (float)(225F + (degree) * (180F / Math.PI));
                    v = (degree * (this.Maximum - this.Minimum) / 270F);
                }
            }

            if (v > this.Maximum)
                v = this.Maximum;

            if (v < this.Minimum)
                v = this.Minimum;

            return v;
        }


        // converts geomatrical position in to value..
        // <param name="p">Point that is to be converted</param>
        // <returns>Value derived from position</returns>
        private int getValueFromPosition(Point p)
        {

            double degree = 0.0;
            int v = 0;
            try
            {
                if ((p.X <= pKnob.X))
                {
                    degree = (pKnob.Y - p.Y) / (pKnob.X - p.X);
                    degree = Math.Atan(degree);
                    degree = (degree) * (180 / Math.PI) + 45;
                    v = (int)Math.Round((double)((degree * (this.Maximum - this.Minimum)) / 270.0));
                }
                else if ((p.X > pKnob.X))
                {
                    degree = (p.Y - pKnob.Y) / (p.X - pKnob.X);
                    degree = Math.Atan(degree);
                    degree = 225 + (degree) * (180 / Math.PI);
                    v = (int)Math.Round((double)((degree * (this.Maximum - this.Minimum)) / 270.0));
                }

                if ((v > Maximum)) v = Maximum;
                if ((v < Minimum)) v = Minimum;
            }
            catch
            {
            }
            return v;
        }
        public virtual PointF getPositionFromValue(float val)
        {
            PointF pos = new PointF(0.0F, 0.0F);

            float indicatorOffset = 10F;
            float drawRatio = 1F;

            // Elimina la divisione per 0
            if ((this.Maximum - this.Minimum) == 0)
                return pos;

            float _indicatorOffset = indicatorOffset * drawRatio;

            float degree = 270F * val / (this.Maximum - this.Minimum);
            degree = (degree + 135F) * (float)Math.PI / 180F;

            pos.X = (int)(Math.Cos(degree) * ((this.rKnob.Width * 0.5F) - indicatorOffset) + this.rKnob.X + (this.rKnob.Width * 0.5F));
            pos.Y = (int)(Math.Sin(degree) * ((this.rKnob.Width * 0.5F) - indicatorOffset) + this.rKnob.Y + (this.rKnob.Height * 0.5F));

            return pos;
        }
        #endregion

        #region " support functions "
        // Method which checks is particular point is in rectangle
        // <param name="p">Point to be Checked</param>
        // <param name="r">Rectangle</param>
        // <returns>true is Point is in rectangle, else false</returns>
        public bool isPointinRectangle(Point p, Rectangle r)
        {
            bool flag = false;
            if ((p.X > r.X & p.X < r.X + r.Width & p.Y > r.Y & p.Y < r.Y + r.Height))
            {
                flag = true;
            }
            return flag;
        }

        public void DrawInsetCircle(ref Graphics g, ref Rectangle r, Pen p)
        {
            int i;
            Pen p1 = new Pen(MetroDrawingMethods.GetDarkColor(p.Color, 50));
            Pen p2 = new Pen(MetroDrawingMethods.GetLightColor(p.Color, 50));

            for (i = 0; i <= p.Width; i++)
            {
                Rectangle r1 = new Rectangle(r.X + i, r.Y + i, r.Width - i * 2, r.Height - i * 2);
                g.DrawArc(p2, r1, -45, 180);
                g.DrawArc(p1, r1, 135, 180);
            }
        }

        #endregion

        #region "   Color Management   "
        private void InitColors()
        {
            this.BorderStyle = BorderStyle.None;
            this.KnobColor = MetroPaint.BackColor.KnobControl(Theme);
            this.KnobBorderColor = MetroPaint.BorderColor.KnobControl(Theme);
            this.ForeColor = !useStyleColors ? MetroPaint.ForeColor.KnobControl(Theme) : MetroPaint.GetStyleColor(Style);
            this.KnobBackColor = MetroPaint.BackColor.KnobControl(Theme);

            setDimensions(); this.Invalidate();
        }
        #endregion

    }
}

