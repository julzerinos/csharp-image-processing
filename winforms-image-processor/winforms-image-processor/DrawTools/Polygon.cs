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
        public List<Point> points = null;
        public int thickness;

        protected Filler filler = null;

        public Polygon(Color color, int thicc) : base(color)
        {
            thickness = thicc - 1;
            shapeType = DrawingShape.POLY;
            supportsAA = true;
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

        public void SetFiller(Color color)
        {
            filler = new Filler(points, fillColor: color);
        }

        public void SetFiller(string filename)
        {
            filler = new Filler(points, fillImage: new Bitmap(filename));
        }

        public void UnSetFiller()
        {
            filler = null;
        }

        public override List<ColorPoint> GetPixels(params object[] param)
        {
            var pixels = new List<ColorPoint>();

            if (filler != null)
                pixels.AddRange(filler.FillPoints());

            for (int i = 0; i <= points.Count - 2; i++)
                pixels.AddRange(new MidPointLine(shapeColor, thickness, points[i], points[i + 1]).GetPixels());

            return pixels;
        }

        public override string howToDraw()
        {
            return "Click each point and click on first to finish.";
        }

        public override void MovePoints(Point displacement)
        {
            for (int i = 0; i < points.Count; i++)
                points[i] = points[i] + (Size)displacement;

            if (filler != null)
                filler.UpdatePoints(points);
        }

        public override List<ColorPoint> GetPixelsAA(Bitmap bmp)
        {
            var pixels = new List<ColorPoint>();

            if (filler != null)
                pixels.AddRange(filler.FillPoints());

            for (int i = 0; i <= points.Count - 2; i++)
                pixels.AddRange((new MidPointLine(shapeColor, thickness, points[i], points[i + 1])).GetPixelsAA(bmp));

            return pixels;
        }

        public override string ToString()
        {
            return "Polygon";
        }
    }
}
