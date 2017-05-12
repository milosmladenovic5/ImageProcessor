using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab.Compression
{
    public class HuffmanTree
    {
        private HuffmanNode root;
        private Dictionary<byte, List<bool>> compressionDict = new Dictionary<byte, List<bool>>();
        private Dictionary<string, byte> decompressionDict = new Dictionary<string, byte>();

        public void DeserializeDictionary(byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                this.decompressionDict = formatter.Deserialize(stream) as Dictionary<string, byte>;
            }   
        }

        public BitArray Encode(byte[] data)
        {
            List<bool> res = new List<bool>();

            for (int i=0; i<data.Length; i++)
               res.AddRange(this.compressionDict[data[i]]);

            return (new BitArray(res.ToArray()));
        }

        public byte[] Decode (BitArray encodedData)
        {
            string key = string.Empty;
            List<byte> res = new List<byte>();

            for (int i=0; i<encodedData.Length; i++)
            {
                if(encodedData[i]==true)
                {
                    key += '1';
                }
                else
                {
                    key += '0';
                }

                if (this.decompressionDict.ContainsKey(key))
                {
                    res.Add(this.decompressionDict[key]);
                    key = string.Empty;
                }
            }

            return res.ToArray();
        }

        //kreiranje stabla iz liste cvorova
        public void CreateTree(List<HuffmanNode> nodesList)
        {
            while(nodesList.Count>1)
            {
                HuffmanNode firstNode = nodesList[0];
                nodesList.RemoveAt(0);
                HuffmanNode secondNode = nodesList[0];
                nodesList.RemoveAt(0);
                nodesList.Add(new HuffmanNode(firstNode, secondNode));
                nodesList.Sort();
            }
            this.root = nodesList.First();

            SetCodesToTree(string.Empty, this.root);
        }

        //treba serijalizovati recnik za dekompresiju u niz bajtova da bi se upisao sa fajlom\
        public byte[] SerializeDictionaryToBytes()
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, this.decompressionDict);
                return stream.ToArray();
            }
        }

        public List<HuffmanNode> GenerateListOfNodes(byte[] data)
        {
            List<HuffmanNode> nodeList = new List<HuffmanNode>();
            int[] statistics = GetFrequency(data);

            for(int i=0; i<256; i++)
                nodeList.Add(new HuffmanNode((byte)i, statistics[i]));

            nodeList.Sort();
            return nodeList;
        }

        public int [] GetFrequency(byte[] data)//ne poziva se eksterno
        {
            int[] ret = new int[256];

            for(int i=0; i<data.Length; i++)
            {
                ret[data[i]]++;
            }
            return ret;
        }

        public void SetCodesToTree(string code, HuffmanNode root)
        {
            if (root == null)
                return;

            if (root.leftChild == null && root.rightChild == null)
            {
                this.compressionDict.Add(root.value, StringToBoolList(code));
                this.decompressionDict.Add(code, root.value);
                return;
            }

            SetCodesToTree(code + '0', root.leftChild);
            SetCodesToTree(code + '1', root.rightChild);
        }
        
        public List<bool> StringToBoolList(string code)
        {
            List<bool> boolList = new List<bool>();
            for(int i=0; i<code.Length; i++)
            {
                if (code[i] == '0')
                    boolList.Add(false);
                else if (code[i] == '1')
                    boolList.Add(true);
            }

            return boolList;
        }
    }
}
