using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace winforms_image_processor
{
    public struct CustomKernel
    {
        public double[,] kernel;
        public int divisor;

        public int offset;
        public Point anchor;

        public CustomKernel(double[,] kernel, int div, int off, Point anch)
        {
            this.kernel = kernel;
            divisor = div;
            offset = off;
            anchor = anch;

            Debug.Print(this.ToString());
        }

        public double[,] GetKernel()
        {
            double[,] kernelCopy = new double[kernel.GetLength(0), kernel.GetLength(1)];
            for (int i = 0; i < kernel.GetLength(0); i++)
                for (int j = 0; j < kernel.GetLength(1); j++)
                    kernelCopy[i, j] = kernel[i, j] / divisor;
            return kernelCopy;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Kernel with following values:\n");
            foreach (int i in kernel)
            {
                sb.Append(i);
                sb.Append(", ");
            }
            sb.Append("\n");
            sb.Append(divisor);
            sb.Append("\n");
            sb.Append(offset);
            sb.Append("\n");
            sb.Append(anchor.ToString());
            return sb.ToString();
        }
    }

    public static class Kernel
    {
        public static CustomKernel customKernel = new CustomKernel(new double[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, 1, 0, Point.Empty);

        public static CustomKernel SharpenKernel = new CustomKernel(new double[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } }, 1, 0, Point.Empty);
        public static CustomKernel OutlineKernel = new CustomKernel(new double[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } }, 1, 0, Point.Empty);
        public static CustomKernel EmbossKernel = new CustomKernel(new double[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } }, 1, 0, Point.Empty);
        public static CustomKernel BoxBlurKernel = new CustomKernel(new double[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, 9, 0, Point.Empty);
        public static CustomKernel GaussianBlurKernel = new CustomKernel(new double[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } }, 16, 0, Point.Empty);

    }
}
