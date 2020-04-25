using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    public static class BitmapExtension
    {
        public static void SetPixelFast(this Bitmap bmp, int x, int y, Color color)
        {
            var newValues = new byte[] { color.B, color.G, color.R, 255 };

            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
                );

            if (
                data.Stride * y + 4 * x < data.Stride * data.Height
                && data.Stride * y + 4 * x >= 0
                && x * 4 < data.Stride
                && y < data.Height
                )
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;

                    for (int i = 0; i < 4; i++)
                        ptr[data.Stride * y + 4 * x + i] = newValues[i];
                }

            bmp.UnlockBits(data);
        }

        public static Color GetPixelFast(this Bitmap bmp, int x, int y)
        {
            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            Color col = Color.FromArgb(0, 0, 0, 0); ;

            if (data.Stride * y + 4 * x < data.Stride * data.Height && data.Stride * y + 4 * x >= 0)
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;

                    col = Color.FromArgb(
                        ptr[data.Stride * y + 4 * x + 3],
                        ptr[data.Stride * y + 4 * x + 2],
                        ptr[data.Stride * y + 4 * x + 1],
                        ptr[data.Stride * y + 4 * x + 0]
                    );
                }

            bmp.UnlockBits(data);

            return col;
        }

        public static byte[] GetBitmapDataBytes(this Bitmap bmp, out int stride)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData srcData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            stride = srcData.Stride;
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            bmp.UnlockBits(srcData);

            return buffer;
        }

        public static byte[] GetBitmapDataBytes(this Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData srcData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            bmp.UnlockBits(srcData);

            return buffer;
        }

        public static void SetBitmapDataBytes(this Bitmap bmp, byte[] bytes)
        {
            BitmapData resData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
                );

            Marshal.Copy(bytes, 0, resData.Scan0, bytes.Length);

            bmp.UnlockBits(resData);
        }

        static public Bitmap ApplyFilter(this Bitmap bmp, Func<byte[], byte[]> filter)
        {
            byte[] buffer = bmp.GetBitmapDataBytes();
            byte[] result = filter(buffer);

            Bitmap bmpRes = new Bitmap(bmp.Width, bmp.Height);
            bmpRes.SetBitmapDataBytes(result);

            return bmpRes;
        }

        static public Bitmap ApplyFilter(this Bitmap bmp, Func<byte[], int, CustomKernel, byte[]> filter, CustomKernel customKernel)
        {
            byte[] buffer = bmp.GetBitmapDataBytes(out int stride);
            byte[] result = filter(buffer, stride, customKernel);

            Bitmap bmpRes = new Bitmap(bmp.Width, bmp.Height);
            bmpRes.SetBitmapDataBytes(result);

            return bmpRes;
        }
    }
}