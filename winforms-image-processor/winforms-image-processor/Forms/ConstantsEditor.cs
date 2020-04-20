using System;
using System.Windows.Forms;

namespace winforms_image_processor
{
    public partial class ConstantsEditor : Form
    {
        public ConstantsEditor()
        {
            InitializeComponent();

            numericUpDown1.Value = (decimal)Constants.filterGammaValue;
            numericUpDown2.Value = (decimal)Constants.filterBrightnessValue;
            numericUpDown3.Value = (decimal)Constants.filterContrastValue;
            numericUpDown4.Value = Constants.filterOctreeColorLimit;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Constants.filterGammaValue = (double)numericUpDown1.Value;
            Constants.filterBrightnessValue = (double)numericUpDown2.Value;
            Constants.filterContrastValue = (double)numericUpDown3.Value;
            Constants.filterOctreeColorLimit = (int)numericUpDown4.Value;

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
