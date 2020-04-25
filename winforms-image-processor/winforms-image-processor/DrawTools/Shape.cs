using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    abstract class Shape
    {
        public DrawingShape shapeType;
        public Color shapeColor;

        public Shape(Color color)
        { shapeColor = color; }

        public abstract override string ToString();

        abstract public int AddPoint(Point point);

        abstract public List<Point> GetPixels();

        abstract public void MovePoints(Point displacement);
    }
}
