using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class Polygon : Shape
    {
        List<Point> points = null;
        int thickness;

        public Polygon(Color color, int thicc) : base(color)
        {
            thickness = thicc - 1;
            shapeType = DrawingShape.POLY;
        }

        public override int AddPoint(Point point)
        {
            if (points == null)
                points = new List<Point> { point };
            else
            {
                double dist = (point.X - points[0].X) * (point.X - points[0].X) + (point.Y - points[0].Y) * (point.Y - points[0].Y);
                if (dist < 100)
                {
                    points.Add(points[0]);
                    return 1;
                }
                else
                    points.Add(point);
            }
            return 0;
        }

        public int AddPoint(Point point, out MidPointLine tmpLine)
        {
            tmpLine = null;
            if (points == null)
                return AddPoint(point);

            int returnValue = AddPoint(point);
            tmpLine = new MidPointLine(shapeColor, thickness, points[points.Count - 2], points.Last());
            return returnValue;
        }

        public override List<Point> GetPixels()
        {
            var pixels = new List<Point>();

            for (int i = 0; i <= points.Count - 2; i++)
                pixels.AddRange(new MidPointLine(shapeColor, thickness, points[i], points[i + 1]).GetPixels());

            return pixels;
        }

        public override void MovePoints(Point displacement)
        {
            for (int i = 0; i < points.Count; i++)
               points[i] = points[i] + (Size)displacement;
        }

        public Bitmap SetPixelsAA(Bitmap bmp)
        {
            for (int i = 0; i <= points.Count - 2; i++)
                (new MidPointLine(shapeColor, thickness, points[i], points[i + 1])).SetPixelsAA(bmp);

            return bmp;
        }

        public override string ToString()
        {
            return "Polygon";
        }
    }
}
