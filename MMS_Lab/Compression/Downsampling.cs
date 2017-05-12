using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.Compression
{

        public class DownsampleFormat
        {
            public int bmpStride;
            public int bmpWidth;
            public int bmpHeight;
            public int Ylen;
            public int Cblen;
            public int Crlen;
            public int code;
            public byte[] data;

            public DownsampleFormat(int bmpStride, int bmpWidth, int bmpHeight, int Ylen, int Cblen, int Crlen, int code)
            {
                this.bmpStride = bmpStride;
                this.bmpWidth = bmpWidth;
                this.bmpHeight = bmpHeight;
                this.Ylen = Ylen;
                this.Cblen = Cblen;
                this.Crlen = Crlen;
                this.code = code;
            }

        }
        public class Downsampling
        {
            private byte[] YData;
            private byte[] CbData;
            private byte[] CrData;
            private int bmpStride;
            private int bmpWidth;
            private int bmpHeight;

            public Downsampling() { }
            public Downsampling(Bitmap bitmap)
            {
                BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = bmData.Stride;
                int size = stride * bmData.Height;

                this.bmpStride = stride;
                this.bmpWidth = bitmap.Width;
                this.bmpHeight = bitmap.Height;

                #region ExtractData

                byte[] data = new byte[size];
                this.YData = new byte[size / 3]; 
                this.CbData = new byte[size / 3];
                this.CrData = new byte[size / 3];

                Marshal.Copy(bmData.Scan0, data, 0, size);

                int j = 0; 
                for (int i = 0; i < size; i += 3)
                {
                    double Y = ((0.299 * data[i + 2]) + (0.587 * data[i + 1]) + (0.114 * data[i]));
                    double Cb = (128 - (0.168736 * data[i + 2]) + (0.331264 * data[i + 1]) + (0.5 * data[i]));
                    double Cr = (128 + (0.5 * data[i + 2]) + (0.418688 * data[i + 1]) + (0.081312 * data[i]));

                    this.YData[j] = (byte)Y;
                    this.CbData[j] = (byte)Cb;
                    this.CrData[j] = (byte)Cr;
                    j++;
                }

                bitmap.UnlockBits(bmData);
                #endregion
            }

            public DownsampleFormat Downsample(int code)
            {
                // BGR
                byte[] YTemp;
                byte[] CbTemp;
                byte[] CrTemp;

                if (code == 1) // downsample Cb, Cr
                {
                    YTemp = this.YData;
                    CbTemp = DownsampleChannel(this.CbData);
                    CrTemp = DownsampleChannel(this.CrData);
                }
                else if (code == 2)
                {
                    YTemp = DownsampleChannel(this.YData);
                    CbTemp = this.CbData;
                    CrTemp = DownsampleChannel(this.CrData);
                }
                else
                {
                    YTemp = DownsampleChannel(this.YData);
                    CbTemp = DownsampleChannel(this.CbData);
                    CrTemp = this.CrData;
                }


                int Ylen = YTemp.Length;
                int Cblen = CbTemp.Length;
                int Crlen = CrTemp.Length;

                DownsampleFormat result = new DownsampleFormat(this.bmpStride, this.bmpWidth, this.bmpHeight, Ylen, Cblen, Crlen, code);
                result.data = new byte[Ylen + Cblen + Crlen];
                Array.Copy(YTemp, 0, result.data, 0, Ylen);
                Array.Copy(CbTemp, 0, result.data, Ylen, Cblen);
                Array.Copy(CrTemp, 0, result.data, Ylen + Cblen, Crlen);
                return result;

            }
            public byte[] DownsampleChannel(byte[] channel)
            {
                List<byte> result = new List<byte>();
                int check = 0;
                int checkLine = 0;
                for (int i = 0; i < channel.Length; i++)
                {

                    if (check == 2 || check == 3)
                    {

                        if (check == 3) check = 0;
                        if (check == 2) check++;
                        continue;
                    }

                    result.Add(channel[i]);
                    check++;
                    checkLine++;
                    if (checkLine == this.bmpStride * 2)
                    {
                        if (check != 0)
                            check = 3;
                        else
                            check = 0;
                        checkLine = 0;
                    }
                }
                return result.ToArray();
            }

            public Bitmap RestoreBitmap(DownsampleFormat df)
            {
                if (df.code == 1)
                    return RestoreCbCr(df);
                else if (df.code == 2)
                    return RestoreYCr(df);
                else
                    return RestoreYCb(df);
            }

            public Bitmap RestoreCbCr(DownsampleFormat df)
            {
                Bitmap result = new Bitmap(df.bmpWidth, df.bmpHeight);
                byte[] data = new byte[df.bmpHeight * df.bmpStride];

                int check = 0;
                int checkLine = 0;
                int cb = df.Cblen;
                int cr = df.Crlen;
                byte c1, c0;

                int j = 0;
                for (int i = 0; i < data.Length; i += 3)
                {
                    if (check == 2 || check == 3)
                    {
                        c1 = 0;
                        c0 = 0;
                        if (check == 3) check = 0;
                    }
                    else
                    {
                        c1 = df.data[j + df.Cblen];
                        c0 = df.data[j + df.Cblen + df.Crlen];
                    }

                    data[i + 2] = df.data[j];
                    data[i + 1] = c1;
                    data[i] = c0;
                    check++;
                    checkLine += 3;
                    j++;
                    if (checkLine == df.bmpWidth * 2)
                    {
                        if (check != 0)
                            check = 3;
                        else
                            check = 0;
                        checkLine = 0;
                    }
                }

                BitmapData bmData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Marshal.Copy(data, 0, bmData.Scan0, data.Length);

                result.UnlockBits(bmData);
                return result;
            }
            public Bitmap RestoreYCr(DownsampleFormat df)
            {
                Bitmap result = new Bitmap(df.bmpWidth, df.bmpHeight);
                byte[] data = new byte[df.bmpHeight * df.bmpStride];

                int check = 0;
                int checkLine = 0;
                byte c2, c0;

                int j = df.Ylen;
                for (int i = 0; i < data.Length; i += 3)
                {
                    if (check == 2 || check == 3)
                    {
                        c2 = 0;
                        c0 = 0;
                        if (check == 3) check = 0;
                    }
                    else
                    {
                        c2 = df.data[j - df.Ylen];
                        c0 = df.data[j + df.Crlen];
                    }

                    data[i + 2] = c2;
                    data[i + 1] = df.data[j];
                    data[i] = c0;
                    check++;
                    checkLine += 3;
                    j++;
                    if (checkLine == df.bmpWidth * 2)
                    {
                        if (check != 0)
                            check = 3;
                        else
                            check = 0;
                        checkLine = 0;
                    }
                }

                BitmapData bmData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Marshal.Copy(data, 0, bmData.Scan0, data.Length);

                result.UnlockBits(bmData);
                return result;
            }

            public Bitmap RestoreYCb(DownsampleFormat df)
            {
                Bitmap result = new Bitmap(df.bmpWidth, df.bmpHeight);
                byte[] data = new byte[df.bmpHeight * df.bmpStride];

                int check = 0;
                int checkLine = 0;
                byte c2, c1;

                int j = df.Ylen + df.Cblen;
                for (int i = 0; i < data.Length; i += 3)
                {
                    if (check == 2 || check == 3)
                    {
                        c2 = 0;
                        c1 = 0;
                        if (check == 3) check = 0;
                    }
                    else
                    {
                        c2 = df.data[j - df.Cblen - df.Ylen];
                        c1 = df.data[j - df.Cblen];
                    }

                    data[i + 2] = c2;
                    data[i + 1] = c1;
                    data[i] = df.data[j];
                    check++;
                    checkLine += 3;
                    j++;
                    if (checkLine == df.bmpWidth * 2)
                    {
                        if (check != 0)
                            check = 3;
                        else
                            check = 0;
                        checkLine = 0;
                    }
                }

                BitmapData bmData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Marshal.Copy(data, 0, bmData.Scan0, data.Length);

                result.UnlockBits(bmData);
                return result;
            }
        }
    }

