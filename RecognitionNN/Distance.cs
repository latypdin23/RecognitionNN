using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    interface Distance
    {
        void EvclidDistance(int vectorNumber, double[,] t, double[,] d,int vectors);
    }
}
