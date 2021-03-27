using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    [Serializable]
    class Fill : Shape
    {
        public Point seedPoint;

        public Fill(Color col, Point p) : base(col)
        {
            shapeType = DrawingShape.FILL;
            seedPoint = p;
        }

        public override int AddPoint(Point point)
        {
            throw new NotImplementedException();
        }

        public override List<ColorPoint> GetPixels(params object[] param)
        {
            throw new NotImplementedException();
        }

        public override List<ColorPoint> GetPixelsAA(Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public override string howToDraw()
        {
            throw new NotImplementedException();
        }

        public override void MovePoints(Point displacement)
        {
            seedPoint += (Size)displacement;
        }

        public override string ToString()
        {
            return "Filled region";
        }
    }
}
