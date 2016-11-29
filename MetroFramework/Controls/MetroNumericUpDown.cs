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
using System.Reflection;
using System.Drawing.Drawing2D;

namespace MetroFramework.Controls
{
    [Designer(typeof(MetroNumericUpDownDesigner))]
    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.NumericUpDown))]
    public class MetroNumericUpDown : NumericUpDown, IMetroControl
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
            set { metroStyleManager = value; }
        }

        #endregion

        #region ... Properties - Enum ...

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

        private bool useAlternateColors = false;
        [Category("Metro Appearance")]
        public bool UseAlternateColors
        {
            get { return useAlternateColors; }
            set { useAlternateColors = value; Invalidate(); }
        }

        private bool customDrawButtons = false;
        [Category("Metro Appearance")]
        public bool CustomDrawButtons
        {
            get { return customDrawButtons; }
            set { customDrawButtons = value; InitColors(); Invalidate(); }
        }
        #endregion

        #region ... Constructor ...
        public MetroNumericUpDown()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.ResizeRedraw |
                      ControlStyles.UserPaint |
                      ControlStyles.SupportsTransparentBackColor, true);

            InitColors();
        }
        #endregion

        #region ... Colors Management ...

        private void InitColors()
        {
            //set font
            this.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);

            //set BackGroudColor
            base.BackColor = MetroPaint.BackColor.Form(Theme);

            Refresh();
            Update();

        }
        #endregion

        #region ... Override Methods ...

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Set Colors
            Color nudTextColor = !useStyleColors ? MetroPaint.ForeColor.NumericUpDown.Normal(Theme) : MetroPaint.GetStyleColor(Style);
            if (!this.Enabled)
                nudTextColor = MetroPaint.ForeColor.NumericUpDown.Disabled(Theme);

            Color nudTextBackColor = !useAlternateColors ? MetroPaint.BackColor.NumericUpDown.Normal(Theme) : MetroPaint.BackColor.NumericUpDown.Alternate(Theme);
            Color nudControlsBorderColor = MetroPaint.BorderColor.NumericUpDown.Normal(Theme);
            Color nudControlsBackColor = MetroPaint.BackColor.NumericUpDown.Normal(Theme);
            Color nudControlsForeColor = MetroPaint.BorderColor.NumericUpDown.Normal(Theme);

            bool Debug = false; //for check consistency of the Rectangles

            var gr = e.Graphics;
            Rectangle nudRect = this.ClientRectangle;
            nudRect.Height -= 1;
            nudRect.Width -= 1;


            gr.FillRectangle(new SolidBrush(nudControlsBackColor), nudRect);
            gr.DrawRectangle(new Pen(nudControlsBorderColor), nudRect);

            if (Debug) gr.DrawRectangle(new Pen(Color.Red), nudRect);

            foreach (Control c in this.Controls)
            {
                if (!(c is TextBox)) // -->Up Down Buttons
                {

                    //override inner's control paintevent (Button UP & Down)
                    typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, c, new object[] { true });
                    c.Paint += (sender, pev) =>
                    {
                        var g = pev.Graphics;
                        int h = c.Height;
                        int w = c.Width;

                        Rectangle buttonsUpDownRect = c.ClientRectangle;
                        buttonsUpDownRect.Height -= 1;
                        buttonsUpDownRect.Width -= 1;

                        //control rectnagles
                        Rectangle UpArrowRect = new Rectangle(1, 1, w - 3, h / 2 - 2);
                        Rectangle DownArrowRect = new Rectangle(1, h / 2, w - 3, h / 2 - 2);

                        //Draw Controls Rectangle
                        g.DrawRectangle(new Pen(nudControlsBackColor), buttonsUpDownRect);
                        if (Debug) g.DrawRectangle(new Pen(Color.Green), buttonsUpDownRect);

                        //Cutom Draw Buttons
                        if (customDrawButtons)
                        {
                            //ClearBackGround                   
                            g.FillRectangle(new SolidBrush(nudControlsBackColor), buttonsUpDownRect);

                            //draw BackGround
                            g.FillRectangle(new SolidBrush(nudControlsBackColor), UpArrowRect);
                            g.FillRectangle(new SolidBrush(nudControlsBackColor), DownArrowRect);
                            g.DrawRectangle(new Pen(nudControlsBorderColor), UpArrowRect);
                            g.DrawRectangle(new Pen(nudControlsBorderColor), DownArrowRect);
                            if (Debug) g.DrawRectangle(new Pen(Color.Violet), UpArrowRect);
                            if (Debug) g.DrawRectangle(new Pen(Color.Blue), DownArrowRect);

                            //draw Arrows
                            MetroDrawingMethods.PaintUpArrow(g, UpArrowRect, nudControlsForeColor, 0, -1);
                            MetroDrawingMethods.PaintDownArrow(g, DownArrowRect, nudControlsForeColor, 0, 0);
                        }
                    };

                }
                else if ((c is TextBox))
                {
                    //Set Textbox fore Color
                    c.ForeColor = nudTextColor;
                    if (Debug) c.ForeColor = Color.Red;

                    //Set Textbox back Color
                    c.BackColor = nudTextBackColor;
                    if (Debug) c.BackColor = Color.Yellow;

                    c.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);
                }

            }

        }
        #endregion

        #region  ... Experimental ...
        /// <summary>
        /// Experimental
        /// </summary>
        bool MouseIsOnUPArrow = false;
        bool MouseIsOnDownArrow = false;
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Graphics g = this.CreateGraphics();
            int h = ClientRectangle.Height;

            if (this.ClientRectangle.Contains(e.X, e.Y))
            {
                if (e.Y < h)
                {
                    MouseIsOnUPArrow = true;
                }
                else
                {
                    MouseIsOnDownArrow = true;
                }
            }
            else
            {
                MouseIsOnUPArrow = false;
                MouseIsOnDownArrow = false;
            }
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            MouseIsOnUPArrow = false;
            MouseIsOnDownArrow = false;

            Invalidate();
        }
        #endregion

    }
}
