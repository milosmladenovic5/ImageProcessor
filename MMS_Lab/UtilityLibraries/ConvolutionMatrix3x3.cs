using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.UtilityLibraries
{
    public class ConvolutionMatrix3x3
    {
        public ConvolutionMatrix3x3()
        {
            Pixel = 1;
            Factor = 1;
        }

        public void Apply(int Val)
        {
            TopLeft = TopMid = TopRight = MidLeft = MidRight = BottomLeft = BottomMid = BottomRight = Pixel = Val;
        }

        public int TopLeft { get; set; }

        public int TopMid { get; set; }

        public int TopRight { get; set; }

        public int MidLeft { get; set; }

        public int MidRight { get; set; }

        public int BottomLeft { get; set; }

        public int BottomMid { get; set; }

        public int BottomRight { get; set; }

        public int Pixel { get; set; }

        public int Factor { get; set; }

        public int Offset { get; set; }

        public int Factor5x5 { get; set; }

        public int Factor7x7 { get; set; }
    }
}
