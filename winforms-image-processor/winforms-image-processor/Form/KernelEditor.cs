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
    public partial class KernelEditor : Form
    {
        public KernelEditor()
        {
            InitializeComponent();

        }



        public void SetUpKernelEditor()
        {
            tableLayoutPanel1.ColumnCount = Kernel.customKernel.kernel.GetLength(0);
            tableLayoutPanel1.RowCount = Kernel.customKernel.kernel.GetLength(1);

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                {
                    NumericUpDown numericUpDown = new NumericUpDown();
                    tableLayoutPanel1.Controls.Add(numericUpDown, column, row);
                    numericUpDown.Dock = DockStyle.Fill;
                    numericUpDown.Enabled = false;
                }
            }
        }
    }
}
