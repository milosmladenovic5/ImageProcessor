using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MMS_Lab.Model
{
    public class Model : IModel
    {
        string imageName;
        Bitmap mainImg;

        Bitmap YChannel;
        Bitmap CbChannel;
        Bitmap CrChannel;

        public Bitmap getMainImage()
        {
            return mainImg;
        }

        public void setYChannel(Bitmap img)
        {
            this.YChannel = img;
        }

        public Bitmap getYChannel()
        {
            return YChannel;
        }

        public void setCbChannel(Bitmap img)
        {
            this.CbChannel = img;
        }

        public Bitmap getCbChannel()
        {
            return CbChannel;
        }

        public void setCrChannel(Bitmap img)
        {
            this.CrChannel = img;
        }
        
        public Bitmap getCrChannel()
        {
            return CrChannel;
        }

        public string getImageName()
        {
            return this.imageName;
        }

        public void setImageName(string name)
        {
            this.imageName = name;
        }

        public void setMainImage(Bitmap img)
        {
            this.mainImg = img;
        }


    }
}
