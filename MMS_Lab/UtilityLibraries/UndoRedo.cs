using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MMS_Lab.UtilityLibraries
{
    public class UndoRedo
    {
        Bitmap [] undoBuffer;
        Bitmap [] redoBuffer;

        public int currentUndoCount { get; set; }
        public int currentRedoCount { get; set; }

        public int bufferSize { get; set; }


        public UndoRedo()
        {
            this.bufferSize = int.Parse(ConfigurationManager.AppSettings["bufferSize"]);
            this.undoBuffer = new Bitmap[bufferSize];
            this.redoBuffer = new Bitmap[bufferSize];

            currentUndoCount = 0;
            currentRedoCount = 0;

        }

        public void AddToUndoBuffer(Bitmap image)
        {
            if (currentUndoCount == this.bufferSize)
            {
                for (int i = 0; i < undoBuffer.Length - 1; i++)
                {
                    this.undoBuffer[i] = this.undoBuffer[i + 1];
                }
                this.undoBuffer[undoBuffer.Length - 1] = image;
            }
            else
                this.undoBuffer[currentUndoCount] = image;

            currentUndoCount++;
        }

        public Bitmap GetFromUndoBuffer()
        {
            Bitmap image = null;

            if (currentUndoCount>0)
            {
                image = this.undoBuffer[currentUndoCount - 1];
            }
          
            if(currentUndoCount > 1)
                currentUndoCount--;

            return image;
        }

        public void AddToRedoBuffer(Bitmap image)
        {
         
            this.redoBuffer[currentRedoCount] = image;
            currentRedoCount++;
        }

        public Bitmap GetFromRedoBuffer()
        {
            Bitmap image = null;
            if(currentRedoCount>0)
            {
                image = this.redoBuffer[currentRedoCount - 1];
            }

            if (currentRedoCount>1)
               currentRedoCount--;

            return image;
        }
    }
}
