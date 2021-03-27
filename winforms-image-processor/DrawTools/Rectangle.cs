using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class Rectangle : Shape
    {
        public Point? startPoint = null;
        public Point? endPoint = null;
        int thickness;

        List<Point> orderedPoints = new List<Point>();

        public Rectangle(Color color, int thicc) : base(color)
        {
            shapeType = DrawingShape.RECT;
            thickness = thicc;
            supportsAA = true;
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

        void OrderPoints()
            // 0 highest y and lowest x
        {
            Point revStartPoint = new Point(startPoint.Value.X, endPoint.Value.Y);
            Point revEndPoint = new Point(endPoint.Value.X, startPoint.Value.Y);

            if (startPoint.Value.X > endPoint.Value.X)
            {
                if (startPoint.Value.Y > endPoint.Value.Y)
                {
                    orderedPoints.Add(revEndPoint);
                    orderedPoints.Add(startPoint.Value);
                    orderedPoints.Add(revStartPoint);
                    orderedPoints.Add(endPoint.Value);

                    return;
                }

                orderedPoints.Add(endPoint.Value);
                orderedPoints.Add(revStartPoint);
                orderedPoints.Add(startPoint.Value);
                orderedPoints.Add(revEndPoint);

                return;
            }

            if (startPoint.Value.Y > endPoint.Value.Y)
            {
                orderedPoints.Add(startPoint.Value);
                orderedPoints.Add(revEndPoint);
                orderedPoints.Add(endPoint.Value);
                orderedPoints.Add(revStartPoint);

                return;
            }

            orderedPoints.Add(revStartPoint);
            orderedPoints.Add(endPoint.Value);
            orderedPoints.Add(revEndPoint);
            orderedPoints.Add(startPoint.Value);

            return;
        }

        public Point GetCorner(int corner)
        {
            OrderPoints();

            return orderedPoints[corner];
        }

        public override List<ColorPoint> GetPixels(params object[] param)
        {
            var points = new List<ColorPoint>();

            Point upperLeft = new Point(startPoint.Value.X, endPoint.Value.Y);
            Point lowerRight = new Point(endPoint.Value.X, startPoint.Value.Y);

            points.AddRange(new MidPointLine(shapeColor, thickness, startPoint.Value, upperLeft).GetPixels());
            points.AddRange(new MidPointLine(shapeColor, thickness, upperLeft, endPoint.Value).GetPixels());
            points.AddRange(new MidPointLine(shapeColor, thickness, endPoint.Value, lowerRight).GetPixels());
            points.AddRange(new MidPointLine(shapeColor, thickness, lowerRight, startPoint.Value).GetPixels());

            return points;
        }

        public override List<ColorPoint> GetPixelsAA(Bitmap bmp)
        {
            var points = new List<ColorPoint>();

            Point upperLeft = new Point(startPoint.Value.X, endPoint.Value.Y);
            Point lowerRight = new Point(endPoint.Value.X, startPoint.Value.Y);

            points.AddRange(new MidPointLine(shapeColor, thickness, startPoint.Value, upperLeft).GetPixelsAA(bmp));
            points.AddRange(new MidPointLine(shapeColor, thickness, upperLeft, endPoint.Value).GetPixelsAA(bmp));
            points.AddRange(new MidPointLine(shapeColor, thickness, endPoint.Value, lowerRight).GetPixelsAA(bmp));
            points.AddRange(new MidPointLine(shapeColor, thickness, lowerRight, startPoint.Value).GetPixelsAA(bmp));

            return points;
        }

        public override void MovePoints(Point displacement)
        {
            startPoint = startPoint.Value + (Size)displacement;
            endPoint = endPoint.Value + (Size)displacement;
        }

        public override string ToString()
        {
            return "Rectangle";
        }

        public override string howToDraw()
        {
            return "Click point lower left and point upper right.";
        }
    }
}
