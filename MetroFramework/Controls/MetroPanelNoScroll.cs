/**
 * MetroFramework - Modern UI for WinForms
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Design;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;
using MetroFramework.Controls;

namespace MetroFramework.Controls
{
    [Designer(typeof(MetroPanelNoScrollDesigner))]
    [System.Drawing.ToolboxBitmapAttribute(typeof(Panel))]
    public class MetroPanelNoScroll : ListView, IMetroControl
    {
        #region ... Interface ...

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

        #region ... Properties ...
        int _borderWidth = 1;
        [Browsable(true), Category("Metro Appearance")]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; Invalidate(); }
        }

        bool _paintBackColor = true;
        [Browsable(true), Category("Metro Appearance")]
        public bool PaintBackColor
        {
            get { return _paintBackColor; }
            set { _paintBackColor = value; InitColors(); }
        }

        Color _borderColor = Color.Gray;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), Category("Metro Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        Color _backColor = Color.Gray;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), Category("Metro Appearance")]
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; Invalidate(); }
        }

        #endregion

        #region ... Constructor ...
        public MetroPanelNoScroll()
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            //init values
            InitColors();
        }
        #endregion

        #region ... Init Colors ...
        private void InitColors()
        {
            _backColor = MetroPaint.BackColor.Panel.Normal(Theme);
            _borderColor = (this.Enabled) ? MetroPaint.BorderColor.Panel.Normal(Theme) : MetroPaint.BorderColor.Panel.Disabled(Theme);

            Invalidate();
        }
        #endregion

        #region ... Overrides ...
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height); // e.ClipRectangle;
            Rectangle borderrect = new Rectangle(0, 0, this.Width, this.Height); // e.ClipRectangle;

            using (Brush brush = new SolidBrush(Color.Red))
            {
                e.Graphics.FillRectangle(brush, rect);
            }


            if (this._borderWidth > 0)
            {
                e.Graphics.DrawRectangle(new Pen(this._borderColor, _borderWidth), borderrect);
            }

        }
        #endregion
    }

}

