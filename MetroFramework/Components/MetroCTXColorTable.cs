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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Interfaces;
using MetroFramework.Design;
using MetroFramework.Drawing;

namespace MetroFramework.Components
{
    class MetroCTXColorTable : ProfessionalColorTable
    {
        MetroThemeStyle _theme = MetroThemeStyle.Light;
        MetroColorStyle _style = MetroColorStyle.Blue;

        public MetroCTXColorTable(MetroFramework.MetroThemeStyle Theme, MetroColorStyle Style)
        {
            _theme = Theme;
            _style = Style;
        }

        public override Color MenuStripGradientBegin
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color MenuStripGradientEnd
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color MenuItemSelected
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color MenuBorder
        {
            get { return MetroPaint.BackColor.Form(_theme); }
        }

        public override Color MenuItemBorder
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonSelectedBorder
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonPressedGradientBegin
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonPressedGradientEnd
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get { return Color.Transparent; }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get {  return Color.Transparent; }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get {  return Color.Transparent; }
        }

        public override Color StatusStripGradientBegin
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color StatusStripGradientEnd
        {
            get { return MetroPaint.GetStyleColor(_style); }
        }

        public override Color ToolStripGradientBegin
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripGradientEnd
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color ToolStripBorder
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color SeparatorDark
        {
            get { return MetroPaint.BackColor.Form(_theme); }
        }

        public override Color SeparatorLight
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color GripDark
        {
            get { return MetroPaint.BackColor.Form(_theme); }
        }

        public override Color GripLight
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color OverflowButtonGradientBegin
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }

        public override Color OverflowButtonGradientEnd
        {
            get { return ControlPaint.Light(MetroPaint.BackColor.Form(_theme)); }
        }
    }
}
