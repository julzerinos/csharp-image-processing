using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    static class ColorspaceTools
    {
        public static void RGBtoYBR(int R, int G, int B, out int Y, out int Cb, out int Cr)
        {
            Y = (int)((0.299 * R) + (0.587 * G) + (0.114 * B));
            Cb = (int)(128 - (0.168736 * R) + (0.331264 * G) + (0.5 * B));
            Cr = (int)(128 + (0.5 * R) + (0.418688 * G) + (0.081312 * B));
        }
    }
}
