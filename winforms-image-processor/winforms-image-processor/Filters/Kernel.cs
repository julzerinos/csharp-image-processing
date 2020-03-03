using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    public struct CustomKernel
    {
        public double[,] kernel;
        public int divisor;

        public CustomKernel(double[,] kernel, int div)
        {
            this.kernel = kernel;
            divisor = div;
        }

        public double[,] GetKernel()
        {
            double[,] kernel = this.kernel;
            for (int i = 0; i < kernel.GetLength(0); i++)
                for (int j = 0; j < kernel.GetLength(1); j++)
                    kernel[i, j] /= divisor;
            return kernel;
        }
    }

    public static class Kernel
    {
        public static CustomKernel customKernel =       new CustomKernel(new double[3,3], 0);

        public static CustomKernel SharpenKernel =      new CustomKernel(new double[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } },        1);
        public static CustomKernel OutlineKernel =      new CustomKernel(new double[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } },    1);
        public static CustomKernel EmbossKernel =       new CustomKernel(new double[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } },         1);
        public static CustomKernel BoxBlurKernel =      new CustomKernel(new double[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } },            9);
        public static CustomKernel GaussianBlurKernel = new CustomKernel(new double[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } },            16);

    }
}
