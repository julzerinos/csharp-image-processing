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
            SetUpKernelEditor();
        }



        public void SetUpKernelEditor()
        {

            CustomKernel customKernel = Kernel.customKernel;

            numericUpDownDivisor.Value = customKernel.divisor;
            numericUpDown1.Value = customKernel.kernel.GetLength(0);
            numericUpDown2.Value = customKernel.kernel.GetLength(1);

            SetUpTableLayout(customKernel.kernel.GetLength(0), customKernel.kernel.GetLength(1), customKernel.kernel);
        }

        public void SetUpTableLayout(int columns, int rows, double[,] kernel)
        {
            tableLayoutPanel1.ColumnCount = columns;
            tableLayoutPanel1.RowCount = rows;

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                {
                    NumericUpDown numericUpDown = new NumericUpDown();
                    tableLayoutPanel1.Controls.Add(numericUpDown, column, row);
                    numericUpDown.Value = (decimal)kernel[column, row];
                    numericUpDown.Dock = DockStyle.Fill;
                }
            }
        }

        public void SetUpTableLayout(int columns, int rows)
        {
            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                    tableLayoutPanel1.GetControlFromPosition(column, row).Dispose();

            tableLayoutPanel1.ColumnCount = columns;
            tableLayoutPanel1.RowCount = rows;

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                {
                    NumericUpDown numericUpDown = new NumericUpDown();
                    tableLayoutPanel1.Controls.Add(numericUpDown, column, row);
                    numericUpDown.Value = 0;
                    numericUpDown.Dock = DockStyle.Fill;
                }
            }
        }

        public void SetKernelFromEditor()
        {
            double[,] kernel = new double[tableLayoutPanel1.ColumnCount, tableLayoutPanel1.RowCount];
            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                {
                    NumericUpDown numUpDown = (NumericUpDown)tableLayoutPanel1.GetControlFromPosition(column, row);
                    kernel[column, row] = (double)numUpDown.Value;
                }
            }

            int value = (int)numericUpDownDivisor.Value;

            if (value == 0)
            {
                foreach (double i in kernel)
                    value += (int)i;

                numericUpDownDivisor.Value = value;
            }

            Kernel.customKernel = new CustomKernel(kernel, value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(label3, "Leave 0 for automatic calculation.");
        }

        private void RowCol_OnValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numUpDown = (NumericUpDown)sender;
            if (numUpDown.Value % 2 == 0)
            {
                numUpDown.Value = numUpDown.Value - 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetUpTableLayout((int)numericUpDown1.Value, (int)numericUpDown2.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetKernelFromEditor();
            Close();
        }
    }
}
