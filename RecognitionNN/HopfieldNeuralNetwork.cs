using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class HopfieldNeuralNetwork
    {
        public int sizeOfVector;



        public int[] y;
        public int[,] hop;
        public int[,] w;

        public int result;
        public int allBlack;
        public double resprocBlack;
        public double resprocWhite;
        public double resproc;

        //параметры
        public int pix = 100;
        public int iterations=200;
        public int PercentEnter=4;
        public int PercentExit=2;

        public int ch = 15;
        public int chWhite = 3;
        public int CountEnter = 40;
        public int CountExit = 20;


        public HopfieldNeuralNetwork(double[,] database,int mainIndex, KohonenNeuralNetwork kohonenNeuralNetwork) //перевод значений в бинарный формат
        {
            sizeOfVector = kohonenNeuralNetwork.sizeOfVector;
            y = new int[sizeOfVector];
            hop = new int[kohonenNeuralNetwork.maxClusters, kohonenNeuralNetwork.sizeOfVector];

            for (int i = 0; i < sizeOfVector; i++)
            {
                if (database[mainIndex, i] >this.pix) y[i] = 1;
                else y[i] = -1;
            }

            for (int i = 0; i < kohonenNeuralNetwork.maxClusters; i++)
            {
                for (int j = 0; j < kohonenNeuralNetwork.sizeOfVector; j++)
                {
                    if (kohonenNeuralNetwork.w[i, j] > this.pix)
                        hop[i, j] = 1;
                    else hop[i, j] = -1;
                }
            }

        }


        public int[,] CreateMemoryMatrix(int maxClusters)
        {
             w = new int[sizeOfVector, sizeOfVector];
            int[,] tm = new int[sizeOfVector, sizeOfVector];//Временный массив

            //Нахожу W
            for (int k = 0; k < maxClusters; k++)
            {
                //Транспонировать матрицу
                for (int i = 0; i < sizeOfVector; i++)
                    for (int j = 0; j < sizeOfVector; j++)
                        tm[i, j] = hop[k, i] * hop[k, j];

                //Нахожу сумму tm[i]
                for (int i = 0; i < sizeOfVector; i++)
                    for (int j = 0; j < sizeOfVector; j++)
                        w[i, j] += tm[i, j];
            }
            //Обнуляю главнаую диагональ w
            for (int i = 0; i < sizeOfVector; i++) w[i, i] = 0;

            return w;
        }
        public int[] ActivateFunction(ref int[] yy)
        {
            int[] a = new int[sizeOfVector];

            for (int i = 0; i < sizeOfVector; i++)
                for (int j = 0; j < sizeOfVector; j++)
                {
                    a[i] += w[i, j] * yy[j];
                }

            //Обрабатываю y' ф-ей активации
            for (int i = 0; i < sizeOfVector; i++)
                if (a[i] >= 0) a[i] = 1;
                else a[i] = -1;

            return a;
        }


        public void Method1(int[] yy, int num, ref int procBlack, ref int procWhite, ref int NotprocBlack, ref int NotprocWhite, ref int[] picture, ref int all)
        {
            for (int j = 0; j < sizeOfVector; j++)
            {
                if ((yy[j] == hop[num, j]) && (yy[j] == 1)) //если пиксель изображения совпадает с пикселем паттерна и имеет черный цвет
                {
                    procBlack++;
                    all++;
                    picture[j] = 1000;
                }
                if ((yy[j] == hop[num, j]) && (yy[j] == -1)) //если пиксель изображения совпадает с пикселем паттерна и имеет белый цвет
                {
                    procWhite++;
                }
                if ((yy[j] != hop[num, j]) && (yy[j] == 1)) //если пиксель изображения НЕ совпадает с пикселем паттерна и  изобрадение имеет черный цвет
                {
                    all++;
                    NotprocBlack++;

                }
                if ((y[j] != hop[num, j]) && (hop[num, j] == 1)) //если пиксель изображения НЕ совпадает с пикселем паттерна и  изображение имеет белый цвет
                {
                    NotprocWhite++;
                }
            }
        }
        public void Recognition1(int maxClusters, ref int iteration, ref int[] picture)
        {
            result = -1;
            resprocBlack = 0;
            resprocWhite = 0;

            int[] yy = new int[sizeOfVector];
            for (int i = 0; i < sizeOfVector; i++)
            {
                yy[i] = y[i];
            }

            // Первая проверка
            int maxProc = -sizeOfVector;
            double maxProcInPercent = 0.0;
            for (int num = 0; num < maxClusters; num++)
            {
                int procBlack = 0;
                int procWhite = 0;

                int NotprocBlack = 0;
                int NotprocWhite = 0;
                int all = 0;
                Method1(yy,num,ref procBlack, ref procWhite, ref NotprocBlack, ref NotprocWhite, ref picture, ref all);

                if (procBlack > maxProc)
                {
                    maxProc = procBlack;
                    resprocWhite = procWhite;
                    result = num;
                    allBlack = all;

                    maxProcInPercent =((double)maxProc / sizeOfVector)*100;
                    resprocBlack = maxProcInPercent;

                }
            }
            iteration = -1;

            if (sizeOfVector != 0 && maxClusters != 0 && maxProcInPercent < PercentEnter)
            {
                w = CreateMemoryMatrix(maxClusters);


                int[] a = new int[sizeOfVector];//ф-ия активации
                bool b = false; int k = -1;//Счётчик
                int l = -1;//Индекс подходящей строки

                // для вывода на экран
                int[] pict = new int[sizeOfVector];
                for (int i = 0; i < sizeOfVector; i++)
                {
                    pict[i] = 0;
                }

                while (!b && k < iterations)
                {
                    //Вычисляю y'
                    a = ActivateFunction(ref yy);

                    //Сравниваю
                    bool st = false;
                    for (int i = 0; i < maxClusters; i++)
                    {
                        st = true;
                        for (int j = 0; j < sizeOfVector; j++)
                            if (hop[i, j] != a[j]) { st = false; break; }
                        if (st) { l = i; break; }
                    }
                    if (l != -1)
                    {
                        b = true;
                    }
                    else
                    {
                        // Подсчет процентов
                        maxProc = 0;
                        maxProcInPercent = 0.0;
                        for (int num = 0; num < maxClusters; num++)
                        {
                            int procBlack = 0;
                            int procWhite = 0;

                            int NotprocBlack = 0;
                            int NotprocWhite = 0;
                            int all = 0;
                            Method1(a, num, ref procBlack, ref procWhite, ref NotprocBlack, ref NotprocWhite, ref picture, ref all);

                            if (procBlack > maxProc)
                            {
                                maxProc = procBlack;
                                resprocWhite = procWhite;
                                result = num;
                                allBlack = all;

                                maxProcInPercent = ((double)maxProc / sizeOfVector) * 100;
                                resprocBlack = maxProcInPercent;

                            }

                        }

                    }
                    if (maxProcInPercent > PercentExit) break;
                    else
                    {
                        k++;//Увеличиваю счетчик
                            //Обновляем у'
                        for (int i = 0; i < sizeOfVector; i++)
                            yy[i] = a[i];
                    }

                    //Обнуляем a
                    for (int i = 0; i < sizeOfVector; i++)
                        a[i] = 0;
                }
                iteration = k;
            }
        }

        public void FromVectorToMatrix(int  size,int num, int[] yy, ref double[,] tempArrY, ref double[,] tempArrMas)
        {

            int yIndex = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tempArrY[i, j] = yy[yIndex];
                    tempArrMas[i, j] = hop[num, yIndex];
                    yIndex++;
                }
            }
        }
        public void Method2(int size, double[,] tempArrY, double[,] tempArrMas, ref int proc, ref int procWhite, ref int procCol, ref int procColWhite,
            ref int countStringsWithBlack, ref int countStringsWithWhite, ref int countColumnsWithBlack, ref int countColumnsWithWhite, ref int[]picture)
        {
            int black = 0, white = 0; // совпавшие черные и белые
            int countBlackY = 0, countBlackMas = 0;
            int countWhiteY = 0, countWhiteMas = 0;
            //по строкам
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (tempArrY[i, j] == 1) countBlackY++;
                    if (tempArrMas[i, j] == 1) countBlackMas++;
                    if (tempArrY[i, j] == -1) countWhiteY++;
                    if (tempArrMas[i, j] == -1) countWhiteMas++;

                    if ((tempArrY[i, j] == tempArrMas[i, j]) && (tempArrY[i, j] == -1))
                    {
                        white++;
                    }
                    if ((tempArrY[i, j] == tempArrMas[i, j]) && (tempArrY[i, j] == 1))
                    {
                        black++;
                        picture[i * size + j] = 1000;
                    }

                }
                if ((countBlackY != 0) || (countBlackMas != 0)) countStringsWithBlack++;
                if ((countWhiteY != 0) || (countWhiteMas != 0)) countStringsWithWhite++;
                double t1 = 0, t2 = 0;
                double t3 = 0, t4 = 0;
                if (countBlackY == 0) t1 = 0;
                else
                    t1 = ((double)black / countBlackY) * 100;
                if (countBlackMas == 0) t2 = 0;
                else
                    t2 = ((double)black / countBlackMas) * 100;

                if (countWhiteY == 0) t3 = 0;
                else
                    t3 = ((double)white / countWhiteY) * 100;
                if (countWhiteMas == 0) t4 = 0;
                else
                    t4 = ((double)white / countWhiteMas) * 100;

                if ((Math.Abs(t1 - t2) < ch) && (black != 0) && ((countBlackY != 0) || (countBlackMas != 0))) proc++;
                if ((Math.Abs(t3 - t4) < chWhite) && (white != 0) && ((countWhiteY != 0) || (countWhiteMas != 0))) procWhite++;

                black = 0; white = 0; countBlackY = 0; countBlackMas = 0; countWhiteY = 0; countWhiteMas = 0;
            }

            //по столбцам
            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    if (tempArrY[i, j] == 1) countBlackY++;
                    if (tempArrMas[i, j] == 1) countBlackMas++;
                    if (tempArrY[i, j] == -1) countWhiteY++;
                    if (tempArrMas[i, j] == -1) countWhiteMas++;

                    if ((tempArrY[i, j] == tempArrMas[i, j]) && (tempArrY[i, j] == -1))
                    {
                        white++;
                    }
                    if ((tempArrY[i, j] == tempArrMas[i, j]) && (tempArrY[i, j] == 1))
                    {
                        black++;
                    }

                }
                if ((countBlackY != 0) || (countBlackMas != 0)) countColumnsWithBlack++;
                if ((countWhiteY != 0) || (countWhiteMas != 0)) countColumnsWithWhite++;
                double t1 = 0, t2 = 0;
                double t3 = 0, t4 = 0;
                if (countBlackY == 0) t1 = 0;
                else
                    t1 = ((double)black / countBlackY) * 100;
                if (countBlackMas == 0) t2 = 0;
                else
                    t2 = ((double)black / countBlackMas) * 100;

                if (countWhiteY == 0) t3 = 0;
                else
                    t3 = ((double)white / countWhiteY) * 100;
                if (countWhiteMas == 0) t4 = 0;
                else
                    t4 = ((double)white / countWhiteMas) * 100;

                if ((Math.Abs(t1 - t2) < ch) && (black != 0) && ((countBlackY != 0) || (countBlackMas != 0))) procCol++;
                if ((Math.Abs(t3 - t4) < chWhite) && (white != 0) && ((countWhiteY != 0) || (countWhiteMas != 0))) procColWhite++;

                black = 0; white = 0; countBlackY = 0; countBlackMas = 0; countWhiteY = 0; countWhiteMas = 0;
            }
        }
        public void Recognition2(int size, int maxClusters, ref int iteration, ref int[] picture)
        {
            result = -1;
            resprocBlack = 0;
            resprocWhite = 0;
            resproc = 0;

            int[] yy = new int[sizeOfVector];
            for (int i = 0; i < sizeOfVector; i++)
            {
                yy[i] = y[i];
            }

            int[] compareStrings = new int[size];
            int[] compareColumns = new int[size];

            // Первая проверка

            double maxProc = 0;


            for (int num = 0; num < maxClusters; num++)
            {
                int proc = 0, procWhite = 0;
                int procCol = 0, procColWhite = 0;


                // Заполнение из вектора в матрицы
                double[,] tempArrY = new double[size, size];  
                double[,] tempArrMas = new double[size, size]; 
                FromVectorToMatrix(size,num, yy, ref tempArrY, ref tempArrMas);

                // f-метрика
                int countStringsWithBlack = 0, countStringsWithWhite = 0;
                int countColumnsWithBlack = 0, countColumnsWithWhite = 0;
                double res = 0;
                Method2(size, tempArrY, tempArrMas, ref proc, ref procWhite,
                    ref procCol, ref procColWhite, ref countStringsWithBlack, ref countStringsWithWhite,
                    ref countColumnsWithBlack, ref countColumnsWithWhite, ref picture);


                res =( (double)proc / countStringsWithBlack) * 100 + ((double)procWhite / countStringsWithWhite) * 100 + ((double)procCol / countColumnsWithBlack )* 100 + ((double)procColWhite / countColumnsWithWhite) * 100;
                res = res / 4;
                if (res > maxProc)
                {
                    maxProc = res;
                    resproc = maxProc;
                    //resprocBlack = maxProc;
                    result = num;

                }
            }

            iteration = -1;
            if (sizeOfVector != 0 && maxClusters != 0 && resproc < CountEnter)
            {

                int k = 0;

                w = CreateMemoryMatrix(maxClusters);


                int[] a = new int[sizeOfVector];//ф-ия активации
                bool b = false; k = -1;//Счётчик
                int l = -1;//Индекс подходящей строки

                // для вывода на экран
                int[] pict = new int[sizeOfVector];
                for (int i = 0; i < sizeOfVector; i++)
                {
                    pict[i] = 0;
                }

                while (!b && k < iterations)
                {
                    //Вычисляю y'
                    a = ActivateFunction(ref y);

                    //Сравниваю
                    bool st = false;
                    for (int i = 0; i < maxClusters; i++)
                    {
                        st = true;
                        for (int j = 0; j < sizeOfVector; j++)
                            if (hop[i, j] != a[j]) { st = false; break; }
                        if (st) { l = i; break; }
                    }
                    if (l != -1)
                    {
                        b = true;
                    }
                    else
                    {
                        // Подсчет процентов
                        maxProc = 0;

                        for (int num = 0; num < maxClusters; num++)
                        {
                            int proc = 0, procWhite = 0;
                            int procCol = 0, procColWhite = 0;


                            // Заполнение из вектора в матрицы
                            double[,] tempArrY = new double[size, size];  
                            double[,] tempArrMas = new double[size, size]; 
                            int yIndex = 0;
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    tempArrY[i, j] = yy[yIndex];
                                    tempArrMas[i, j] = hop[num, yIndex];
                                    yIndex++;
                                }
                            }

                            // f-метрика
                            int countStringsWithBlack = 0, countStringsWithWhite = 0;
                            int countColumnsWithBlack = 0, countColumnsWithWhite = 0;
                            double res = 0;
                            Method2(size, tempArrY, tempArrMas, ref proc, ref procWhite,
                                ref procCol, ref procColWhite, ref countStringsWithBlack, ref countStringsWithWhite,
                                ref countColumnsWithBlack, ref countColumnsWithWhite, ref picture);
                           

                            res = (double)proc / countStringsWithBlack * 100 + (double)procWhite / countStringsWithWhite * 100 + (double)procCol / countColumnsWithBlack * 100 + (double)procColWhite / countColumnsWithWhite * 100;
                            res = res / 4;
                            if (res > maxProc)
                            {
                                maxProc = res;
                                resproc = maxProc;

                                result = num;

                            }
                        }

                    }
                    if (resproc > CountExit) break;
                    else
                    {
                        k++;//Увеличиваю счетчик
                            //Обновляем у'
                        for (int i = 0; i < sizeOfVector; i++)
                            y[i] = a[i];
                    }

                    //Обнуляем a
                    for (int i = 0; i < sizeOfVector; i++)
                        a[i] = 0;
                }
                iteration = k;
            }
        }


       
    }
}
