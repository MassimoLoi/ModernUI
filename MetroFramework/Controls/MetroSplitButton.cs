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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.ComponentModel;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Design;


namespace MetroFramework.Controls
{
    [Designer(typeof(MetroSplitButtonDesigner))]
    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.Button))]
    public class MetroSplitButton : MetroButtonStyled
    {

        #region "   Fields   "
        private const int PushButtonWidth = 16;
        private readonly static int BorderSize = SystemInformation.Border3DSize.Width * 2;
        private bool skipNextOpen = false;
        private Rectangle dropDownRectangle = new Rectangle();
        private bool showSplit = true;
        #endregion

        #region "   Constructor   "

        public MetroSplitButton()
        {
            AutoSize = true;
            this.FlatAppearance = true;

        }
        #endregion

        #region "   Properties   "
        [DefaultValue(true)]
        public bool ShowSplit
        {
            set
            {
                if (value != showSplit)
                {
                    showSplit = value;
                    Invalidate();
                    if (Parent != null)
                    {
                        Parent.PerformLayout();
                    }
                }
            }
            get
            {
                return showSplit;
            }
        }
        #endregion

        #region "   Overrides   "

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);
            if (showSplit && !string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + PushButtonWidth > preferredSize.Width)
            {
                return preferredSize + new Size(PushButtonWidth + BorderSize * 2, 0);
            }
            return preferredSize;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData.Equals(Keys.Down) && showSplit)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (showSplit && kevent.KeyCode.Equals(Keys.Down))
            {
                ShowContextMenuStrip();
            }

            base.OnKeyDown(kevent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseDown(e);
                return;
            }

            if (dropDownRectangle.Contains(e.Location))
            {
                ShowContextMenuStrip();
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (!showSplit)
            {
                base.OnMouseUp(mevent);
                return;
            }

            if (ContextMenuStrip == null || !ContextMenuStrip.Visible)
            {
                base.OnMouseUp(mevent);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (!showSplit)
            {
                return;
            }

            Graphics g = pevent.Graphics;
            Rectangle bounds = ClientRectangle;

            // calculate the current dropdown rectangle. 
            dropDownRectangle = new Rectangle(bounds.Right - PushButtonWidth - 1, BorderSize, PushButtonWidth, bounds.Height - BorderSize * 2);

            int internalBorder = BorderSize;
            Rectangle focusRect =
               new Rectangle(internalBorder,
                          internalBorder,
                          bounds.Width - dropDownRectangle.Width - internalBorder,
                          bounds.Height - (internalBorder * 2));

            Pen shadow = SystemPens.ButtonShadow;
            Pen face = SystemPens.ButtonFace;


            shadow = new Pen(MetroPaint.BorderColor.Button.Disabled(Theme));
            face = new Pen(MetroPaint.BorderColor.Button.Normal(Theme));

           // if (palette != null)
           // {
           //     shadow = new Pen(palette.ColorTable.GripDark);
           //     face = new Pen(palette.ColorTable.GripLight);
           // }

            if (RightToLeft == RightToLeft.Yes)
            {
                dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = dropDownRectangle.Right;

                // TODO: Fix Lines here? 
                // draw two lines at the edge of the dropdown button 
                g.DrawLine(shadow, bounds.Left + PushButtonWidth, BorderSize, bounds.Left + PushButtonWidth, bounds.Bottom - BorderSize);
                g.DrawLine(face, bounds.Left + PushButtonWidth + 1, BorderSize, bounds.Left + PushButtonWidth + 1, bounds.Bottom - BorderSize);
            }
            else
            {
                // draw two lines at the edge of the dropdown button 
                g.DrawLine(shadow, bounds.Right - PushButtonWidth, BorderSize, bounds.Right - PushButtonWidth, bounds.Bottom - BorderSize);
                g.DrawLine(face, bounds.Right - PushButtonWidth - 1, BorderSize, bounds.Right - PushButtonWidth - 1, bounds.Bottom - BorderSize);
            }

            // Draw an arrow in the correct location 
            MetroDrawingMethods.PaintDownArrow(g, dropDownRectangle, MetroPaint.BorderColor.Button.Disabled(Theme), 0, 0);
        }
        #endregion

        #region "   Contextmenu Handling   "
        private void ShowContextMenuStrip()
        {
            if (skipNextOpen)
            {
                // we were called because we're closing the context menu strip 
                // when clicking the dropdown button. 
                skipNextOpen = false;
                return;
            }

            if (ContextMenu != null)
            {
                ContextMenu.Show(this, FindForm().PointToScreen(Location) + new Size(0, Height));
                //ContextMenu.Closed += ContextMenu_Closed;
            }
            else if (ContextMenuStrip != null)
            {
                ContextMenuStrip.Closing += ContextMenuStrip_Closing;
                ContextMenuStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }



        void ContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenu kcm = sender as ContextMenu;
            if (kcm != null)
            {
                //kcm.Closed -= ContextMenu_Closed;
            }

            //if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked) 
            //{ 
            skipNextOpen = (dropDownRectangle.Contains(PointToClient(Cursor.Position)));
            //} 
        }

        void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            if (cms != null)
            {
                cms.Closing -= ContextMenuStrip_Closing;
            }

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                skipNextOpen = (dropDownRectangle.Contains(PointToClient(Cursor.Position)));
            }
        }
        #endregion
    }
}

