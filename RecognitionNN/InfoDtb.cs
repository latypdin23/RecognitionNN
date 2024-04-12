using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class InfoDtb
    {
        public int trainCount;
        public int testCount;
        public  int width;
        public  int height;
        public InfoDtb(int trainCount,int testCount,int width,int height)
        {
            this.trainCount = trainCount;
            this.testCount = testCount;
            this.width = width;
            this.height = height;
        }

        public double[,] arr;
        public double[,] arrT;
        public byte[] label;
        public byte[] labelT;
    }
}
