namespace MetroFramework.Demo
{
    partial class DemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroSplitContainer1 = new MetroFramework.Controls.MetroSplitContainer();
            this.metroPropertyGrid2 = new MetroFramework.Controls.MetroPropertyGrid();
            this.metroListView1 = new MetroFramework.Controls.MetroListView();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroScrollBar1 = new MetroFramework.Controls.MetroScrollBar();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            this.mlSelectedTheme = new MetroFramework.Controls.MetroLabel();
            this.metroLabel23 = new MetroFramework.Controls.MetroLabel();
            this.mlSelectedColor = new MetroFramework.Controls.MetroLabel();
            this.metroLabel21 = new MetroFramework.Controls.MetroLabel();
            this.metroTileSwitch = new MetroFramework.Controls.MetroTile();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroPropertyGrid1 = new MetroFramework.Controls.MetroPropertyGrid();
            this.metroGroupBox1 = new MetroFramework.Controls.MetroGroupBox();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanelNoScroll();
            this.metroMonthCalendar1 = new MetroFramework.Controls.MetroMonthCalendar();
            this.mlKnobValue = new MetroFramework.Controls.MetroLabel();
            this.metroKnobControl1 = new MetroFramework.Controls.MetroKnobControl();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.metroRendererManager1 = new MetroFramework.Components.MetroRendererManager(this.components);
            this.metroSplitContainer1.Panel1.SuspendLayout();
            this.metroSplitContainer1.Panel2.SuspendLayout();
            this.metroSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.metroGroupBox1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroSplitContainer1
            // 
            this.metroSplitContainer1.CustomBackground = false;
            this.metroSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSplitContainer1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroSplitContainer1.Location = new System.Drawing.Point(20, 60);
            this.metroSplitContainer1.Name = "metroSplitContainer1";
            // 
            // metroSplitContainer1.Panel1
            // 
            this.metroSplitContainer1.Panel1.Controls.Add(this.metroPropertyGrid2);
            this.metroSplitContainer1.Panel1.Controls.Add(this.metroPanel1);
            this.metroSplitContainer1.Panel1.Controls.Add(this.metroPropertyGrid1);
            // 
            // metroSplitContainer1.Panel2
            // 
            this.metroSplitContainer1.Panel2.Controls.Add(this.metroGroupBox1);
            this.metroSplitContainer1.Size = new System.Drawing.Size(871, 680);
            this.metroSplitContainer1.SplitterDistance = 313;
            this.metroSplitContainer1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroSplitContainer1.StyleManager = this.metroStyleManager1;
            this.metroSplitContainer1.TabIndex = 2;
            this.metroSplitContainer1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroPropertyGrid2
            // 
            this.metroPropertyGrid2.CategoryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroPropertyGrid2.CustomBackground = false;
            this.metroPropertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPropertyGrid2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroPropertyGrid2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.metroPropertyGrid2.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroPropertyGrid2.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroPropertyGrid2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.metroPropertyGrid2.Location = new System.Drawing.Point(0, 162);
            this.metroPropertyGrid2.Name = "metroPropertyGrid2";
            this.metroPropertyGrid2.SelectedObject = this.metroListView1;
            this.metroPropertyGrid2.Size = new System.Drawing.Size(313, 272);
            this.metroPropertyGrid2.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroPropertyGrid2.StyleManager = this.metroStyleManager1;
            this.metroPropertyGrid2.TabIndex = 8;
            this.metroPropertyGrid2.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroListView1
            // 
            this.metroListView1.AlternateRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroListView1.AlternateRowColorEnabled = true;
            this.metroListView1.AutoSizeLastColumn = true;
            this.metroListView1.EnableDragDrop = false;
            this.metroListView1.EnableHeaderGlow = false;
            this.metroListView1.EnableHeaderHotTrack = false;
            this.metroListView1.EnableHeaderRendering = true;
            this.metroListView1.EnableSelectionBorder = false;
            this.metroListView1.EnableSorting = true;
            this.metroListView1.EnableVistaCheckBoxes = true;
            this.metroListView1.FontSize = MetroFramework.MetroListViewSize.Medium;
            this.metroListView1.FontWeight = MetroFramework.MetroListViewWeight.Regular;
            this.metroListView1.ForceLeftAlign = false;
            this.metroListView1.FullRowSelect = true;
            this.metroListView1.ItemHeight = 0;
            this.metroListView1.LineAfter = -1;
            this.metroListView1.LineBefore = -1;
            this.metroListView1.Location = new System.Drawing.Point(0, 0);
            this.metroListView1.Name = "metroListView1";
            this.metroListView1.OwnerDraw = true;
            this.metroListView1.SelectEntireRowOnSubItem = true;
            this.metroListView1.Size = new System.Drawing.Size(510, 225);
            this.metroListView1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroListView1.StyleManager = this.metroStyleManager1;
            this.metroListView1.TabIndex = 34;
            this.metroListView1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroListView1.UseCompatibleStateImageBehavior = false;
            this.metroListView1.View = System.Windows.Forms.View.Details;
            this.metroListView1.VScrollbar = this.metroScrollBar1;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // metroScrollBar1
            // 
            this.metroScrollBar1.BorderWidth = 1;
            this.metroScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.metroScrollBar1.LargeChange = 10;
            this.metroScrollBar1.Location = new System.Drawing.Point(492, 0);
            this.metroScrollBar1.Maximum = 100;
            this.metroScrollBar1.Minimum = 0;
            this.metroScrollBar1.MouseWheelBarPartitions = 10;
            this.metroScrollBar1.Name = "metroScrollBar1";
            this.metroScrollBar1.Orientation = MetroFramework.Controls.MetroScrollOrientation.Vertical;
            this.metroScrollBar1.ScrollbarSize = 18;
            this.metroScrollBar1.Size = new System.Drawing.Size(18, 225);
            this.metroScrollBar1.TabIndex = 35;
            this.metroScrollBar1.Text = "metroScrollBar1";
            this.metroScrollBar1.UseSelectable = true;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.metroTile2);
            this.metroPanel1.Controls.Add(this.mlSelectedTheme);
            this.metroPanel1.Controls.Add(this.metroLabel23);
            this.metroPanel1.Controls.Add(this.mlSelectedColor);
            this.metroPanel1.Controls.Add(this.metroLabel21);
            this.metroPanel1.Controls.Add(this.metroTileSwitch);
            this.metroPanel1.Controls.Add(this.metroTile1);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroPanel1.HorizontalScrollbar = false;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(313, 162);
            this.metroPanel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroPanel1.StyleManager = this.metroStyleManager1;
            this.metroPanel1.TabIndex = 7;
            this.metroPanel1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroPanel1.VerticalScrollbar = false;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroTile2
            // 
            this.metroTile2.ActiveControl = null;
            this.metroTile2.Location = new System.Drawing.Point(207, 20);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(92, 80);
            this.metroTile2.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTile2.StyleManager = this.metroStyleManager1;
            this.metroTile2.TabIndex = 24;
            this.metroTile2.Text = "Open Form";
            this.metroTile2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTile2.TileCount = 0;
            this.metroTile2.Click += new System.EventHandler(this.metroTile2_Click);
            // 
            // mlSelectedTheme
            // 
            this.mlSelectedTheme.AutoSize = true;
            this.mlSelectedTheme.CustomBackground = false;
            this.mlSelectedTheme.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.mlSelectedTheme.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.mlSelectedTheme.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.mlSelectedTheme.Location = new System.Drawing.Point(130, 110);
            this.mlSelectedTheme.Name = "mlSelectedTheme";
            this.mlSelectedTheme.Size = new System.Drawing.Size(115, 19);
            this.mlSelectedTheme.Style = MetroFramework.MetroColorStyle.Blue;
            this.mlSelectedTheme.StyleManager = this.metroStyleManager1;
            this.mlSelectedTheme.TabIndex = 23;
            this.mlSelectedTheme.Text = "Selected Theme";
            this.mlSelectedTheme.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mlSelectedTheme.UseStyleColors = false;
            // 
            // metroLabel23
            // 
            this.metroLabel23.AutoSize = true;
            this.metroLabel23.CustomBackground = false;
            this.metroLabel23.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel23.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel23.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel23.Location = new System.Drawing.Point(15, 110);
            this.metroLabel23.Name = "metroLabel23";
            this.metroLabel23.Size = new System.Drawing.Size(109, 19);
            this.metroLabel23.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel23.StyleManager = this.metroStyleManager1;
            this.metroLabel23.TabIndex = 22;
            this.metroLabel23.Text = "Selected Theme: ";
            this.metroLabel23.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel23.UseStyleColors = false;
            // 
            // mlSelectedColor
            // 
            this.mlSelectedColor.AutoSize = true;
            this.mlSelectedColor.CustomBackground = false;
            this.mlSelectedColor.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.mlSelectedColor.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.mlSelectedColor.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.mlSelectedColor.Location = new System.Drawing.Point(130, 129);
            this.mlSelectedColor.Name = "mlSelectedColor";
            this.mlSelectedColor.Size = new System.Drawing.Size(107, 19);
            this.mlSelectedColor.Style = MetroFramework.MetroColorStyle.Blue;
            this.mlSelectedColor.StyleManager = this.metroStyleManager1;
            this.mlSelectedColor.TabIndex = 21;
            this.mlSelectedColor.Text = "Selected Color";
            this.mlSelectedColor.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mlSelectedColor.UseStyleColors = false;
            // 
            // metroLabel21
            // 
            this.metroLabel21.AutoSize = true;
            this.metroLabel21.CustomBackground = false;
            this.metroLabel21.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel21.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel21.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel21.Location = new System.Drawing.Point(15, 129);
            this.metroLabel21.Name = "metroLabel21";
            this.metroLabel21.Size = new System.Drawing.Size(96, 19);
            this.metroLabel21.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel21.StyleManager = this.metroStyleManager1;
            this.metroLabel21.TabIndex = 20;
            this.metroLabel21.Text = "Selected Style: ";
            this.metroLabel21.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel21.UseStyleColors = false;
            // 
            // metroTileSwitch
            // 
            this.metroTileSwitch.ActiveControl = null;
            this.metroTileSwitch.Location = new System.Drawing.Point(109, 20);
            this.metroTileSwitch.Name = "metroTileSwitch";
            this.metroTileSwitch.Size = new System.Drawing.Size(92, 80);
            this.metroTileSwitch.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTileSwitch.StyleManager = this.metroStyleManager1;
            this.metroTileSwitch.TabIndex = 6;
            this.metroTileSwitch.Text = "Switch Style";
            this.metroTileSwitch.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTileSwitch.TileCount = 0;
            this.metroTileSwitch.Click += new System.EventHandler(this.metroTileSwitch_Click);
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(11, 20);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(92, 80);
            this.metroTile1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTile1.StyleManager = this.metroStyleManager1;
            this.metroTile1.TabIndex = 5;
            this.metroTile1.Text = "Switch Theme";
            this.metroTile1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTile1.TileCount = 0;
            this.metroTile1.Click += new System.EventHandler(this.metroTile1_Click);
            // 
            // metroPropertyGrid1
            // 
            this.metroPropertyGrid1.CategoryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroPropertyGrid1.CustomBackground = false;
            this.metroPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroPropertyGrid1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroPropertyGrid1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.metroPropertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroPropertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroPropertyGrid1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.metroPropertyGrid1.Location = new System.Drawing.Point(0, 434);
            this.metroPropertyGrid1.Name = "metroPropertyGrid1";
            this.metroPropertyGrid1.SelectedObject = this.metroGroupBox1;
            this.metroPropertyGrid1.Size = new System.Drawing.Size(313, 246);
            this.metroPropertyGrid1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroPropertyGrid1.StyleManager = this.metroStyleManager1;
            this.metroPropertyGrid1.TabIndex = 0;
            this.metroPropertyGrid1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroGroupBox1
            // 
            this.metroGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGroupBox1.BorderStyle = MetroFramework.Controls.MetroGroupBox.BorderMode.Header;
            this.metroGroupBox1.Controls.Add(this.metroPanel2);
            this.metroGroupBox1.Controls.Add(this.metroMonthCalendar1);
            this.metroGroupBox1.Controls.Add(this.mlKnobValue);
            this.metroGroupBox1.Controls.Add(this.metroKnobControl1);
            this.metroGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroGroupBox1.DrawBottomLine = false;
            this.metroGroupBox1.DrawShadows = false;
            this.metroGroupBox1.Font = new System.Drawing.Font("Segoe UI Light", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGroupBox1.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroGroupBox1.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroGroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.metroGroupBox1.Name = "metroGroupBox1";
            this.metroGroupBox1.PaintDefault = false;
            this.metroGroupBox1.Size = new System.Drawing.Size(554, 680);
            this.metroGroupBox1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroGroupBox1.StyleManager = this.metroStyleManager1;
            this.metroGroupBox1.TabIndex = 0;
            this.metroGroupBox1.TabStop = false;
            this.metroGroupBox1.Text = "metroGroupBox1";
            this.metroGroupBox1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroGroupBox1.UseStyleColors = false;
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroPanel2.BorderWidth = 1;
            this.metroPanel2.Controls.Add(this.metroScrollBar1);
            this.metroPanel2.Controls.Add(this.metroListView1);
            this.metroPanel2.Location = new System.Drawing.Point(35, 394);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.PaintBackColor = true;
            this.metroPanel2.Size = new System.Drawing.Size(510, 225);
            this.metroPanel2.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroPanel2.StyleManager = this.metroStyleManager1;
            this.metroPanel2.TabIndex = 37;
            this.metroPanel2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroPanel2.UseCompatibleStateImageBehavior = false;
            // 
            // metroMonthCalendar1
            // 
            this.metroMonthCalendar1.Location = new System.Drawing.Point(318, 110);
            this.metroMonthCalendar1.Name = "metroMonthCalendar1";
            this.metroMonthCalendar1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroMonthCalendar1.StyleManager = this.metroStyleManager1;
            this.metroMonthCalendar1.TabIndex = 33;
            this.metroMonthCalendar1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // mlKnobValue
            // 
            this.mlKnobValue.AutoSize = true;
            this.mlKnobValue.CustomBackground = false;
            this.mlKnobValue.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.mlKnobValue.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.mlKnobValue.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.mlKnobValue.Location = new System.Drawing.Point(164, 274);
            this.mlKnobValue.Name = "mlKnobValue";
            this.mlKnobValue.Size = new System.Drawing.Size(16, 19);
            this.mlKnobValue.Style = MetroFramework.MetroColorStyle.Blue;
            this.mlKnobValue.StyleManager = this.metroStyleManager1;
            this.mlKnobValue.TabIndex = 25;
            this.mlKnobValue.Text = "0";
            this.mlKnobValue.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mlKnobValue.UseStyleColors = false;
            // 
            // metroKnobControl1
            // 
            this.metroKnobControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.metroKnobControl1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.metroKnobControl1.LargeChange = 20;
            this.metroKnobControl1.Location = new System.Drawing.Point(71, 75);
            this.metroKnobControl1.Maximum = 100;
            this.metroKnobControl1.Minimum = 0;
            this.metroKnobControl1.Name = "metroKnobControl1";
            this.metroKnobControl1.ShowLargeScale = true;
            this.metroKnobControl1.ShowSmallScale = false;
            this.metroKnobControl1.Size = new System.Drawing.Size(265, 196);
            this.metroKnobControl1.SizeLargeScaleMarker = 6;
            this.metroKnobControl1.SizeSmallScaleMarker = 3;
            this.metroKnobControl1.SmallChange = 5;
            this.metroKnobControl1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroKnobControl1.StyleManager = this.metroStyleManager1;
            this.metroKnobControl1.TabIndex = 0;
            this.metroKnobControl1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroKnobControl1.UseStyleColors = false;
            this.metroKnobControl1.Value = 0;
            this.metroKnobControl1.ValueChanged += new MetroFramework.Controls.ValueChangedEventHandler(this.metroKnobControl1_ValueChanged);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroRendererManager1
            // 
            this.metroRendererManager1.Renderers = MetroFramework.Components.Renderer.MetroRenderer;
            this.metroRendererManager1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroRendererManager1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 760);
            this.Controls.Add(this.metroSplitContainer1);
            this.Name = "DemoForm";
            this.StyleManager = this.metroStyleManager1;
            this.Text = "DemoForm";
            this.metroSplitContainer1.Panel1.ResumeLayout(false);
            this.metroSplitContainer1.Panel2.ResumeLayout(false);
            this.metroSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroGroupBox1.ResumeLayout(false);
            this.metroGroupBox1.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MetroGroupBox metroGroupBox1;
        private Controls.MetroSplitContainer metroSplitContainer1;
        private Controls.MetroPropertyGrid metroPropertyGrid1;
        private Controls.MetroPanel metroPanel1;
        private Controls.MetroTile metroTileSwitch;
        private Controls.MetroTile metroTile1;
        private Controls.MetroLabel mlSelectedTheme;
        private Controls.MetroLabel metroLabel23;
        private Controls.MetroLabel mlSelectedColor;
        private Controls.MetroLabel metroLabel21;
        private Controls.MetroPropertyGrid metroPropertyGrid2;
        private Components.MetroToolTip metroToolTip1;
        private Components.MetroStyleManager metroStyleManager1;
        private Components.MetroRendererManager metroRendererManager1;
        private Controls.MetroTile metroTile2;
        private Controls.MetroKnobControl metroKnobControl1;
        private Controls.MetroLabel mlKnobValue;
        private Controls.MetroMonthCalendar metroMonthCalendar1;
        private Controls.MetroListView metroListView1;
        private Controls.MetroScrollBar metroScrollBar1;
        private Controls.MetroPanelNoScroll metroPanel2;
    }
}