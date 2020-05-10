using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{

    public struct ColorPoint
    {
        public Color Color;
        public Point Point;

        public ColorPoint(Color col, Point p)
        {
            Color = col;
            Point = p;
        }
    }

    [Serializable]
    abstract class Shape
    {
        public DrawingShape shapeType;
        public Color shapeColor;
        public bool supportsAA = false;

        public Shape(Color color)
        { shapeColor = color; }

        public abstract override string ToString();

        abstract public int AddPoint(Point point);

        abstract public List<ColorPoint> SetPixelsAA(Bitmap bmp);

        abstract public List<ColorPoint> GetPixels(params object[] param);

        abstract public void MovePoints(Point displacement);

        abstract public string howToDraw();
    }
}
