using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace winforms_image_processor
{
    public struct ActiveEdgeTableEntry
    {
        public double yMax;
        public double yMin;
        public double mInv;
        public double xOfMin;
        public double xOfMax;

        public ActiveEdgeTableEntry(Point start, Point end)
        {
            Point lower = start.Y > end.Y ? end : start;
            Point higher = start.Y > end.Y ? start : end;

            yMax = higher.Y;
            yMin = lower.Y;
            xOfMax = higher.X;
            xOfMin = lower.X;
            mInv = (xOfMax - xOfMin) / (yMax - yMin);
        }

        public ActiveEdgeTableEntry(ActiveEdgeTableEntry aete)
        {
            yMax = aete.yMax;
            yMin = aete.yMin;
            xOfMax = aete.xOfMax;
            xOfMin = aete.xOfMin + aete.mInv;
            mInv = aete.mInv;
        }

        public override string ToString()
        {
            return $"{yMin} && {xOfMin} && {yMax} && {xOfMax} && {mInv}";
        }
    }

    class Filler
    {
        List<ColorPoint> points = null;

        List<Point> vertices = new List<Point>();
        List<KeyValuePair<int, int>> indicies = new List<KeyValuePair<int, int>>();
        List<ActiveEdgeTableEntry> AET = new List<ActiveEdgeTableEntry>();

        Color? fillColor;
        Bitmap fillImage;

        public Filler(List<Point> pVertices, Color? fillColor = null, Bitmap fillImage = null)
        {
            var dict = new Dictionary<int, int>();

            for (int i = 0; i < pVertices.Count; i++)
            {
                vertices.Add(pVertices[i]);
                dict.Add(i, pVertices[i].Y);
            }

            indicies = dict.OrderBy(x => x.Value).ToList();

            this.fillColor = fillColor;
            if (fillImage != null)
                this.fillImage = fillImage;
        }

        public void UpdatePoints(List<Point> points)
        {
            vertices = new List<Point>(points);
        }

        public List<ColorPoint> FillPoints(Rectangle boundingRect = null)
        {
            if (this.points != null)
                return this.points;

            points = new List<ColorPoint>();

            int k = 0;
            int i = indicies[k].Key;
            int y, ymin;
            y = ymin = vertices[indicies[0].Key].Y;
            int ymax = vertices[indicies[indicies.Count - 1].Key].Y;

            int len = vertices.Count;

            while (y < ymax)
            {
                while (vertices[i].Y == y)
                {
                    if (vertices[(i - 1 + len) % len].Y > vertices[i].Y)
                        AET.Add(new ActiveEdgeTableEntry(vertices[i], vertices[(i - 1 + len) % len]));

                    if (vertices[(i + 1) % len].Y > vertices[i].Y)
                        AET.Add(new ActiveEdgeTableEntry(vertices[i], vertices[(i + 1) % len]));

                    i = indicies[++k].Key;
                }

                AET.Sort(delegate (ActiveEdgeTableEntry e1, ActiveEdgeTableEntry e2)
                {
                    return e1.xOfMin.CompareTo(e2.xOfMin);
                });

                for (int t = 0; t < AET.Count; t+=2)
                    for (int x1 = (int)AET[t].xOfMin; x1 <= AET[(t + 1) % AET.Count].xOfMin; x1++)
                    {
                        Color col = fillColor.HasValue ? fillColor.Value : fillImage.GetPixelFast(x1 % fillImage.Width, y % fillImage.Height);
                        if (boundingRect != null && (x1 > boundingRect.GetCorner(1).X || x1 < boundingRect.GetCorner(0).X || y > boundingRect.GetCorner(0).Y || y < boundingRect.GetCorner(2).Y))
                            continue;
                        points.Add(new ColorPoint(col, new Point(x1, y)));
                    }

                ++y;
                for (int t = 0; t < AET.Count; t++)
                {
                    AET[t] = new ActiveEdgeTableEntry(AET[t]);
                    if (AET[t].yMax == y)
                        AET.RemoveAt(t--);
                }

            }

            return points;
        }
    }
}
