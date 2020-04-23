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

        public override List<ValueTuple<Point, Color>> GetPixelsAA(byte[] buffer, int stride)
        {
            return GuptaSproullAlgorithm(startPoint.Value, endPoint.Value, new KeyValuePair<byte[], int>(buffer, stride));
        }

        List<Point> BresenhamMidPointAlgorithm(Point start, Point end)
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


        List<ValueTuple<Point, Color>> GuptaSproullAlgorithm(Point start, Point end, KeyValuePair<byte[], int> buffer)
        // try:
        // http://elynxsdk.free.fr/ext-docs/Rasterization/Antialiasing/Gupta%20sproull%20antialiased%20lines.htm
        // todo: change dict to other structure
        {
            var cpDict = new List<ValueTuple<Point, Color>>();

            int x1 = start.X, y1 = start.Y;
            int x2 = end.X, y2 = end.Y;

            int dx = x2 - x1;
            int dy = y2 - y1;

            int du, dv, u, x, y, ix, iy;

            // By switching to (u,v), we combine all eight octant
            int adx = dx < 0 ? -dx : dx;
            int ady = dy < 0 ? -dy : dy;
            x = x1;
            y = y1;
            if (adx > ady)
            {
                du = adx;
                dv = ady;
                u = x2;
                ix = dx < 0 ? -1 : 1;
                iy = dy < 0 ? -1 : 1;
            }
            else
            {
                du = ady;
                dv = adx;
                u = y2;
                ix = dx < 0 ? -1 : 1;
                iy = dy < 0 ? -1 : 1;
            }

            int uEnd = u + du;
            int d = (2 * dv) - du; // Initial value as in Bresenham's
            int incrS = 2 * dv; // Δd for straight increments
            int incrD = 2 * (dv - du); // Δd for diagonal increments
            int twovdu = 0; // Numerator of distance
            double invD = 1.0 / (2.0 * Math.Sqrt(du * du + dv * dv)); // Precomputed inverse denominator
            double invD2du = 2.0 * (du * invD); // Precomputed constant

            if (adx > ady)
            {
                do
                {
                    Color color = Color.FromArgb(buffer.Key[x + buffer.Value * y + 3], buffer.Key[x + buffer.Value * y + 2], buffer.Key[x + buffer.Value * y + 1], buffer.Key[x + buffer.Value * y]);
                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y), newColorPixel(color, twovdu * invD)));
                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y + iy), newColorPixel(color, invD2du - twovdu * invD)));
                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y - iy), newColorPixel(color, invD2du + twovdu * invD)));

                    //newColorPixel(pw, pr, x, y, twovdu * invD, color);
                    //newColorPixel(pw, pr, x, y + iy, invD2du - twovdu * invD, color);
                    //newColorPixel(pw, pr, x, y - iy, invD2du + twovdu * invD, color);

                    if (d < 0)
                    {
                        // Choose straight
                        twovdu = d + du;
                        d += incrS;

                    }
                    else
                    {
                        // Choose diagonal
                        twovdu = d - du;
                        d += incrD;
                        y += iy;
                    }
                    u++;
                    x += ix;
                } while (u < uEnd);
            }
            else
            {
                do
                {
                    Color color = Color.FromArgb(buffer.Key[x + buffer.Value * y + 3], buffer.Key[x + buffer.Value * y + 2], buffer.Key[x + buffer.Value * y + 1], buffer.Key[x + buffer.Value * y]);

                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y), newColorPixel(color, twovdu * invD)));
                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y + iy), newColorPixel(color, invD2du - twovdu * invD)));
                    cpDict.Add(new ValueTuple<Point, Color>(new Point(x, y - iy), newColorPixel(color, invD2du + twovdu * invD)));

                    //newColorPixel(pw, pr, x, y, twovdu * invD, color);
                    //newColorPixel(pw, pr, x, y + iy, invD2du - twovdu * invD, color);
                    //newColorPixel(pw, pr, x, y - iy, invD2du + twovdu * invD, color);

                    if (d < 0)
                    {
                        // Choose straight
                        twovdu = d + du;
                        d += incrS;
                    }
                    else
                    {
                        // Choose diagonal
                        twovdu = d - du;
                        d += incrD;
                        x += ix;
                    }
                    u++;
                    y += iy;
                } while (u < uEnd);
            }

            return cpDict;
        }

        Color newColorPixel(Color old, double dist)
        {
            double value = 1 - Math.Pow((dist * 2 / 3), 2);
            Color color = Color.FromArgb(shapeColor.R, shapeColor.G, shapeColor.B);
            return ColorInterpolator.InterpolateBetween(old, color, value);
        }


    }
}
