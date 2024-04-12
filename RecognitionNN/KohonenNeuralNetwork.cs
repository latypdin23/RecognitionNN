using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class KohonenNeuralNetwork
    {
        public int maxClusters;
        public int sizeOfVector;
        public double[,] w;
        public double[] d;
        public int vectors;

        public KohonenNeuralNetwork(int sizeOfVector,int vectors)
        {
            this.sizeOfVector = sizeOfVector;
            this.vectors = vectors;
        }

        public static double decayRate = 0.96;
        public static double min_h = 0.01;
        public static double h = 0.6;



        public void EvclidDist(int vectorNumber, double[,] t)
        {
            for (int i = 0; i < maxClusters; i++)
            {
                d[i] = 0.0;
                for (int j = 0; j < sizeOfVector; j++)
                {
                    d[i] += Math.Pow((w[i, j] - t[vectorNumber, j]), 2);
                }
            }
        }
        public int Minimum()
        {
            double min = d[0];
            int cl = 0;
            for (int i = 1; i < maxClusters; i++)
                if (d[i] < min)
                {
                    min = d[i]; cl = i;
                }
            return cl;
        }
        public void Training(double[,] pattern)
        {
            int iterations = 0;
            int dMin;
            do
            {
                iterations += 1;

                for (int vecNum = 0; vecNum < vectors; vecNum++)
                {
                    //расстояние
                    EvclidDist(vecNum, pattern);
                    //нейрон победитель
                    dMin = Minimum();

                    //обновление весов
                    for (int i = 0; i < sizeOfVector; i++)
                    {
                        w[dMin, i] = w[dMin, i] + (h * (pattern[vecNum, i] - w[dMin, i]));
                    }
                }
                //обновление функции "соседства нейронов".
                h = decayRate * h;

            } while (h > min_h);
        }
    }
}
