using System;
using System.Collections.Generic;
using System.Drawing;
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

        private ImageProcessorForm imageProcessorForm;
        private string kernel;
        public KernelEditor(Form callingForm, string filter)
        {
            imageProcessorForm = callingForm as ImageProcessorForm;
            kernel = filter;
            InitializeComponent();
            SetUpKernelEditor();
        }

        public static List<string> filterTypes = new List<string>()
        {
            "",
            "Sharpen",
            "Gaussian Blur",
            "Box Blur",
            "Emboss",
            "Outline"
        };

        public void SetUpKernelEditor()
        {
            Console.WriteLine(kernel);
            CustomKernel customKernel = Kernel.customKernels[kernel];

            CBFilterType.DataSource = filterTypes;

            NUDDivisor.Minimum = decimal.MinValue;
            NUDDivisor.Maximum = decimal.MaxValue;

            NUDDivisor.Value = customKernel.divisor;
            NUDKernelColumn.Value = customKernel.kernel.GetLength(0);
            NUDKernelRow.Value = customKernel.kernel.GetLength(1);

            NUDOffset.Value = customKernel.offset;
            NUDAnchorCol.Value = customKernel.anchor.X;
            NUDAnchorRow.Value = customKernel.anchor.Y;

            SetUpTableLayout(customKernel.kernel.GetLength(0), customKernel.kernel.GetLength(1), customKernel.kernel);
        }

        public void SetUpTableLayout(int columns, int rows, double[,] kernel)
        {
            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                    if (tableLayoutPanel1.GetControlFromPosition(column, row) != null)
                        tableLayoutPanel1.GetControlFromPosition(column, row).Dispose();

            tableLayoutPanel1.ColumnCount = columns;
            tableLayoutPanel1.RowCount = rows;

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
                {
                    NumericUpDown numericUpDown = new NumericUpDown();
                    tableLayoutPanel1.Controls.Add(numericUpDown, column, row);
                    numericUpDown.Minimum = decimal.MinValue;
                    numericUpDown.Maximum = decimal.MaxValue;
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
                    numericUpDown.Minimum = decimal.MinValue;
                    numericUpDown.Maximum = decimal.MaxValue;
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

            int value = (int)NUDDivisor.Value;

            if (value == 0)
            {
                foreach (double i in kernel)
                    value += (int)i;
                NUDDivisor.Value = value;
            }
            if (value == 0)
                NUDDivisor.Value = 1;

            Kernel.customKernels[this.kernel] = new CustomKernel(
                kernel,
                value,
                (int)NUDOffset.Value,
                new Point((int)NUDAnchorCol.Value, (int)NUDAnchorRow.Value)
                );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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

        private void button1_Click(object sender, EventArgs e)
        {
            SetKernelFromEditor();

            DialogResult = DialogResult.OK;

            Close();
        }

        private void Redraw_Click(object sender, EventArgs e)
        {
            switch ((string)CBFilterType.SelectedItem)
            {
                case "":
                    SetUpTableLayout((int)NUDKernelColumn.Value, (int)NUDKernelRow.Value);
                    break;
                case "Sharpen":
                    SetUpTableLayout(3, 3, Kernel.SharpenKernel.kernel);
                    NUDDivisor.Value = Kernel.SharpenKernel.divisor;
                    break;
                case "Gaussian Blur":
                    SetUpTableLayout(3, 3, Kernel.GaussianBlurKernel.kernel);
                    NUDDivisor.Value = Kernel.GaussianBlurKernel.divisor;
                    break;
                case "Box Blur":
                    SetUpTableLayout(3, 3, Kernel.BoxBlurKernel.kernel);
                    NUDDivisor.Value = Kernel.BoxBlurKernel.divisor;
                    break;
                case "Emboss":
                    SetUpTableLayout(3, 3, Kernel.EmbossKernel.kernel);
                    NUDDivisor.Value = Kernel.EmbossKernel.divisor;
                    break;
                case "Outline":
                    SetUpTableLayout(3, 3, Kernel.OutlineKernel.kernel);
                    NUDDivisor.Value = Kernel.OutlineKernel.divisor;
                    break;
            }

            CBFilterType.SelectedIndex = 0;
        }
    }
}
