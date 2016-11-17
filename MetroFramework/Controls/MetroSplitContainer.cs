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

    [ToolboxBitmap(typeof(SplitContainer))]
    public class MetroSplitContainer : SplitContainer, IMetroControl
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

        #region Fields

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            { return base.ForeColor; }
            set
            { base.ForeColor = value; }
        }

        private bool useCustomBackground = false;
        [Category("Metro Appearance")]
        public bool CustomBackground
        {
            get { return useCustomBackground; }
            set { useCustomBackground = value; }
        }

        #endregion

        #region Constructor

        public MetroSplitContainer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);


            this.Font = MetroFonts.Default(12f);

            InitColors();

        }

        #endregion

        #region Paint Methods


        Color splitterBorderColor;
        Color splitterBackColor;
        private void InitColors()
        {

            if (!Enabled)
            {
                splitterBorderColor = MetroPaint.BorderColor.SplitContainer.Disabled(Theme);
                splitterBackColor = MetroPaint.BackColor.SplitContainer.Disabled(Theme);
            }
            else
            {
                splitterBorderColor = MetroPaint.BorderColor.SplitContainer.Normal(Theme);
                splitterBackColor = MetroPaint.BackColor.SplitContainer.Normal(Theme);
            }

            if (useCustomBackground)
            {
                splitterBorderColor = BackColor;
                splitterBackColor = BackColor;

            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle rect = SplitterRectangle;

            InitColors();
            using (SolidBrush bru = new SolidBrush(splitterBackColor))
            {
                g.FillRectangle(bru, rect);

                //for differentiating the border Line
                using (Pen pen = new Pen(splitterBorderColor))
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
                        g.DrawLine(pen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1);
                    }
                    else
                    {
                        g.DrawLine(pen, rect.Left, rect.Top, rect.Right - 1, rect.Top);
                        g.DrawLine(pen, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                    }
                }
            }
        }


        #endregion
    }
}
