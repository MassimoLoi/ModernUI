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

namespace MetroFramework.Controls
{
    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.GroupBox))]
    public class MetroGroupBox : GroupBox, IMetroControl
    {
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

        #region ... Properties - Enum ...  

        public enum BorderMode
        {
            None,
            Header,
            FullCustom,
            Full
        }

        private BorderMode _borderMode = BorderMode.Header;
        [Browsable(true), Category("Metro Appearance")]
        public BorderMode BorderStyle
        {
            get { return _borderMode; }
            set { _borderMode = value; Refresh(); }
        }

        Boolean _drawBottomLine = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("false")]
        public Boolean DrawBottomLine
        {
            get { return _drawBottomLine; }
            set { _drawBottomLine = value; Invalidate(); }
        }

        Boolean _drawShadows = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("false")]
        public Boolean DrawShadows
        {
            get { return _drawShadows; }
            set { _drawShadows = value; Invalidate(); }
        }

        Boolean _paintDefault = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("false")]
        public Boolean PaintDefault
        {
            get { return _paintDefault; }
            set { _paintDefault = value; Invalidate(); }
        }
        private MetroLabelSize metroLabelSize = MetroLabelSize.Medium;
        [Category("Metro Appearance")]
        public MetroLabelSize FontSize
        {
            get { return metroLabelSize; }
            set { metroLabelSize = value; Refresh(); }
        }

        private MetroLabelWeight metroLabelWeight = MetroLabelWeight.Light;
        [Category("Metro Appearance")]
        public MetroLabelWeight FontWeight
        {
            get { return metroLabelWeight; }
            set { metroLabelWeight = value; Refresh(); }
        }

        private bool useStyleColors = false;
        [Category("Metro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; Invalidate(); }
        }

        #endregion

        #region ... Constructor ...
        public MetroGroupBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            InitColors();
        }
        #endregion

        private void InitColors()
        {
            //set font
            this.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);

            //set BackGroudColor
            this.BackColor = MetroPaint.BackColor.GroupBox.Normal(Theme);

            //set ForeColor
            this.ForeColor = !useStyleColors ? MetroPaint.ForeColor.GroupBox.Normal(Theme) : MetroPaint.GetStyleColor(Style);

            //reset FlatStyle
            if (this.FlatStyle == FlatStyle.System) this.FlatStyle = FlatStyle.Standard;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_paintDefault || this._borderMode == BorderMode.Full) base.OnPaint(e);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //paint default
            base.OnPaintBackground(e);

            //call to reset colors
            InitColors();

            //Fill Color
            e.Graphics.FillRectangle(new SolidBrush(MetroPaint.BackColor.GroupBox.Normal(Theme)), e.ClipRectangle);

            // set Size
            Size sz = TextRenderer.MeasureText(Text, Font);

            switch (_borderMode)
            { 
                case BorderMode.Full:
                break;
                
                case BorderMode.FullCustom:
                    //text
                    TextRenderer.DrawText(e.Graphics, Text, Font, new Point(6, 0), ForeColor);
                    sz = TextRenderer.MeasureText(Text, Font);

                    //draw top line
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                        new Point(sz.Width + 6, sz.Height / 2 + 1),
                        new Point(Width - 3, sz.Height / 2 + 1));

                    //draw top shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                            new Point(sz.Width + 6, sz.Height / 2 + 2),
                            new Point(Width - 3, sz.Height / 2 + 2));
                    }

                    //draw bottom line
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                            new Point(0 + 2, this.Height - 3),
                            new Point(this.Width - 3, this.Height - 3));

                    //draw bottom shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                               new Point(0 + 2, this.Height - 3 - 1),
                               new Point(this.Width - 3, this.Height - 3 - 1));
                    }

                    //draw right
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                            new Point(Width - 2, sz.Height / 2 + 2),
                            new Point(this.Width - 2, this.Height - 4));

                    //draw right shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                                new Point(Width - 3, sz.Height / 2 + 2),
                                new Point(this.Width - 3, this.Height - 4));
                    }

                    //draw left
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                            new Point(0 + 1, sz.Height / 2 + 2),
                            new Point(0 + 1, this.Height - 4));

                    //draw left shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                                new Point(0 + 2, sz.Height / 2 + 2),
                                new Point(0 + 2, this.Height - 4));
                    }


                    //draw smallchunk
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                            new Point(0 + 2, sz.Height / 2 + 1),
                            new Point(0 + 5, sz.Height / 2 + 1));

                    //draw smallchunk shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                                new Point(0 + 3, sz.Height / 2 + 2),
                                new Point(0 + 5, sz.Height / 2 + 2));
                    }

                    break;

                case BorderMode.Header:
                    //text
                    TextRenderer.DrawText(e.Graphics, Text, Font, new Point(6, 0), ForeColor);
                    sz = TextRenderer.MeasureText(Text, Font);

                    //draw top line
                    e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                        new Point(sz.Width + 5, sz.Height / 2 + 1),
                        new Point(Width - 3, sz.Height / 2 + 1));

                    //draw shadow
                    if (_drawShadows)
                    {
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                            new Point(sz.Width + 6, sz.Height / 2 + 2),
                            new Point(Width - 2, sz.Height / 2 + 2));
                    }

                    if (_drawBottomLine)
                    {
                        //draw bottom line
                        e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Normal(Theme)),
                            new Point(0 + 1, this.Height - 3),
                            new Point(this.Width - 3, this.Height - 3));

                        //draw shadow
                        if (_drawShadows)
                        {
                            e.Graphics.DrawLine(new Pen(MetroPaint.BorderColor.GroupBox.Shadow(Theme)),
                                new Point(0 + 2, this.Height - 3 + 1),
                                new Point(this.Width - 2, this.Height - 3 + 1));
                        }
                    }
                    break;

                case BorderMode.None:
                    //text
                    TextRenderer.DrawText(e.Graphics, Text, Font, new Point(6, 0), ForeColor);
                    sz = TextRenderer.MeasureText(Text, Font);
                    break;


                   
            }
        }


    }
}
