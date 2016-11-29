using System;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;
using System.Security;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MetroFramework.Forms
{
    public class MetroForm : Form, IMetroForm
    {
        #region Enums

        public enum MetroFormTextAlign
        {
            Left,
            Center,
            Right
        }

        public enum MetroFormShadowType
        {
            None,
            Flat,
            DropShadow,
            SystemShadow,
            AeroShadow
        }

        public enum MetroFormBorderStyle
        {
            None,
            FixedSingle
        }

        public enum BackImgLocation
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        #endregion

        #region Variables
        private const int FirstButtonSpacerWidth = 40;
        private const int TopBottomMinMaximizeHitboxRange = 50;
        private bool _isMouseXyWithinTopHeaderArea;
        private bool _isInitialized;
        private readonly bool _isVistaOrHigher = IsWinVistaOrHigher();
        #endregion

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
        [Browsable(true)]
        [Category("Metro Appearance")]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Fields

        [Browsable(true)]
        [Category("Metro Appearance")]
        public override Color BackColor
        {
            get
            {
                return MetroPaint.BackColor.Form(Theme);
            }
        }

        private const int borderWidth = 1;
        [Category("Metro Appearance")]
        public  int BorderWidth
        {
            get
            {
                return borderWidth;
            }
        }

        private MetroFormBorderStyle formBorderStyle = MetroFormBorderStyle.None;
        [DefaultValue(MetroFormBorderStyle.None)]
        [Browsable(true)]
        [Category("Metro Appearance")]
        public MetroFormBorderStyle BorderStyle
        {
            get { return formBorderStyle; }
            set { formBorderStyle = value; }
        }


        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        private bool isMovable = true;
        [Category("Metro Appearance")]
        public bool Movable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        [Category("Metro Appearance")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                value.Top = Math.Max(value.Top, DisplayHeader ? 60 : 30);
                base.Padding = value;
            }
        }

        [Category("Metro Appearance")]
        protected override Padding DefaultPadding
        {
            get { return new Padding(20, DisplayHeader ? 60 : 20, 20, 20); }
        }

        private bool displayHeader = true;
        [Category("Metro Appearance")]
        [DefaultValue(true)]
        public bool DisplayHeader
        {
            get { return displayHeader; }
            set
            {
                if (value != displayHeader)
                {
                    Padding p = base.Padding;
                    p.Top += value ? 30 : -35;
                    base.Padding = p;
                }
                displayHeader = value;
            }
        }

        private bool isResizable = true;
        [Category("Metro Appearance")]
        public bool Resizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        private Bitmap _image = null;
        private Image backImage;
        [Category("Metro Appearance")]
        [DefaultValue(null)]
        public Image BackImage
        {
            get { return backImage; }
            set
            {
                backImage = value;
                if (value != null) _image = new Bitmap(value); // ApplyInvert(new Bitmap(value));
                Refresh();
            }
        }

        private Padding backImagePadding = new Padding(200, 10, 0, 0);
        [Category("Metro Appearance")]
        public Padding BackImagePadding
        {
            get { return backImagePadding; }
            set
            {
                backImagePadding = value;
                Refresh();
            }
        }

        private int backImageMaxSize = 50;
        [Category("Metro Appearance")]
        public int BackImageMaxSize
        {
            get { return backImageMaxSize; }
            set
            {
                backImageMaxSize = value;
                Refresh();
            }
        }

        private BackImgLocation backImageLocation;
        [Category("Metro Appearance")]
        [DefaultValue(BackImgLocation.TopLeft)]
        public BackImgLocation BackImageLocation
        {
            get { return backImageLocation; }
            set
            {
                backImageLocation = value;
                Refresh();
            }
        }

        private DwmApi.MARGINS dwmMargins;
        private bool isMarginOk;

        private bool isAeroEnabled = false;
        [Category("Metro Appearance")]
        public bool AeroEnabled
        {
            get { return isAeroEnabled; }
        }


        #endregion

        #region Constructor

        public MetroForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);


            if (!_isVistaOrHigher)
            {
                RemoveCloseButton(this);
                FormBorderStyle = FormBorderStyle.None;
            }

        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor = MetroPaint.BackColor.Form(Theme);
            Color foreColor = MetroPaint.ForeColor.Title(Theme);

            e.Graphics.Clear(backColor);

            using (SolidBrush b = MetroPaint.GetStyleBrush(Style))
            {
                Rectangle topRect = new Rectangle(0, 0, Width, 5);
                e.Graphics.FillRectangle(b, topRect);
            }

            //Border Style
            if (BorderStyle != MetroFormBorderStyle.None)
            {
                Color c = MetroPaint.BorderColor.Form(Theme);

                using (Pen pen = new Pen(c))
                {
                    e.Graphics.DrawLines(pen, new[]
                        {
                            new Point(0, borderWidth),
                            new Point(0, Height - 1),
                            new Point(Width - 1, Height - 1),
                            new Point(Width - 1, borderWidth)
                        });
                }
            }

            //drwa Image
            if (backImage != null && backImageMaxSize != 0)
            {
                Image img = MetroDrawingMethods.ResizeImage(backImage, new Rectangle(0, 0, backImageMaxSize, backImageMaxSize));
                //if (_imageinvert)
                //{
                //    img = MetroImage.ResizeImage((Theme == MetroThemeStyle.Dark) ? _image : backImage, new Rectangle(0, 0, backImageMaxSize, backImageMaxSize));
                //}

                switch (backImageLocation)
                {
                    case BackImgLocation.TopLeft:
                        e.Graphics.DrawImage(img, 0 + backImagePadding.Left, 0 + backImagePadding.Top);
                        break;
                    case BackImgLocation.TopRight:
                        e.Graphics.DrawImage(img, ClientRectangle.Right - (backImagePadding.Right + img.Width), 0 + backImagePadding.Top);
                        break;
                    case BackImgLocation.BottomLeft:
                        e.Graphics.DrawImage(img, 0 + backImagePadding.Left, ClientRectangle.Bottom - (img.Height + backImagePadding.Bottom));
                        break;
                    case BackImgLocation.BottomRight:
                        e.Graphics.DrawImage(img, ClientRectangle.Right - (backImagePadding.Right + img.Width),
                                             ClientRectangle.Bottom - (img.Height + backImagePadding.Bottom));
                        break;
                }
            }

            //draw Text
            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Title, new Point(20, 20), foreColor);

            //design Grip
            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(MetroPaint.ForeColor.FormGrip(Theme)))
                {
                    Size resizeHandleSize = new Size(2, 2);
                    e.Graphics.FillRectangles(b, new Rectangle[] {
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
                    });
                }
            }
        }

        #endregion

        #region Management Methods

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (!_isVistaOrHigher && _isInitialized)
            {
                Refresh();
            }
        }

        public bool FocusMe()
        {
            return WinApi.SetForegroundWindow(Handle);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!_isInitialized)
            {
                if (ControlBox)
                {
                    AddWindowButton(WindowButtons.Close);

                    if (MaximizeBox)
                        AddWindowButton(WindowButtons.Maximize);

                    if (MinimizeBox)
                        AddWindowButton(WindowButtons.Minimize);

                    if (HelpButton)
                        AddWindowButton(WindowButtons.Help);

                    UpdateWindowButtonPosition();
                }

                _isInitialized = true;
            }

            if (DesignMode) return;

            if (_isVistaOrHigher)
            {
                DwmApi.DwmExtendFrameIntoClientArea(Handle, ref dwmMargins);
            }
            else
            {
                Refresh();
            }

        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateWindowButtonPosition();
        }

        #endregion

        #region DWM Methods

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode)
            {
                IntPtr result = default(IntPtr);

                if (_isVistaOrHigher)
                {
                    int dwmHandled = DwmApi.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref result);
                    if (dwmHandled == 1)
                    {
                        m.Result = result;
                        return;
                    }
                }
                else if (m.Msg == (int)WinApi.Messages.WM_NCPAINT || m.Msg == (int)WinApi.Messages.WM_SIZING || m.Msg == (int)WinApi.Messages.WM_NCACTIVATE)
                {
                    using (var graphics = Graphics.FromHwnd(m.HWnd))
                    {
                        using (SolidBrush b = MetroPaint.GetStyleBrush(Style))
                        {
                            graphics.FillRectangle(b, new Rectangle(0, 0, Width, 5));
                        }
                    }
                }

                if (m.Msg == (int)WinApi.Messages.WM_NCCALCSIZE && (int)m.WParam == 1)
                {
                    WinApi.NCCALCSIZE_PARAMS nccsp = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(WinApi.NCCALCSIZE_PARAMS));

                    nccsp.rect0.Top += 0;
                    nccsp.rect0.Bottom += 0;
                    nccsp.rect0.Left += 0;
                    nccsp.rect0.Right += 0;

                    if (!isMarginOk)
                    {
                        dwmMargins.cyTopHeight = 0;
                        dwmMargins.cxLeftWidth = borderWidth;
                        dwmMargins.cyBottomHeight = borderWidth;
                        dwmMargins.cxRightWidth = borderWidth;
                        isMarginOk = true;
                    }

                    Marshal.StructureToPtr(nccsp, m.LParam, false);

                    m.Result = IntPtr.Zero;
                }
                else if (m.Msg == (int)WinApi.Messages.WM_LBUTTONDBLCLK)
                {
                    // Alow the form to be normalized / maximized when
                    // clicked inside our top header area rectangle.
                    if (_isMouseXyWithinTopHeaderArea)
                    {
                        WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
                        _isMouseXyWithinTopHeaderArea = false;
                    }
                }
                else if (m.Msg == (int)WinApi.Messages.WM_NCHITTEST && (int)m.Result == 0)
                {
                    m.Result = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                }
                else if (m.Msg == (int)WinApi.Messages.WM_GETMINMAXINFO)
                {
                    OnGetMinMaxInfo(m.HWnd, m.LParam);
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        [SecuritySafeCritical]
        private unsafe void OnGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            WinApi.MINMAXINFO* pmmi = (WinApi.MINMAXINFO*)lParam;

            //YOROCA MDI PARENT
            Screen s = Screen.FromHandle(hwnd);
            //if (IsMdiChild)
            if (this.Parent != null)
            {
                pmmi->ptMaxSize.x = this.Parent.ClientRectangle.Size.Width;
                pmmi->ptMaxSize.y = this.Parent.ClientRectangle.Size.Height;
            }
            else
            {
                pmmi->ptMaxSize.x = s.WorkingArea.Width;
                pmmi->ptMaxSize.y = s.WorkingArea.Height;
            }
            pmmi->ptMaxPosition.x = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            pmmi->ptMaxPosition.y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            //if (MinimumSize.Width > 0) pmmi->ptMinTrackSize.x = MinimumSize.Width;
            //if (MinimumSize.Height > 0) pmmi->ptMinTrackSize.y = MinimumSize.Height;
            //if (MaximumSize.Width > 0) pmmi->ptMaxTrackSize.x = MaximumSize.Width;
            //if (MaximumSize.Height > 0) pmmi->ptMaxTrackSize.y = MaximumSize.Height;
        }

        private IntPtr HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            Rectangle testRect = Rectangle.Empty;

            Point p = new Point((Int16)lparam, (Int16)((int)lparam >> 16));
            int vPadding = Math.Max(Padding.Right, Padding.Bottom);


            // Determine if mouse xy is within our range of the "top header" area 
            // which allows for double clicking for either minimize/maximizing form.
            testRect = RectangleToScreen(new Rectangle(0, 0, Width - FirstButtonSpacerWidth, TopBottomMinMaximizeHitboxRange));
            _isMouseXyWithinTopHeaderArea = testRect.Contains(p);

            testRect = RectangleToScreen(new Rectangle(0, 0, dwmMargins.cxLeftWidth, dwmMargins.cxLeftWidth));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTTOPLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, dwmMargins.cxRightWidth));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTTOPRIGHT);

            testRect = RectangleToScreen(new Rectangle(0, Height - dwmMargins.cyBottomHeight, dwmMargins.cxLeftWidth, dwmMargins.cyBottomHeight));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTBOTTOMLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - dwmMargins.cxRightWidth, Height - dwmMargins.cyBottomHeight, dwmMargins.cxRightWidth, dwmMargins.cyBottomHeight));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTBOTTOMRIGHT);

            if (isResizable)
            {
                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(p))
                    return new IntPtr((int)WinApi.HitTest.HTBOTTOMRIGHT);
            }

            testRect = RectangleToScreen(new Rectangle(0, 0, Width, dwmMargins.cxLeftWidth));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTTOP);

            testRect = RectangleToScreen(new Rectangle(0, dwmMargins.cxLeftWidth, Width, dwmMargins.cyTopHeight - dwmMargins.cxLeftWidth));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTCAPTION);

            testRect = RectangleToScreen(new Rectangle(0, 0, dwmMargins.cxLeftWidth, Height));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTLEFT);

            testRect = RectangleToScreen(new Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, Height));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTRIGHT);

            testRect = RectangleToScreen(new Rectangle(0, Height - dwmMargins.cyBottomHeight, Width, dwmMargins.cyBottomHeight));
            if (testRect.Contains(p))
                return new IntPtr((int)WinApi.HitTest.HTBOTTOM);


            return new IntPtr((int)WinApi.HitTest.HTCLIENT);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && Movable)
            {
                if (this.Width - borderWidth > e.Location.X && e.Location.X > borderWidth && e.Location.Y > borderWidth)
                    MoveControl(Handle);
            }
        }

        private void MoveControl(IntPtr hWnd)
        {
            WinApi.ReleaseCapture();
            WinApi.SendMessage(hWnd, (int)WinApi.Messages.WM_NCLBUTTONDOWN, (int)WinApi.HitTest.HTCAPTION, 0);
        }

        #endregion

        #region Window Buttons

        private enum WindowButtons
        {
            Minimize,
            Maximize,
            Close,
            Help
        }

        private Dictionary<WindowButtons, MetroFormButton> windowButtonList;

        private void AddWindowButton(WindowButtons button)
        {
            if (windowButtonList == null)
                windowButtonList = new Dictionary<WindowButtons, MetroFormButton>();

            if (windowButtonList.ContainsKey(button))
                return;

            MetroFormButton newButton = new MetroFormButton();

            if (button == WindowButtons.Close)
            {
                newButton.Text = "r";
            }
            else if (button == WindowButtons.Minimize)
            {
                newButton.Text = "0";
            }
            else if (button == WindowButtons.Maximize)
            {
                if (WindowState == FormWindowState.Normal)
                    newButton.Text = "1";
                else
                    newButton.Text = "2";
            }
            else if (button == WindowButtons.Help)
            {
                newButton.Text = "s";
            }

            newButton.Tag = button;
            newButton.Size = new Size(30, 20);
            newButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newButton.Click += new EventHandler(WindowButton_Click);
            Controls.Add(newButton);

            windowButtonList.Add(button, newButton);
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            MetroFormButton btn = sender as MetroFormButton;
            if (btn != null)
            {
                WindowButtons btnFlag = (WindowButtons)btn.Tag;
                if (btnFlag == WindowButtons.Close)
                {
                    Close();
                }
                else if (btnFlag == WindowButtons.Minimize)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else if (btnFlag == WindowButtons.Maximize)
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Maximized;
                        btn.Text = "2";
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        btn.Text = "1";
                    }
                }
                else if (btnFlag == WindowButtons.Help)
                {
                    OnHelpButtonClicked(new CancelEventArgs(false));
                    btn.Text = "s";
                }
            }
        }

        private void UpdateWindowButtonPosition()
        {

            // Button drawing priority.
            var priorityOrder = new Dictionary<int, WindowButtons>(4)
                                {
                                    {0, WindowButtons.Close},
                                    {1, WindowButtons.Maximize},
                                    {2, WindowButtons.Minimize},
                                    {3, WindowButtons.Help}
                                };

            // Position of the first button drawn
            var firstButtonLocation = new Point(Width - FirstButtonSpacerWidth, 8);

            // Number of buttons drawn in total
            var lastDrawedButtonPosition = firstButtonLocation.X - 20;

            // Object representation of the first button drawn
            MetroFormButton firstButton = null;

            // Only one button to draw.
            if (windowButtonList.Count == 1)
            {
                foreach (var button in windowButtonList)
                {
                    button.Value.Location = firstButtonLocation;
                    break;
                }
                return;
            }

            // Draw buttons in priority order
            foreach (var button in priorityOrder)
            {

                var buttonExists = windowButtonList.ContainsKey(button.Value);

                if (firstButton == null && buttonExists)
                {
                    firstButton = windowButtonList[button.Value];
                    firstButton.Location = firstButtonLocation;
                    continue;
                }

                if (firstButton == null || !buttonExists) continue;

                windowButtonList[button.Value].Location = new Point(lastDrawedButtonPosition, 8);
                lastDrawedButtonPosition = lastDrawedButtonPosition - 20;
            }

            Refresh();
        }

        private class MetroFormButton : Button, IMetroControl
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

            private bool isHovered = false;
            private bool isPressed = false;

            #endregion

            #region Constructor

            public MetroFormButton()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
            }

            #endregion

            #region Paint Methods

            protected override void OnPaint(PaintEventArgs e)
            {
                Color backColor, foreColor;

                if (Parent != null)
                    backColor = Parent.BackColor;
                else
                    backColor = MetroPaint.BackColor.Form(Theme);

                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Link.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Link.Press(Theme);
                }
                else if (!Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Link.Disabled(Theme);
                }
                else
                {
                    foreColor = MetroPaint.ForeColor.Link.Normal(Theme);
                }

                e.Graphics.Clear(backColor);
                Font buttonFont = new Font("Webdings", 9.25f);
                TextRenderer.DrawText(e.Graphics, Text, buttonFont, ClientRectangle, foreColor, backColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            #endregion

            #region Mouse Methods

            protected override void OnMouseEnter(EventArgs e)
            {
                isHovered = true;
                Invalidate();

                base.OnMouseEnter(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isPressed = true;
                    Invalidate();
                }

                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                isPressed = false;
                Invalidate();

                base.OnMouseUp(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                isHovered = false;
                Invalidate();

                base.OnMouseLeave(e);
            }

            #endregion
        }

        #endregion

        #region Windows XP
        private static bool IsWinVistaOrHigher()
        {
            var os = Environment.OSVersion;
            return (os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6);
        }

        public static void RemoveCloseButton(Form frm)
        {
            IntPtr hMenu = WinApi.GetSystemMenu(frm.Handle, false);
            if (hMenu == IntPtr.Zero)
            {
                return;
            }
            int n = WinApi.GetMenuItemCount(hMenu);
            if (n <= 0)
            {
                return;
            }
            WinApi.RemoveMenu(hMenu, (uint)(n - 1), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.RemoveMenu(hMenu, (uint)(n - 2), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.DrawMenuBar(frm.Handle);

        }

        [SecuritySafeCritical]
        private static bool IsAeroThemeEnabled()
        {
            if (Environment.OSVersion.Version.Major <= 5) return false;

            bool aeroEnabled;
            DwmApi.DwmIsCompositionEnabled(out aeroEnabled);
            return aeroEnabled;
        }

        private static bool IsDropShadowSupported()
        {
            return Environment.OSVersion.Version.Major > 5 && SystemInformation.IsDropShadowEnabled;
        }

        #endregion


    }
}