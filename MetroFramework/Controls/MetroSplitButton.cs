using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.ComponentModel;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;


namespace MetroFramework.Controls
{
    [System.Drawing.ToolboxBitmapAttribute(typeof(System.Windows.Forms.Button))]
    public class MetroSplitButton : MetroButton
    {

        private const int PushButtonWidth = 14;
        private readonly static int BorderSize = SystemInformation.Border3DSize.Width * 2;
        private bool skipNextOpen = false;
        private Rectangle dropDownRectangle = new Rectangle();
        private bool showSplit = true;

        public MetroSplitButton()
        {
            AutoSize = true;
        }

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


            shadow = new Pen(MetroPaint.ColorTable.Grip.Dark(Theme, Style));
            face = new Pen(MetroPaint.ColorTable.Grip.Light(Theme, Style));

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
            PaintArrow(g, dropDownRectangle);
        }

        private static void PaintArrow(Graphics g, Rectangle dropDownRect)
        {
            Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            //if the width is odd - favor pushing it over one pixel right. 
            middle.X += (dropDownRect.Width % 2);

            Point[] arrow = new Point[] { new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1), new Point(middle.X, middle.Y + 2) };

            g.FillPolygon(SystemBrushes.ControlText, arrow);
        }

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
    }
}

