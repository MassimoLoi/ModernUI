
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
    class MetroCTXRenderer : ToolStripProfessionalRenderer
    {
        private MetroFramework.MetroThemeStyle theme = MetroThemeStyle.Default;
        private MetroColorStyle style = MetroColorStyle.Default;

        public MetroCTXRenderer(MetroFramework.MetroThemeStyle Theme, MetroColorStyle Style)
            : base(new MetroCTXColorTable(Theme, Style))
        {
            theme = Theme;
            style = Style;
        }

        #region Overrides
        /// <summary>
        /// Raises the RenderItemText event.
        /// </summary>
        /// <param name="e">A ToolStripItemTextRenderEventArgs that contains the event data.</param>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // e.TextFont = MetroFonts.Default(12);

            if ((e.ToolStrip is MenuStrip) ||
                (e.ToolStrip is ToolStrip) ||
                (e.ToolStrip is ContextMenuStrip) ||
                (e.ToolStrip is ToolStripDropDownMenu))
            {

                // We set the color depending on the enabled state
                if (!e.Item.Enabled)
                    e.TextColor = MetroPaint.ForeColor.MenuItem.Disabled(theme);
                else
                {
                    if (e.ToolStrip is MenuStrip || e.ToolStrip is ContextMenuStrip || e.ToolStrip is ToolStrip)
                    {
                        if (!e.Item.Pressed && !e.Item.Selected)
                        {
                            e.TextColor = MetroPaint.ForeColor.MenuItem.Normal(theme);
                        }
                        else
                        {
                            e.TextColor = MetroPaint.ForeColor.MenuItem.Press(theme, style);
                        }
                    }

                    if (e.ToolStrip is StatusStrip)
                    {
                        if (!e.Item.Pressed && !e.Item.Selected)
                        {
                            e.TextColor = MetroPaint.ForeColor.MenuItem.Normal(theme);
                        }
                        else
                        {
                            e.TextColor = MetroPaint.ForeColor.MenuItem.Hover(theme, style);
                        }
                    }
                }

                base.OnRenderItemText(e);

            }
            else
            {
                base.OnRenderItemText(e);
            }
        }
        #endregion


    }
}

