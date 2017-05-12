using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.SeamCarving
{
    public class PixelPos
    {
        public int x { get; set; }
        public int y { get; set; }


        public PixelPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(PixelPos other)
        {
            return x.CompareTo(other.x);
        }
    }
}
