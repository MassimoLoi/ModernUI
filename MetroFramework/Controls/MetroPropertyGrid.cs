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
    [Designer(typeof(MetroPropertyGridDesigner))]
    [ToolboxBitmap(typeof(PropertyGrid))]
    public class MetroPropertyGrid : PropertyGrid, IMetroControl
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
        public override Font Font
        {
            get
            { return base.Font; }
            set
            { base.Font = value; }
        }

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

        public MetroPropertyGrid()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);


            this.Font = MetroFonts.Default(12f);

        }

        #endregion

        #region Paint Methods


        Color backColor, foreColor, borderColor;
        private void InitColors()
        {

           if (!DesignMode)
                this.ToolStripRenderer = new MetroCTXRenderer(Theme, Style);

            if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.PropertyGrid.Disabled(Theme);
                backColor = MetroPaint.BackColor.PropertyGrid.Disabled(Theme);
                borderColor = MetroPaint.BorderColor.PropertyGrid.Disabled(Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.PropertyGrid.Normal(Theme);
                backColor = MetroPaint.BackColor.PropertyGrid.Normal(Theme);
                borderColor = MetroPaint.BorderColor.PropertyGrid.Normal(Theme);
            }

            if (useCustomBackground)
                backColor = BackColor;

            this.HelpBackColor = backColor;
            this.HelpForeColor = foreColor;
            this.LineColor = borderColor;
            this.CategoryForeColor = foreColor;
            this.ForeColor = ForeColor;
                      
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            InitColors();
            e.Graphics.FillRectangle(new SolidBrush(backColor), e.ClipRectangle);

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.FillRectangle(new SolidBrush(backColor), e.ClipRectangle);
            //
        }

        #endregion
    }
}
