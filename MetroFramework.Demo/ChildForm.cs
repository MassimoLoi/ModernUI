using MetroFramework.Components;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MetroFramework.Demo
{
    public partial class ChildForm : MetroForm
    {
        MetroStyleManager metroStyleManager;
        public ChildForm(MetroStyleManager msm)
        {
            InitializeComponent();

            metroStyleManager = new MetroStyleManager(this.components);
            metroStyleManager.Theme = msm.Theme;
            metroStyleManager.Style = msm.Style;

            this.StyleManager = metroStyleManager;
            this.StyleManager.Update();
        }
    }
}
