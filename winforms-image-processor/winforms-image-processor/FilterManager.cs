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
            { "Inversion",              (bmp) => FunctionFilterInversion(bmp) },
            { "Sharpen",                (bmp) => ConvolutionFilterSharpen(bmp) },
            { "Gamma",                  (bmp) => FunctionFilterGamma(bmp) },
            { "Gaussian Blur",          (bmp) => ConvolutionFilterGaussianBlur(bmp) },
            { "Blur",                   (bmp) => ConvolutionFilterBlur(bmp) },
            
        };

        public static Bitmap RecreateFilterStateFromState(Bitmap originalImage, List<string> state)
        {
            Debug.Print("Recreating filter state");

            foreach (string filter in state)
            {
                originalImage = filterMapping[filter](originalImage);
            }

            return originalImage;
        }

        ///////////////////////
        // Function Filters //
        /////////////////////

        static public Bitmap FunctionFilterInversion(Bitmap bmp)
        // Inversion filter     https://stackoverflow.com/questions/33024881/invert-image-faster-in-c-sharp
        {
            Bitmap bmpInv = new Bitmap(bmp);
            for (int y = 0; (y <= (bmpInv.Height - 1)); y++)
            {
                for (int x = 0; (x <= (bmpInv.Width - 1)); x++)
                {
                    Color inv = bmpInv.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    bmpInv.SetPixel(x, y, inv);
                }
            }
            return bmpInv;
        }

        static public Bitmap FunctionFilterGamma(Bitmap img)
        // Gamma filter         https://epochabuse.com/csharp-gamma-correction/
        {
            double gamma = 1.5f;
            double c = 1d;

            int width = img.Width;
            int height = img.Height;
            BitmapData srcData = img.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            img.UnlockBits(srcData);
            int current = 0;
            int cChannels = 3;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * srcData.Stride + x * 4;
                    for (int i = 0; i < cChannels; i++)
                    {
                        double range = (double)buffer[current + i] / 255;
                        double correction = c * Math.Pow(range, gamma);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }
            Bitmap resImg = new Bitmap(width, height);
            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            return resImg;
        }

        //static public Bitmap FunctionFilterBrightness(Bitmap bmp)
        ////Brightness correct    https://www.developerfusion.com/article/5441/image-manipulation-brightness-and-contrast/2/
        //{
        //    int r, g, b = 0;

        //    //Ready the whole bitmap for reading and writing
        //    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

        //    unsafe
        //    {
        //        //Sets the pointer to the first pixel
        //        byte* ptr = (byte*)bmpData.Scan0;
        //        int bytesPerPixel = SupportFunctions.BytesPerPixel(bmp.PixelFormat);

        //        /*Line offset for pointer.  
        //        Since we only have 3 bytes per pixel in this format
        //        and since lines are aligned at 4 byte boundaries
        //        we may need this for certain sized bitmaps)*/
        //        int remain = bmpData.Stride - bmpData.Width * bytesPerPixel;

        //        //Check for darken/lighten outside of loop to improve speed
        //        //Loops over each pixel
        //        if (darken == false)
        //        {
        //            for (int y = 0; y < bmpData.Height; y++)
        //            {
        //                for (int x = 0; x < bmpData.Width; x++)
        //                {
        //                    b = (int)ptr[0] + adjustment;
        //                    g = (int)ptr[1] + adjustment;
        //                    r = (int)ptr[2] + adjustment;

        //                    if (b > 255)
        //                        b = 255;
        //                    if (g > 255)
        //                        g = 255;
        //                    if (r > 255)
        //                        r = 255;

        //                    ptr[0] = (byte)b;
        //                    ptr[1] = (byte)g;
        //                    ptr[2] = (byte)r;

        //                    ptr += bytesPerPixel;
        //                }

        //                ptr += remain;
        //            }
        //        }
        //        else //(darken == true)
        //        {
        //            for (int y = 0; y < bmpData.Height; y++)
        //            {
        //                for (int x = 0; x < bmpData.Width; x++)
        //                {
        //                    b = (int)ptr[0] - adjustment;
        //                    g = (int)ptr[1] - adjustment;
        //                    r = (int)ptr[2] - adjustment;

        //                    if (b < 0)
        //                        b = 0;
        //                    if (g < 0)
        //                        g = 0;
        //                    if (r < 0)
        //                        r = 0;

        //                    ptr[0] = (byte)b;
        //                    ptr[1] = (byte)g;
        //                    ptr[2] = (byte)r;

        //                    ptr += bytesPerPixel;
        //                }

        //                ptr += remain;
        //            }
        //        }
        //    }

        //    bmp.UnlockBits(bmpData);
        //    return bmp;
        //}

        /////////////////////////
        // Convultion Filters //
        ///////////////////////
        
        static public Bitmap ConvolutionFilterSharpen(Bitmap image)
        // Sharpen filter:      https://stackoverflow.com/questions/903632/sharpen-on-a-bitmap-using-c-sharp/1319999
        {
            Bitmap sharpenImage = new Bitmap(image);

            int filterWidth = 3;
            int filterHeight = 3;
            int width = image.Width;
            int height = image.Height;

            // Create sharpening filter.
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + width) % width;
                            int imageY = (y - filterHeight / 2 + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            sharpenImage.UnlockBits(pbits);

            return sharpenImage;
        }

        static public Bitmap ConvolutionFilterBlur(Bitmap image)
        // Blur                 https://stackoverflow.com/questions/44827093/how-to-apply-blur-effect-on-an-image-in-c
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);
            Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
            Int32 blurSize = 10;

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (int x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (int x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (int y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return blurred;
        }

        static public Bitmap ConvolutionFilterGaussianBlur(Bitmap srcImage)
        // GBlur                https://epochabuse.com/gaussian-blur/
        {
            // Kernel Creation
            double weight = 5;
            int lenght = 9;

            double[,] kernel = new double[lenght, lenght];
            double kernelSum = 0;
            int foff = (lenght - 1) / 2;
            double distance = 0;
            double constant = 1d / (2 * Math.PI * weight * weight);
            for (int y = -foff; y <= foff; y++)
            {
                for (int x = -foff; x <= foff; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * weight * weight);
                    kernel[y + foff, x + foff] = constant * Math.Exp(-distance);
                    kernelSum += kernel[y + foff, x + foff];
                }
            }
            for (int y = 0; y < lenght; y++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    kernel[y, x] = kernel[y, x] * 1d / kernelSum;
                }
            }

            // Blurring
            int width = srcImage.Width;
            int height = srcImage.Height;
            BitmapData srcData = srcImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            srcImage.UnlockBits(srcData);
            int colorChannels = 3;
            double[] rgb = new double[colorChannels];
            int kcenter = 0;
            int kpixel = 0;
            for (int y = foff; y < height - foff; y++)
            {
                for (int x = foff; x < width - foff; x++)
                {
                    for (int c = 0; c < colorChannels; c++)
                    {
                        rgb[c] = 0.0;
                    }
                    kcenter = y * srcData.Stride + x * 4;
                    for (int fy = -foff; fy <= foff; fy++)
                    {
                        for (int fx = -foff; fx <= foff; fx++)
                        {
                            kpixel = kcenter + fy * srcData.Stride + fx * 4;
                            for (int c = 0; c < colorChannels; c++)
                            {
                                rgb[c] += (double)(buffer[kpixel + c]) * kernel[fy + foff, fx + foff];
                            }
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        if (rgb[c] > 255)
                        {
                            rgb[c] = 255;
                        }
                        else if (rgb[c] < 0)
                        {
                            rgb[c] = 0;
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        result[kcenter + c] = (byte)rgb[c];
                    }
                    result[kcenter + 3] = 255;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resultData.Scan0, bytes);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }

    }
}
