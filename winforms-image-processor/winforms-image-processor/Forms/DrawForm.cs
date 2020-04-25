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
    public enum DrawingShape { EMPTY, LINE, CIRCLE, POLY };

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

        Color backColor = Color.Green;
        Bitmap NewBitmap()
        {
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(backColor);

            return bmp;
        }

        private void DrawForm_ResizeEnd(object sender, EventArgs e)
        {
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0)
                return;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            RefreshShapes();
        }

        void UpdateLabel()
        {
            StringBuilder sb;
            if (currentShape == null)
                sb = new StringBuilder();
            else
            {
                sb = new StringBuilder("Currently drawing: ");
                sb.Append(currentShape.ToString());

            }

            label1.Text = sb.ToString();
        }

        void RefreshShapes()
        {
            Bitmap bmp = NewBitmap();
            foreach (var shape in shapes)
            {
                if (!antialiasingToolStripMenuItem.Checked || shape.shapeType == DrawingShape.CIRCLE)
                    DrawShape(bmp, shape);
                else
                {
                    if (shape.shapeType == DrawingShape.LINE)
                        bmp = ((MidPointLine)shape).SetPixelsAA(bmp);
                    else
                        bmp = ((Polygon)shape).SetPixelsAA(bmp);
                }
            }
            pictureBox1.Image = bmp;
        }

        Bitmap DrawShape(Bitmap bmp, Shape shape)
        {
            foreach (var point in shape.GetPixels())
            {
                if (point.X >= pictureBox1.Width || point.Y >= pictureBox1.Height || point.X <= 0 || point.Y <= 0)
                    continue;

                bmp.SetPixelFast(point.X, point.Y, shape.shapeColor);
            }

            return bmp;
        }

        Shape currentShape = null;
        DrawingShape currentDrawingShape = DrawingShape.EMPTY;
        bool drawing = false;
        bool moving = false;
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
                if (currentShape.shapeType == DrawingShape.POLY)
                {
                    if (1 == ((Polygon)currentShape).AddPoint(e.Location, out MidPointLine line))
                        drawMode(false);

                    if (line != null)
                        pictureBox1.Image = DrawShape((Bitmap)pictureBox1.Image, line);

                    return;
                }

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

        private void polygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMode(true, new Polygon(colorDialog1.Color, (int)numericUpDown1.Value));
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

            switch (shapes[listBox1.SelectedIndex].shapeType)
            {
                case DrawingShape.CIRCLE:
                    drawMode(true, new MidPointCircle(colorDialog1.Color), listBox1.SelectedIndex);
                    break;
                case DrawingShape.LINE:
                    drawMode(true, new MidPointLine(colorDialog1.Color, (int)numericUpDown1.Value), listBox1.SelectedIndex);
                    break;
                case DrawingShape.POLY:
                    drawMode(true, new Polygon(colorDialog1.Color, (int)numericUpDown1.Value), listBox1.SelectedIndex);
                    break;

                default:
                case DrawingShape.EMPTY:
                    break;
            }
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
                    shapes.Clear();
                    foreach (var shape in ShapeSerializer.Load<Shape>(openFileDialog.FileName))
                        shapes.Add(shape);
                }
                else
                    return;
            }

            RefreshShapes();
        }

        private void antialiasingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;
            RefreshShapes();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                backColor = colorDialog2.Color;
                RefreshShapes();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            moving = true;
        }

        System.Drawing.Point movestart;
        System.Drawing.Point moveend;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moving)
                return;

            movestart = e.Location;
            splitContainer2.Panel1.Enabled = false;
            checkBox1.Checked = false;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moving)
                return;

            moveend = e.Location;

            moving = false;
            checkBox1.Checked = false;

            splitContainer2.Panel1.Enabled = true;

            ((Shape)listBox1.SelectedItem).MovePoints(moveend - (System.Drawing.Size)movestart);

            RefreshShapes();
        }
    }
}
