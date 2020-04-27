using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class MidPointCircle : Shape
    {
        Point? center = null;
        int? radius = null;

        public MidPointCircle(Color color) : base(color)
        { shapeType = DrawingShape.CIRCLE; }

        public MidPointCircle(Color color, Point center, int radius) : base(color)
        {
            shapeType = DrawingShape.CIRCLE;
            this.center = center;
            this.radius = radius;
        }

        public override string ToString()
        {
            return "Circle";
        }

        public override int AddPoint(Point point)
        {
            if (center == null)
                center = point;
            else if (radius == null)
            {
                radius = (int)Math.Sqrt(Math.Pow(point.X - center.Value.X, 2) + Math.Pow(point.Y - center.Value.Y, 2));
                return 1;
            }
            return 0;
        }

        public List<Point> getSemiCircle(double angle)
        {
            if (!center.HasValue || !radius.HasValue)
                throw new MissingMemberException();

            if (radius.Value == 0)
                return new List<Point>() { center.Value };

            var points = new List<Point>();

            int x = radius.Value, y = 0;
            int P = 1 - x;

            while (x > y)
            {

                y++;

                if (P <= 0)
                    P = P + 2 * y + 1;
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                if (x < y)
                    break;

                points.Add(new Point(x + center.Value.X, y + center.Value.Y));
                points.Add(new Point(x + center.Value.X, -y + center.Value.Y));

                if (x != y)
                {
                    points.Add(new Point(y + center.Value.X, x + center.Value.Y));
                    points.Add(new Point(y + center.Value.X, -x + center.Value.Y));
                }
            }

            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);

            for (int i = 0; i < points.Count; i++)
            {
                x = (int)Math.Round((points[i].X - center.Value.X) * cos - (points[i].Y - center.Value.Y) * sin + center.Value.X);
                y = (int)Math.Round((points[i].X - center.Value.X) * sin + (points[i].Y - center.Value.Y) * cos + center.Value.Y);

                points[i] = new Point(x, y);

                Console.WriteLine((points[i]));
            }

            return points;
        }

        public override List<Point> GetPixels()
        // https://www.geeksforgeeks.org/mid-point-circle-drawing-algorithm/
        {
            if (!center.HasValue || !radius.HasValue)
                throw new MissingMemberException();

            if (radius.Value == 0)
                return new List<Point>() { center.Value };

            var points = new List<Point>();

            int x = radius.Value, y = 0;
            int P = 1 - x;

            while (x > y)
            {

                y++;

                if (P <= 0)
                    P = P + 2 * y + 1;
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                if (x < y)
                    break;

                points.Add(new Point(x + center.Value.X, y + center.Value.Y));
                points.Add(new Point(-x + center.Value.X, y + center.Value.Y));
                points.Add(new Point(x + center.Value.X, -y + center.Value.Y));
                points.Add(new Point(-x + center.Value.X, -y + center.Value.Y));

                // If the generated point is on the  
                // line x = y then the perimeter points 
                // have already been printed 
                if (x != y)
                {
                    points.Add(new Point(y + center.Value.X, x + center.Value.Y));
                    points.Add(new Point(-y + center.Value.X, x + center.Value.Y));
                    points.Add(new Point(y + center.Value.X, -x + center.Value.Y));
                    points.Add(new Point(-y + center.Value.X, -x + center.Value.Y));
                }
            }

            return points;
        }

        public override void MovePoints(Point displacement)
        {
            center = center.Value + (Size)displacement;
        }

        public override string howToDraw()
        {
            return "Click center and radius";
        }
    }
}
