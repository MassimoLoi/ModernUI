using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MetroFramework.Forms;
using System.Data;

namespace MetroFramework.Demo
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();

            DataTable _table = new DataTable();
            _table.ReadXml(Application.StartupPath + @"\Data\Books.xml");
            metroGrid1.DataSource = _table;

            metroGrid1.Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
            metroGrid1.AllowUserToAddRows = false;

            mlSelectedColor.Text = metroStyleManager.Style.ToString();
            mlSelectedTheme.Text = metroStyleManager.Theme.ToString();
            mlHotTrack.Text = this.metroTabControl1.HotTrack.ToString();
            //metroToggle4.
        }

        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;
            rendererManager1.Style = metroStyleManager.Style;

            mlSelectedColor.Text = metroStyleManager.Style.ToString();
            mlSelectedTheme.Text = metroStyleManager.Theme.ToString();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
            rendererManager1.Theme = metroStyleManager.Theme;

            mlSelectedTheme.Text = metroStyleManager.Theme.ToString();
        }

       private int steps = 0;
        private void metroButton4_Click(object sender, EventArgs e)
        {
            Color target = Color.Purple;

            if (steps == 1)
                target = Color.RoyalBlue;
            if (steps == 2)
            {
                target = Color.YellowGreen;
                steps = -1;
            }

            steps++;

            MetroFramework.Animation.ColorBlendAnimation myColorAnim = new MetroFramework.Animation.ColorBlendAnimation();
            myColorAnim.Start(panel1, "BackColor", target, 1);
            myColorAnim.AnimationCompleted += new EventHandler(myColorAnim_AnimationCompleted);

            metroButton4.Enabled = false;
        }

        void myColorAnim_AnimationCompleted(object sender, EventArgs e)
        {
            metroButton4.Enabled = true;
        }


        private void metroButton6_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(metroButton6, new Point(0, metroButton6.Height));
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            MetroTaskWindow.ShowTaskWindow(this, "SubControl in TaskWindow", new TaskWindowControl(), 10); 
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this, "This is a sample MetroMessagebox `Yes` and `No` button", "MetroMessagebox", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this, "This is a sample MetroMessagebox `Retry` and `Cancel` button.  With warning style.", "MetroMessagebox", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void metroKnobControl1_ValueChanged(object Sender)
        {
            lblKnobValue.Text = metroKnobControl1.Value.ToString();
        }
    }
}