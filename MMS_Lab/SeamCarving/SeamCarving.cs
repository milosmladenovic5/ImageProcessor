using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using MMS_Lab.UtilityLibraries;

namespace MMS_Lab.SeamCarving
{
    public class SeamCarving
    {
        Bitmap bmp;
        Bitmap edgeDetectedBmp;
        List<PixelPos> path;
        int width;
        int height;
        int[,] energyMap;

        Color[,] imgColors;

         public SeamCarving(Bitmap img)
         {
            this.bmp = (Bitmap)img.Clone();
            this.width = img.Width;
            this.height = img.Height;

            this.edgeDetectedBmp = (Bitmap)img.Clone();
            Filters.ToGrayscale(edgeDetectedBmp);
            Filters.EdgeDetectHorizontal(edgeDetectedBmp);

            this.PopulateColorMatrix();
            this.FillEnergyMap();

            int minPos = GetBottomRowMinYPosition();
            this.PopulatePath(minPos);
            this.DrawPath();

          }

        public Bitmap GetChangedBmp()
        {
            return this.edgeDetectedBmp;
        }

        private void DrawPath()
        {
            BitmapData bmData = this.edgeDetectedBmp.LockBits(new Rectangle(0, 0, edgeDetectedBmp.Width, edgeDetectedBmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            int stride = bmData.Stride;
            int size = stride * bmData.Height;

            PixelPos[] positions = this.path.ToArray();
            int len = positions.Length;

            unsafe
            {

                for (int y = 0; y < bmp.Height; ++y)
                {
                    byte* row = (byte*)bmData.Scan0 + (y * bmData.Stride);
                    int columnOffset = 0;

                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        if (positions[len - 1 - y].y == x)
                        {
                            row[columnOffset] = 0;
                            row[columnOffset + 1] = 0;
                            row[columnOffset + 2] = 255;
                        }
                        columnOffset += 4;
                    }
                }
            }
            edgeDetectedBmp.UnlockBits(bmData);

        }


        private void PopulateColorMatrix()
        {
            BitmapData bmData = this.edgeDetectedBmp.LockBits(new Rectangle(0, 0, edgeDetectedBmp.Width, edgeDetectedBmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            int stride = bmData.Stride;
            int size = stride * bmData.Height;


            this.energyMap = new int[stride, bmData.Height];
            this.imgColors = new Color[width, height];

            unsafe
            {

                for (int y = 0; y < bmp.Height; ++y)
                {
                    byte* row = (byte*)bmData.Scan0 + (y * bmData.Stride);
                    int columnOffset = 0;

                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        this.imgColors[x, y] = Color.FromArgb(1, R, G, B);
                        columnOffset += 4;
                    }
                }
            }
            edgeDetectedBmp.UnlockBits(bmData);
        }

        private void FillEnergyMap()
        {           
            int width = imgColors.GetLength(0);
            int height = imgColors.GetLength(1);

            for ( int i=0; i<width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int val = 0;
                    if (j == 0)
                        val += ColorDist(imgColors[i, j], imgColors[i, j + 1]);
                    else if (j == height - 1)
                        val += ColorDist(imgColors[i, j], imgColors[i, j - 1]);
                    else
                        val += ColorDist(imgColors[i, j - 1], imgColors[i, j + 1]);

                    if (i == 0)
                        val += ColorDist(imgColors[i, j], imgColors[i + 1, j]);
                    else if (i == width - 1)
                        val += ColorDist(imgColors[i, j], imgColors[i - 1, j]);
                    else
                        val += ColorDist(imgColors[i - 1, j], imgColors[i + 1, j]);

                    energyMap[i, j] = val;
                }
            }
        }

        private int ColorDist(Color A, Color B)
        {
            int red = (A.R - B.R);
            int green = (A.G - B.G);
            int blue = (A.B - B.B);
            double sum = red * red + green * green + blue * blue;
            return (int)Math.Sqrt(sum);
        }

        public Bitmap RemovePathFromImage()
        {
            BitmapData bmData = this.bmp.LockBits(new Rectangle(0, 0, edgeDetectedBmp.Width, edgeDetectedBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int bmpStride = bmData.Stride;
            int bmpSize = bmpStride * bmData.Height;

            Bitmap retBmp = new Bitmap(this.edgeDetectedBmp.Width - 1, edgeDetectedBmp.Height);
            BitmapData retData = retBmp.LockBits(new Rectangle(0, 0, retBmp.Width, retBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int retStride = retData.Stride;
            int retSize = retStride * retData.Height;

            PixelPos[] positions = this.path.ToArray();
            int len = positions.Length;

            unsafe
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    byte* row = (byte*)bmData.Scan0 + (y * bmpStride);

                    int columnOffset = 0;
                    int innerCnt = 0;

                    byte* resRow = (byte*)retData.Scan0 + (y * retStride);

                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        if (positions[len - 1 - y].y != x)
                        {
                            resRow[innerCnt] = row[columnOffset];
                            resRow[innerCnt + 1] = row[columnOffset + 1];
                            resRow[innerCnt + 2] = row[columnOffset + 2];

                            innerCnt += 3;
                        }

                       columnOffset += 3;
                    }
                }
            }

            bmp.UnlockBits(bmData);
            retBmp.UnlockBits(retData);

            return retBmp;
        }

        private void PopulatePath(int minYpos)
        {
            int width = imgColors.GetLength(0);
            int height = imgColors.GetLength(1);
            int currPos = minYpos;

            //prvi piksel odozdo
            this.path = new List<PixelPos>();
            this.path.Add(new PixelPos(height - 1, minYpos));

            for (int i=height-1; i>0; i--)
            {
                int newYindex = 0;

                if (currPos == 0)
                    newYindex = GetIndexOfLesserEl(i - 1, currPos, currPos + 1);
                else if (currPos == width - 1)
                    newYindex = GetIndexOfLesserEl(i - 1, currPos, currPos - 1);
                else
                    newYindex = GetIndexOfLesserEl(i - 1, GetIndexOfLesserEl(i - 1, currPos, currPos - 1), currPos + 2);

                currPos = newYindex;
                this.path.Add(new PixelPos(i - 1, newYindex));
            }
        }    

        private int GetBottomRowMinYPosition()
        {
            int min = energyMap[0, 0];
            int height = energyMap.GetLength(1);
            int minYPos = 0 ;

            for (int i=0; i < energyMap.GetLength(0); i++)
            {
                if (energyMap[i, height - 1] < min)
                {
                    min = energyMap[i, height - 1];
                    minYPos = i;
                }
            }

            return minYPos;
        }


        #region helperFunctions
        private Color getAvg(Color A, Color B)
        {
            return Color.FromArgb((A.R + B.R) >> 1, (A.G + B.G) >> 1, (A.B + B.B) >> 1);
        }

        private Color getCopy(Color A)
        {
            int r = A.R, g = A.G, b = A.B;
            return Color.FromArgb(r, g, b);
        }

        public int PixelValue(Color c)
        {
            return c.R + c.G + c.B;
        }

        public int GetIndexOfLesserEl(int height, int first, int second)
        {
            if (energyMap[height, first] < energyMap[height, second])
            {
                return first;
            }
            else
                return second;
        }

        public Bitmap GetMainImage()
        {
            return this.bmp;
        }
        #endregion
    }
}
