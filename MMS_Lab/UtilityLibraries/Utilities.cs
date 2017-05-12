using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMS_Lab.Compression;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;

namespace MMS_Lab.UtilityLibraries
{
    public static class Utilities
    {
        public static long[] GetHistogram(Bitmap picture)
        {
            long[] myHistogram = new long[256];

            for (int i = 0; i < picture.Size.Width; i++)
                for (int j = 0; j < picture.Size.Height; j++)
                {
                    Color c = picture.GetPixel(i, j);

                    long Temp = 0;
                    Temp += c.R;
                    Temp += c.G;
                    Temp += c.B;

                    Temp = (int)Temp / 3;
                    myHistogram[Temp]++;
                }

            return myHistogram;
        }

        public static float[] ComputeXYUnitValues(long[] myValues, int myOffset, long myMaxValue, int width, int height)
        {
            float [] retVal = new float[2];
            retVal[0] = (float)(height - (2 * myOffset)) / myMaxValue;
            retVal[1] = (float)(width - (2 * myOffset)) / (myValues.Length - 1);

            return retVal;
        }

        public static Bitmap DrawHistogram(Bitmap bmp)
        {
            Bitmap img = new Bitmap(bmp.Width, bmp.Height);

            Graphics g = Graphics.FromImage(img);

            g.DrawRectangle(new System.Drawing.Pen(new SolidBrush(Color.Black), 1), 0, 0, img.Width - 1, img.Height - 1);

            int myOffset = 5;
            long[] myValues = Utilities.GetHistogram(bmp);
            long myMaxValue = Utilities.getMaxim(myValues);

            float [] values = ComputeXYUnitValues(myValues, myOffset, myMaxValue ,img.Width,img.Height);

            Pen pen = new Pen(new SolidBrush(Color.Black), values[1]);
            for (int i = 0; i < myValues.Length; i++)
            {
                g.DrawLine(pen, new PointF(myOffset + (i * values[1]), img.Height - myOffset), new PointF(myOffset + (i * values[1]), img.Height - myOffset - myValues[i] * values[0]));
            }

            return img;
        }

        public static double TriggerFormAndGetResult(string formText, string labelText)
        {
            double val = 0;

            Parameters.label = labelText;

            using (var form = new Parameters())
            {
                form.Name = formText;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    val = form.val;
                }
            }

            return val;            
        }

        public static void Save(Bitmap bitmap)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int size = bmData.Stride * bmData.Height;
                byte[] data = new byte[size];


