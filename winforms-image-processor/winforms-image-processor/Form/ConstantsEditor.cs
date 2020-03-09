using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winforms_image_processor
{
    public partial class ConstantsEditor : Form
    {
        public ConstantsEditor()
        {
            InitializeComponent();

            numericUpDown1.Value = (decimal)Constants.filterGammaValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Constants.filterGammaValue = (double)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
