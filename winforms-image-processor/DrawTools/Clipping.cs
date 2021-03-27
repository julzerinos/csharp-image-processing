using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    public struct Line
    {
        public Point Start;
        public Point End;

        public Line(Point s, Point e)
        {
            End = e;
            Start = s;
        }

        public override string ToString()
        {
            return $"{Start.ToString()} && {End.ToString()}";
        }
    }

    [Serializable]
    class Clipping
    {
        // https://pastebin.com/NA01gacf

        private Point _clipMin, _clipMax;

        public IEnumerable<Point> GetBoundingPolygon()
        {
            yield return _clipMin;
            yield return new Point(_clipMax.X, _clipMin.Y);
            yield return _clipMax;
            yield return new Point(_clipMin.X, _clipMax.Y);
        }

        public void SetBoundingRectangle(Rectangle rect)
        {
            _clipMin = rect.GetCorner(3);
            _clipMax = rect.GetCorner(1);
        }

        private delegate bool ClippingHandler(float p, float q);

        public bool ClipLine(ref Line line)
        {
            Point P = line.End - (Size)line.Start;
            float tMinimum = 0, tMaximum = 1;

            ClippingHandler pqClip = delegate (float directedProjection,
            float directedDistance)
            {
                if (directedProjection == 0)
                {
                    if (directedDistance < 0)
                        return false;
                }
                else
                {
                    float amount = directedDistance / directedProjection;
                    if (directedProjection < 0)
                    {
                        if (amount > tMaximum)
                            return false;
                        else if (amount > tMinimum)
                            tMinimum = amount;
                    }
                    else
                    {
                        if (amount < tMinimum)
                            return false;
                        else if (amount < tMaximum)
                            tMaximum = amount;
                    }
                }
                return true;
            };

            if (pqClip(-P.X, line.Start.X - _clipMin.X))
            {
                if (pqClip(P.X, _clipMax.X - line.Start.X))
                {
                    if (pqClip(-P.Y, line.Start.Y - _clipMin.Y))
                    {
                        if (pqClip(P.Y, _clipMax.Y - line.Start.Y))
                        {
                            if (tMaximum < 1)
                            {
                                line.End.X = (int)(line.Start.X + tMaximum * P.X);
                                line.End.Y = (int)(line.Start.Y + tMaximum * P.Y);
                            }
                            if (tMinimum > 0)
                            {
                                line.Start.X = (int)(line.Start.X + tMinimum * P.X);
                                line.Start.Y = (int)(line.Start.Y + tMinimum * P.Y);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //public bool ClipLine(ref MidPointLine line)
        //{
        //    Point P = line.endPoint.Value - (Size)line.startPoint.Value;
        //    float tMinimum = 0, tMaximum = 1;

        //    ClippingHandler pqClip = delegate (float directedProjection,
        //    float directedDistance)
        //    {
        //        if (directedProjection == 0)
        //        {
        //            if (directedDistance < 0)
        //                return false;
        //        }
        //        else
        //        {
        //            float amount = directedDistance / directedProjection;
        //            if (directedProjection < 0)
        //            {
        //                if (amount > tMaximum)
        //                    return false;
        //                else if (amount > tMinimum)
        //                    tMinimum = amount;
        //            }
        //            else
        //            {
        //                if (amount < tMinimum)
        //                    return false;
        //                else if (amount < tMaximum)
        //                    tMaximum = amount;
        //            }
        //        }
        //        return true;
        //    };

        //    if (pqClip(-P.X, line.startPoint.Value.X - _clipMin.X))
        //    {
        //        if (pqClip(P.X, _clipMax.X - line.startPoint.Value.X))
        //        {
        //            if (pqClip(-P.Y, line.startPoint.Value.Y - _clipMin.Y))
        //            {
        //                if (pqClip(P.Y, _clipMax.Y - line.startPoint.Value.Y))
        //                {
        //                    if (tMaximum < 1)
        //                        line.endPoint = new Point((int)(line.startPoint.Value.X + tMaximum * P.X), (int)(line.startPoint.Value.Y + tMaximum * P.Y));
        //                    if (tMinimum > 0)
        //                        line.startPoint = new Point((int)(line.startPoint.Value.X + tMinimum * P.X), (int)(line.startPoint.Value.Y + tMinimum * P.Y));
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        public override string ToString()
        {
            return "Liang-Barsky algorithm";
        }

        // This code was implemented by Grishul Eugeny as part of preparation
        // to exam in ITMO university
    }
}