                System.Runtime.InteropServices.Marshal.Copy(bmData.Scan0, data, 0, size);
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            }
        }

        public static Bitmap LoadCompressed()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.InitialDirectory = path;
            openFileDialog.Filter = "MM Huffman's compressed file(*.mmc) |";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                return DecompressHuffman(openFileDialog.FileName);
            }
            else
                return null;
        }

        public static void SaveCompressed (DownsampleFormat format)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            dialog.InitialDirectory = path;
            dialog.RestoreDirectory = true;
            dialog.Filter = "MM Huffman's compressed file(*.mmc) |";

            if (DialogResult.OK == dialog.ShowDialog())
            {
                CompressHuffman(format, dialog.FileName);
            }
            else
            {
                MessageBox.Show("Error saving file.");
            }
        }

        public static void CompressHuffman(DownsampleFormat format, string dest)
        {
            DownsampleFormat downFormat = format;
            byte[] data = new byte[format.Ylen + format.Cblen + format.Crlen];
            format.data.CopyTo(data, 0);
           
            HuffmanTree tree = new HuffmanTree();
            tree.CreateTree(tree.GenerateListOfNodes(data));
            

            byte[] decompressDict = tree.SerializeDictionaryToBytes();

            byte[] dictionarySize = BitConverter.GetBytes(decompressDict.Length); //svaki integer ima duzinu od 4 bajta u C#
            byte[] bmpWidth = BitConverter.GetBytes(format.bmpWidth);//4 bajta
            byte[] bmpHeight = BitConverter.GetBytes(format.bmpHeight);//4 bajta
            byte[] stride = BitConverter.GetBytes(format.bmpStride);//4 bajta
            byte[] downsampleChannels = BitConverter.GetBytes(format.code);//4 bajta
            byte[] YDataLen = BitConverter.GetBytes(format.Ylen);//4 bajta
            byte[] CbDataLen = BitConverter.GetBytes(format.Cblen);//4 bajta
            byte[] CrDataLen = BitConverter.GetBytes(format.Crlen);//4 bajta

            using (var writer = new FileStream(dest,FileMode.Create))
            {
                writer.Write(bmpWidth, 0, bmpWidth.Length);
                writer.Write(bmpHeight, 0, bmpHeight.Length);
                writer.Write(stride, 0, stride.Length);
                writer.Write(downsampleChannels, 0, downsampleChannels.Length);

                writer.Write(YDataLen, 0, YDataLen.Length);
                writer.Write(CbDataLen, 0, CbDataLen.Length);
                writer.Write(CrDataLen, 0, CrDataLen.Length);

                writer.Write(dictionarySize, 0, dictionarySize.Length);
                writer.Write(decompressDict, 0, decompressDict.Length);
                
                byte[] allData = BitArrayToByteArray(tree.Encode(data));

                writer.Write(allData, 0, allData.Length);
            }
        }

        public static Bitmap DecompressHuffman (string imgSrc)
        {
            //moram da pocnem odmah otvaranjem fajla i ucitavanjem bajtova redom kako su upisivani
            byte[] bmpWidth;
            byte[] bmpHeight;
            byte[] stride;
            byte[] downsampleChannels;
            byte[] dictionarySize;
            byte[] dictionary;
            byte[] YDataLen;
            byte[] CbDataLen;
            byte[] CrDataLen;
            byte[] imageData;
            HuffmanTree tree = new HuffmanTree();

            using (var reader = new BinaryReader(File.Open(imgSrc, FileMode.Open)))
            {
                bmpWidth = reader.ReadBytes(4);
                bmpHeight = reader.ReadBytes(4);
                stride = reader.ReadBytes(4);
                downsampleChannels = reader.ReadBytes(4);

                YDataLen = reader.ReadBytes(4);
                CbDataLen = reader.ReadBytes(4);
                CrDataLen = reader.ReadBytes(4);

                dictionarySize = reader.ReadBytes(4);

                int dictSize = BitConverter.ToInt32(dictionarySize, 0);

                dictionary = reader.ReadBytes(dictSize);
                tree.DeserializeDictionary(dictionary);

                List<byte> compressedData = new List<byte>();
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    compressedData.Add(reader.ReadByte());
                }

                imageData = tree.Decode(new BitArray(compressedData.ToArray()));
            }

            int width = BitConverter.ToInt32(bmpWidth, 0);
            int height = BitConverter.ToInt32(bmpHeight, 0);
            int strideInt = BitConverter.ToInt32(stride, 0);
            int downampleChannelsInt = BitConverter.ToInt32(downsampleChannels, 0);
            int yDataLen = BitConverter.ToInt32(YDataLen, 0);
            int cbDataLen = BitConverter.ToInt32(CbDataLen, 0);
            int crDataLen = BitConverter.ToInt32(CrDataLen, 0);


            DownsampleFormat format = new DownsampleFormat(strideInt, width, height, yDataLen, cbDataLen, crDataLen, downampleChannelsInt);
            format.data = imageData;
            Downsampling sampling = new Downsampling();

            return sampling.RestoreBitmap(format);
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
        
        public static long getMaxim(long[] Vals)
        {
            long max = 0;
            for (int i = 0; i < Vals.Length; i++)
            {
                if (Vals[i] > max)
                    max = Vals[i];
            }
            return max;
        }
    }
}
