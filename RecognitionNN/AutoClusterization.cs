using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class AutoClusterization:Distance
    {
        public int sizeOfVector;
        public int countOfNumbers;

        public AutoClusterization(int sizeOfVector,int countOfNumbers)
        {
            this.sizeOfVector = sizeOfVector;
            this.countOfNumbers = countOfNumbers;

        }
        public void EvclidDistance(int vectorNumber, double[,] t, double[,] d,int vectors)
        {
            for (int i = 0; i < vectors; i++)
            {
                for (int j = 0; j < sizeOfVector; j++)
                {
                    d[vectorNumber, i] += Math.Pow((t[i, j] - t[vectorNumber, j]), 2);
                }
            }
        }

        public void ComputeAndWritingIntoFile(string fileName,string fileName2, double maxDist, double[,] distance,int vectors)
        {
            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter swr = new StreamWriter(aFile);
            aFile.Seek(0, SeekOrigin.End);

            FileStream aFile2 = new FileStream(fileName2, FileMode.OpenOrCreate);
            StreamWriter swr2 = new StreamWriter(aFile2);
            aFile2.Seek(0, SeekOrigin.End);

            for (double x = 0.1; x <= 0.9; x += 0.02)
            {
                // 3 этап
                double normalDist = maxDist * x;
                swr2.WriteLine(normalDist);
                // 4 этап
                AutoCluster[] table = new AutoCluster[vectors];
                for (int i = 0; i < vectors; i++)
                {
                    table[i] = new AutoCluster();
                    table[i].index = i;
                    table[i].NumCluster = -1;
                    table[i].flag = false;
                }
                int cluster = 0;
                for (int num = 0; num < vectors; num++)
                {
                    if (table[num].flag == false)
                    {
                        for (int i = num; i < vectors; i++)
                        {
                            if (distance[num, i] <= normalDist)
                            {
                                table[i].flag = true;
                            }
                            else
                            {
                                cluster++;
                                break;
                            }
                        }
                    }
                }
                swr.WriteLine((cluster + 1).ToString());

            }
            swr.Close();
            swr2.Close();

        }
        public void Training(double[,] pattern,int vectors, int num)
        {
            double[,] distance = new double[vectors, vectors];
            //1 этап
            for (int vecNum = 0; vecNum < vectors; vecNum++)
            {
                EvclidDistance(vecNum, pattern, distance,vectors);
            }
            //2 этап
            double maxDist = distance[0, 0];
            for (int i = 0; i < vectors; i++)
            {
                for (int j = 0; j < vectors; j++)
                {
                    if (distance[i, j] > maxDist)
                        maxDist = distance[i, j];
                }
            }

            // Для всех процентов
            string fileName = "CountOfCluster" + num.ToString() + ".txt";
            string fileName2 = "RadiusOfCluster" + num.ToString() + ".txt";
            ComputeAndWritingIntoFile(fileName,fileName2, maxDist, distance, vectors);

        }

    }
}
