using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMS_Lab.Controller;
using MMS_Lab.Model;
using MMS_Lab.View;
using Microsoft.VisualBasic;
using MMS_Lab.UtilityLibraries;

namespace MMS_Lab
{
    public partial class MainWindow : Form, IView
    {
        IController controller;
        bool unsafeFilters;
        bool inplaceBlur;
        bool mainImg;


        public void AddListener(IController controller)
        {
            this.controller = controller;
            this.firstImgChannel.Visible = false; this.firstImgChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            this.allImgChannels.Visible = false; this.allImgChannels.SizeMode = PictureBoxSizeMode.StretchImage;
            this.secondImgChannel.Visible = false; this.secondImgChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            this.thirdImgChannel.Visible = false; this.thirdImgChannel.SizeMode = PictureBoxSizeMode.StretchImage;
            this.unsafeFilters = true;
            this.inplaceBlur = false;
            this.mainImg = false;

            controller.unsafeClick();
            this.showChannelsToolStripMenuItem.Checked = false;
            this.unsafeFilterToolStripMenuItem.Checked = true;
            this.EnableDisableImageOptions();

            this.saveChannelImageToolStripMenuItem.Enabled = false;
        }

        private void EnableDisableImageOptions()
        {
            this.optionsToolStripMenuItem.Enabled = !this.optionsToolStripMenuItem.Enabled;
            this.editToolStripMenuItem.Enabled = !this.editToolStripMenuItem.Enabled;
            this.filtersToolStripMenuItem.Enabled = !this.filtersToolStripMenuItem.Enabled;
            this.saveImage.Enabled = !this.saveImage.Enabled;
            //this.saveChannelImageToolStripMenuItem.Enabled = !this.saveChannelImageToolStripMenuItem.Enabled;
            this.saveCompressedToolStripMenuItem.Enabled = !this.saveCompressedToolStripMenuItem.Enabled;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage(object sender, EventArgs e)
        {
            this.loadImgDialog.ShowDialog();
        }

        private void loadImagePath(object sender, CancelEventArgs e)
        {
            string imagePath = this.loadImgDialog.FileName;
            controller.onLoadImageClick(imagePath);

            if (this.optionsToolStripMenuItem.Enabled==false)
                this.EnableDisableImageOptions();
        }

        public void ShowImage(Image img, string imgName)
        {
            this.mainPic.SizeMode = PictureBoxSizeMode.StretchImage;
            mainPic.Image = img;
            this.pictDimensions.Text = "Dimensions: " + img.Width + " X " + img.Height + " |  \t File name: " + imgName;
            this.Invalidate();
        }

        private void Save(object sender, EventArgs e)
        {
            Image img = mainPic.Image;
            controller.onSaveImageClick(img);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UnsafeClick(object sender, EventArgs e)
        {
            this.unsafeFilterToolStripMenuItem.Checked = !unsafeFilterToolStripMenuItem.Checked;
            this.unsafeFilters = !unsafeFilters;
            controller.unsafeClick();
        }

        public void ShowImages(Bitmap unffiltImg, Bitmap firstChannel, Bitmap secondChannel, Bitmap thirdChannel, string imgName)
        {
            MakeChannelsVisible();

            this.allImgChannels.Image = unffiltImg;
            this.firstImgChannel.Image = firstChannel;
            this.secondImgChannel.Image = secondChannel;
            this.thirdImgChannel.Image = thirdChannel;

        }

        public void ChangeChannelsVisibility()
        {
            this.mainPic.Visible = !mainPic.Visible;

            this.allImgChannels.Visible = !allImgChannels.Visible;
            this.firstImgChannel.Visible = !firstImgChannel.Visible;
            this.secondImgChannel.Visible = !secondImgChannel.Visible;
            this.thirdImgChannel.Visible = !thirdImgChannel.Visible;
        }


        private void ShowChannels(object sender, EventArgs e)
        {
            controller.showChannels();

            if (showChannelsToolStripMenuItem.Checked)
                //znaci da moramo da prikazemo glavnu
                controller.showMainImage();
            else
                controller.showChannelsClick();

            this.showChannelsToolStripMenuItem.Checked = !this.showChannelsToolStripMenuItem.Checked;
            this.saveChannelImageToolStripMenuItem.Enabled = !this.saveChannelImageToolStripMenuItem.Enabled;
        }

        public void MakeChannelsVisible()
        {
            this.mainPic.Visible = false;

            this.allImgChannels.Visible = true;
            this.firstImgChannel.Visible = true;
            this.secondImgChannel.Visible = true;
            this.thirdImgChannel.Visible = true;
        }

        public void MakeChannelsInvisible()
        {
            this.mainPic.Visible = true;

            this.allImgChannels.Visible = false;
            this.firstImgChannel.Visible = false;
            this.secondImgChannel.Visible = false;
            this.thirdImgChannel.Visible = false;
        } 

        private void ShowMainImage(object sender, EventArgs e)
        {
            mainImg = !mainImg;
            this.showMainImageToolStripMenuItem.Checked = !showMainImageToolStripMenuItem.Checked;

            if (this.showChannelsToolStripMenuItem.Checked)
                this.showChannelsToolStripMenuItem.Checked = false;

            if (mainImg)
            {
                controller.showChannels();
                controller.showMainImage();
            }
            else
                controller.showChannelsClick();
        }

        private void InplaceBlur(object sender, EventArgs e)
        {       
            this.blurWithoutInfluenceToolStripMenuItem.Checked = !this.blurWithoutInfluenceToolStripMenuItem.Checked;
            this.inplaceBlur = !inplaceBlur;
        }
           
        public void DisableChannelImages()
        {
            this.firstImgChannel.Image = null;
            this.secondImgChannel.Image = null;
            this.thirdImgChannel.Image = null;
        }

        #region FILTERS_CLICK_EVENT

        private void Brightness(object sender, EventArgs e)
        {
            controller.brightnessClick();
        }

        private void Contrast(object sender, EventArgs e)
        {
            controller.contrastClick();
        }

        private void GaussianBlur(object sender, EventArgs e)
        {
            controller.gaussianBlurClick(inplaceBlur);
        }

        private void EdgeDetect(object sender, EventArgs e)
        {
            controller.edgeDetectClick();
        }

        private void Water(object sender, EventArgs e)
        {
            controller.waterClick();
        }

        #endregion

        #region HISTOGRAMS

        //private void ComputeXYUnitValues(long[] myValues)
        //{
        //    myYUnit = (float)(pb.Height - (2 * myOffset)) / myMaxValue;
        //    myXUnit = (float)(pb.Width - (2 * myOffset)) / (myValues.Length - 1);
        //}

        #endregion

        private void SaveImageChannels(object sender, EventArgs e)
        {
            if (this.firstImgChannel.Image != null)
            {
                int val = 4;
                while (val > 3)
                    val = (int)Utilities.TriggerFormAndGetResult("Save channel", "Input channel to save: 1, 2 or 3");

                if (val == 1)
                    controller.onSaveImageClick(this.firstImgChannel.Image);
                else if (val == 2)
                    controller.onSaveImageClick(this.secondImgChannel.Image);
                else if (val == 3)
                    controller.onSaveImageClick(this.thirdImgChannel.Image);
            }
            else
                MessageBox.Show("Generate channels first.");
        }


        private void ShiftedAndScaled(object sender, EventArgs e)
        {
            if (this.showChannelHistogramsToolStripMenuItem.Checked == true)
                this.showChannelHistogramsToolStripMenuItem.Checked = false;

            controller.shiftedAndScaledClick();
        }

        private void OnResize(object sender, EventArgs e)
        {
            this.imgContainer.Width = this.Width - 20;
            this.imgContainer.Height = this.Height - 100;
            this.mainPic.Width = this.Width - 30;
            this.mainPic.Height = this.Height - 100;

            this.allImgChannels.Width = this.Width / 2 - 40;
            this.allImgChannels.Height = (this.Height - 100) / 2 - 20;

            this.firstImgChannel.Location = new Point(this.Width / 2 + 40, this.firstImgChannel.Location.Y);
            this.firstImgChannel.Width = this.Width / 2 - 40;
            this.firstImgChannel.Height = (this.Height - 100) / 2 - 20;

            this.secondImgChannel.Location = new Point(this.secondImgChannel.Location.X, (this.Height - 100) / 2 + 10);
            this.secondImgChannel.Width = this.Width / 2 - 40;
            this.secondImgChannel.Height = (this.Height - 100) / 2 - 20;

            this.thirdImgChannel.Location = new Point(this.Width / 2 + 40, (this.Height - 100) / 2 + 10);
            this.thirdImgChannel.Width = this.Width / 2 - 40;
            this.thirdImgChannel.Height = (this.Height - 100) / 2 - 20;
        }

        private void Downsample(object sender, EventArgs e)
        {
            controller.DownsampleClick();
        }

        private void SaveCompressed(object sender, EventArgs e)
        {
            controller.SaveCompressedClick();
        }

        private void LoadCompressed(object sender, EventArgs e)
        {
            this.controller.LoadCompressedClick();
        }
      
        private void EditWAV(object sender, EventArgs e)
        {
            this.controller.EditWAVFileClick();
        }

        private void PlayWAV(object sender, EventArgs e)
        {
            this.controller.PlayWAWFileClick();
        }

        private void SeamCarving(object sender, EventArgs e)
        {
            this.controller.SeamCarvingClick();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.L))
            {
                this.loadImgDialog.ShowDialog();
            }
            else if(keyData == (Keys.Control | Keys.S))
            {
                Image img = mainPic.Image;
                controller.onSaveImageClick(img);
            }
            else if (keyData == (Keys.Control | Keys.Z))
            {
                controller.undoClick();
            }
            else if (keyData == (Keys.Control | Keys.Y))
            {
                controller.redoClick();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Undo(object sender, EventArgs e)
        {
            this.controller.undoClick();
        }

        private void Redo(object sender, EventArgs e)
        {
            this.controller.redoClick();
        }

        private void ConvolutionFilters(object sender, EventArgs e)
        {
            if (this.showMainImageToolStripMenuItem.Checked)
                this.showMainImageToolStripMenuItem.Checked = false;

            if (!this.showChannelsToolStripMenuItem.Checked)
                this.showChannelsToolStripMenuItem.Checked = true;

            controller.VariableConvolutionFiltersClick();
        }

        private void DrawHistograms(object sender, EventArgs e)
        {
            if(showChannelHistogramsToolStripMenuItem.Checked==false)
                this.showChannelHistogramsToolStripMenuItem.Checked = true;
            controller.DrawHistograms();
        }
    }
}
