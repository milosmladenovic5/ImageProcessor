using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMS_Lab.Model;
using MMS_Lab.View;
using System.Windows.Forms;
using static MMS_Lab.MainWindow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace MMS_Lab.UtilityLibraries
{
    public static class Filters
    {
        public static bool unsafeFilters { get; set; }

        public static void Contrast(int contrast, Bitmap image)
        {
            float contr = (100.0f + contrast) / 100.0f;
            contr *= contr;
            
            if (!Filters.unsafeFilters)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    for (int k = 0; k < image.Height; k++)
                    {
                        Color c = image.GetPixel(j, k);

                        float red = c.R / 255.0f;
                        float green = c.G / 255.0f;
                        float blue = c.B / 255.0f;

                        red = (((red - 0.5f) * contr) + 0.5f) * 255.0f;
                        green = (((green - 0.5f) * contr) + 0.5f) * 255.0f;
                        blue = (((blue - 0.5f) * contr) + 0.5f) * 255.0f;


                        image.SetPixel(j, k, Color.FromArgb(Clamp((int)red, 0, 255), Clamp((int)green, 0, 255), Clamp((int)blue, 0, 255)));
                    }
                }

            }
            else
            {
                BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
                int stride = bmData.Stride;

                unsafe
                {

                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = (byte*)bmData.Scan0 + (y * bmData.Stride);
                        int columnOffset = 0;

                        for (int x = 0; x < image.Width; ++x)
                        {
                            byte B = row[columnOffset];
                            byte G = row[columnOffset + 1];
                            byte R = row[columnOffset + 2];

                            float red = R / 255.0f;
                            float green = G / 255.0f;
                            float blue = B / 255.0f;

                            red = (((red - 0.5f) * contr) + 0.5f) * 255.0f;
                            green = (((green - 0.5f) * contr) + 0.5f) * 255.0f;
                            blue = (((blue - 0.5f) * contr) + 0.5f) * 255.0f;

                            row[columnOffset] = (byte)Clamp((int)blue, 0, 255);
                            row[columnOffset + 1] = (byte)Clamp((int)green, 0, 255);
                            row[columnOffset + 2] = (byte)Clamp((int)red, 0, 255);

                            columnOffset += 4;

                        }
                    }
                    image.UnlockBits(bmData);

                }
            }

        }

        public static void Brightness(int brightness, Bitmap image)
        {
            if (!Filters.unsafeFilters)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    for (int k = 0; k < image.Height; k++)
                    {
                        Color c = image.GetPixel(j, k);

                        int red = c.R + brightness;
                        int green = c.G + brightness;
                        int blue = c.B + brightness;

                        if (red < 0) red = 0;
                        if (red > 255) red = 255;

                        if (green < 0) green = 0;
                        if (green > 255) green = 255;

                        if (blue < 0) blue = 0;
                        if (blue > 255) blue = 255;

                        image.SetPixel(j, k, Color.FromArgb(red, green, blue));
                    }
                }
            }
            else
            {
                BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

                unsafe
                {
                    for (int y = 0; y < image.Height; ++y)
                    {
                        byte* row = (byte*)bmData.Scan0 + (y * bmData.Stride);
                        int columnOffset = 0;

                        for (int x = 0; x < image.Width; ++x)
                        {

                            byte B = row[columnOffset];
                            byte G = row[columnOffset + 1];
                            byte R = row[columnOffset + 2];

                            float red = R + brightness;
                            float green = G + brightness;
                            float blue = B + brightness;

                            if (red < 0) red = 0;
                            if (red > 255) red = 255;

                            if (green < 0) green = 0;
                            if (green > 255) green = 255;

                            if (blue < 0) blue = 0;
                            if (blue > 255) blue = 255;

                            row[columnOffset] = (byte)blue;
                            row[columnOffset + 1] = (byte)green;
                            row[columnOffset + 2] = (byte)red;

                            columnOffset += 4;

                        }
                    }
                }
                image.UnlockBits(bmData);
            }
        }

        public static void GaussianBlur(int blurAmount, Bitmap image)
        {
            ConvolutionMatrix3x3 Matrix = new ConvolutionMatrix3x3();
            Matrix.Apply(1);
            Matrix.Pixel = blurAmount;
            Matrix.TopMid = Matrix.MidLeft = Matrix.MidRight = Matrix.BottomMid = 2;
            Matrix.Factor = blurAmount + 12;

            // int[,] CM = this.model.getConvolutionMatrix();
            int Factor = Matrix.Factor;

            int TopLeft = Matrix.TopLeft;
            int TopMid = Matrix.TopMid;
            int TopRight = Matrix.TopRight;
            int MidLeft = Matrix.MidLeft;
            int MidRight = Matrix.MidRight;
            int BottomLeft = Matrix.BottomLeft;
            int BottomMid = Matrix.BottomMid;
            int BottomRight = Matrix.BottomRight;
            int Pixel = Matrix.Pixel;
            int Offset = Matrix.Offset;


                Bitmap TempBmp = (Bitmap)image.Clone();

                BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                    byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                    int Pix = 0;
                    int Stride = bmpData.Stride;
                    int DoubleStride = Stride * 2;
                    int Width = image.Width - 2;
                    int Height = image.Height - 2;
                    int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                    for (int y = 0; y < Height; ++y)
                        for (int x = 0; x < Width; ++x)
                        {
                            Pix = (((((TempPtr[2] * TopLeft) + (TempPtr[5] * TopMid) + (TempPtr[8] * TopRight)) +
                              ((TempPtr[2 + Stride] * MidLeft) + (TempPtr[5 + Stride] * Pixel) + (TempPtr[8 + Stride] * MidRight)) +
                              ((TempPtr[2 + DoubleStride] * BottomLeft) + (TempPtr[5 + DoubleStride] * BottomMid) + (TempPtr[8 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[5 + Stride] = (byte)Pix;

                            Pix = (((((TempPtr[1] * TopLeft) + (TempPtr[4] * TopMid) + (TempPtr[7] * TopRight)) +
                                  ((TempPtr[1 + Stride] * MidLeft) + (TempPtr[4 + Stride] * Pixel) + (TempPtr[7 + Stride] * MidRight)) +
                                  ((TempPtr[1 + DoubleStride] * BottomLeft) + (TempPtr[4 + DoubleStride] * BottomMid) + (TempPtr[7 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[4 + Stride] = (byte)Pix;

                            Pix = (((((TempPtr[0] * TopLeft) + (TempPtr[3] * TopMid) + (TempPtr[6] * TopRight)) +
                                  ((TempPtr[0 + Stride] * MidLeft) + (TempPtr[3 + Stride] * Pixel) + (TempPtr[6 + Stride] * MidRight)) +
                                  ((TempPtr[0 + DoubleStride] * BottomLeft) + (TempPtr[3 + DoubleStride] * BottomMid) + (TempPtr[6 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[3 + Stride] = (byte)Pix;

                            ptr += 3;
                            TempPtr += 3;
                        }
                }

                image.UnlockBits(bmpData);
                TempBmp.UnlockBits(TempBmpData);
        }

        public static void GaussianBlurWithoutCentralPixels(int blurAmount, Bitmap image)
        {
            Bitmap newImage = (Bitmap)image.Clone();

            ConvolutionMatrix3x3 Matrix = new ConvolutionMatrix3x3();
            Matrix.Apply(1);
            Matrix.Pixel = blurAmount;
            Matrix.TopMid = Matrix.MidLeft = Matrix.MidRight = Matrix.BottomMid = 2;
            Matrix.Factor = blurAmount + 12;

            int[,] CM = null;// this.model.getConvolutionMatrix();
            int Factor = Matrix.Factor;

            int TopLeft = Matrix.TopLeft;
            int TopMid = Matrix.TopMid;
            int TopRight = Matrix.TopRight;
            int MidLeft = Matrix.MidLeft;
            int MidRight = Matrix.MidRight;
            int BottomLeft = Matrix.BottomLeft;
            int BottomMid = Matrix.BottomMid;
            int BottomRight = Matrix.BottomRight;
            int Pixel = Matrix.Pixel;
            int Offset = Matrix.Offset;

            if (!Filters.unsafeFilters)
            {
                for (int j = 1; j < image.Width - 1; j++)
                {
                    for (int k = 1; k < image.Height - 1; k++)
                    {
                        Color c = image.GetPixel(j, k);

                        Color left = image.GetPixel(j - 1, k);
                        Color right = image.GetPixel(j + 1, k);
                        Color up = image.GetPixel(j, k - 1);
                        Color down = image.GetPixel(j, k + 1);
                        Color upLeft = image.GetPixel(j - 1, k - 1);
                        Color upRight = image.GetPixel(j + 1, k - 1);
                        Color downLeft = image.GetPixel(j - 1, k + 1);
                        Color downRight = image.GetPixel(j + 1, k + 1);

                        float red = (float)(c.R + left.R * CM[0, 0] + right.R * CM[0, 1] + up.R * CM[0, 2] + down.R * CM[1, 0] + upLeft.R * CM[1, 2] + upRight.R * CM[2, 0] + downLeft.R * CM[2, 1] + downRight.R * CM[2, 2]) / 9;
                        float green = (float)(c.G + left.G * CM[0, 0] + right.G * CM[0, 1] + up.G * CM[0, 2] + down.G * CM[1, 0] + upLeft.G * CM[1, 2] + upRight.G * CM[2, 0] + downLeft.G * CM[2, 1] + downRight.G * CM[2, 2]) / 9;
                        float blue = (float)(c.B + left.B * CM[0, 0] + right.B * CM[0, 1] + up.B * CM[0, 2] + down.B * CM[1, 0] + upLeft.B * CM[1, 2] + upRight.B * CM[2, 0] + downLeft.B * CM[2, 1] + downRight.B * CM[2, 2]) / 9;

                        newImage.SetPixel(j, k, Color.FromArgb(Clamp((int)red, 0, 255), Clamp((int)green, 0, 255), Clamp((int)blue, 0, 255)));
                    }
                }

            }
            else
            {
                Bitmap TempBmp = (Bitmap)image.Clone();

                BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                    byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                    int Pix = 0;
                    int Stride = bmpData.Stride;
                    int DoubleStride = Stride * 2;
                    int Width = image.Width - 2;
                    int Height = image.Height - 2;
                    int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                    for (int y = 0; y < Height; ++y)
                        for (int x = 0; x < Width; ++x)
                        {
                            Pix = (((((TempPtr[2] * TopLeft) + (TempPtr[5] * TopMid) + (TempPtr[8] * TopRight)) +
                              ((TempPtr[2 + Stride] * MidLeft) + (TempPtr[5 + Stride] * Pixel) + (TempPtr[8 + Stride] * MidRight)) +
                              ((TempPtr[2 + DoubleStride] * BottomLeft) + (TempPtr[5 + DoubleStride] * BottomMid) + (TempPtr[8 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[5 + Stride] = (byte)Pix;

                            Pix = (((((TempPtr[1] * TopLeft) + (TempPtr[4] * TopMid) + (TempPtr[7] * TopRight)) +
                                  ((TempPtr[1 + Stride] * MidLeft) + (TempPtr[4 + Stride] * Pixel) + (TempPtr[7 + Stride] * MidRight)) +
                                  ((TempPtr[1 + DoubleStride] * BottomLeft) + (TempPtr[4 + DoubleStride] * BottomMid) + (TempPtr[7 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[4 + Stride] = (byte)Pix;

                            Pix = (((((TempPtr[0] * TopLeft) + (TempPtr[3] * TopMid) + (TempPtr[6] * TopRight)) +
                                  ((TempPtr[0 + Stride] * MidLeft) + (TempPtr[3 + Stride] * Pixel) + (TempPtr[6 + Stride] * MidRight)) +
                                  ((TempPtr[0 + DoubleStride] * BottomLeft) + (TempPtr[3 + DoubleStride] * BottomMid) + (TempPtr[6 + DoubleStride] * BottomRight))) / Factor) + Offset);

                            if (Pix < 0) Pix = 0;
                            else if (Pix > 255) Pix = 255;

                            ptr[3 + Stride] = (byte)Pix;

                            ptr += 3;
                            TempPtr += 3;
                        }
                }

                image.UnlockBits(bmpData);
                TempBmp.UnlockBits(TempBmpData);
            }

        }

        public static void EdgeDetectHorizontal(Bitmap b)
        {
            Bitmap bmTemp = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bmTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 9;
                    p2 += 9;

                    for (int x = 9; x < nWidth - 9; ++x)
                    {
                        nPixel = ((p2 + stride - 9)[0] +
                            (p2 + stride - 6)[0] +
                            (p2 + stride - 3)[0] +
                            (p2 + stride)[0] +
                            (p2 + stride + 3)[0] +
                            (p2 + stride + 6)[0] +
                            (p2 + stride + 9)[0] -
                            (p2 - stride - 9)[0] -
                            (p2 - stride - 6)[0] -
                            (p2 - stride - 3)[0] -
                            (p2 - stride)[0] -
                            (p2 - stride + 3)[0] -
                            (p2 - stride + 6)[0] -
                            (p2 - stride + 9)[0]);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        (p + stride)[0] = (byte)nPixel;

                        ++p;
                        ++p2;
                    }

                    p += 9 + nOffset;
                    p2 += 9 + nOffset;
                }
            }

            b.UnlockBits(bmData);
            bmTemp.UnlockBits(bmData2);
        }

        public static void WaterFilter(Bitmap b, int nWave)
        {
            int nWidth = b.Width;
            int nHeight = b.Height;

            Point[,] fp = new Point[nWidth, nHeight];
            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double newX, newY;
            double xo, yo;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    xo = ((double)nWave * Math.Sin(2.0 *  Math.PI * (float)y / 128.0));
                    yo = ((double)nWave * Math.Cos(2.0 * Math.PI * (float)x / 128.0));

                    newX = (x + xo);
                    newY = (y + yo);

                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = (int)newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = 0;
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < nHeight)
                    {
                        fp[x, y].Y = (int)newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].Y = 0;
                        pt[x, y].Y = 0;
                    }
                }

            OffsetFilterAbs(b, pt);
        }

        public static bool OffsetFilterAbs(Bitmap b, Point[,] Offset)
        {
            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - b.Width * 3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = (int)Offset[x, y].X;
                        yOffset = (int)Offset[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public static void GaussianBlur3x3InPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {
            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            IntPtr Scan0 = bmData.Scan0;


            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 2;
                int nHeight = bitmap.Height - 2;

                float nPixel;


                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {

                        nPixel = ((((p[2] * cm.TopLeft) + (p[5] * cm.TopMid) + (p[8] * cm.TopRight) +
                              (p[2 + stride] * cm.MidLeft) + (p[5 + stride] * cm.Pixel) + (p[8 + stride] * cm.MidRight) +
                              (p[2 + stride2] * cm.BottomLeft) + (p[5 + stride2] * cm.BottomMid) + (p[8 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((p[1] * cm.TopLeft) + (p[4] * cm.TopMid) + (p[7] * cm.TopRight) +
                              (p[1 + stride] * cm.MidLeft) + (p[4 + stride] * cm.Pixel) + (p[7 + stride] * cm.MidRight) +
                              (p[1 + stride2] * cm.BottomLeft) + (p[4 + stride2] * cm.BottomMid) + (p[7 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((p[0] * cm.TopLeft) + (p[3] * cm.TopMid) + (p[6] * cm.TopRight) +
                              (p[0 + stride] * cm.MidLeft) + (p[3 + stride] * cm.Pixel) + (p[6 + stride] * cm.MidRight) +
                              (p[0 + stride2] * cm.BottomLeft) + (p[3 + stride2] * cm.BottomMid) + (p[6 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        if (x == 0 )
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if (x == nWidth)
                        {
                            p[6] = 255;
                            p[7] = 255;
                            p[8] = 255;
                        }

                        p += 3;

                    }
                    p += nOffset;

                }
            }
            bitmap.UnlockBits(bmData);


        }

        public static void GaussianBlur3x3OutOfPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {

            Bitmap bSrc = (Bitmap)bitmap.Clone();


            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            IntPtr Scan0 = bmData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 2;
                int nHeight = bitmap.Height - 2;

                float nPixel;


                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        #region UNFLIPPED KERNEL

                        nPixel = ((((pSrc[2] * cm.TopLeft) + (pSrc[5] * cm.TopMid) + (pSrc[8] * cm.TopRight) +
                            (pSrc[2 + stride] * cm.MidLeft) + (pSrc[5 + stride] * cm.Pixel) + (pSrc[8 + stride] * cm.MidRight) +
                            (pSrc[2 + stride2] * cm.BottomLeft) + (pSrc[5 + stride2] * cm.BottomMid) + (pSrc[8 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);
                        // crvena piksela iz prvog reda, crvena piksela iz drugog i treceg reda
                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * cm.TopLeft) + (pSrc[4] * cm.TopMid) + (pSrc[7] * cm.TopRight) +
                            (pSrc[1 + stride] * cm.MidLeft) + (pSrc[4 + stride] * cm.Pixel) + (pSrc[7 + stride] * cm.MidRight) +
                            (pSrc[1 + stride2] * cm.BottomLeft) + (pSrc[4 + stride2] * cm.BottomMid) + (pSrc[7 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * cm.TopLeft) + (pSrc[3] * cm.TopMid) + (pSrc[6] * cm.TopRight) +
                            (pSrc[0 + stride] * cm.MidLeft) + (pSrc[3 + stride] * cm.Pixel) + (pSrc[6 + stride] * cm.MidRight) +
                            (pSrc[0 + stride2] * cm.BottomLeft) + (pSrc[3 + stride2] * cm.BottomMid) + (pSrc[6 + stride2] * cm.BottomRight)) / cm.Factor) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;
                        #endregion

                        if (x == 0 )
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if( x== nWidth)
                        {
                            p[6] = 255;
                            p[7] = 255;
                            p[8] = 255;
                        }

                        p += 3;
                        pSrc += 3;
                    }


                    p += nOffset;
                    pSrc += nOffset;
                }
            }
            bitmap.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }

        public static void GaussianBlur5x5InPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {

            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


            int stride = bmData.Stride;
            int stride2 = stride * 2;
            int stride3 = stride * 3;
            int stride4 = stride * 4;
            IntPtr Scan0 = bmData.Scan0;


            unsafe
            {
                byte* p = (byte*)(void*)Scan0;


                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 4;
                int nHeight = bitmap.Height - 4;

                float nPixel;


                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {

                        nPixel = ((((p[2] * cm.TopLeft) + (p[5] * cm.TopLeft) + (p[8] * cm.TopMid) + (p[11] * cm.TopMid) + (p[14] * cm.TopRight) +
                                    (p[2 + stride] * cm.MidLeft) + (p[5 + stride] * cm.TopLeft) + (p[8 + stride] * cm.TopMid) + (p[11 + stride] * cm.TopRight) + (p[14 + stride] * cm.TopRight) +
                                    (p[2 + stride2] * cm.MidLeft) + (p[5 + stride2] * cm.MidLeft) + (p[8 + stride2] * cm.Pixel) + (p[11 + stride2] * cm.MidRight) + (p[14 + stride2] * cm.MidRight) +
                                    (p[2 + stride3] * cm.BottomLeft) + (p[5 + stride3] * cm.BottomLeft) + (p[8 + stride3] * cm.BottomMid) + (p[11 + stride3] * cm.BottomRight) + (p[14 + stride3] * cm.MidRight) +
                                    (p[2 + stride4] * cm.BottomLeft) + (p[5 + stride4] * cm.BottomMid) + (p[8 + stride4] * cm.BottomMid) + (p[11 + stride4] * cm.BottomRight) + (p[14 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[8 + stride2] = (byte)nPixel;

                        nPixel = ((((p[1] * cm.TopLeft) + (p[4] * cm.TopLeft) + (p[7] * cm.TopMid) + (p[10] * cm.TopMid) + (p[13] * cm.TopRight) +
                                    (p[1 + stride] * cm.MidLeft) + (p[4 + stride] * cm.TopLeft) + (p[7 + stride] * cm.TopMid) + (p[10 + stride] * cm.TopRight) + (p[13 + stride] * cm.TopRight) +
                                    (p[1 + stride2] * cm.MidLeft) + (p[4 + stride2] * cm.MidLeft) + (p[7 + stride2] * cm.Pixel) + (p[10 + stride2] * cm.MidRight) + (p[13 + stride2] * cm.MidRight) +
                                    (p[1 + stride3] * cm.BottomLeft) + (p[4 + stride3] * cm.BottomLeft) + (p[7 + stride3] * cm.BottomMid) + (p[10 + stride3] * cm.BottomRight) + (p[13 + stride3] * cm.MidRight) +
                                    (p[1 + stride4] * cm.BottomLeft) + (p[4 + stride4] * cm.BottomMid) + (p[7 + stride4] * cm.BottomMid) + (p[10 + stride4] * cm.BottomRight) + (p[13 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[7 + stride2] = (byte)nPixel;

                        nPixel = ((((p[0] * cm.TopLeft) + (p[3] * cm.TopLeft) + (p[6] * cm.TopMid) + (p[9] * cm.TopMid) + (p[12] * cm.TopRight) +
                                    (p[0 + stride] * cm.MidLeft) + (p[3 + stride] * cm.TopLeft) + (p[6 + stride] * cm.TopMid) + (p[9 + stride] * cm.TopRight) + (p[12 + stride] * cm.TopRight) +
                                    (p[0 + stride2] * cm.MidLeft) + (p[3 + stride2] * cm.MidLeft) + (p[6 + stride2] * cm.Pixel) + (p[9 + stride2] * cm.MidRight) + (p[12 + stride2] * cm.MidRight) +
                                    (p[0 + stride3] * cm.BottomLeft) + (p[3 + stride3] * cm.BottomLeft) + (p[6 + stride3] * cm.BottomMid) + (p[9 + stride3] * cm.BottomRight) + (p[12 + stride3] * cm.MidRight) +
                                    (p[0 + stride4] * cm.BottomLeft) + (p[3 + stride4] * cm.BottomMid) + (p[6 + stride4] * cm.BottomMid) + (p[9 + stride4] * cm.BottomRight) + (p[12 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[6 + stride2] = (byte)nPixel;

                        if (x == 0 )
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if (x == nWidth-1 )
                        {
                            p[9] = 255;
                            p[10] = 255;
                            p[11] = 255;
                        }

                        p += 3;

                    }
                    p += nOffset;

                }
            }
            bitmap.UnlockBits(bmData);

        }

        public static void GaussianBlur5x5OutOfPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {

            Bitmap bSrc = (Bitmap)bitmap.Clone();


            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            int stride3 = stride * 3;
            int stride4 = stride * 4;
            IntPtr Scan0 = bmData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 4;
                int nHeight = bitmap.Height - 4;

                float nPixel;


                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {

                        nPixel = ((((pSrc[2] * cm.TopLeft) + (pSrc[5] * cm.TopLeft) + (pSrc[8] * cm.TopMid) + (pSrc[11] * cm.TopMid) + (pSrc[14] * cm.TopRight) +
                                    (pSrc[2 + stride] * cm.MidLeft) + (pSrc[5 + stride] * cm.TopLeft) + (pSrc[8 + stride] * cm.TopMid) + (pSrc[11 + stride] * cm.TopRight) + (pSrc[14 + stride] * cm.TopRight) +
                                    (pSrc[2 + stride2] * cm.MidLeft) + (pSrc[5 + stride2] * cm.MidLeft) + (pSrc[8 + stride2] * cm.Pixel) + (pSrc[11 + stride2] * cm.MidRight) + (pSrc[14 + stride2] * cm.MidRight) +
                                    (pSrc[2 + stride3] * cm.BottomLeft) + (pSrc[5 + stride3] * cm.BottomLeft) + (pSrc[8 + stride3] * cm.BottomMid) + (pSrc[11 + stride3] * cm.BottomRight) + (pSrc[14 + stride3] * cm.MidRight) +
                                    (pSrc[2 + stride4] * cm.BottomLeft) + (pSrc[5 + stride4] * cm.BottomMid) + (pSrc[8 + stride4] * cm.BottomMid) + (pSrc[11 + stride4] * cm.BottomRight) + (pSrc[14 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[8 + stride2] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * cm.TopLeft) + (pSrc[4] * cm.TopLeft) + (pSrc[7] * cm.TopMid) + (pSrc[10] * cm.TopMid) + (pSrc[13] * cm.TopRight) +
                                    (pSrc[1 + stride] * cm.MidLeft) + (pSrc[4 + stride] * cm.TopLeft) + (pSrc[7 + stride] * cm.TopMid) + (pSrc[10 + stride] * cm.TopRight) + (pSrc[13 + stride] * cm.TopRight) +
                                    (pSrc[1 + stride2] * cm.MidLeft) + (pSrc[4 + stride2] * cm.MidLeft) + (pSrc[7 + stride2] * cm.Pixel) + (pSrc[10 + stride2] * cm.MidRight) + (pSrc[13 + stride2] * cm.MidRight) +
                                    (pSrc[1 + stride3] * cm.BottomLeft) + (pSrc[4 + stride3] * cm.BottomLeft) + (pSrc[7 + stride3] * cm.BottomMid) + (pSrc[10 + stride3] * cm.BottomRight) + (pSrc[13 + stride3] * cm.MidRight) +
                                    (pSrc[1 + stride4] * cm.BottomLeft) + (pSrc[4 + stride4] * cm.BottomMid) + (pSrc[7 + stride4] * cm.BottomMid) + (pSrc[10 + stride4] * cm.BottomRight) + (pSrc[13 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[7 + stride2] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * cm.TopLeft) + (pSrc[3] * cm.TopLeft) + (pSrc[6] * cm.TopMid) + (pSrc[9] * cm.TopMid) + (pSrc[12] * cm.TopRight) +
                                    (pSrc[0 + stride] * cm.MidLeft) + (pSrc[3 + stride] * cm.TopLeft) + (pSrc[6 + stride] * cm.TopMid) + (pSrc[9 + stride] * cm.TopRight) + (pSrc[12 + stride] * cm.TopRight) +
                                    (pSrc[0 + stride2] * cm.MidLeft) + (pSrc[3 + stride2] * cm.MidLeft) + (pSrc[6 + stride2] * cm.Pixel) + (pSrc[9 + stride2] * cm.MidRight) + (pSrc[12 + stride2] * cm.MidRight) +
                                    (pSrc[0 + stride3] * cm.BottomLeft) + (pSrc[3 + stride3] * cm.BottomLeft) + (pSrc[6 + stride3] * cm.BottomMid) + (pSrc[9 + stride3] * cm.BottomRight) + (pSrc[12 + stride3] * cm.MidRight) +
                                    (pSrc[0 + stride4] * cm.BottomLeft) + (pSrc[3 + stride4] * cm.BottomMid) + (pSrc[6 + stride4] * cm.BottomMid) + (pSrc[9 + stride4] * cm.BottomRight) + (pSrc[12 + stride4] * cm.BottomRight)) / cm.Factor5x5) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[6 + stride2] = (byte)nPixel;

                        if (x == 0 )
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if (x == nWidth - 1)
                        {
                            p[9] = 255;
                            p[10] = 255;
                            p[11] = 255;
                        }

                        p += 3;
                        pSrc += 3;
                    }
                    p += nOffset;
                    pSrc += nOffset;
                }
            }
            bitmap.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }

        public static void GaussianBlur7x7OutOfPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {

            Bitmap bSrc = (Bitmap)bitmap.Clone();


            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            int stride3 = stride * 3;
            int stride4 = stride * 4;
            int stride5 = stride * 5;
            int stride6 = stride * 6;
            IntPtr Scan0 = bmData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 6;
                int nHeight = bitmap.Height - 6;

                float nPixel;
                //int c; // color startPos


                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        //c = 2;

                        nPixel = ((((pSrc[2] * cm.TopLeft) + (pSrc[5] * cm.TopLeft) + (pSrc[11] * cm.TopMid) + (pSrc[14] * cm.TopMid) + (pSrc[20] * cm.TopRight) +
                                    /* prvi nula */ (pSrc[5 + stride] * cm.TopLeft) + (pSrc[8 + stride] * cm.TopLeft) + (pSrc[11 + stride] * cm.TopMid) + (pSrc[14 + stride] * cm.TopMid) + (pSrc[17 + stride] * cm.TopRight) + (pSrc[20 + stride] * cm.TopRight) +
                                    (pSrc[2 + stride2] * cm.MidLeft) + (pSrc[5 + stride2] * cm.MidLeft) + (pSrc[8 + stride2] * cm.TopLeft) + (pSrc[11 + stride2] * cm.TopMid) + (pSrc[14 + stride2] * cm.TopRight) + (pSrc[17 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (pSrc[2 + stride3] * cm.MidLeft) + (pSrc[5 + stride3] * cm.MidLeft) + (pSrc[8 + stride3] * cm.MidLeft) + (pSrc[11 + stride3] * cm.Pixel) + (pSrc[14 + stride3] * cm.MidRight) + (pSrc[17 + stride3] * cm.MidRight) + (pSrc[20 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (pSrc[5 + stride4] * cm.BottomLeft) + (pSrc[8 + stride4] * cm.BottomLeft) + (pSrc[11 + stride4] * cm.BottomMid) + (pSrc[14 + stride4] * cm.BottomRight) + (pSrc[17 + stride4] * cm.MidRight) + (pSrc[20 + stride4] * cm.MidRight) +
                                    (pSrc[2 + stride5] * cm.BottomLeft) + (pSrc[5 + stride5] * cm.BottomLeft) + (pSrc[8 + stride5] * cm.BottomMid) + (pSrc[11 + stride5] * cm.BottomMid) + (pSrc[14 + stride5] * cm.BottomRight) + (pSrc[17 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (pSrc[2 + stride6] * cm.BottomLeft)  /*(pSrc[5 + stride6] * 0)*/ + (pSrc[8 + stride6] * cm.BottomMid) + (pSrc[11 + stride6] * cm.BottomMid) /*+ (pSrc[14 + stride6] * 0)*/ + (pSrc[17 + stride6] * cm.BottomRight) + (pSrc[20 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[11 + stride4] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * cm.TopLeft) + (pSrc[4] * cm.TopLeft) + (pSrc[10] * cm.TopMid) + (pSrc[13] * cm.TopMid) + (pSrc[19] * cm.TopRight) +
                                    /* prvi nula */ (pSrc[4 + stride] * cm.TopLeft) + (pSrc[7 + stride] * cm.TopLeft) + (pSrc[10 + stride] * cm.TopMid) + (pSrc[13 + stride] * cm.TopMid) + (pSrc[16 + stride] * cm.TopRight) + (pSrc[19 + stride] * cm.TopRight) +
                                    (pSrc[1 + stride2] * cm.MidLeft) + (pSrc[4 + stride2] * cm.MidLeft) + (pSrc[7 + stride2] * cm.TopLeft) + (pSrc[10 + stride2] * cm.TopMid) + (pSrc[13 + stride2] * cm.TopRight) + (pSrc[16 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (pSrc[1 + stride3] * cm.MidLeft) + (pSrc[4 + stride3] * cm.MidLeft) + (pSrc[7 + stride3] * cm.MidLeft) + (pSrc[10 + stride3] * cm.Pixel) + (pSrc[13 + stride3] * cm.MidRight) + (pSrc[16 + stride3] * cm.MidRight) + (pSrc[19 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (pSrc[4 + stride4] * cm.BottomLeft) + (pSrc[7 + stride4] * cm.BottomLeft) + (pSrc[10 + stride4] * cm.BottomMid) + (pSrc[13 + stride4] * cm.BottomRight) + (pSrc[16 + stride4] * cm.MidRight) + (pSrc[19 + stride4] * cm.MidRight) +
                                    (pSrc[1 + stride5] * cm.BottomLeft) + (pSrc[4 + stride5] * cm.BottomLeft) + (pSrc[7 + stride5] * cm.BottomMid) + (pSrc[10 + stride5] * cm.BottomMid) + (pSrc[13 + stride5] * cm.BottomRight) + (pSrc[16 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (pSrc[1 + stride6] * cm.BottomLeft)  /*(pSrc[4 + stride6] * 0)*/ + (pSrc[7 + stride6] * cm.BottomMid) + (pSrc[10 + stride6] * cm.BottomMid) /*+ (pSrc[13 + stride6] * 0)*/ + (pSrc[16 + stride6] * cm.BottomRight) + (pSrc[19 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[10 + stride4] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * cm.TopLeft) + (pSrc[3] * cm.TopLeft) + (pSrc[9] * cm.TopMid) + (pSrc[12] * cm.TopMid) + (pSrc[18] * cm.TopRight) +
                                    /* prvi nula */ (pSrc[3 + stride] * cm.TopLeft) + (pSrc[6 + stride] * cm.TopLeft) + (pSrc[9 + stride] * cm.TopMid) + (pSrc[12 + stride] * cm.TopMid) + (pSrc[15 + stride] * cm.TopRight) + (pSrc[18 + stride] * cm.TopRight) +
                                    (pSrc[0 + stride2] * cm.MidLeft) + (pSrc[3 + stride2] * cm.MidLeft) + (pSrc[6 + stride2] * cm.TopLeft) + (pSrc[9 + stride2] * cm.TopMid) + (pSrc[12 + stride2] * cm.TopRight) + (pSrc[15 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (pSrc[0 + stride3] * cm.MidLeft) + (pSrc[3 + stride3] * cm.MidLeft) + (pSrc[6 + stride3] * cm.MidLeft) + (pSrc[9 + stride3] * cm.Pixel) + (pSrc[12 + stride3] * cm.MidRight) + (pSrc[15 + stride3] * cm.MidRight) + (pSrc[18 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (pSrc[3 + stride4] * cm.BottomLeft) + (pSrc[6 + stride4] * cm.BottomLeft) + (pSrc[9 + stride4] * cm.BottomMid) + (pSrc[12 + stride4] * cm.BottomRight) + (pSrc[15 + stride4] * cm.MidRight) + (pSrc[18 + stride4] * cm.MidRight) +
                                    (pSrc[0 + stride5] * cm.BottomLeft) + (pSrc[3 + stride5] * cm.BottomLeft) + (pSrc[6 + stride5] * cm.BottomMid) + (pSrc[9 + stride5] * cm.BottomMid) + (pSrc[12 + stride5] * cm.BottomRight) + (pSrc[15 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (pSrc[0 + stride6] * cm.BottomLeft)  /*(pSrc[3 + stride6] * 0)*/ + (pSrc[6 + stride6] * cm.BottomMid) + (pSrc[9 + stride6] * cm.BottomMid) /*+ (pSrc[13 + stride6] * 0)*/ + (pSrc[15 + stride6] * cm.BottomRight) + (pSrc[18 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[9 + stride4] = (byte)nPixel;

                        if (x == 0)
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if (x == nWidth - 1 )
                        {
                            p[14] = 255;
                            p[15] = 255;
                            p[16] = 255;
                        }

                        p += 3;
                        pSrc += 3;

                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }
            bitmap.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }

        public static void GaussianBlur7x7InPlaceUnsafe(Bitmap bitmap, ConvolutionMatrix3x3 cm)
        {

            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


            int stride = bmData.Stride;
            int stride2 = stride * 2;
            int stride3 = stride * 3;
            int stride4 = stride * 4;
            int stride5 = stride * 5;
            int stride6 = stride * 6;
            IntPtr Scan0 = bmData.Scan0;


            unsafe
            {
                byte* p = (byte*)(void*)Scan0;


                int nOffset = stride - bitmap.Width * 3;
                int nWidth = bitmap.Width - 6;
                int nHeight = bitmap.Height - 6;

                float nPixel;



                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        //c = 2;

                        nPixel = ((((p[2] * cm.TopLeft) + (p[5] * cm.TopLeft) + (p[11] * cm.TopMid) + (p[14] * cm.TopMid) + (p[20] * cm.TopRight) +
                                    /* prvi nula */ (p[5 + stride] * cm.TopLeft) + (p[8 + stride] * cm.TopLeft) + (p[11 + stride] * cm.TopMid) + (p[14 + stride] * cm.TopMid) + (p[17 + stride] * cm.TopRight) + (p[20 + stride] * cm.TopRight) +
                                    (p[2 + stride2] * cm.MidLeft) + (p[5 + stride2] * cm.MidLeft) + (p[8 + stride2] * cm.TopLeft) + (p[11 + stride2] * cm.TopMid) + (p[14 + stride2] * cm.TopRight) + (p[17 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (p[2 + stride3] * cm.MidLeft) + (p[5 + stride3] * cm.MidLeft) + (p[8 + stride3] * cm.MidLeft) + (p[11 + stride3] * cm.Pixel) + (p[14 + stride3] * cm.MidRight) + (p[17 + stride3] * cm.MidRight) + (p[20 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (p[5 + stride4] * cm.BottomLeft) + (p[8 + stride4] * cm.BottomLeft) + (p[11 + stride4] * cm.BottomMid) + (p[14 + stride4] * cm.BottomRight) + (p[17 + stride4] * cm.MidRight) + (p[20 + stride4] * cm.MidRight) +
                                    (p[2 + stride5] * cm.BottomLeft) + (p[5 + stride5] * cm.BottomLeft) + (p[8 + stride5] * cm.BottomMid) + (p[11 + stride5] * cm.BottomMid) + (p[14 + stride5] * cm.BottomRight) + (p[17 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (p[2 + stride6] * cm.BottomLeft)  /*(p[5 + stride6] * 0)*/ + (p[8 + stride6] * cm.BottomMid) + (p[11 + stride6] * cm.BottomMid) /*+ (p[14 + stride6] * 0)*/ + (p[17 + stride6] * cm.BottomRight) + (p[20 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[11 + stride4] = (byte)nPixel;

                        nPixel = ((((p[1] * cm.TopLeft) + (p[4] * cm.TopLeft) + (p[10] * cm.TopMid) + (p[13] * cm.TopMid) + (p[19] * cm.TopRight) +
                                    /* prvi nula */ (p[4 + stride] * cm.TopLeft) + (p[7 + stride] * cm.TopLeft) + (p[10 + stride] * cm.TopMid) + (p[13 + stride] * cm.TopMid) + (p[16 + stride] * cm.TopRight) + (p[19 + stride] * cm.TopRight) +
                                    (p[1 + stride2] * cm.MidLeft) + (p[4 + stride2] * cm.MidLeft) + (p[7 + stride2] * cm.TopLeft) + (p[10 + stride2] * cm.TopMid) + (p[13 + stride2] * cm.TopRight) + (p[16 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (p[1 + stride3] * cm.MidLeft) + (p[4 + stride3] * cm.MidLeft) + (p[7 + stride3] * cm.MidLeft) + (p[10 + stride3] * cm.Pixel) + (p[13 + stride3] * cm.MidRight) + (p[16 + stride3] * cm.MidRight) + (p[19 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (p[4 + stride4] * cm.BottomLeft) + (p[7 + stride4] * cm.BottomLeft) + (p[10 + stride4] * cm.BottomMid) + (p[13 + stride4] * cm.BottomRight) + (p[16 + stride4] * cm.MidRight) + (p[19 + stride4] * cm.MidRight) +
                                    (p[1 + stride5] * cm.BottomLeft) + (p[4 + stride5] * cm.BottomLeft) + (p[7 + stride5] * cm.BottomMid) + (p[10 + stride5] * cm.BottomMid) + (p[13 + stride5] * cm.BottomRight) + (p[16 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (p[1 + stride6] * cm.BottomLeft)  /*(p[4 + stride6] * 0)*/ + (p[7 + stride6] * cm.BottomMid) + (p[10 + stride6] * cm.BottomMid) /*+ (p[13 + stride6] * 0)*/ + (p[16 + stride6] * cm.BottomRight) + (p[19 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[10 + stride4] = (byte)nPixel;

                        nPixel = ((((p[0] * cm.TopLeft) + (p[3] * cm.TopLeft) + (p[9] * cm.TopMid) + (p[12] * cm.TopMid) + (p[18] * cm.TopRight) +
                                    /* prvi nula */ (p[3 + stride] * cm.TopLeft) + (p[6 + stride] * cm.TopLeft) + (p[9 + stride] * cm.TopMid) + (p[12 + stride] * cm.TopMid) + (p[15 + stride] * cm.TopRight) + (p[18 + stride] * cm.TopRight) +
                                    (p[0 + stride2] * cm.MidLeft) + (p[3 + stride2] * cm.MidLeft) + (p[6 + stride2] * cm.TopLeft) + (p[9 + stride2] * cm.TopMid) + (p[12 + stride2] * cm.TopRight) + (p[15 + stride2] * cm.TopRight) + /* zadnji nula */
                                    (p[0 + stride3] * cm.MidLeft) + (p[3 + stride3] * cm.MidLeft) + (p[6 + stride3] * cm.MidLeft) + (p[9 + stride3] * cm.Pixel) + (p[12 + stride3] * cm.MidRight) + (p[15 + stride3] * cm.MidRight) + (p[18 + stride3] * cm.MidRight) +
                                    /*prvi nula */ (p[3 + stride4] * cm.BottomLeft) + (p[6 + stride4] * cm.BottomLeft) + (p[9 + stride4] * cm.BottomMid) + (p[12 + stride4] * cm.BottomRight) + (p[15 + stride4] * cm.MidRight) + (p[18 + stride4] * cm.MidRight) +
                                    (p[0 + stride5] * cm.BottomLeft) + (p[3 + stride5] * cm.BottomLeft) + (p[6 + stride5] * cm.BottomMid) + (p[9 + stride5] * cm.BottomMid) + (p[12 + stride5] * cm.BottomRight) + (p[15 + stride5] * cm.BottomRight) + /* zadnji nula */
                                    (p[0 + stride6] * cm.BottomLeft)  /*(p[3 + stride6] * 0)*/ + (p[6 + stride6] * cm.BottomMid) + (p[9 + stride6] * cm.BottomMid) /*+ (p[13 + stride6] * 0)*/ + (p[15 + stride6] * cm.BottomRight) + (p[18 + stride6] * cm.BottomRight)) / cm.Factor7x7) + cm.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                
                        p[9 + stride4] = (byte)nPixel;

                        if (x == 0)
                        {
                            p[0] = 255;
                            p[1] = 255;
                            p[2] = 255;
                        }
                        else if (x == nWidth - 1)
                        {
                            p[14] = 255;
                            p[15] = 255;
                            p[16] = 255;
                        }

                        p += 3;

                    }
                    p += nOffset;

                }
            }
            bitmap.UnlockBits(bmData);

        }

        public static void ShiftedAndScaledUnsafe(Bitmap bmp, float shift, float scale)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            int height = bmp.Height;
            int width = bmp.Width;

            unsafe
            {
                for (int y = 0; y < height; ++y)
                {
                    byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
                    int collumnOffset = 0;

                    for (int x = 0; x < width; ++x)
                    {
                        byte R = row[collumnOffset];
                        byte G = row[collumnOffset + 1];
                        byte B = row[collumnOffset + 2];

                        float Y = (byte)((0.299 * R) + (0.587 * G) + (0.114 * B));
                        float Cb = (byte)(128 - (0.168736 * R) + (0.331264 * G) + (0.5 * B));
                        float Cr = (byte)(128 + (0.5 * R) + (0.418688 * G) + (0.081312 * B));

                        Y = (Y + shift) * scale;
                        Cb = (Y + shift) * scale;
                        Cr = (Y + shift) * scale;

                        row[collumnOffset] = (byte)Clamp((int)Y, 0, 255);
                        row[collumnOffset + 1] = (byte)Clamp((int) Cb, 0, 255);
                        row[collumnOffset + 2] = (byte)Clamp((int)Cr, 0, 255);

                        collumnOffset += 4;
                    }
                }
            }

            bmp.UnlockBits(bmpData);
        }

        public static void EdgeDetectPrewittUnsafe(Bitmap b, byte nThreshold)
        {
            Bitmap bTemp = (Bitmap)b.Clone();

            ConvolutionMatrix3x3 m = new ConvolutionMatrix3x3();
            m.Apply(0);
            m.TopLeft = m.MidLeft = m.BottomLeft = -1;
            m.TopRight = m.MidRight = m.BottomRight = 1;
            m.Offset = 0;

            Conv3x3(b, m);
            Conv3x3(bTemp, m);

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = (int)Math.Sqrt((p[0] * p[0]) + (p2[0] * p2[0]));
                        if (nPixel < nThreshold) nPixel = nThreshold;
                        if (nPixel > 255) nPixel = 255;
                        p[0] = (byte)nPixel;
                        ++p;
                        ++p2;
                    }
                    p += nOffset;
                    p2 += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bTemp.UnlockBits(bmData2);
        }

        public static void ToGrayscale(Bitmap b)
        {
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        byte val = (byte)(p[0] * 0.11 + p[1] * 0.59 + p[2] * 0.3);
                        p[0] = val;
                        p[1] = val;
                        p[2] = val;

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            return;
        }


    public static bool Conv3x3(Bitmap b, ConvolutionMatrix3x3 m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }
                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public static void ExtractChannels(Bitmap mainBmp, Bitmap firstChannel, Bitmap secondChannel, Bitmap thirdChannel)
        {
            
            if (Filters.unsafeFilters == false)
            {
                for (int i = 0; i < mainBmp.Width; i++)
                {
                    for (int j = 0; j < mainBmp.Height; j++)
                    {
                        Color c = mainBmp.GetPixel(i, j);

                        double yComp = 0 + 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
                        double cbComp = 128 - 0.168736 * c.R - 0.331264 * c.G + 0.5 * c.B;
                        double crComp = 128 + 0.5 * c.R - 0.41886 * c.G - 0.081312 * c.B;

                        firstChannel.SetPixel(i, j, Color.FromArgb((int)yComp, (int)yComp, (int)yComp));
                        secondChannel.SetPixel(i, j, Color.FromArgb((int)yComp, (int)cbComp, 0));
                        thirdChannel.SetPixel(i, j, Color.FromArgb((int)cbComp, 255 - (int)crComp, (int)crComp));
                    }
                }

            }
            else
            {
                //ovde ide unsafe kod za pretvaranje iz RGB u YCbCr
                BitmapData bmData = mainBmp.LockBits(new Rectangle(0, 0, mainBmp.Width, mainBmp.Height), ImageLockMode.ReadWrite, mainBmp.PixelFormat);
                int stride = bmData.Stride;

                BitmapData firstData = firstChannel.LockBits(new Rectangle(0, 0, firstChannel.Width, firstChannel.Height), ImageLockMode.ReadWrite, firstChannel.PixelFormat);
                BitmapData secondData = secondChannel.LockBits(new Rectangle(0, 0, secondChannel.Width, secondChannel.Height), ImageLockMode.ReadWrite, secondChannel.PixelFormat);
                BitmapData thirdData = thirdChannel.LockBits(new Rectangle(0, 0, thirdChannel.Width, thirdChannel.Height), ImageLockMode.ReadWrite, thirdChannel.PixelFormat);

                unsafe
                {
                    for (int y = 0; y < mainBmp.Height; ++y)
                    {
                        byte* row0 = (byte*)bmData.Scan0 + (y * bmData.Stride);
                        byte* row1 = (byte*)firstData.Scan0 + (y * firstData.Stride);
                        byte* row2 = (byte*)secondData.Scan0 + (y * secondData.Stride);
                        byte* row3 = (byte*)thirdData.Scan0 + (y * thirdData.Stride);

                        int columnOffset = 0;

                        for (int x = 0; x < mainBmp.Width; ++x)
                        {

                            byte blue = row0[columnOffset];
                            byte green = row0[columnOffset + 1];
                            byte red = row0[columnOffset + 2];

                            row1[columnOffset] = row1[columnOffset + 1] = row1[columnOffset + 2] = (byte)((0.299 * blue) + (0.587 * green) + (0.114 * red));
                            row2[columnOffset] = row2[columnOffset + 1] = row2[columnOffset + 2] = (byte)(128 - (0.168736 * blue) + (0.331264 * green) + (0.5 * red));
                            row3[columnOffset] = row3[columnOffset + 1] = row3[columnOffset + 2] = (byte)(128 + (0.5 * blue) + (0.418688 * green) + (0.081312 * red));

                            columnOffset += 4;
                        }

                    }
                }
                mainBmp.UnlockBits(bmData);
                firstChannel.UnlockBits(firstData);
                secondChannel.UnlockBits(secondData);
                thirdChannel.UnlockBits(thirdData);
            }

        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

    }
}
