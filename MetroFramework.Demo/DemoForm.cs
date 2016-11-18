using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MetroFramework.Forms;
using System.Data;


namespace MetroFramework.Demo
{
    public partial class DemoForm : MetroForm
    {
        public DemoForm()
        {
            InitializeComponent();
        }

        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager1.Style = (MetroColorStyle)next;
            metroRendererManager1.Style = metroStyleManager1.Style;

            mlSelectedColor.Text = metroStyleManager1.Style.ToString();
            mlSelectedTheme.Text = metroStyleManager1.Theme.ToString();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
            metroRendererManager1.Theme = metroStyleManager1.Theme;

            mlSelectedTheme.Text = metroStyleManager1.Theme.ToString();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            ChildForm mf = new ChildForm(metroStyleManager1);
            mf.Show();
        }

        private void metroKnobControl1_ValueChanged(object Sender)
        {
            mlKnobValue.Text = metroKnobControl1.Value.ToString();
        }

    }
}
