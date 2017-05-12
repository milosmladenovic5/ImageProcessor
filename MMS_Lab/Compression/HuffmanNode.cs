using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.Compression
{
    public class HuffmanNode: IComparable<HuffmanNode>
    {
        public byte value;
        public string code;
        public int frequency;
        public HuffmanNode leftChild;
        public HuffmanNode rightChild;

        public HuffmanNode(byte value, int frequency)
        {
            this.value = value;
            this.frequency = frequency;

            this.rightChild = this.leftChild = null;
        }

        public HuffmanNode(HuffmanNode lc, HuffmanNode rc)
        {
            this.leftChild = lc;
            this.rightChild = rc;

            if(lc.frequency >= rc.frequency)
            {
                HuffmanNode add = lc;
                lc = rc;
                rc = add; 
            }

            this.frequency = lc.frequency + rc.frequency;
        }

        public int CompareTo(HuffmanNode otherNode) 
        {
            return this.frequency.CompareTo(otherNode.frequency);
        }
        
    }
}
