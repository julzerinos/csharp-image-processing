using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace winforms_image_processor
{
    public enum DrawingShape { EMPTY, LINE, CIRCLE };

    public partial class DrawForm : Form
    {

        public DrawForm()
        {
            InitializeComponent();

            listBox1.DataSource = shapes;
        }

        BindingList<Shape> shapes = new BindingList<Shape>();

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapes.Clear();

            pictureBox1.Image = NewBitmap();

            toolsToolStripMenuItem.Enabled = true;
        }

        Bitmap NewBitmap()
        {
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Green);

            return bmp;
        }

        private void DrawForm_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            RefreshShapes();
        }

        void UpdateLabel()
        {
            switch (currentDrawingShape)
            {
                case DrawingShape.LINE:
                    label1.Text = "Currently drawing: line";
                    break;
                case DrawingShape.CIRCLE:
                    label1.Text = "Currently drawing: circle";
                    break;
                case DrawingShape.EMPTY:
                default:
                    label1.Text = "";
                    break;
            }
        }

        void RefreshShapes()
        {
            Bitmap bmp = NewBitmap();
            foreach (var shape in shapes)
            {
                if (!antialiasingToolStripMenuItem.Checked || shape.shapeType == DrawingShape.CIRCLE)
                    foreach (var point in shape.GetPixels())
                    {
                        if (point.X >= pictureBox1.Width || point.Y >= pictureBox1.Height || point.X <= 0 || point.Y <= 0)
                            continue;

                        bmp.SetPixelFast(point.X, point.Y, shape.shapeColor);
                    }
                else
                {
                    bmp = ((MidPointLine)shape).SetPixelsAA(bmp);
                }
            }
            pictureBox1.Image = bmp;
        }


        Shape currentShape = null;
        DrawingShape currentDrawingShape = DrawingShape.EMPTY;
        bool drawing;
        int index;

        void drawMode(
            bool status,
            Shape shape = null,
            int modify_index = -1
            )
        {
            if (!status && index == -1)
            {
                shapes.Add(currentShape);
                RefreshShapes();
            }
            else if (!status)
            {
                shapes[index] = currentShape;
                RefreshShapes();
            }

            splitContainer2.Panel1.Enabled = !status;

            index = modify_index;
            drawing = status;
            currentDrawingShape = shape == null ? DrawingShape.EMPTY : shape.shapeType;
            currentShape = shape;
            UpdateLabel();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (1 == currentShape.AddPoint(e.Location))
                    drawMode(false);
            }
        }

        private void midpointCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMode(true, new MidPointCircle(colorDialog1.Color));
        }

        private void midpointLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMode(true, new MidPointLine(colorDialog1.Color, (int)numericUpDown1.Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.BackColor = colorDialog1.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (shapes.Count == 0)
                return;

            if (shapes[listBox1.SelectedIndex].shapeType == DrawingShape.CIRCLE)
                drawMode(true, new MidPointCircle(colorDialog1.Color), listBox1.SelectedIndex);
            else
                drawMode(true, new MidPointLine(colorDialog1.Color, (int)numericUpDown1.Value), listBox1.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (shapes.Count == 0)
                return;

            shapes.RemoveAt(listBox1.SelectedIndex);

            RefreshShapes();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = "c:\\Downloads";
            saveFileDialog1.Filter = "Vector shapes (*.cg2020)|*.cg2020";
            saveFileDialog1.DefaultExt = "dat";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Title = "Save the filtered image";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                ShapeSerializer.Save(saveFileDialog1.FileName, shapes);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);


            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\Downloads";
                openFileDialog.Filter = "Vector shapes (*.cg2020)|*.cg2020";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    shapes = ShapeSerializer.Load<Shape>(openFileDialog.FileName);
                }
                else
                    return;
            }

            RefreshShapes();
        }

        private void antialiasingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            RefreshShapes();
        }
    }
}
