using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    abstract class Shape
    {
        abstract public int AddPoint(Point point);

        abstract public List<Point> GetPixels();
    }
}
