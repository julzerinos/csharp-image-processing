using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    /// <summary>
    /// The FilterManager Class is responsible for applying filters to the given image.
    /// </summary>
    static class FilterManager
    // Expression lambdas:  https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
    {
        public static Dictionary<string, Func<Bitmap, Bitmap>> filterMapping = new Dictionary<string, Func<Bitmap, Bitmap>> {
            { "Inversion",              (bmp) => bmp.ApplyFilter(Inversion) },
            { "Gamma",                  (bmp) => bmp.ApplyFilter(Gamma)},
            { "Brightness Correction",  (bmp) => bmp.ApplyFilter(Brightness) },
            { "Contrast Correction",    (bmp) => bmp.ApplyFilter(Contrast) },

            { "Grayscale",              (bmp) => bmp.ApplyFilter(Grayscale) },
            { "Average Dither",         (bmp) => bmp.ApplyFilter(AverageDither) },
            { "Octree Quantization",    (bmp) => bmp.ApplyFilter(OctreeQuantization) },

            { "Sharpen",                (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.SharpenKernel) },
            { "Gaussian Blur",          (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.GaussianBlurKernel) },
            { "Box Blur",               (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.BoxBlurKernel) },
            { "Emboss",                 (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.EmbossKernel) },
            { "Outline",                (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.OutlineKernel) },
        };

        public static void UpdateFilterMapping(string filter)
        {
            filterMapping.Add(
                filter,
                (bmp) => bmp.ApplyFilter(ApplyKernel, Kernel.customKernels[filter])
                );
        }

        public static Bitmap RecreateFilterStateFromState(Bitmap originalImage, List<string> state)
        {
            Debug.Print("Recreating filter state");

            foreach (string filter in state)
                originalImage = filterMapping[filter](originalImage);

            return originalImage;
        }

        ///////////////////////
        // Function Filters //
        /////////////////////

        static public byte[] Inversion(byte[] buffer)
        // Inversion filter     https://stackoverflow.com/questions/33024881/invert-image-faster-in-c-sharp
        {
            byte[] result = new byte[buffer.Length];
            int inv = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i + 1) % 4 == 0)
                {
                    result[i] = buffer[i];
                    continue;
                }

                inv = 255 - buffer[i];
                result[i] = (byte)inv;
            }
            return result;
        }

        static public byte[] Gamma(byte[] buffer)
        // Gamma filter         https://epochabuse.com/csharp-gamma-correction/
        {
            var gamma = Constants.filterGammaValue;

            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i + 1) % 4 == 0)
                {
                    result[i] = buffer[i];
                    continue;
                }

                double range = (double)buffer[i] / 255;
                double correction = 1d * Math.Pow(range, gamma);
                result[i] = (byte)(correction * 255);

            }
            return result;
        }

        static public byte[] Brightness(byte[] buffer)
        //Brightness correct    https://www.developerfusion.com/article/5441/image-manipulation-brightness-and-contrast/2/
        //Brightness algorithm  https://www.dfstudios.co.uk/articles/programming/image-programming-algorithms/image-processing-algorithms-part-4-brightness-adjustment/
        {
            var brightness = Constants.filterBrightnessValue;

            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i + 1) % 4 == 0)
                {
                    result[i] = buffer[i];
                    continue;
                }
                result[i] = (byte)(brightness > 0 ? Math.Min(buffer[i] + brightness, 255) : Math.Max(buffer[i] + brightness, 0));

            }

            return result;
        }

        static public byte[] Contrast(byte[] buffer)
        //Contrast algorithm    https://www.dfstudios.co.uk/articles/programming/image-programming-algorithms/image-processing-algorithms-part-5-contrast-adjustment/
        {
            var corrFactor = Math.Pow((100.0 + Constants.filterContrastValue) / 100.0, 2);

            byte[] result = new byte[buffer.Length];

            int cnt = 0, value;
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i + 1) % 4 == 0)
                {
                    result[i] = buffer[i];
                    continue;
                }

                value = (int)(((((buffer[i] / 255.0) - 0.5) * corrFactor) + 0.5) * 255.0);
                cnt = (value < 0) ? 0 : (value > 255) ? 255 : value;
                result[i] = (byte)cnt;
            }
            return result;
        }

        /////////////////////////
        // Convultion Filters //
        ///////////////////////

        static public byte[] ApplyKernel(byte[] buffer, int stride, CustomKernel customKernel)
        {
            double[,] kernel = customKernel.GetKernel();
            byte[] result = new byte[buffer.Length];

            // 1D Bitmap byte data
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i + 1) % 4 == 0)
                {
                    result[i] = buffer[i];
                    continue;
                }

                double newByte = 0;
                bool ignorePixel = false;

                // Kernel Columns
                int lowerColBound = -kernel.GetLength(0) / 2 - customKernel.anchor.X;
                int upperColBound = kernel.GetLength(0) / 2 - customKernel.anchor.X;
                for (int j = lowerColBound; j <= upperColBound; j++)
                {
                    if ((i + 4 * j < 0) || (i + 4 * j >= buffer.Length) || ignorePixel)
                    {
                        newByte = 0;
                        break;
                    }

                    // Kernel Rows
                    int lowerRowBound = -kernel.GetLength(1) / 2 - customKernel.anchor.Y;
                    int upperRowBound = kernel.GetLength(1) / 2 - customKernel.anchor.Y;
                    for (int k = lowerRowBound; k <= upperRowBound; k++)
                    {
                        if ((i + 4 * j + k * stride < 0) || (i + 4 * j + k * stride >= buffer.Length))
                        {
                            ignorePixel = true;
                            break;
                        }

                        newByte += kernel[
                            j - lowerColBound,
                            k - lowerRowBound]
                            * buffer[i + 4 * j + k * stride]
                            + customKernel.offset;
                    }
                }

                result[i] = (byte)(newByte < 0 ? 0 : newByte > 255 ? 255 : newByte);
            }

            return result;
        }

        ////////////////
        // Grayscale //
        //////////////

        static public byte[] Grayscale(byte[] buffer)
        // Little Endian byte order https://stackoverflow.com/questions/8104461/pixelformat-format32bppargb-seems-to-have-wrong-byte-order
        {
            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i = i + 4)
            {
                result[i + 3] = buffer[i + 3];

                double grayscale = (buffer[i + 3] * 0.3) + (buffer[i + 2] * 0.59) + (buffer[i] * 0.11);
                result[i] = result[i + 1] = result[i + 2] = (byte)grayscale;
            }

            return result;
        }

        ////////////////////////////////////
        // Dither & Quantization Filters //
        //////////////////////////////////

        static public byte[] AverageDither(byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];
            Point[] sums = new Point[4] { Point.Empty, Point.Empty, Point.Empty, Point.Empty };

            for (int i = 0; i < buffer.Length; i++)
            {
                sums[i % 4].X += buffer[i];
                sums[i % 4].Y++;
            }

            var thresh = new int[4];
            for (int i = 0; i < 4; i++)
                thresh[i] = sums[i].X / sums[i].Y;

            thresh[3] = 255;

            for (int i = 0; i < buffer.Length; i++)
                result[i] = (byte)(thresh[i % 4] >= buffer[i] ? 255 : 0);

            return result;
        }

        static public byte[] OctreeQuantization(byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];

            OctreeQuantizer octree = new OctreeQuantizer();

            for (int i = 0; i < buffer.Length; i += 4)
                octree.AddColor(Color.FromArgb(buffer[i + 2], buffer[i + 1], buffer[i]));

            var limitedPalette = octree.GetPalette(Constants.filterOctreeColorLimit);

            for (int i = 0; i < buffer.Length; i += 4)
            {
                Color currentPixel = Color.FromArgb(buffer[i + 2], buffer[i + 1], buffer[i]);
                Color resultPixel = limitedPalette[octree.GetPaletteIndex(currentPixel)];

                result[i + 0] = resultPixel.B;
                result[i + 1] = resultPixel.G;
                result[i + 2] = resultPixel.R;
                result[i + 3] = 255;
            }

            return result;
        }

        ///////////////////////////////
        // YCbCr Colorspace Filters //
        /////////////////////////////

        static public byte[] YGrayscale(byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i += 4)
            {
                ColorspaceTools.RGBtoYBR(buffer[i + 2], buffer[i + 1], buffer[i], out int Y, out _, out _);

                result[i + 0] = result[i + 1] = result[i + 2] = (byte)Y;
                result[i + 3] = 255;
            }

            return result;
        }

        static public byte[] CbInterpolate(byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i += 4)
            {
                ColorspaceTools.RGBtoYBR(buffer[i + 2], buffer[i + 1], buffer[i], out int Y, out int Cb, out int Cr);

                result[i] = (byte)((1 - Cb) * 0 + Cb * 255);
                result[i + 1] = (byte) ((1 - Cb) * 255 + Cb * 0);
                result[i + 2] = (byte) ((1 - Cb) * 127 + Cb * 127);
                result[i + 3] = 255;
            }

            return result;
        }

        static public byte[] CrInterpolate(byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];

            for (int i = 0; i < buffer.Length; i += 4)
            {
                ColorspaceTools.RGBtoYBR(buffer[i + 2], buffer[i + 1], buffer[i], out int Y, out int Cb, out int Cr);

                result[i] = (byte)((1 - Cr) * 127 + Cr * 127);
                result[i + 1] = (byte)((1 - Cr) * 255 + Cr * 0);
                result[i + 2] = (byte)((1 - Cr) * 0 + Cr * 255);
                result[i + 3] = 255;
            }

            return result;
        }
    }
}
