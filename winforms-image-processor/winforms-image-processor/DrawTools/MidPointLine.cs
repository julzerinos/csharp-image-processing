using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class MidPointLine : Shape
    {
        public Point? startPoint = null;
        public Point? endPoint = null;
        public int thickness;

        public MidPointLine(Color color, int thicc) : base(color)
        {
            thickness = thicc - 1;
            shapeType = DrawingShape.LINE;
        }

        public override string ToString()
        {
            return "Line";
        }

        public override int AddPoint(Point point)
        {
            if (startPoint == null)
                startPoint = point;
            else if (endPoint == null)
            {
                endPoint = point;
                return 1;
            }
            return 0;
        }

        public override List<Point> GetPixels()
        {
            return BresenhamMidPointAlgorithm((Point)startPoint, (Point)endPoint);
        }

        public List<Point> BresenhamMidPointAlgorithm(Point start, Point end)
        // https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        {
            List<Point> points = new List<Point>();

            int x = start.X, y = start.Y;
            int x2 = end.X, y2 = end.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                points.Add(new Point(x, y));
                if (Math.Abs(h) > Math.Abs(w))
                    for (int j = 1; j < thickness; j++)
                    {
                        points.Add(new Point(x - j, y));
                        points.Add(new Point(x + j, y));
                    }
                else if (Math.Abs(w) > Math.Abs(h))
                    for (int j = 1; j < thickness; j++)
                    {
                        points.Add(new Point(x, y - j));
                        points.Add(new Point(x, y + j));
                    }

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return points;
        }
    }
}
