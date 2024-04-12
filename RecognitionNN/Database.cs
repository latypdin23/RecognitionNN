using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class Database
    {

        public  int width;
        public  int height;
        public double[,] arrObj;
        public double[,] arrObjT;

        public int countOfImagesForeachNumberTrain;
        public int countOfImagesForeachNumberTest;

        public Database(int n, int m,int height,int width)
        {
            this.height = height; this.width = width;
            arrObj = new double[n, height*width];
            arrObjT = new double[m, height * width];
        }

        public void SeparateDatabase(ref int testValue, ref int[,] arrayForNumber, ref int[,] testArray, int index)
        {
            testValue++;
            for (int j = 0; j < height*width; j++)
            {
                arrayForNumber[testValue, j] = testArray[index, j];
            };
        }

    }
}
