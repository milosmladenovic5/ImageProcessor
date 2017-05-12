using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMS_Lab.Controller
{
    public interface IController
    {
        void onLoadImageClick(string imgPath);
        void onSaveImageClick(Image img);
        void unsafeClick();

        void showChannelsClick();

        void brightnessClick();
        void contrastClick();
        void gaussianBlurClick(bool influence);
        void edgeDetectClick();
        void waterClick();
        void shiftedAndScaledClick();

        void showMainImage();
        //void DrawHistogram(string channel, PaintEventArgs e);
        void DrawHistograms();

        void VariableConvolutionFiltersClick();

        void undoClick();
        void redoClick();

        void DownsampleClick();

        void SaveCompressedClick();

        void LoadCompressedClick();

        void showChannels();

        void EditWAVFileClick();
        void PlayWAWFileClick();

        void SeamCarvingClick();

        Bitmap[] GetYCbCrChannels();    
    }
}
