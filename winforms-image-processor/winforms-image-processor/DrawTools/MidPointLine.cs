using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    class MidPointLine : Shape
    {
        public Point? startPoint = null;
        public Point? endPoint = null;

        /// <summary>
        /// Adds either start or end point to a line
        /// </summary>
        /// <param name="point"></param>
        /// <returns>return 0 when adding start point, 1 when end point</returns>
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

        /// <summary>
        /// Calculates pixels for midpoint line.
        /// source: https://www.geeksforgeeks.org/mid-point-line-generation-algorithm/
        /// </summary>
        /// <returns>List of points for midpoint line</returns>
        public override List<Point> GetPixels()
        {
            if (!endPoint.HasValue || !startPoint.HasValue)
                throw new MissingMemberException();

            var points = new List<Point>();

            int dx = endPoint.Value.X - startPoint.Value.X;
            int dy = endPoint.Value.Y - startPoint.Value.Y;

            int d = dy - (dx / 2);
            int x = startPoint.Value.X, y = startPoint.Value.Y;

            points.Add(new Point(x, y));

            while (x < endPoint.Value.X)
            {
                x++;

                if (d < 0)
                    d += dy;
                else
                {
                    d += (dy - dx);
                    y++;
                }

                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}
