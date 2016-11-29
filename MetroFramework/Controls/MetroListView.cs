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
    [Designer(typeof(MetroListViewDesigner))]
    [System.Drawing.ToolboxBitmapAttribute(typeof(ListView))]
    public class MetroListView : ListView, IMetroControl
    {
        #region "   Interface   "

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

        #region ... Structs ...
        [StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        //ListView item information
        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct LVITEM
        {
            public uint mask;
            public int iItem;
            public int iSubItem;
            public uint state;
            public uint stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
        }
        #endregion

        #region ... Enums ...

        private enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        private enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        //fnBar values
        private enum SBTYPES
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }
        //lpsi values
        private enum LPCSCROLLINFO
        {
            SIF_RANGE = 0x0001,
            SIF_PAGE = 0x0002,
            SIF_POS = 0x0004,
            SIF_DISABLENOSCROLL = 0x0008,
            SIF_TRACKPOS = 0x0010,
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS)
        }


        public enum ScrollBarCommands
        {
            SB_LINEUP = 0,
            SB_LINELEFT = 0,
            SB_LINEDOWN = 1,
            SB_LINERIGHT = 1,
            SB_PAGEUP = 2,
            SB_PAGELEFT = 2,
            SB_PAGEDOWN = 3,
            SB_PAGERIGHT = 3,
            SB_THUMBPOSITION = 4,
            SB_THUMBTRACK = 5,
            SB_TOP = 6,
            SB_LEFT = 6,
            SB_BOTTOM = 7,
            SB_RIGHT = 7,
            SB_ENDSCROLL = 8
        }
        #endregion

        #region ... Members ...
        private bool _hasFocus = false;
        private IContainer components;
        private ImageList ilCheckBoxes;
        private Font _originalFont;
        private Color _originalForeColor;
        private ImageList ilHeight;
        private const int _minimumItemHeight = 18;
        private ListViewColumnSorter lvwColumnSorter;
        private Color _gradientStartColor = Color.White;
        private Color _gradientEndColor = Color.Gray;
        private Color _gradientMiddleColor = Color.LightGray;

        public delegate void ScrollPositionChangedDelegate(MetroListView listview, int pos);

        public event ScrollPositionChangedDelegate ScrollPositionChanged;
        public event Action<MetroListView> ItemAdded;
        public event Action<MetroListView> ItemsRemoved;

        private int _disableChangeEvents = 0;

        #endregion

        #region ... Properties ...

        //for setting the minimum height of an item
        public virtual int ItemHeight { get; set; }

        ///
        /// Indicates if the current view is being utilized in the VS.NET IDE or not.
        ///
        private bool _designMode;
        public new bool DesignMode
        {
            get
            {
                //return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
                return _designMode; ;
            }
        }

        private int _lineBefore = -1;
        [Browsable(true), Category("Metro Appearance")]
        public int LineBefore
        {
            get { return _lineBefore; }
            set { _lineBefore = value; }
        }

        private int _lineAfter = -1;
        [Browsable(true), Category("Metro Appearance")]
        public int LineAfter
        {
            get { return _lineAfter; }
            set { _lineAfter = value; }
        }
        Boolean _enableDragDrop = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("False")]
        public Boolean EnableDragDrop
        {
            get { return _enableDragDrop; }
            set { _enableDragDrop = value; }
        }

        Color _alternateRowColor = Color.LightGray;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("Color.Gray")]
        public Color AlternateRowColor
        {
            get { return _alternateRowColor; }
            set { _alternateRowColor = value; InitColors(); }
        }

        Boolean _alternateRowColorEnabled = true;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("true")]
        public Boolean AlternateRowColorEnabled
        {
            get { return _alternateRowColorEnabled; }
            set { _alternateRowColorEnabled = value; InitColors(); }
        }

        Boolean _selectEntireRowOnSubItem = true;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean SelectEntireRowOnSubItem
        {
            get { return _selectEntireRowOnSubItem; }
            set { _selectEntireRowOnSubItem = value; }
        }

        //Boolean _indendFirstItem = true;
        /*[Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean IndendFirstItem
        {
            get { return _indendFirstItem; }
            set { _indendFirstItem = value; }
        }*/

        Boolean _forceLeftAlign = false;
        [Browsable(false), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean ForceLeftAlign
        {
            get { return _forceLeftAlign; }
            set { _forceLeftAlign = value; }
        }

        Boolean _autoSizeLastColumn = true;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean AutoSizeLastColumn
        {
            get { return _autoSizeLastColumn; }
            set { _autoSizeLastColumn = value; Invalidate(); }
        }

        Boolean _enableSorting = true;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean EnableSorting
        {
            get { return _enableSorting; }
            set { _enableSorting = value; }
        }

        Boolean _enableHeaderRendering = true;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean EnableHeaderRendering
        {
            get { return _enableHeaderRendering; }
            set { _enableHeaderRendering = value; InitColors(); }
        }

        Boolean _enableHeaderGlow = false;
        [Browsable(false), Category("Metro Appearance")]
        [DefaultValue("False")]
        public Boolean EnableHeaderGlow
        {
            get { return _enableHeaderGlow; }
            set { _enableHeaderGlow = value; InitColors(); }
        }

        Boolean _enableHeaderHotTrack = false;
        [Browsable(false), Category("Metro Appearance")]
        [DefaultValue("False")]
        public Boolean EnableHeaderHotTrack
        {
            get { return _enableHeaderHotTrack; }
            set
            {
                _enableHeaderHotTrack = value;
                if (value)
                {
                    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                }
                else
                {
                    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                }
                UpdateStyles();
                InitColors();
            }
        }

        Boolean _enableVistaCheckBoxes = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("False")]
        public Boolean EnableVistaCheckBoxes
        {
            get { return _enableVistaCheckBoxes; }
            set { _enableVistaCheckBoxes = value; Invalidate(); }
        }

        Boolean _enableSelectionBorder = false;
        [Browsable(true), Category("Metro Appearance")]
        [DefaultValue("True")]
        public Boolean EnableSelectionBorder
        {
            get { return _enableSelectionBorder; }
            set { _enableSelectionBorder = value; Invalidate(); }
        }

        private MetroListViewSize metroListViewSize = MetroListViewSize.Medium;

        [Category("Metro Appearance")]
        [Browsable(false)]
        public MetroListViewSize FontSize
        {
            get { return metroListViewSize; }
            set { metroListViewSize = value; Refresh(); }
        }

        private MetroListViewWeight metroListViewWeight = MetroListViewWeight.Regular;
        [Category("Metro Appearance")]
        [Browsable(false)]
        public MetroListViewWeight FontWeight
        {
            get { return metroListViewWeight; }
            set { metroListViewWeight = value; Refresh(); }
        }
        #endregion

        #region ... WinApi CONST ...
        private const UInt32 WM_VSCROLL = 0x0115;
        private const UInt32 WM_HSCROLL = 0x114;
        private const UInt32 WM_NCCALCSIZE = 0x83;

        private const UInt32 LVM_FIRST = 0x1000;
        private const UInt32 LVM_INSERTITEMA = (LVM_FIRST + 7);
        private const UInt32 LVM_INSERTITEMW = (LVM_FIRST + 77);
        private const UInt32 LVM_DELETEITEM = (LVM_FIRST + 8);
        private const UInt32 LVM_DELETEALLITEMS = (LVM_FIRST + 9);

        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;
        private const int GWL_STYLE = -16;

        private const Int32 SB_HORZ = 0;
        private const Int32 SB_VERT = 1;
        private const Int32 SB_BOTH = 3;

        #endregion

        #region ... Constructor ....
        public MetroListView()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            UpdateStyles();

            //for image list
            InitializeComponent();


            if (this.View == System.Windows.Forms.View.Details)
                this.OwnerDraw = true;

            //store the original font
            _originalFont = (Font)this.Font.Clone();

            //store the original foreColor
            _originalForeColor = (Color)this.ForeColor;

            //Init Colors
            InitColors();

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            lvwColumnSorter = new ListViewColumnSorter();
            this.ListViewItemSorter = lvwColumnSorter;

            if (_selectEntireRowOnSubItem == true)
            {
                this.FullRowSelect = true;
            }

            //vista
            _enableVistaCheckBoxes = IsOsVistaOrGreater();

            //Design Mode
            _designMode = (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");

            //set a minimum item height
            if (this.SmallImageList == null)
            {
                this.SmallImageList = ilHeight;
            }

            SelectedIndexChanged += new EventHandler(CustomListView_SelectedIndexChanged);

        }

        private bool IsOsVistaOrGreater()
        {
            bool result = false;
            Version OSVer = System.Environment.OSVersion.Version;
            if (OSVer.Major >= 6)
                result = true;
            else
                result = false;

            return result;
        }
        #endregion

        #region ... Initialize Component ....
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetroListView));
            this.ilCheckBoxes = new System.Windows.Forms.ImageList(this.components);
            this.ilHeight = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ilCheckBoxes
            // 
            this.ilCheckBoxes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCheckBoxes.ImageStream")));
            this.ilCheckBoxes.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCheckBoxes.Images.SetKeyName(0, "XpNotChecked.gif");
            this.ilCheckBoxes.Images.SetKeyName(1, "XpChecked.gif");
            this.ilCheckBoxes.Images.SetKeyName(2, "VistaNotChecked.png");
            this.ilCheckBoxes.Images.SetKeyName(3, "VistaChecked.png");
            this.ilCheckBoxes.Images.SetKeyName(4, "Win10NotChecked.ico");
            this.ilCheckBoxes.Images.SetKeyName(5, "Win10Checked.ico");
            // 
            // ilHeight
            // 
            this.ilHeight.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilHeight.ImageSize = new System.Drawing.Size(1, 16);
            this.ilHeight.TransparentColor = System.Drawing.Color.Transparent;
            this.ResumeLayout(false);

        }

        private void InitColors()
        {

            _gradientStartColor = MetroPaint.GetStyleColor(Style); // _palette.ColorTable.StatusStripGradientBegin;
            _gradientEndColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.OverflowButtonGradientEnd;
            _gradientMiddleColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.StatusStripGradientEnd;

            _alternateRowColor = MetroPaint.BackColor.Form(Theme); //_palette.ColorTable.ToolStripContentPanelGradientBegin;

            this.BackColor = MetroPaint.BackColor.DataGrid.Normal(Theme);

            Invalidate();
        }
        #endregion

        #region ... DrawItem and SubItem ...
        //Draw Item
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {

            //set font 
            //get Colors And Font
            this.ForeColor = MetroPaint.ForeColor.ListView.Normal(Theme); // GetForeTextColor(PaletteState.Normal);
            this.Font = MetroFonts.ListView(metroListViewSize, metroListViewWeight);


            if (!DesignMode)
            {
                Rectangle rect = e.Bounds;
                Graphics g = e.Graphics;

                //minimum item height
                if (rect.Height < _minimumItemHeight)
                { rect.Height = (int)_minimumItemHeight; }


                //rect.Height -= 1;
                //rect.Width -= 1;


                //force Left align on items
                if (_forceLeftAlign == true)
                {
                    foreach (ColumnHeader col in this.Columns)
                    {
                        col.TextAlign = HorizontalAlignment.Left;
                    }
                }

                if (this.View == View.Details)
                {
                    if (((e.State & ListViewItemStates.Selected) != 0) && (e.Item.Selected == true))
                    {
                        InternalRenderer(ref g, ref rect); //InternalRenderer
                    }
                    else
                    {
                        // Draw the background for an unselected item.
                        e.DrawDefault = true;
                    }
                }
                // Draw the item text for views other than the Details view.
                else
                {
                    // Draw the background for an unselected item.
                    e.DrawDefault = true;
                }
            }
            else
                e.DrawDefault = true;

        }


        //Draw SubItem
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            //set font 
            //get Colors And Font
            this.ForeColor = MetroPaint.ForeColor.ListView.Normal(Theme); // GetForeTextColor(PaletteState.Normal);
            this.Font = MetroFonts.ListView(metroListViewSize, metroListViewWeight); //GetForeTextFont(PaletteState.Normal);

            if (!DesignMode)
            {
                Rectangle rect = e.Bounds;
                Graphics g = e.Graphics;

                //minimum item height
                if (rect.Height < _minimumItemHeight)
                { rect.Height = (int)_minimumItemHeight; }

                //rect.Height -= 1;
                //rect.Width -= 1;

                //for correct string drawing Space
                int MeasureStringWidth = e.Header.Width;

                // We want to anti alias the drawing for nice smooth curves
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                //force Left align on items
                if (_forceLeftAlign == true)
                {
                    foreach (ColumnHeader col in this.Columns)
                    {
                        col.TextAlign = HorizontalAlignment.Left;
                    }
                }

                if (this.View == View.Details)
                {
                    if ((e.ItemState & ListViewItemStates.Selected) != 0)
                    {
                        //CheckBox present?
                        if ((this.CheckBoxes == true) && (e.ColumnIndex == 0))
                        {
                            //string CheckState = "V";

                            Image Check;
                            if (e.Item.Checked == true)
                            {
                                //CheckState = "V";
                                if (_enableVistaCheckBoxes == true)
                                    Check = ilCheckBoxes.Images[3];
                                else
                                    Check = ilCheckBoxes.Images[5];
                            }
                            else
                            {
                                //CheckState = "O";
                                if (_enableVistaCheckBoxes == true)
                                    Check = ilCheckBoxes.Images[2];
                                else
                                    Check = ilCheckBoxes.Images[4];
                            }

                            g.DrawImage(Check, rect.X + 2, rect.Y + 2, 16, 16);
                            //g.DrawRectangle(new Pen(Color.Red), rect.X + 2, rect.Y + 2, 16, 16);

                            //move the rect to the right
                            rect.Offset(16, 0);

                            //fix for string drawing
                            MeasureStringWidth -= 19;

                        }

                        //Picture Present?
                        if (e.ColumnIndex == 0)
                        {
                            if (this.SmallImageList != null && SmallImageList.Images.Count >0)
                            {
                                Size imgSize = this.SmallImageList.ImageSize;
                                //g.DrawRectangle(new Pen(Color.Green), rect.X + 3, rect.Y + 2, imgSize.Width, imgSize.Height);
                                try
                                { 
                                    this.SmallImageList.Draw(g, rect.X + 4, rect.Y + 3, imgSize.Width, imgSize.Height, e.Item.ImageIndex); 
                                }
                                catch
                                { }

                                //move the rect to the right
                                 rect.Offset(15, 0);

                                //fix for string drawing
                                MeasureStringWidth -= imgSize.Width;
                            }
                        }

                        rect.Offset(5, 2);

                        //drawText
                        Color textColor = MetroPaint.ForeColor.ListView.Selected(Theme, Style); // GetForeTextColor(PaletteState.CheckedPressed);
                        //if (!_hasFocus)
                        //    textColor = GetForeTextColor(PaletteState.Normal);


                        //compact the text:
                        string TextToDraw = CompactString(e.SubItem.Text, MeasureStringWidth, this.Font, TextFormatFlags.EndEllipsis);

                        //Draw String
                        e.Graphics.DrawString(TextToDraw, this.Font, new SolidBrush(textColor), rect);


                        //e.DrawFocusRectangle(rect);
                    }
                    else
                    {
                        // Draw the background for an unselected item.
                        e.DrawDefault = true;
                    }
                }
                // Draw the item text for views other than the Details view.
                else
                {
                    // Draw the background for an unselected item.
                    e.DrawDefault = true;
                }

            }
            else
                e.DrawDefault = true;

        }
        #endregion

        #region   ... Drawing Renderers ...
        private Color GetForeTextColorHeader(bool isHot)
        {
            Color textColor = MetroPaint.ForeColor.DataGrid.ColumnHeadersDefaultCellStyle(Theme, Style);
            if (isHot) textColor = MetroPaint.ForeColor.DataGrid.ColumnHeadersDefaultCellStyleSelectionForeColor(Theme, Style);

            return textColor;
        }
        private Color GetForeTextColor(bool isHot)
        {
            Color textColor = MetroPaint.ForeColor.DataGrid.Normal(Theme);
            if (isHot) textColor = MetroPaint.ForeColor.DataGrid.DefaultCellStyleSelectionForeColor(Theme, Style);

            return textColor;
        }

        private Font GetHeaderForeTextFont()
        {
            Font textFont = MetroFonts.ListView(metroListViewSize, MetroListViewWeight.Bold);

            //return value
            return textFont;
        }


        private void InternalRenderer(ref Graphics g, ref Rectangle rect)
        {
            _gradientStartColor = MetroPaint.GetStyleColor(Style);
            _gradientEndColor = MetroPaint.GetStyleColor(Style);
            _gradientMiddleColor = MetroPaint.GetStyleColor(Style);

            //draw
            MetroDrawingMethods.DrawGradient(g, rect, _gradientStartColor, _gradientEndColor, 90F, _enableSelectionBorder, _gradientMiddleColor, 1);
        }

        private void InternalRendererHeader(ref Graphics g, ref Rectangle rect, bool bHot, ref DrawListViewColumnHeaderEventArgs e)
        {
            //set colors
            Color gradStartColor;
            Color gradEndColor;
            Color gradMiddleColor;
            Color borderColor = MetroPaint.BorderColor.DataGrid.GridColor(Theme); //; // _palette.ColorTable.ToolStripBorder;

            if (e.State == ListViewItemStates.Selected)
            {
                gradStartColor = MetroPaint.GetStyleColor(Style);// _palette.ColorTable.ButtonSelectedGradientBegin;
                gradMiddleColor = MetroPaint.GetStyleColor(Style);// _palette.ColorTable.ButtonCheckedGradientEnd;
                gradEndColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.ButtonCheckedGradientBegin;

            }
            else
            {
                if (bHot)
                {
                    gradStartColor = Color.White;// _palette.ColorTable.ButtonSelectedGradientBegin;
                    gradMiddleColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.ButtonSelectedGradientEnd;
                    gradEndColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.ButtonSelectedGradientBegin;
                }
                else
                {
                    gradStartColor = MetroPaint.GetStyleColor(Style);//_palette.ColorTable.ToolStripGradientBegin;
                    gradMiddleColor = MetroPaint.GetStyleColor(Style); //_palette.ColorTable.ToolStripGradientEnd;
                    gradEndColor = MetroPaint.GetStyleColor(Style); ; //_palette.ColorTable.ToolStripGradientBegin;
                }
            }
            //Fill Gradient
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, gradStartColor, gradMiddleColor, LinearGradientMode.Vertical))
            {
                if (!_enableHeaderGlow)
                    g.FillRectangle(brush, rect);
                else
                    MetroDrawingMethods.DrawListViewHeader(g, rect, gradStartColor, gradMiddleColor, 90F);
            }

            //DrawBorder
            g.DrawRectangle(new Pen(borderColor), rect);

            //Draw light lines
            //oriz
            //g.DrawLine(new Pen(Color.White), new Point(rect.X + 1, rect.Y + 1), new Point(rect.X + rect.Width - 1, rect.Y + 1));
            //vert
            //g.DrawLine(new Pen(Color.White), new Point(rect.X + 1, rect.Y + 1), new Point(rect.X + 1, rect.Y + rect.Height - 1));

            if (e.ColumnIndex == this.Columns.Count - 1)
                g.DrawLine(new Pen(borderColor), new Point(rect.X + rect.Width - 1, rect.Y), new Point(rect.X + rect.Width - 1, rect.Y + rect.Height + 0));

        }


        private void HeaderPressedOffset(ref Rectangle rect, ListViewItemStates State)
        {
            //min rect height for SystemColors (Theme disabled)
            if ((int)rect.Height > (int)15)
            {
                //Draw Normal
                if (State == ListViewItemStates.Selected)
                { rect.Offset(3, 3); }
                else
                { rect.Offset(2, 2); }
            }
            else
            {
                //draw a little up
                if (State == ListViewItemStates.Selected)
                { rect.Offset(3, 2); }
                else
                { rect.Offset(2, 1); }
            }
        }
        #endregion

        #region ... Draw Header ...
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            if (!DesignMode)
            {

                if (_enableHeaderRendering == true)
                {
                    Rectangle rect = e.Bounds;
                    rect.Height = rect.Height - 2;
                    rect.Width = rect.Width - 0;
                    Graphics g = e.Graphics;

                    Point mouse = new Point();
                    mouse = PointToClient(Control.MousePosition);

                    //Header HotTrack
                    bool bHot = false;
                    if (_enableHeaderHotTrack)
                    {
                        //Mod
                        Invalidate();

                        Rectangle mouserect = new Rectangle();
                        mouserect = e.Bounds;
                        mouserect.Width += 2; //expand the rectangle to make the check more stable
                        mouserect.Height += 2;

                        if (mouserect.Contains(mouse))
                            bHot = true;
                    }

                    //ClearType
                    try
                    { e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; }
                    catch
                    { }


                    //Empty Area
                    g.FillRectangle(new SolidBrush(Color.White), rect);

                    //design with The Correct renderer
                    try
                    {
                        InternalRendererHeader(ref g, ref  rect, bHot, ref  e); //InternalRendererHeader
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    //OffSet Header
                    HeaderPressedOffset(ref rect, e.State);

                    //get Colors And Font
                    Color textColor = GetForeTextColorHeader(bHot);
                    Font textFont = GetHeaderForeTextFont();

                    //set text properties
                    StringFormat TextFormat = new StringFormat();
                    TextFormat.FormatFlags = StringFormatFlags.NoWrap;
                    TextFormat.Alignment = ConvertHorizontalAlignmentToStringAlignment(e.Header.TextAlign);

                    //string Ellipsis
                    string ColumnHeaderString = CompactString(e.Header.Text, rect.Width, textFont, TextFormatFlags.EndEllipsis);

                    //draw Text
                    g.DrawString(ColumnHeaderString, textFont, new SolidBrush(textColor), rect, TextFormat);
                    //e.DrawText();


                    //Draw sort indicator
                    if (this.Columns[e.ColumnIndex].Tag != null)
                    {
                        SortOrder sort = (SortOrder)this.Columns[e.ColumnIndex].Tag;

                        // Prepare arrow
                        if (sort == SortOrder.Ascending)
                            g.FillRectangle(new SolidBrush(Color.Red), rect.X + rect.Width - 8, rect.Y, 8, 8);
                        else if (sort == SortOrder.Descending)
                            g.FillRectangle(new SolidBrush(Color.Green), rect.X + rect.Width - 8, rect.Y, 8, 8);
                    }

                }
                else
                    e.DrawDefault = true;

            }
            else
                e.DrawDefault = true;

        }

        #endregion

        #region ... Helper Subs ...
        public StringAlignment ConvertHorizontalAlignmentToStringAlignment(HorizontalAlignment input)
        {

            switch (input)
            {
                case HorizontalAlignment.Center:
                    return StringAlignment.Center;

                case HorizontalAlignment.Right:
                    return StringAlignment.Far;

                case HorizontalAlignment.Left:
                    return StringAlignment.Near;

            }
            return StringAlignment.Center;
        }

        private void CleanColumnsTags()
        {
            int i;

            for (i = 0; i < this.Columns.Count; i++)
            {
                this.Columns[i].Tag = null;
            }
            Invalidate();
        }

        //create Graphics Path
        private GraphicsPath CreateRectGraphicsPath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            return path;
        }

        /// <summary>
        /// Draw a line with insertion marks at each end
        /// </summary>
        /// <param name="X1">Starting position (X) of the line</param>
        /// <param name="X2">Ending position (X) of the line</param>
        /// <param name="Y">Position (Y) of the line</param>
        private void DrawInsertionLine(int X1, int X2, int Y)
        {
            using (Graphics g = this.CreateGraphics())
            {
                g.DrawLine(Pens.Red, X1, Y, X2 - 1, Y);

                Point[] leftTriangle = new Point[3] {
                            new Point(X1,      Y-4),
                            new Point(X1 + 7,  Y),
                            new Point(X1,      Y+4)
                        };
                Point[] rightTriangle = new Point[3] {
                            new Point(X2,     Y-4),
                            new Point(X2 - 8, Y),
                            new Point(X2,     Y+4)
                        };
                g.FillPolygon(Brushes.Red, leftTriangle);
                g.FillPolygon(Brushes.Red, rightTriangle);
            }
        }


        private string CompactString(string MyString, int Width, Font Font, TextFormatFlags FormatFlags)
        {
            string result = string.Copy(MyString);

            TextRenderer.MeasureText(result, Font, new Size(Width, 0), FormatFlags | TextFormatFlags.ModifyString);

            return result;
        }

        #endregion

        #region ... Scrollbar Implemenation ...
        private void BeginDisableChangeEvents()
        {
            _disableChangeEvents++;
        }

        private void EndDisableChangeEvents()
        {
            if (_disableChangeEvents > 0)
                _disableChangeEvents--;
        }

        private MetroScrollBar _vScrollbar = null;

        public MetroScrollBar VScrollbar
        {
            get { return _vScrollbar; }
            set
            {
                if (value != null)
                {
                    UpdateScrollbar();

                    value.Scroll += new ScrollEventHandler(_grid_Scroll); //ValueChangedDelegate(value_ValueChanged);
                    //public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
                }

                _vScrollbar = value;
            }
        }

        void CustomListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateScrollbar();
        }

        void _grid_Scroll(object sender, ScrollEventArgs e)
        {
            SetScrollPosition(_vScrollbar.Value);
        }

        private void value_ValueChanged(MetroScrollBar sender, int newValue)
        {
            if (_disableChangeEvents > 0)
                return;

            SetScrollPosition(_vScrollbar.Value);
        }

        public void GetScrollPosition(out int min, out int max, out int pos, out int smallchange, out int largechange)
        {
            SCROLLINFO scrollinfo = new SCROLLINFO();
            scrollinfo.cbSize = (uint)Marshal.SizeOf(typeof(SCROLLINFO));
            scrollinfo.fMask = (int)ScrollInfoMask.SIF_ALL;
            if (GetScrollInfo(this.Handle, (int)SBTYPES.SB_VERT, ref scrollinfo))
            {
                min = scrollinfo.nMin;
                max = scrollinfo.nMax;
                pos = scrollinfo.nPos;
                smallchange = 1;
                largechange = (int)scrollinfo.nPage;
            }
            else
            {
                min = 0;
                max = 0;
                pos = 0;
                smallchange = 0;
                largechange = 0;
            }
        }


        private void UpdateScrollbar()
        {
            if (_vScrollbar != null)
            {
                int max, min, pos, smallchange, largechange;
                GetScrollPosition(out min, out max, out pos, out smallchange, out largechange);

                BeginDisableChangeEvents();
                _vScrollbar.Value = pos;
                _vScrollbar.Maximum = max - largechange + 1;
                _vScrollbar.Minimum = min;
                _vScrollbar.SmallChange = smallchange;
                _vScrollbar.LargeChange = largechange;
                EndDisableChangeEvents();
            }
        }

        public void SetScrollPosition(int pos)
        {
            pos = Math.Min(Items.Count - 1, pos);

            if (pos < 0 || pos >= Items.Count)
                return;

            SuspendLayout();
            EnsureVisible(pos);

            for (int i = 0; i < 10; i++)
            {
                if (TopItem != null && TopItem.Index != pos)
                    TopItem = Items[pos];
            }

            ResumeLayout();
        }


        protected void OnItemAdded()
        {
            if (_disableChangeEvents > 0) return;

            UpdateScrollbar();

            if (ItemAdded != null)
                ItemAdded(this);
        }

        protected void OnItemsRemoved()
        {
            if (_disableChangeEvents > 0) return;

            UpdateScrollbar();

            if (ItemsRemoved != null)
                ItemsRemoved(this);
        }

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return (int)GetWindowLong32(hWnd, nIndex);
            else
                return (int)(long)GetWindowLongPtr64(hWnd, nIndex);
        }

        public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
                return (int)SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            else
                return (int)(long)SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }
        #endregion

        #region ... Overrides ...
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        // The LVItem being dragged
        private ListViewItem _itemDnD = null;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_enableDragDrop)
            {
                _itemDnD = this.GetItemAt(e.X, e.Y);
                // if the LV is still empty, no item will be found anyway, so we don't have to consider this case
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_enableDragDrop)
            {
                if (_itemDnD == null)
                    return;

                try
                {
                    // calculate the bottom of the last item in the LV so that you don't have to stop your drag at the last item
                    int lastItemBottom = Math.Min(e.Y, this.Items[this.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                    // use 0 instead of e.X so that you don't have to keep inside the columns while dragging
                    ListViewItem itemOver = this.GetItemAt(0, lastItemBottom);

                    if (itemOver == null)
                        return;

                    Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);

                    // find out if we insert before or after the item the mouse is over
                    bool insertBefore;
                    if (e.Y < rc.Top + (rc.Height / 2))
                    {
                        insertBefore = true;
                    }
                    else
                    {
                        insertBefore = false;
                    }

                    if (_itemDnD != itemOver) // if we dropped the item on itself, nothing is to be done
                    {
                        if (insertBefore)
                        {
                            this.Items.Remove(_itemDnD);
                            this.Items.Insert(itemOver.Index, _itemDnD);
                        }
                        else
                        {
                            this.Items.Remove(_itemDnD);
                            this.Items.Insert(itemOver.Index + 1, _itemDnD);
                        }
                    }

                    // clear the insertion line
                    this.LineAfter =
                    this.LineBefore = -1;

                    this.Invalidate();

                }
                finally
                {
                    // finish drag&drop operation
                    _itemDnD = null;
                    Cursor = Cursors.Default;
                }
            }

            base.OnMouseUp(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _hasFocus = true;
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            _hasFocus = false;
            base.OnLostFocus(e);
            //Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            //Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_enableDragDrop)
            {
                if (_itemDnD == null)
                    return;

                // Show the user that a drag operation is happening
                Cursor = Cursors.Hand;

                // calculate the bottom of the last item in the LV so that you don't have to stop your drag at the last item
                int lastItemBottom = Math.Min(e.Y, this.Items[this.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                // use 0 instead of e.X so that you don't have to keep inside the columns while dragging
                ListViewItem itemOver = this.GetItemAt(0, lastItemBottom);

                if (itemOver == null)
                    return;

                Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
                if (e.Y < rc.Top + (rc.Height / 2))
                {
                    this.LineBefore = itemOver.Index;
                    this.LineAfter = -1;
                }
                else
                {
                    this.LineBefore = -1;
                    this.LineAfter = itemOver.Index;
                }

                // invalidate the LV so that the insertion line is shown
                Invalidate();
            }
            base.OnMouseMove(e);
        }
        protected override void OnItemMouseHover(ListViewItemMouseHoverEventArgs e)
        {
            base.OnItemMouseHover(e);
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            if (_enableSorting == true)
            {
                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorter.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorter.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorter.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorter.SortColumn = e.Column;
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }

                //set info for sort image
                //CleanColumnsTag();
                //this.Columns[e.Column].Tag = lvwColumnSorter.Order;

                // Perform the sort with these new sort options.
                this.Sort();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_vScrollbar != null)
                _vScrollbar.Value -= 3 * Math.Sign(e.Delta);
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_VSCROLL) //Vertical
            {
                int max, min, pos, smallchange, largechange;
                GetScrollPosition(out min, out max, out pos, out smallchange, out largechange);

                if (ScrollPositionChanged != null)
                    ScrollPositionChanged(this, pos);

                if (_vScrollbar != null)
                    _vScrollbar.Value = pos;
            }
            else if (m.Msg == WM_HSCROLL) //Horizontal
            {
                //to do
            }
            else if (m.Msg == WM_NCCALCSIZE) // WM_NCCALCSIZE
            {
                int style = (int)GetWindowLong(this.Handle, GWL_STYLE);
                if ((style & WS_VSCROLL) == WS_VSCROLL) //Vertical
                    SetWindowLong(this.Handle, GWL_STYLE, style & ~WS_VSCROLL);

                if ((style & WS_HSCROLL) == WS_HSCROLL) //Horizontal
                    SetWindowLong(this.Handle, GWL_STYLE, style & ~WS_HSCROLL);

            }
            else if (m.Msg == LVM_INSERTITEMA || m.Msg == LVM_INSERTITEMW)
                OnItemAdded();
            else if (m.Msg == LVM_DELETEITEM || m.Msg == LVM_DELETEALLITEMS)
                OnItemsRemoved();

            //To avoid errors on designer
            if (!DesignMode)
            {
                if (_autoSizeLastColumn)
                {
                    // if the control is in details view mode and columns
                    // have been added, then intercept the WM_PAINT message
                    // and reset the last column width to fill the list view
                    switch (m.Msg)
                    {
                        case WinApi.WM_PAINT:
                            if (this.View == View.Details && this.Columns.Count > 0)
                                this.Columns[this.Columns.Count - 1].Width = -2;
                            for (int i = 0; i < this.Columns.Count - 1; i++)
                            {
                                this.Columns[i].Width = this.Columns[i].Width;
                            }
                            if (_enableDragDrop)
                            {
                                if (LineBefore >= 0 && LineBefore < Items.Count)
                                {
                                    Rectangle rc = Items[LineBefore].GetBounds(ItemBoundsPortion.Entire);
                                    DrawInsertionLine(rc.Left, rc.Right, rc.Top);
                                }
                                if (LineAfter >= 0 && LineBefore < Items.Count)
                                {
                                    Rectangle rc = Items[LineAfter].GetBounds(ItemBoundsPortion.Entire);
                                    DrawInsertionLine(rc.Left, rc.Right, rc.Bottom);
                                }
                            }
                            break;

                        case WinApi.WM_NCHITTEST:
                            //DRAWITEMSTRUCT dis = (DRAWITEMSTRUCT)Marshal.PtrToStructure(message.LParam, typeof(DRAWITEMSTRUCT));

                            //ColumnHeader ch = this.Columns[dis.itemID];
                            break;
                    }
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        #endregion

        #region ... PInvokes ...
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowScrollBar(IntPtr hwnd, int wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);
        #endregion

    }
}
