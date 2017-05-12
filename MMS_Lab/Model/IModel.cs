using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.Model
{
    public interface IModel
    {
        void setMainImage(Bitmap img);
        Bitmap getMainImage();

        void setYChannel(Bitmap img);
        Bitmap getYChannel();

        void setCbChannel(Bitmap img);
        Bitmap getCbChannel();

        void setCrChannel(Bitmap img);
        Bitmap getCrChannel();

        void setImageName(string name);
        string getImageName();

        //void SetBufferSize(int size);

       // void AddToUndoBuffer(Bitmap image);
        //Bitmap GetFromUndoBuffer();

        //void AddToRedoBuffer(Bitmap image);
        //Bitmap GetFromRedoBuffer();

       // void applyFilter(string filter);
       // string getLastAppliedFilter();
    }
}
