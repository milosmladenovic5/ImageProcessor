using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI.Audio;
using System.IO;
using System.Windows.Forms;
using MMS_Lab.UtilityLibraries;

namespace MMS_Lab.Audio
{
    public static class WAV_Processing
    {
        public static void LoadWAVFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string startingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.InitialDirectory = startingPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "WAV Audio files(*.wav) |";

            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                OpenWAVFile(openFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("There was an error during file opening.");
            }

        }

        public static void OpenWAVFile(string path)
        {
            int channelsNumber;

            byte[] data;
            int[] inputValues;
            int bytesPerSample;

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.ReadBytes(22);                
                channelsNumber = reader.ReadInt16();
                inputValues = GetInputValues(channelsNumber);

                reader.ReadBytes(10);
                bytesPerSample = reader.ReadInt16()/8;                
            }

            data = File.ReadAllBytes(path);
            SaveWAVFile(EditWAVFile(data, inputValues, bytesPerSample));
        }

        public static byte[] EditWAVFile(byte[] data, int [] inputValues, int bytesPerSample)
        {
            byte[] result = new byte[data.Length];
            //do 44. bajta su sve zaglavlja i slicno, to cemo da prepisemo, tj. vratimo kako je bilo
            for(int i=0; i<44; i++)
            {
                result[i] = data[i];
            }

            int channelSize = bytesPerSample / inputValues.Length;
            int channelNumber = 0;

            //sada citamo konkretne podatke, znači broj kanala je inputValues.Length, a broj bajtova po kanalu je bytesPerSample/inputValues.Length
            for (int i=44; i<data.Length; i+=bytesPerSample)
            {
                for(int j = 0; j<bytesPerSample; j+=channelSize)
                {
                    for(int k=0; k<channelSize; k++)
                    {
                        if((int)data[i+j+k] > inputValues[channelNumber])
                        {
                            result[i + j + k] = (byte)inputValues[channelNumber];
                        }
                    }
                    channelNumber++;
                }
                channelNumber = 0;
            }
            return result;
        }

        public static void SaveWAVFile(byte[] data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = "WAV files (*.wav)|*.wav";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                using (var writer = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    writer.Write(data, 0, data.Length);
                }
            }
            MessageBox.Show("Successfuly saved edited audio file.");
        }

        public static int [] GetInputValues(int count)
        {
            int[] result = new int[count];
            for (int i=0; i<count; i++)
            {
                string label = "Enter value number: " + i;
                result[i] = Filters.Clamp((int)Utilities.TriggerFormAndGetResult("Channel value ", label), 0, 255);
            }
            return result;
        }
    }
}
