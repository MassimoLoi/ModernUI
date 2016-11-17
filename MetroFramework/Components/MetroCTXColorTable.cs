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

        #region ... MenuStrip ...
        public override Color MenuStripGradientBegin
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuStripGradientBegin(_theme, _style); }
        }

        public override Color MenuStripGradientEnd
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuStripGradientEnd(_theme, _style); }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemSelectedGradientBegin(_theme, _style); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemSelectedGradientEnd(_theme, _style); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemPressedGradientBegin(_theme, _style); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemPressedGradientEnd(_theme, _style); }
        }

        public override Color MenuItemSelected
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemSelected(_theme, _style); }
        }

        public override Color MenuBorder
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuBorder(_theme, _style); }
        }

        public override Color MenuItemBorder
        {
            get { return MetroPaint.ColorTable.MenuStrip.MenuItemBorder(_theme, _style); }
        }
        #endregion

        #region ... ToolStrip ...

        public override Color ToolStripDropDownBackground
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripDropDownBackground(_theme, _style); }
        }

        public override Color ToolStripGradientBegin
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripGradientBegin(_theme, _style); }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripGradientMiddle(_theme, _style); }
        }

        public override Color ToolStripGradientEnd
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripGradientEnd(_theme, _style); }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripPanelGradientBegin(_theme, _style); }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripPanelGradientEnd(_theme, _style); }
        }

        public override Color ToolStripBorder
        {
            get { return MetroPaint.ColorTable.ToolStrip.ToolStripBorder(_theme, _style); }
        }
        #endregion

        #region ... Image ...
        public override Color ImageMarginGradientBegin
        {
            get { return MetroPaint.ColorTable.Image.ImageMarginGradientBegin(_theme, _style); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return MetroPaint.ColorTable.Image.ImageMarginGradientMiddle(_theme, _style); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return MetroPaint.ColorTable.Image.ImageMarginGradientEnd(_theme, _style); }
        }

        #endregion

        #region ... Button ...
        public override Color ButtonSelectedBorder
        {
            get { return MetroPaint.ColorTable.Button.ButtonSelectedBorder(_theme, _style); }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get { return MetroPaint.ColorTable.Button.ButtonSelectedGradientBegin(_theme, _style); }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get { return MetroPaint.ColorTable.Button.ButtonSelectedGradientMiddle(_theme, _style); }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get { return MetroPaint.ColorTable.Button.ButtonSelectedGradientEnd(_theme, _style); }
        }

        public override Color ButtonPressedGradientBegin
        {
            get { return MetroPaint.ColorTable.Button.ButtonPressedGradientBegin(_theme, _style); }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get { return MetroPaint.ColorTable.Button.ButtonPressedGradientMiddle(_theme, _style); }
        }

        public override Color ButtonPressedGradientEnd
        {
            get { return MetroPaint.ColorTable.Button.ButtonPressedGradientEnd(_theme, _style); }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get { return MetroPaint.ColorTable.Button.ButtonCheckedGradientBegin(_theme, _style); }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get {  return MetroPaint.ColorTable.Button.ButtonCheckedGradientMiddle(_theme, _style); }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get {  return MetroPaint.ColorTable.Button.ButtonCheckedGradientEnd(_theme, _style); }
        }

        public override Color OverflowButtonGradientBegin
        {
            get { return MetroPaint.ColorTable.Button.OverflowButtonGradientBegin(_theme, _style); }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get { return MetroPaint.ColorTable.Button.OverflowButtonGradientMiddle(_theme, _style); }
        }

        public override Color OverflowButtonGradientEnd
        {
            get { return MetroPaint.ColorTable.Button.OverflowButtonGradientEnd(_theme, _style); }
        }
        #endregion

        #region ... StatusStrip ...
        public override Color StatusStripGradientBegin
        {
            get { return MetroPaint.ColorTable.StatusStrip.StatusStripGradientBegin(_theme, _style); }
        }

        public override Color StatusStripGradientEnd
        {
            get { return MetroPaint.ColorTable.StatusStrip.StatusStripGradientEnd(_theme, _style); }
        }

        #endregion

        #region ... Misc ...

        public override Color SeparatorDark
        {
            get { return MetroPaint.ColorTable.Separator.Dark(_theme, _style); }
        }

        public override Color SeparatorLight
        {
            get { return MetroPaint.ColorTable.Separator.Light(_theme,_style); }
        }

        public override Color GripDark
        {
            get { return MetroPaint.ColorTable.Grip.Dark(_theme,_style); }
        }

        public override Color GripLight
        {
            get { return ControlPaint.Light(MetroPaint.ColorTable.Grip.Light(_theme,_style)); }
        }

        #endregion
    }
}
