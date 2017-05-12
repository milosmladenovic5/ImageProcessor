namespace MMS_Lab
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.saveChannelImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoCtrZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoCtrlYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unsafeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMainImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blurWithoutInfluenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showChannelHistogramsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downsampleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seamCarvingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contrastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gaussianBlurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeDetectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftedAndScaledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convolutionFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playWAVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWAVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.pictDimensions = new System.Windows.Forms.ToolStripStatusLabel();
            this.imgContainer = new System.Windows.Forms.GroupBox();
            this.thirdImgChannel = new System.Windows.Forms.PictureBox();
            this.secondImgChannel = new System.Windows.Forms.PictureBox();
            this.firstImgChannel = new System.Windows.Forms.PictureBox();
            this.allImgChannels = new System.Windows.Forms.PictureBox();
            this.mainPic = new System.Windows.Forms.PictureBox();
            this.loadImgDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.imgContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thirdImgChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondImgChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstImgChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allImgChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPic)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.audioToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(790, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImage,
            this.saveImage,
            this.saveChannelImageToolStripMenuItem,
            this.loadCompressedToolStripMenuItem,
            this.saveCompressedToolStripMenuItem,
            this.exitProgram});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadImage
            // 
            this.loadImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("loadImage.BackgroundImage")));
            this.loadImage.Image = ((System.Drawing.Image)(resources.GetObject("loadImage.Image")));
            this.loadImage.Name = "loadImage";
            this.loadImage.Size = new System.Drawing.Size(167, 22);
            this.loadImage.Text = "Load           Ctrl+L";
            this.loadImage.Click += new System.EventHandler(this.LoadImage);
            // 
            // saveImage
            // 
            this.saveImage.Image = ((System.Drawing.Image)(resources.GetObject("saveImage.Image")));
            this.saveImage.Name = "saveImage";
            this.saveImage.Size = new System.Drawing.Size(167, 22);
            this.saveImage.Text = "Save           Ctrl+S";
            this.saveImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.saveImage.Click += new System.EventHandler(this.Save);
            // 
            // saveChannelImageToolStripMenuItem
            // 
            this.saveChannelImageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveChannelImageToolStripMenuItem.Image")));
            this.saveChannelImageToolStripMenuItem.Name = "saveChannelImageToolStripMenuItem";
            this.saveChannelImageToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.saveChannelImageToolStripMenuItem.Text = "Save channel  ";
            this.saveChannelImageToolStripMenuItem.Click += new System.EventHandler(this.SaveImageChannels);
            // 
            // loadCompressedToolStripMenuItem
            // 
            this.loadCompressedToolStripMenuItem.Name = "loadCompressedToolStripMenuItem";
            this.loadCompressedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.loadCompressedToolStripMenuItem.Text = "Load compressed";
            this.loadCompressedToolStripMenuItem.Click += new System.EventHandler(this.LoadCompressed);
            // 
            // saveCompressedToolStripMenuItem
            // 
            this.saveCompressedToolStripMenuItem.Name = "saveCompressedToolStripMenuItem";
            this.saveCompressedToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.saveCompressedToolStripMenuItem.Text = "Save compressed";
            this.saveCompressedToolStripMenuItem.Click += new System.EventHandler(this.SaveCompressed);
            // 
            // exitProgram
            // 
            this.exitProgram.Image = ((System.Drawing.Image)(resources.GetObject("exitProgram.Image")));
            this.exitProgram.Name = "exitProgram";
            this.exitProgram.Size = new System.Drawing.Size(167, 22);
            this.exitProgram.Text = "Exit";
            this.exitProgram.Click += new System.EventHandler(this.CloseForm);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoCtrZToolStripMenuItem,
            this.redoCtrlYToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoCtrZToolStripMenuItem
            // 
            this.undoCtrZToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("undoCtrZToolStripMenuItem.Image")));
            this.undoCtrZToolStripMenuItem.Name = "undoCtrZToolStripMenuItem";
            this.undoCtrZToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.undoCtrZToolStripMenuItem.Text = "Undo       Ctrl+Z";
            this.undoCtrZToolStripMenuItem.Click += new System.EventHandler(this.Undo);
            // 
            // redoCtrlYToolStripMenuItem
            // 
            this.redoCtrlYToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("redoCtrlYToolStripMenuItem.Image")));
            this.redoCtrlYToolStripMenuItem.Name = "redoCtrlYToolStripMenuItem";
            this.redoCtrlYToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.redoCtrlYToolStripMenuItem.Text = "Redo        Ctrl+Y";
            this.redoCtrlYToolStripMenuItem.Click += new System.EventHandler(this.Redo);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unsafeFilterToolStripMenuItem,
            this.showMainImageToolStripMenuItem,
            this.blurWithoutInfluenceToolStripMenuItem,
            this.showChannelHistogramsToolStripMenuItem,
            this.downsampleToolStripMenuItem,
            this.seamCarvingToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // unsafeFilterToolStripMenuItem
            // 
            this.unsafeFilterToolStripMenuItem.Name = "unsafeFilterToolStripMenuItem";
            this.unsafeFilterToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.unsafeFilterToolStripMenuItem.Text = "Unsafe filter";
            this.unsafeFilterToolStripMenuItem.Click += new System.EventHandler(this.UnsafeClick);
            // 
            // showMainImageToolStripMenuItem
            // 
            this.showMainImageToolStripMenuItem.Name = "showMainImageToolStripMenuItem";
            this.showMainImageToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.showMainImageToolStripMenuItem.Text = "Show main image";
            this.showMainImageToolStripMenuItem.Click += new System.EventHandler(this.ShowMainImage);
            // 
            // blurWithoutInfluenceToolStripMenuItem
            // 
            this.blurWithoutInfluenceToolStripMenuItem.Name = "blurWithoutInfluenceToolStripMenuItem";
            this.blurWithoutInfluenceToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.blurWithoutInfluenceToolStripMenuItem.Text = "Inplace convolution";
            this.blurWithoutInfluenceToolStripMenuItem.Click += new System.EventHandler(this.InplaceBlur);
            // 
            // showChannelHistogramsToolStripMenuItem
            // 
            this.showChannelHistogramsToolStripMenuItem.Name = "showChannelHistogramsToolStripMenuItem";
            this.showChannelHistogramsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.showChannelHistogramsToolStripMenuItem.Text = "Draw histograms";
            this.showChannelHistogramsToolStripMenuItem.Click += new System.EventHandler(this.DrawHistograms);
            // 
            // downsampleToolStripMenuItem
            // 
            this.downsampleToolStripMenuItem.Name = "downsampleToolStripMenuItem";
            this.downsampleToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.downsampleToolStripMenuItem.Text = "Downsample";
            this.downsampleToolStripMenuItem.Click += new System.EventHandler(this.Downsample);
            // 
            // seamCarvingToolStripMenuItem
            // 
            this.seamCarvingToolStripMenuItem.Name = "seamCarvingToolStripMenuItem";
            this.seamCarvingToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.seamCarvingToolStripMenuItem.Text = "Seam carving";
            this.seamCarvingToolStripMenuItem.Click += new System.EventHandler(this.SeamCarving);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showChannelsToolStripMenuItem,
            this.brightnessToolStripMenuItem,
            this.contrastToolStripMenuItem,
            this.gaussianBlurToolStripMenuItem,
            this.edgeDetectToolStripMenuItem,
            this.waterToolStripMenuItem,
            this.shiftedAndScaledToolStripMenuItem,
            this.convolutionFiltersToolStripMenuItem});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // showChannelsToolStripMenuItem
            // 
            this.showChannelsToolStripMenuItem.Name = "showChannelsToolStripMenuItem";
            this.showChannelsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showChannelsToolStripMenuItem.Text = "Show channels";
            this.showChannelsToolStripMenuItem.Click += new System.EventHandler(this.ShowChannels);
            // 
            // brightnessToolStripMenuItem
            // 
            this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
            this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.brightnessToolStripMenuItem.Text = "Brightness";
            this.brightnessToolStripMenuItem.Click += new System.EventHandler(this.Brightness);
            // 
            // contrastToolStripMenuItem
            // 
            this.contrastToolStripMenuItem.Name = "contrastToolStripMenuItem";
            this.contrastToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.contrastToolStripMenuItem.Text = "Contrast";
            this.contrastToolStripMenuItem.Click += new System.EventHandler(this.Contrast);
            // 
            // gaussianBlurToolStripMenuItem
            // 
            this.gaussianBlurToolStripMenuItem.Name = "gaussianBlurToolStripMenuItem";
            this.gaussianBlurToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.gaussianBlurToolStripMenuItem.Text = "Gaussian blur";
            this.gaussianBlurToolStripMenuItem.Click += new System.EventHandler(this.GaussianBlur);
            // 
            // edgeDetectToolStripMenuItem
            // 
            this.edgeDetectToolStripMenuItem.Name = "edgeDetectToolStripMenuItem";
            this.edgeDetectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.edgeDetectToolStripMenuItem.Text = "Edge detect";
            this.edgeDetectToolStripMenuItem.Click += new System.EventHandler(this.EdgeDetect);
            // 
            // waterToolStripMenuItem
            // 
            this.waterToolStripMenuItem.Name = "waterToolStripMenuItem";
            this.waterToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.waterToolStripMenuItem.Text = "Water";
            this.waterToolStripMenuItem.Click += new System.EventHandler(this.Water);
            // 
            // shiftedAndScaledToolStripMenuItem
            // 
            this.shiftedAndScaledToolStripMenuItem.Name = "shiftedAndScaledToolStripMenuItem";
            this.shiftedAndScaledToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.shiftedAndScaledToolStripMenuItem.Text = "Shifted and scaled";
            this.shiftedAndScaledToolStripMenuItem.Click += new System.EventHandler(this.ShiftedAndScaled);
            // 
            // convolutionFiltersToolStripMenuItem
            // 
            this.convolutionFiltersToolStripMenuItem.Name = "convolutionFiltersToolStripMenuItem";
            this.convolutionFiltersToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.convolutionFiltersToolStripMenuItem.Text = "Convolution filters";
            this.convolutionFiltersToolStripMenuItem.Click += new System.EventHandler(this.ConvolutionFilters);
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playWAVToolStripMenuItem,
            this.openWAVToolStripMenuItem});
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // playWAVToolStripMenuItem
            // 
            this.playWAVToolStripMenuItem.Name = "playWAVToolStripMenuItem";
            this.playWAVToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.playWAVToolStripMenuItem.Text = "Play WAV";
            this.playWAVToolStripMenuItem.Click += new System.EventHandler(this.PlayWAV);
            // 
            // openWAVToolStripMenuItem
            // 
            this.openWAVToolStripMenuItem.Name = "openWAVToolStripMenuItem";
            this.openWAVToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openWAVToolStripMenuItem.Text = "Edit WAV";
            this.openWAVToolStripMenuItem.Click += new System.EventHandler(this.EditWAV);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pictDimensions});
            this.statusBar.Location = new System.Drawing.Point(0, 403);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(790, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "+";
            // 
            // pictDimensions
            // 
            this.pictDimensions.Name = "pictDimensions";
            this.pictDimensions.Size = new System.Drawing.Size(0, 17);
            // 
            // imgContainer
            // 
            this.imgContainer.Controls.Add(this.thirdImgChannel);
            this.imgContainer.Controls.Add(this.secondImgChannel);
            this.imgContainer.Controls.Add(this.firstImgChannel);
            this.imgContainer.Controls.Add(this.allImgChannels);
            this.imgContainer.Controls.Add(this.mainPic);
            this.imgContainer.Location = new System.Drawing.Point(0, 27);
            this.imgContainer.Name = "imgContainer";
            this.imgContainer.Size = new System.Drawing.Size(783, 371);
            this.imgContainer.TabIndex = 2;
            this.imgContainer.TabStop = false;
            this.imgContainer.Text = "Image";
            // 
            // thirdImgChannel
            // 
            this.thirdImgChannel.Location = new System.Drawing.Point(404, 196);
            this.thirdImgChannel.Name = "thirdImgChannel";
            this.thirdImgChannel.Size = new System.Drawing.Size(378, 177);
            this.thirdImgChannel.TabIndex = 4;
            this.thirdImgChannel.TabStop = false;
            // 
            // secondImgChannel
            // 
            this.secondImgChannel.Location = new System.Drawing.Point(6, 196);
            this.secondImgChannel.Name = "secondImgChannel";
            this.secondImgChannel.Size = new System.Drawing.Size(376, 177);
            this.secondImgChannel.TabIndex = 3;
            this.secondImgChannel.TabStop = false;
            // 
            // firstImgChannel
            // 
            this.firstImgChannel.Location = new System.Drawing.Point(404, 0);
            this.firstImgChannel.Name = "firstImgChannel";
            this.firstImgChannel.Size = new System.Drawing.Size(379, 179);
            this.firstImgChannel.TabIndex = 2;
            this.firstImgChannel.TabStop = false;
            // 
            // allImgChannels
            // 
            this.allImgChannels.Location = new System.Drawing.Point(6, 0);
            this.allImgChannels.Name = "allImgChannels";
            this.allImgChannels.Size = new System.Drawing.Size(376, 179);
            this.allImgChannels.TabIndex = 1;
            this.allImgChannels.TabStop = false;
            // 
            // mainPic
            // 
            this.mainPic.Location = new System.Drawing.Point(6, 0);
            this.mainPic.Name = "mainPic";
            this.mainPic.Size = new System.Drawing.Size(776, 373);
            this.mainPic.TabIndex = 0;
            this.mainPic.TabStop = false;
            // 
            // loadImgDialog
            // 
            this.loadImgDialog.FileName = "image";
            this.loadImgDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.loadImagePath);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 425);
            this.Controls.Add(this.imgContainer);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Image Processor";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Resize += new System.EventHandler(this.OnResize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.imgContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thirdImgChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondImgChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstImgChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allImgChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImage;
        private System.Windows.Forms.ToolStripMenuItem saveImage;
        private System.Windows.Forms.ToolStripMenuItem exitProgram;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unsafeFilterToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.GroupBox imgContainer;
        private System.Windows.Forms.OpenFileDialog loadImgDialog;
        private System.Windows.Forms.PictureBox mainPic;
        private System.Windows.Forms.ToolStripStatusLabel pictDimensions;
        private System.Windows.Forms.PictureBox thirdImgChannel;
        private System.Windows.Forms.PictureBox secondImgChannel;
        private System.Windows.Forms.PictureBox firstImgChannel;
        private System.Windows.Forms.PictureBox allImgChannels;
        private System.Windows.Forms.ToolStripMenuItem showChannelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gaussianBlurToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contrastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMainImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blurWithoutInfluenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edgeDetectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showChannelHistogramsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChannelImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shiftedAndScaledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downsampleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCompressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCompressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWAVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playWAVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seamCarvingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoCtrZToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoCtrlYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convolutionFiltersToolStripMenuItem;
    }
}

