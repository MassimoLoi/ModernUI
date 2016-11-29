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

            DataTable _table = new DataTable();
            _table.ReadXml(Application.StartupPath + @"\Data\Books.xml");
            PopulateFullListView(_table, 3);
        }


        private void PopulateFullListView(DataTable _table, int _colCount)
        {
            // Clear the ListView control
            metroListView1.Items.Clear();
            int ColCount = _table.Columns.Count;

            if (_colCount != 0)
                ColCount = _colCount;

            //Add columns
            for (int k = 0; k < ColCount; k++)
            {
                metroListView1.Columns.Add(_table.Columns[k].ColumnName);
            }
            // Display items in the ListView control
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                DataRow drow = _table.Rows[i];

                // Only row that have not been deleted
                if (drow.RowState != DataRowState.Deleted)
                {
                    // Define the list items
                    ListViewItem lvi = new ListViewItem(drow[0].ToString());
                    for (int j = 1; j < ColCount; j++)
                    {
                        lvi.SubItems.Add(drow[j].ToString());
                    }
                    // Add the list items to the ListView
                    metroListView1.Items.Add(lvi);
                }
            }
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

