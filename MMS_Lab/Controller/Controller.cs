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
using MMS_Lab.UtilityLibraries;
using MMS_Lab.Compression;
using MMS_Lab.Audio;
using System.Media;
using MMS_Lab.SeamCarving;

namespace MMS_Lab.Controller
{
    public class Controller : IController
    {
        IModel model;
        IView view;
        UndoRedo undoRedo;
        private bool unsafeFilters;
        public bool channelsOn { get; set; }

        public Controller(MMS_Lab.Model.IModel model, MMS_Lab.View.IView view)
        {
            this.model = model;
            this.view = view;
            this.view.AddListener(this);
            this.unsafeFilters = false;
            this.channelsOn = false;
        }

        public void showChannels()
        {
            this.channelsOn = !channelsOn;
        }

        public void onLoadImageClick(string imgPath)
        {
            if (model.getImageName() != null || model.getCbChannel()!=null)
                view.MakeChannelsInvisible();

            string imgName = Path.GetFileName(imgPath);
            model.setImageName(imgName);
            model.setYChannel(null);
            model.setCbChannel(null);
            model.setCrChannel(null);

            Image img = Image.FromFile(imgPath);
            Bitmap bmp = new Bitmap(img);
            model.setMainImage(bmp);

            undoRedo = null;
            undoRedo = new UndoRedo();

            view.ShowImage(img, imgName);
        }

        public struct Point
        {
            public double X;
            public double Y;
        }

        public void onSaveImageClick(Image img)
        {
            if(img!=null)
            {
                Utilities.Save(new Bitmap(img));
            }
        }

        public void showChannelsClick()
        {
            GetYCbCrChannels();
            this.view.ShowImages(model.getMainImage(), model.getYChannel(), model.getCbChannel(), model.getCrChannel(), model.getImageName());
        }

        
        public void unsafeClick()
        {
            this.unsafeFilters = !this.unsafeFilters;
            Filters.unsafeFilters = !Filters.unsafeFilters;         
        }

        public void brightnessClick()
        {
            double val = Utilities.TriggerFormAndGetResult("Brightness", "Brightness value, range from -255 to 255");

            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            undoRedo.currentRedoCount = 0;

            Filters.Brightness((int)val, model.getMainImage());

            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
        }


        public void contrastClick()
        {
            double val = (int)Utilities.TriggerFormAndGetResult("Contrast", "Contrast value, range from -100 to 100");

            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            undoRedo.currentRedoCount = 0;

            Filters.Contrast((int)val, model.getMainImage());

            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
        }

        public void gaussianBlurClick(bool influence)
        {
            int val = (int) Utilities.TriggerFormAndGetResult("Gaussian blur ", "Enter Gaussian blur value:");

            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            undoRedo.currentRedoCount = 0;

            if (influence == false)
                Filters.GaussianBlur(val, model.getMainImage());
            else
                Filters.GaussianBlurWithoutCentralPixels(val, model.getMainImage());
        }

        public void showMainImage()
        {
            view.MakeChannelsInvisible();
            //model.setFirstFiltered(null);
            //model.setSecondFiltered(null);
            //model.setThirdFiltered(null);

            this.view.ShowImage(model.getMainImage(), model.getImageName());
        }
                
        public void undoClick()
        {
            Bitmap lastImage = (Bitmap)undoRedo.GetFromUndoBuffer();

            if(lastImage!=null)
            {
                undoRedo.AddToRedoBuffer((Bitmap)model.getMainImage().Clone());
                model.setMainImage(lastImage);
                lastImage = (Bitmap)lastImage.Clone();
            }

            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
        }

        public void redoClick()
        {
            Bitmap newImage = (Bitmap)undoRedo.GetFromRedoBuffer();

            if(newImage!=null)
            {
                undoRedo.AddToUndoBuffer((Bitmap)model.getMainImage().Clone());
                model.setMainImage(newImage);
                newImage = (Bitmap)newImage.Clone();
            }

            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
        }

        public void edgeDetectClick()
        {
            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            Filters.EdgeDetectHorizontal(model.getMainImage());
  
            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
        }

        public void waterClick()
        {
            int val = (int)Utilities.TriggerFormAndGetResult("Water filter", "Enter water filter value: ");            

            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            undoRedo.currentRedoCount = 0;

            Filters.WaterFilter(model.getMainImage(), val);

            if (channelsOn)
                this.showChannelsClick();
            else
                this.view.ShowImage(model.getMainImage(), model.getImageName());
       }     

        public void VariableConvolutionFiltersClick()
        {
            ConvolutionMatrix3x3 cm = new ConvolutionMatrix3x3();
            cm.Apply(1);
            cm.BottomMid = cm.MidLeft = cm.TopMid = cm.MidRight = 2;
            cm.Factor = 12+3;
            cm.Factor5x5 = 36 + 3;
            cm.Factor7x7 = 60 + 3;
            cm.Offset = 1;

            undoRedo = null;
            undoRedo = new UndoRedo();

            #region
            Bitmap firstChannel = (Bitmap)model.getMainImage().Clone();
            Bitmap secondChannel = (Bitmap)model.getMainImage().Clone();
            Bitmap thirdChannel = (Bitmap)model.getMainImage().Clone();
            #endregion

            //ExtractChannels(model.getMainImage(), firstChannel, secondChannel, thirdChannel);

            #region
            model.setYChannel(firstChannel);
            model.setCbChannel(secondChannel);
            model.setCrChannel(thirdChannel);
            #endregion

            Filters.GaussianBlur3x3InPlaceUnsafe(model.getYChannel(), cm);
            Filters.GaussianBlur5x5InPlaceUnsafe(model.getCbChannel(), cm);
            Filters.GaussianBlur7x7InPlaceUnsafe(model.getCrChannel(), cm);
            channelsOn = !channelsOn;
            
            this.view.ShowImages(model.getMainImage(), model.getYChannel(), model.getCbChannel(), model.getCrChannel(), model.getImageName());
        }

