using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMS_Lab.Controller;
using System.Drawing;
using System.Windows.Forms;

namespace MMS_Lab.View
{
    public interface IView
    {
        void AddListener(IController controller);
        void ShowImage(Image img, string imgName);

        void ShowImages(Bitmap unffiltImg, Bitmap firstChannel, Bitmap secondChannel, Bitmap thirdChannel, string imgName);

        void MakeChannelsVisible();
        void MakeChannelsInvisible();

        void ChangeChannelsVisibility();

        //long[] GetHistogram(Bitmap picture);
        //void DrawHistogram(PaintEventArgs e, Bitmap bmp);
       // int getUndoBufferSize();
    }
}
