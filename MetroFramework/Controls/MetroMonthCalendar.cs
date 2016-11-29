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
using MetroFramework.Native;

namespace MetroFramework.Controls
{
    [Designer(typeof(MetroMonthCalendarDesigner))]
    [ToolboxBitmap(typeof(MonthCalendar)), Browsable(false)]
    public class MetroMonthCalendar : MonthCalendar, IMetroControl
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
            set { metroStyleManager = value; InitColors(); }
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

        #endregion

        #region Constructor

        public MetroMonthCalendar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);


            this.Font = MetroFonts.Default(12f);
            InitColors();
        }

        #endregion

        #region Paint Methods



        private void InitColors()
        {
            Color backColor, foreColor, borderColor, titleBackColor, trailingForeColor, titleForeColor;

            if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.Button.Disabled(Theme);
                backColor = MetroPaint.BackColor.Button.Disabled(Theme);
                borderColor = MetroPaint.BorderColor.Button.Disabled(Theme);
                titleBackColor = MetroPaint.BackColor.Button.Hover(Theme);
                trailingForeColor = MetroPaint.ForeColor.Button.Disabled(Theme);
                titleForeColor = MetroPaint.ForeColor.Button.Disabled(Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.Button.Normal(Theme);
                backColor = MetroPaint.BackColor.Button.Normal(Theme);
                borderColor = MetroPaint.BorderColor.Button.Normal(Theme);
                titleBackColor = MetroPaint.GetStyleColor(Style); // MetroPaint.BackColor.Button.Hover(Theme);
                trailingForeColor = MetroPaint.ForeColor.Button.Press(Theme);
                titleForeColor = MetroPaint.ForeColor.Button.Hover(Theme);
            }

            this.TitleBackColor = titleBackColor; 
            this.TrailingForeColor = trailingForeColor;
            this.TitleForeColor = titleForeColor;
            this.ForeColor = foreColor; 
            this.BackColor = backColor; 

            Invalidate();
            Update();

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

        protected override void CreateHandle()
        {
            base.CreateHandle();
            try { 
                //WinApi.SetWindowTheme(this.Handle, "", ""); 
            }
            catch { }
        } 


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        #endregion

    }
}
