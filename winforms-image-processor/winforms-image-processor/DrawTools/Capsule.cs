using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class Capsule : Shape
    {
        Point? startPoint = null;
        Point? endPoint = null;
        int? radius = null;

        public Capsule(Color color) : base(color)
        { shapeType = DrawingShape.CAPS; }

        public override int AddPoint(Point point)
        {
            if (startPoint == null)
                startPoint = point;
            else if (endPoint == null)
                endPoint = point;
            else if (radius == null)
            {
                radius = (int)Math.Sqrt(Math.Pow(point.X - endPoint.Value.X, 2) + Math.Pow(point.Y - endPoint.Value.Y, 2));
                return 1;
            }
            return 0;
        }

        public override List<ColorPoint> GetPixels(params object[] param)
        {
            var points = new List<ColorPoint>();

            var lowerStart = getAuxPoint(startPoint.Value, endPoint.Value, 1, out double angle1);
            var lowerEnd = getAuxPoint(endPoint.Value, startPoint.Value, -1, out double angle2);

            var upperEnd = getAuxPoint(endPoint.Value, startPoint.Value, 1, out double angle3);
            var upperStart = getAuxPoint(startPoint.Value, endPoint.Value, -1, out double angle4);

            points.AddRange(new MidPointCircle(shapeColor, startPoint.Value, radius.Value).getSemiCircle(angle1 + Math.PI/2));
            points.AddRange(new MidPointCircle(shapeColor, endPoint.Value, radius.Value).getSemiCircle(angle3 + Math.PI/2));

            points.AddRange(new MidPointLine(shapeColor, 1, lowerStart, lowerEnd).GetPixels());
            points.AddRange(new MidPointLine(shapeColor, 1, upperStart, upperEnd).GetPixels());

            return points;
        }

        Point getAuxPoint(Point start, Point end, int side, out double a)
        {
            int ABY = end.Y - start.Y;
            int ABX = end.X - start.X;

            a = Math.Atan2(end.Y - start.Y, end.X - start.X) + side * 90.0 * Math.PI / 180.0; // sign depends on your 

            var Len = Math.Sqrt(ABY * ABY + ABX * ABX);
            var CX = (int)(startPoint.Value.X - radius * ABY / Len).Value;
            var CY = (int)(startPoint.Value.Y + radius * ABX / Len).Value;

            return new Point((int)(start.X + radius * Math.Cos(a)).Value, (int)(start.Y + radius * Math.Sin(a)).Value);
        }

        public override void MovePoints(Point displacement)
        {
            startPoint = startPoint.Value + (Size)displacement;
            endPoint = endPoint.Value + (Size)displacement;
        }

        public override string ToString()
        {
            return "Capsule";
        }

        public override string howToDraw()
        {
            return "Click point 1, point 2 and radius.";
        }

        public override List<ColorPoint> SetPixelsAA(Bitmap bmp)
        {
            throw new NotImplementedException();
        }
    }
}