        public void shiftedAndScaledClick()
        {
            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            undoRedo.currentRedoCount = 0;

            float shift = (float)Utilities.TriggerFormAndGetResult("Shift and scaled", "Enter shift value: ");

            float scale = (float)Utilities.TriggerFormAndGetResult("Shift and scaled", "Enter scale value: ");

            GetYCbCrChannels();

            Filters.ShiftedAndScaledUnsafe(model.getYChannel(), shift, scale);
            Filters.ShiftedAndScaledUnsafe(model.getCbChannel(), shift, scale);
            Filters.ShiftedAndScaledUnsafe(model.getCrChannel(), shift, scale);

            if(!channelsOn)
            {
                channelsOn = true;
            }

            this.DrawHistograms();
        }

        public void DownsampleClick()
        {
            this.channelsOn = true;
            this.view.MakeChannelsVisible();

            Downsampling down = new Downsampling(this.model.getMainImage());
            DownsampleFormat df1 = down.Downsample(1);
            DownsampleFormat df2 = down.Downsample(2);
            DownsampleFormat df3 = down.Downsample(2);

            //Compression.DownsamplingNew.Downsampling down = new Compression.DownsamplingNew.Downsampling(this.model.getMainImage());
            //Compression.DownsamplingNew.DownsampleFormat df1 = down.DownsampleImage("YCb");
            //Compression.DownsamplingNew.DownsampleFormat df2 = down.DownsampleImage("CbCr");
            //Compression.DownsamplingNew.DownsampleFormat df3 = down.DownsampleImage("YCr");

            this.model.setYChannel(down.RestoreBitmap(df1));
            this.model.setCbChannel(down.RestoreBitmap(df2));
            this.model.setCrChannel(down.RestoreBitmap(df3));

            this.view.ShowImages(model.getMainImage(), model.getYChannel(), model.getCbChannel(), model.getCrChannel(), model.getImageName());
        }

        public void SaveCompressedClick()
        {
            Downsampling d = new Downsampling(this.model.getMainImage());
            DownsampleFormat df = d.Downsample(1);
            Utilities.SaveCompressed(df);
        }

        public void LoadCompressedClick()
        {
            Bitmap bmp = Utilities.LoadCompressed();

            model.setMainImage(bmp);

            undoRedo = null;
            undoRedo = new UndoRedo();

            view.ShowImage(bmp, "compressed");
        }

        public void EditWAVFileClick()
        {
            WAV_Processing.LoadWAVFile();
        }

        public void PlayWAWFileClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string startingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.InitialDirectory = startingPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "WAV Audio files(*.wav) |";

            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                SoundPlayer wavSound = new SoundPlayer(openFileDialog.FileName);
                wavSound.Play();
            }
            else
            {
                MessageBox.Show("There was an error during file opening.");
            }
        }

        public void SeamCarvingClick()
        {
            Bitmap newImage = (Bitmap)model.getMainImage().Clone();
            undoRedo.AddToUndoBuffer(newImage);

            //undoRedo.currentRedoCount = 0;

            Bitmap bmp = (Bitmap)this.model.getMainImage().Clone();

            SeamCarving.SeamCarving sc = new SeamCarving.SeamCarving(bmp);
            model.setMainImage(sc.RemovePathFromImage());         

            view.ShowImage(model.getMainImage(), model.getImageName());
        }

        public void DrawHistograms()
        {
            if (!this.channelsOn)
                channelsOn = true;

            Bitmap [] allCh = GetYCbCrChannels();

            Bitmap YChannel = Utilities.DrawHistogram(allCh[0]);
            Bitmap CbChannel = Utilities.DrawHistogram(allCh[1]);
            Bitmap CrChannel = Utilities.DrawHistogram(allCh[2]);

            view.ShowImages(model.getMainImage(), YChannel, CbChannel, CrChannel, model.getImageName());
        }

        public Bitmap [] GetYCbCrChannels()
        {
            Bitmap[] allChannels = new Bitmap[3];

            Bitmap yChannel = (Bitmap)model.getMainImage().Clone();
            Bitmap cbChannel = (Bitmap)model.getMainImage().Clone();
            Bitmap crChannel = (Bitmap)model.getMainImage().Clone();

            if (model.getYChannel() == null)
            {
                Filters.ExtractChannels(model.getMainImage(), yChannel, cbChannel, crChannel);

                this.model.setYChannel(yChannel);
                this.model.setCbChannel(cbChannel);
                this.model.setCrChannel(crChannel);
            }

            allChannels[0] = (Bitmap)model.getYChannel().Clone();
            allChannels[1] = (Bitmap)model.getCbChannel().Clone();
            allChannels[2] = (Bitmap)model.getCrChannel().Clone();

            return allChannels;            
        }
    }
}
