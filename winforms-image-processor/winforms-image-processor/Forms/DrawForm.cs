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
    public partial class DrawForm : Form
    {
        public DrawForm()
        {
            InitializeComponent();
        }

        List<Shape> shapes = new List<Shape>();

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            using (Graphics grp = Graphics.FromImage(bmp))
            {
                grp.FillRectangle(new SolidBrush(Color.White), 0, 0, pictureBox1.Width, pictureBox1.Height);
            }

            pictureBox1.Image = bmp;

            toolsToolStripMenuItem.Enabled = true;
        }

        void UpdateLabel(int obj)
        {
            switch (obj)
            {
                case 1:
                    label1.Text = "Currently drawing: line";
                    break;
                case 2:
                    label1.Text = "Currently drawing: circle";
                    break;
                case 0:
                default:
                    label1.Text = "";
                    break;
            }
        }

        void RefreshShapes()
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            foreach (var shape in shapes)
            {
                foreach (var point in shape.GetPixels())
                {
                    Console.WriteLine(point.ToString());
                    bmp.SetPixelFast(point.X, point.Y, new int[] { 100, 100, 100, 255 });
                }
            }
            pictureBox1.Image = bmp;
        }

        Shape currentShape = null;
        bool drawingLine = false;

        void drawLineMode(bool status)
        {
            if (!status)
            {
                shapes.Add(currentShape);
                Console.WriteLine("add");
            }

            drawingLine = status;
            currentShape = status ? new MidPointLine() : null;
            UpdateLabel(status ? 1 : 0);
        }

        private void midpointLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawLineMode(true);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawingLine)
            {
                if (1 == currentShape.AddPoint(e.Location))
                    drawLineMode(false);
            }

            RefreshShapes();
        }



        //private void FltPictureBox_MouseClick(object sender, MouseEventArgs e)
        //{
        //    Console.WriteLine(e.Location);
        //    Bitmap bmp = (Bitmap)FltPictureBox.Image;
        //    if (e.X < bmp.Width && e.Y < bmp.Height)
        //        bmp.SetPixelFast(e.X, e.Y, new int[] { 0, 0, 0, 0 });
        //    FltPictureBox.Image = bmp;
        //}
    }
}
