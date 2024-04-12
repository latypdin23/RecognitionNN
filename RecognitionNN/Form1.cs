using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NLog;
using NLog.Config;

namespace RecognitionNN
{
    public partial class Form1 : Form
    {
        public NLog.Logger logger = LogManager.GetLogger("Logs");

        public string databaseFile; //для другой базы
        public  InfoDtb dtb;


        public List<Database> databaseForRecognition = new List<Database>();
        public List<KohonenNeuralNetwork> kohonenNeuralNetwork = new List<KohonenNeuralNetwork>();
        public int countOfNumbers = 10;
        public int mainLabelForRecognition;
        public int mainNumberForRecognition;

        public List<bool> ControlList = new List<bool>();
        public void SeparateDatabase(ref int testValue, ref double[,] arrayForNumber, ref int[,] testArray, int index)
        {
            testValue++;
            for (int j = 0; j < dtb.height * dtb.width; j++)
            {
                arrayForNumber[testValue, j] = (double)testArray[index, j];
            };
        }
        public void CreateControlList()
        {
            for(int i=0;i<5;i++)
            {
                bool flag = false;
                ControlList.Add(flag);
            }
        }
        public void DownloadMNISTDatabase(string pixelFile, string labelFile, string pixelFile2, string labelFile2)
        {
            try
            {
                CreateControlList();

                MNIST mnist = new MNIST(dtb.trainCount,dtb.testCount,dtb.height*dtb.width);
                mnist.trainImages = MNIST.LoadData(dtb.trainCount, pixelFile, labelFile, mnist.arr);
                richTextBox1.Text += "The training sample was loaded successfully\n";
                logger.Info("The training sample was loaded successfully");
                mnist.testImages = MNIST.LoadData(dtb.testCount, pixelFile2, labelFile2, mnist.arrT);
                richTextBox1.Text += "The test sample was loaded successfully\n";
                logger.Info("The test sample was loaded successfully");

                dtb.arr = new double[dtb.trainCount, dtb.height * dtb.width];
                dtb.arrT = new double[dtb.testCount, dtb.height * dtb.width];
                dtb.label = new byte[dtb.trainCount];
                dtb.labelT = new byte[dtb.testCount];
                for (int i = 0; i < dtb.trainCount; i++)
                {
                    for (int j = 0; j < dtb.height * dtb.width; j++)
                        dtb.arr[i, j] = (double)(mnist.arr[i, j]);
                    dtb.label[i] = mnist.trainImages[i].label;
                }
                for (int i = 0; i < dtb.testCount; i++)
                {
                    for (int j = 0; j < dtb.height * dtb.width; j++)
                        dtb.arrT[i, j] = (double)(mnist.arrT[i, j]);
                    dtb.labelT[i] = mnist.testImages[i].label;
                }


                // Загрузка отдельных изображений в разные массивы
                var learningSample = new int[countOfNumbers];
                var testSample = new int[countOfNumbers];

                for (int i = 0; i < countOfNumbers; i++)
                {
                    learningSample[i] = -1;
                    testSample[i] = -1;
                }

                // Загрузка отдельных изображений в разные массивы
                int[] k = new int[countOfNumbers]; int[] kT = new int[countOfNumbers];
                for (int i = 0; i < countOfNumbers; i++)
                {
                    k[i] = -1;
                    kT[i] = -1;

                    Database obj = new Database(7000,2000,dtb.height,dtb.width);

                    databaseForRecognition.Add(obj);

                }

                for (int i = 0; i < dtb.trainCount; i++)
                {
                    SeparateDatabase(ref learningSample[mnist.trainImages[i].label], ref databaseForRecognition[mnist.trainImages[i].label].arrObj, ref mnist.arr, i);
                }
                for (int i = 0; i < dtb.testCount; i++)
                {
                    SeparateDatabase(ref testSample[mnist.testImages[i].label], ref databaseForRecognition[mnist.testImages[i].label].arrObjT, ref mnist.arrT, i);
                }

                for (int i = 0; i < countOfNumbers; i++)
                {
                    databaseForRecognition[i].countOfImagesForeachNumberTrain = learningSample[i];
                    databaseForRecognition[i].countOfImagesForeachNumberTest = testSample[i];

                    learningSample[i]++;
                    testSample[i]++;

                    infoAboutDatabase.Rows.Add(i.ToString(), learningSample[i].ToString(), testSample[i].ToString());
                }
            }
            catch (Exception e)
            {
                richTextBox1.Clear();
                richTextBox1.Text += e.Message;
                logger.Error(e.Message.ToString());
                return;
            }
        }

        public void DownloadFashionMNISTDatabase(string pixelFile, string labelFile, string pixelFile2, string labelFile2)
        {
            try
            {
                CreateControlList();

                MNIST mnist = new MNIST(dtb.trainCount, dtb.testCount, dtb.height * dtb.width);
                mnist.trainImages = MNIST.LoadData(dtb.trainCount, pixelFile, labelFile, mnist.arr);
                richTextBox1.Text += "The training sample was loaded successfully\n";
                logger.Info("The training sample was loaded successfully");
                mnist.testImages = MNIST.LoadData(dtb.testCount, pixelFile2, labelFile2, mnist.arrT);
                richTextBox1.Text += "The testing sample was loaded successfully\n";
                logger.Info("The testing sample was loaded successfully");

                dtb.arr = new double[dtb.trainCount, dtb.height * dtb.width];
                dtb.arrT = new double[dtb.testCount, dtb.height * dtb.width];
                dtb.label = new byte[dtb.trainCount];
                dtb.labelT = new byte[dtb.testCount];
                for (int i = 0; i < dtb.trainCount; i++)
                {
                    for (int j = 0; j < dtb.height * dtb.width; j++)
                        dtb.arr[i, j] = (double)(mnist.arr[i, j]);
                    dtb.label[i] = mnist.trainImages[i].label;
                }
                for (int i = 0; i < dtb.testCount; i++)
                {
                    for (int j = 0; j < dtb.height * dtb.width; j++)
                        dtb.arrT[i, j] = (double)(mnist.arrT[i, j]);
                    dtb.labelT[i] = mnist.testImages[i].label;
                }


                // Загрузка отдельных изображений в разные массивы
                var learningSample = new int[countOfNumbers];
                var testSample = new int[countOfNumbers];

                for (int i = 0; i < countOfNumbers; i++)
                {
                    learningSample[i] = -1;
                    testSample[i] = -1;
                }

                // Загрузка отдельных изображений в разные массивы
                int[] k = new int[countOfNumbers]; int[] kT = new int[countOfNumbers];
                for (int i = 0; i < countOfNumbers; i++)
                {
                    k[i] = -1;
                    kT[i] = -1;

                    Database obj = new Database(7000, 2000, dtb.height, dtb.width);

                    databaseForRecognition.Add(obj);

                }

                for (int i = 0; i < dtb.trainCount; i++)
                {
                    SeparateDatabase(ref learningSample[mnist.trainImages[i].label], ref databaseForRecognition[mnist.trainImages[i].label].arrObj, ref mnist.arr, i);
                }
                for (int i = 0; i < dtb.testCount; i++)
                {
                    SeparateDatabase(ref testSample[mnist.testImages[i].label], ref databaseForRecognition[mnist.testImages[i].label].arrObjT, ref mnist.arrT, i);
                }

                for (int i = 0; i < countOfNumbers; i++)
                {
                    databaseForRecognition[i].countOfImagesForeachNumberTrain = learningSample[i];
                    databaseForRecognition[i].countOfImagesForeachNumberTest = testSample[i];

                    learningSample[i]++;
                    testSample[i]++;

                    infoAboutDatabase.Rows.Add(i.ToString(), learningSample[i].ToString(), testSample[i].ToString());
                }
            }
            catch (Exception e)
            {
                richTextBox1.Clear();
                richTextBox1.Text += e.Message;
                logger.Error(e.Message);
                return;
            }
        }
        public Form1()
        {
            InitializeComponent();

        }

        private void ParsingFilling(ref int indJ, ref int[,] ser, string[] s)
        {
            for (int i = 0; i < 40; i++)
            {
                ser[indJ, i] = int.Parse(s[i]);
            }
        }
        private void ReadClustersFromFile(string fileName)
        {
            int step = 40;
            int countObjects = countOfNumbers;
            int[,] seria = new int[countObjects, step];


            for (int j = 0; j < countObjects; j++)
            {
                string[] s = File.ReadAllLines(fileName + j.ToString() + ".txt");
                ParsingFilling(ref j, ref seria, s);                
            }

            for (int j = 0; j < countObjects; j++)
            {
                KohonenNeuralNetwork knn = new KohonenNeuralNetwork(dtb.height*dtb.width,databaseForRecognition[j].countOfImagesForeachNumberTrain); ///rework
                for (int i = 0; i < step; i++)
                {
                    if (seria[j, i] <= 50)
                    {
                        knn.maxClusters = seria[j, i];
                        knn.w= new double[knn.maxClusters, knn.sizeOfVector];
                        knn.d = new double[knn.maxClusters];
                        break;
                    }                 
                }
                kohonenNeuralNetwork.Add(knn);
            }
            for (int i = 0; i < countObjects; i++)
            {
                InfoAboutClusters.Rows.Add(i.ToString(), kohonenNeuralNetwork[i].maxClusters.ToString());
            }
            richTextBox1.Text += "The optimal number of clusters for each digit is determined\n";
            logger.Info("The optimal number of clusters for each digit is determined");
        }
        private void ComputeClusters()
        {
            const string message = "Do you need to do training for automatic clustering?";
            const string caption = "Message";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            AutoClusterization autoClusterization = new AutoClusterization(dtb.height*dtb.width,countOfNumbers);
            List<double[,]> autoDatabasePattern = new List<double[,]>();
            List<double[,]> autoDatabaseTest = new List<double[,]>();

            //запрашивать обучение или нет
            if (result == DialogResult.Yes)
            {

                //массив для обучения
                for (int k = 0; k < countOfNumbers; k++)
                {
                    int vectors = databaseForRecognition[k].countOfImagesForeachNumberTrain;
                    int tesVec = databaseForRecognition[k].countOfImagesForeachNumberTest;
                    int sizePicture = dtb.height * dtb.width;
                    int[] temp = new int[vectors];

                    double[,] pattern = new double[vectors, sizePicture];
                    double[,] tests = new double[tesVec, sizePicture];


                    Random rand = new Random();

                    for (int i = 0; i < vectors; i++)
                    {
                        int ind = rand.Next(0, vectors);
                        temp[i] = ind;
                        for (int j = 0; j < sizePicture; j++)
                        {
                            pattern[i, j] = databaseForRecognition[k].arrObj[ind, j];
                        }
                    }

                    autoDatabasePattern.Add(pattern);
                    autoDatabaseTest.Add(tests);

                    autoClusterization.Training(pattern,vectors,k);
                }

                // Обучение
                richTextBox1.Text += "The number of clusters is determined depending on the allowed cluster radius\n";
                logger.Info("The number of clusters is determined depending on the allowed cluster radius");
                string fileName = "CountOfCluster";
                ReadClustersFromFile(fileName);
            }
            else
            {
                string fileName = "CountOfCluster";
                ReadClustersFromFile(fileName);
            }
        }
        private void CreateClusters()
        {
            const string message = "Do you need to do training for Kohonen Neural Network?";
            const string caption = "Message";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if(result==DialogResult.Yes)
            {
                try
                {

                    Random[] r = new Random[countOfNumbers];
                    Random[] rand = new Random[countOfNumbers];
                    for (int k = 0; k < countOfNumbers; k++)
                    {
                        r[k] = new Random();
                        rand[k] = new Random();

                        for (int i = 0; i < kohonenNeuralNetwork[k].maxClusters; i++)
                        {
                            for (int j = 0; j < kohonenNeuralNetwork[k].sizeOfVector; j++)
                            {
                                kohonenNeuralNetwork[k].w[i, j] = r[k].NextDouble();
                            }
                        }
                    }
                    for (int k = 0; k < 10; k++)
                    {
                        double[,] pattern = new double[databaseForRecognition[k].countOfImagesForeachNumberTrain, kohonenNeuralNetwork[k].sizeOfVector];
                        for (int i = 0; i < databaseForRecognition[k].countOfImagesForeachNumberTrain; i++)
                        {
                            for (int j = 0; j < kohonenNeuralNetwork[k].sizeOfVector; j++)
                            {
                                pattern[i, j] = databaseForRecognition[k].arrObj[i, j];
                            }
                        }

                        kohonenNeuralNetwork[k].Training(pattern);

                        string fileName = "BigCluster" + k.ToString() + ".txt";
                        WritingClusterIntoFile(fileName, kohonenNeuralNetwork[k].w, kohonenNeuralNetwork[k].maxClusters);
                    }
                    richTextBox1.Text += "Clusters are built" + "\n";
                    logger.Info("Clusters are built");
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += ex.ToString() + "\n";
                    logger.Error(ex.ToString());
                }
            }
            else
            {
                try
                {
                    for (int k = 0; k < countOfNumbers; k++)
                    {
                        string fileName = "Clusters" + k.ToString() + ".txt";
                        string[] s = File.ReadAllLines(fileName);

                        for (int i = 0; i <kohonenNeuralNetwork[k].maxClusters; i++)
                        {
                            for (int j = 0; j < kohonenNeuralNetwork[k].sizeOfVector; j++)
                            {
                                kohonenNeuralNetwork[k].w[i,j]= Double.Parse(s[i * kohonenNeuralNetwork[k].sizeOfVector + j]);

                            }
                        }
                    }
                    richTextBox1.Text += "Clusters are read from a file \n";
                    logger.Info("Clusters are read from a file");
                }
                catch(Exception ex)
                {
                    richTextBox1.Text += ex.ToString();
                    logger.Error(ex.ToString());
                }

            }
        }

        private void VerifyClusters()
        {
            const string message = "Do you need to verify again?";
            const string caption = "Message";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                try
                {
                    string fileName = "VerifyKNN.txt";
                    FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
                    StreamWriter swr = new StreamWriter(aFile);
                    aFile.Seek(0, SeekOrigin.End);

                    int[] countOfTrue = new int[countOfNumbers];
                    int[,] fMera = new int[countOfNumbers, countOfNumbers];
                    for (int k = 0; k < 10; k++)
                    {
                        countOfTrue[k] = 0;
                        for (int h = 0; h < 10; h++)
                        {
                            fMera[k, h] = 0;
                        }
                        double[,] test = new double[databaseForRecognition[k].countOfImagesForeachNumberTest, kohonenNeuralNetwork[k].sizeOfVector];
                        for (int i = 0; i < databaseForRecognition[k].countOfImagesForeachNumberTest; i++)
                        {
                            for (int j = 0; j < kohonenNeuralNetwork[k].sizeOfVector; j++)
                            {
                                test[i, j] = databaseForRecognition[k].arrObjT[i, j];
                            }
                        }
                        for (int vecNum = 0; vecNum < databaseForRecognition[k].countOfImagesForeachNumberTest; vecNum++)
                        {
                            double[] minDist = new double[countOfNumbers];

                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                EvclidDistanceForAllClusters(vecNum, test, kohonenNeuralNetwork[j].maxClusters, kohonenNeuralNetwork[j].w, kohonenNeuralNetwork[j].d);
                                int cluster = MinDistance(kohonenNeuralNetwork[j].d, kohonenNeuralNetwork[j].maxClusters);
                                minDist[j] = kohonenNeuralNetwork[j].d[cluster];
                            }

                            double min = Double.MaxValue;
                            int res = -1;
                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                if (minDist[j] < min)
                                {
                                    min = minDist[j];
                                    res = j;
                                }
                            }
                            if (res == k) countOfTrue[k]++;
                            fMera[k, res]++;
                        }
                        float procOfTrueClusters = ((float)countOfTrue[k] / databaseForRecognition[k].countOfImagesForeachNumberTest) * 100;
                        clustersVerifying.Rows.Add(k.ToString(), procOfTrueClusters.ToString() + " %");
                        //MessageBox.Show(countOfTrue[k].ToString());
                    }
                    for (int i = 0; i < countOfNumbers; i++)
                    {
                        for (int j = 0; j < countOfNumbers; j++)
                        {
                            swr.WriteLine(fMera[i, j]);
                        }
                    }
                    swr.Close();
                    richTextBox1.Text += "Clusters are checked \n";
                    logger.Info("Clusters are checked");

                }
                catch (Exception ex)
                {
                    richTextBox1.Text += ex.ToString()+"\n";
                    logger.Error(ex.ToString());
                }
            }
            else
            {
                try
                {
                    string fileName = "VerifyKNNEnter.txt";
                    string[] s = File.ReadAllLines(fileName);
                    int[,] fMera = new int[countOfNumbers, countOfNumbers];
                    for (int i = 0; i < countOfNumbers; i++)
                    {
                        
                        for (int j = 0; j < countOfNumbers; j++)
                        {
                            fMera[i, j] = int.Parse(s[i * (countOfNumbers) + j]);
                        }
                        float procOfTrueClusters = ((float)fMera[i, i] / databaseForRecognition[i].countOfImagesForeachNumberTest) * 100;
                        clustersVerifying.Rows.Add(i.ToString(), procOfTrueClusters.ToString() + " %");
                    }
                    richTextBox1.Text += "Clusters are checked \n";
                    logger.Info("Clusters are checked");
                }
                catch(Exception ex)
                {
                    richTextBox1.Text += ex.ToString() + "\n";
                    logger.Error(ex.ToString());
                }
            }
        }
        public void EvclidDistanceForAllClusters(int vectorNumber, double[,] t, int maxClusters, double[,] w, double[] d)
        {
            for (int i = 0; i < maxClusters; i++)
            {
                d[i] = 0.0;
            }

            for (int i = 0; i < maxClusters; i++)
            {
                for (int j = 0; j < dtb.height*dtb.width; j++)
                {
                    d[i] += Math.Pow((w[i, j] - t[vectorNumber, j]), 2);
                }
            }
        }
        public int MinDistance(double[] d, int maxClusters)
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
        //Автоматическая кластеризация
        private void button2_Click(object sender, EventArgs e)
        {
            ComputeClusters();
            ControlList[0] = true; //автоматическую кластеризацию надо делать самой первой
        }

        //Нейронная сеть Кохонена: Построить кластеры
        private void button1_Click(object sender, EventArgs e)
        {
            if (ControlList[0])
            {
                CreateClusters();

                ControlList[1] = true; //кластеры строятся после определенияколичества кластеров
            }
            else MessageBox.Show("Determine the number of clusters!");

            
        }
        private void WritingClusterIntoFile(string fileName, double[,] tempW, int maxClusterElement)
        {
            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter swr = new StreamWriter(aFile);
            aFile.Seek(0, SeekOrigin.End);
            for (int i = 0; i < maxClusterElement; i++)
            {
                for (int j = 0; j < dtb.height*dtb.width; j++)
                {
                    swr.WriteLine(Math.Round(tempW[i, j], 5));
                }

            }
            swr.Close();
        }

        public static Bitmap DrawImage(int num,double[,]image)
        {
            Bitmap resultY = new Bitmap(168, 168);
            Graphics grY = Graphics.FromImage(resultY);
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; ++j)
                {
                    int pixelColor0 = 255 - (int)image[num,i * 28 + j];
                    Color c0 = Color.FromArgb(pixelColor0, pixelColor0, pixelColor0);
                    SolidBrush sb0 = new SolidBrush(c0);
                    grY.FillRectangle(sb0, j * 6, i * 6, 6, 6);

                }
            }
            return resultY;
        }
        //Показать кластеры
        private void button3_Click(object sender, EventArgs e)
        {
            if (ControlList[1])
                try
                {
                    string result = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of clusters you want to view:");
                    int cluster = int.Parse(result);

                    int[] randomClusterIndexes = new int[6];
                    Random r = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        randomClusterIndexes[i] = r.Next(kohonenNeuralNetwork[cluster].maxClusters);

                    }

                    pictureBox2.Image = DrawImage(randomClusterIndexes[0], kohonenNeuralNetwork[cluster].w);
                    pictureBox3.Image = DrawImage(randomClusterIndexes[1], kohonenNeuralNetwork[cluster].w);
                    pictureBox4.Image = DrawImage(randomClusterIndexes[2], kohonenNeuralNetwork[cluster].w);
                    pictureBox6.Image = DrawImage(randomClusterIndexes[3], kohonenNeuralNetwork[cluster].w);
                    pictureBox7.Image = DrawImage(randomClusterIndexes[4], kohonenNeuralNetwork[cluster].w);
                    pictureBox8.Image = DrawImage(randomClusterIndexes[5], kohonenNeuralNetwork[cluster].w);

                    richTextBox1.Text += "Clusters were drawn successfully\n";
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += ex.ToString();

                }
            else MessageBox.Show("Build clusters!");
        }

        // Проверка НС Кохонена
        private void button4_Click(object sender, EventArgs e)
        {
            if (ControlList[1])
            {
                VerifyClusters();
            }
            else MessageBox.Show("Build clusters!");
        }

        public double[] Sort(int method, ref int old, ref int old2, ref int old3, ref int maxInd0, ref int maxInd1, ref int maxInd2, List<HopfieldNeuralNetwork>hopfieldNeuralNetworks)
        {
            double[] values = new double[3];
            maxInd0 = 0;  maxInd1 = 0;  maxInd2 = 0;
            double p = 0; double pw = 0;
            old = -1;
            for (int i = 0; i < countOfNumbers; i++)
            {
                double sum;
                if (method == 1)
                {
                    double white = ((double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector) * 100;
                     sum = hopfieldNeuralNetworks[i].resprocBlack + white;
                }
                else
                {
                     sum = hopfieldNeuralNetworks[i].resproc;
                }
                if (sum > p)
                {
                    maxInd0 = (int)hopfieldNeuralNetworks[i].result;
                    p = sum;
                    pw = (double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector;
                    pw = pw * 100;
                    old = i;
                }
            }
            values[0] = p;
            p = 0;
            old2 = -1;
            for (int i = 0; i < countOfNumbers; i++)
            {
                double sum;
                if (method == 1)
                {
                    double white = ((double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector) * 100;
                     sum = hopfieldNeuralNetworks[i].resprocBlack + white;
                }
                else
                {
                    sum = hopfieldNeuralNetworks[i].resproc;
                }
                if ((sum > p) && (i != old))
                {
                    maxInd1 = (int)hopfieldNeuralNetworks[i].result;
                    p = sum;
                    pw = (double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector;
                    pw = pw * 100;
                    old2 = i;
                }
            }
            values[1] = p;
            p = 0;
            old3 = -1;
            for (int i = 0; i < countOfNumbers; i++)
            {
                double sum = 0;
                if(method==1)
                {
                    double white = ((double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector) * 100;
                     sum = hopfieldNeuralNetworks[i].resprocBlack + white;
                }
                else
                {
                    sum = hopfieldNeuralNetworks[i].resproc;
                }
                if ((sum > p) && (i != old) && (i != old2))
                {
                    maxInd2 = (int)hopfieldNeuralNetworks[i].result;
                    p = sum;
                    pw = (double)hopfieldNeuralNetworks[i].resprocWhite / hopfieldNeuralNetworks[i].sizeOfVector;
                    pw = pw * 100;
                    old3 = i;
                }
            }
            values[2] = p;
            return values;
        }
        public void Recognition1()
        {
            try
            {
                RecognitionReport report = new RecognitionReport();
                List<HopfieldNeuralNetwork> hopfieldNeuralNetworks = new List<HopfieldNeuralNetwork>();
                for (int i = 0; i < countOfNumbers; i++)
                {
                    HopfieldNeuralNetwork hopfield = new HopfieldNeuralNetwork(databaseForRecognition[mainLabelForRecognition].arrObjT, mainNumberForRecognition, kohonenNeuralNetwork[i]);
                    hopfieldNeuralNetworks.Add(hopfield);
                }
                int[] resInd = new int[countOfNumbers];
                double[] resProcBlack = new double[countOfNumbers];
                double[] resProcWhite = new double[countOfNumbers];
                int[] iter = new int[10];

                List<int[]> pictures = new List<int[]>();
                for (int k = 0; k < countOfNumbers; k++)
                {
                    int[] pict = new int[hopfieldNeuralNetworks[k].sizeOfVector];
                    hopfieldNeuralNetworks[k].result = resInd[k];
                    hopfieldNeuralNetworks[k].resprocBlack = resProcBlack[k]; hopfieldNeuralNetworks[k].resprocWhite = resProcWhite[k];

                    hopfieldNeuralNetworks[k].Recognition1(kohonenNeuralNetwork[k].maxClusters, ref iter[k], ref pict);
                    pictures.Add(pict);

                }

                // Сортировка
                int maxInd0 = 0, maxInd1 = 0, maxInd2 = 0;
                int old = -1, old2 = -1, old3 = -1;
                double[] values = Sort(1,ref old, ref old2, ref old3, ref maxInd0, ref maxInd1, ref maxInd2, hopfieldNeuralNetworks);

                double tmp = Math.Round(values[0], 2);
                report.textBox1.Text = tmp.ToString() + "%";
                tmp = Math.Round(values[1], 2);
                report.textBox2.Text = tmp.ToString() + "%";
                tmp = Math.Round(values[2], 2);
                report.textBox3.Text = tmp.ToString() + "%";

                // Вывод на экран
                report.pictureBox2.Image = DrawImage(maxInd0, kohonenNeuralNetwork[old].w);
                report.pictureBox1.Image = DrawImage(maxInd1, kohonenNeuralNetwork[old2].w);
                report.pictureBox3.Image = DrawImage(maxInd2, kohonenNeuralNetwork[old3].w);

                // Вывод на экран с наложением
                report.pictureBox4.Image = DrawOverlapImage(kohonenNeuralNetwork[old].w, pictures[old], hopfieldNeuralNetworks[old].result);
                report.pictureBox5.Image = DrawOverlapImage(kohonenNeuralNetwork[old2].w, pictures[old2], hopfieldNeuralNetworks[old2].result);
                report.pictureBox6.Image = DrawOverlapImage(kohonenNeuralNetwork[old3].w, pictures[old3], hopfieldNeuralNetworks[old3].result);

                richTextBox1.Text += "Recognition completed successfully\n";
                logger.Info("Recognition completed successfully");
                report.ShowDialog();
            }
            catch(Exception ex)
            {
                richTextBox1.Text += ex.ToString()+"\n";
                logger.Error(ex.ToString());
            }
        }
       
        public void Recognition2()
        {
            try
            {
                RecognitionReport report = new RecognitionReport();
                List<HopfieldNeuralNetwork> hopfieldNeuralNetworks = new List<HopfieldNeuralNetwork>();
                for (int i = 0; i < countOfNumbers; i++)
                {
                    HopfieldNeuralNetwork hopfield = new HopfieldNeuralNetwork(databaseForRecognition[mainLabelForRecognition].arrObjT, mainNumberForRecognition, kohonenNeuralNetwork[i]);
                    hopfieldNeuralNetworks.Add(hopfield);
                }
                int[] resInd = new int[countOfNumbers];
                double[] resProcBlack = new double[countOfNumbers];
                double[] resProcWhite = new double[countOfNumbers];
                int[] iter = new int[countOfNumbers];

                List<int[]> pictures = new List<int[]>();
                for (int k = 0; k < countOfNumbers; k++)
                {
                    int[] pict = new int[hopfieldNeuralNetworks[k].sizeOfVector];
                    hopfieldNeuralNetworks[k].result = resInd[k];
                    hopfieldNeuralNetworks[k].resprocBlack = resProcBlack[k]; hopfieldNeuralNetworks[k].resprocWhite = resProcWhite[k];

                    hopfieldNeuralNetworks[k].Recognition2(dtb.height,kohonenNeuralNetwork[k].maxClusters, ref iter[k], ref pict);
                    pictures.Add(pict);

                }

                // Сортировка
                int maxInd0 = 0, maxInd1 = 0, maxInd2 = 0;
                int old = -1, old2 = -1, old3 = -1;
                double[] values = Sort(2,ref old, ref old2, ref old3, ref maxInd0, ref maxInd1, ref maxInd2, hopfieldNeuralNetworks);

                double tmp = Math.Round(values[0], 2);
                report.textBox1.Text = tmp.ToString() + "%";
                tmp = Math.Round(values[1], 2);
                report.textBox2.Text = tmp.ToString() + "%";
                tmp = Math.Round(values[2], 2);
                report.textBox3.Text = tmp.ToString() + "%";

                // Вывод на экран
                report.pictureBox2.Image = DrawImage(maxInd0, kohonenNeuralNetwork[old].w);
                report.pictureBox1.Image = DrawImage(maxInd1, kohonenNeuralNetwork[old2].w);
                report.pictureBox3.Image = DrawImage(maxInd2, kohonenNeuralNetwork[old3].w);

                // Вывод на экран с наложением
                report.pictureBox4.Image = DrawOverlapImage(kohonenNeuralNetwork[old].w, pictures[old], hopfieldNeuralNetworks[old].result);
                report.pictureBox5.Image = DrawOverlapImage(kohonenNeuralNetwork[old2].w, pictures[old2], hopfieldNeuralNetworks[old2].result);
                report.pictureBox6.Image = DrawOverlapImage(kohonenNeuralNetwork[old3].w, pictures[old3], hopfieldNeuralNetworks[old3].result);

                richTextBox1.Text += "Recognition completed successfully\n";
                logger.Info("Recognition completed successfully");
                report.ShowDialog();
            }
            catch (Exception ex)
            {
               richTextBox1.Text += ex.ToString() + "\n";
                logger.Error(ex.ToString());
             }
        }
        public Bitmap DrawOverlapImage(double[,]image, int[] pictures, int resInd)
        {
            Bitmap overlap1 = new Bitmap(168, 168);
            Graphics grY1 = Graphics.FromImage(overlap1);

            for (int i = 0; i < dtb.height; i++)
            {
                for (int j = 0; j < dtb.width; ++j)
                {
                    int pixelColor0 = 0;
                    if (pictures[i * 28 + j] == 1000)
                        pixelColor0 = 20;
                    else pixelColor0 = 255 - (int)image[resInd, i * dtb.height + j];

                    Color c01;
                    if (pixelColor0 == 20)
                    {
                        c01 = Color.Red;
                    }
                    else
                    {
                        c01 = Color.FromArgb(pixelColor0, pixelColor0, pixelColor0);
                    }
                    SolidBrush sb01 = new SolidBrush(c01);
                    grY1.FillRectangle(sb01, j * 6, i * 6, 6, 6);
                }
            }
            return overlap1;
        }
        // Распознавание методом 1
        private void button5_Click(object sender, EventArgs e)
        {
            if (ControlList[2] && ControlList[1])
            {
                Recognition1();
                ControlList[3] = true;
            }
            else if(!ControlList[1])
            {
                MessageBox.Show("Build clusters");
                
            }
            else if (!ControlList[2])
                MessageBox.Show("Upload image");
        }

        public void DownloadRandomImage()
        {
            try
            {
                Random r = new Random();
                int label = r.Next(0, 9);
                int num = r.Next(0, databaseForRecognition[label].countOfImagesForeachNumberTest);

                mainLabelForRecognition = label;
                mainNumberForRecognition = num;
                Bitmap resultY = DrawImage(num, databaseForRecognition[label].arrObjT);

                Graphics grY = Graphics.FromImage(resultY);
                imageForRecognition.Image = resultY;
                richTextBox1.Text += "The image of digit "+label+ " uploaded successfully\n";
                logger.Info("The image of digit " + label + " uploaded successfully");
            }
            catch(Exception ex)
            {
                richTextBox1.Text += ex.ToString()+"\n";
                logger.Error(ex.ToString());
            }
        }
        // Загрузить изображение
        private void downloadImage_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                DownloadRandomImage();
            else
            {
                try
                {
                    mainNumberForRecognition = int.Parse(textBox1.Text);
                    mainLabelForRecognition = dtb.labelT[mainNumberForRecognition];
                    imageForRecognition.Image = DrawImage(mainNumberForRecognition, dtb.arrT);
                    richTextBox1.Text += "The image of digit " + mainLabelForRecognition + " uploaded successfully\n";
                    logger.Info("The image of digit " + mainLabelForRecognition + " uploaded successfully");
                }
                catch(Exception ex)
                {
                    richTextBox1.Text += ex.ToString() + "\n";
                    logger.Error(ex.ToString());
                }
            }
            ControlList[2] = true;
        }

        //Распознавание методом 2
        private void button6_Click(object sender, EventArgs e)
        {
            if (ControlList[2] && ControlList[1])
            {
                Recognition2();
                ControlList[4] = true;
            }
            else if (!ControlList[1])
            {
                MessageBox.Show("Build clusters");
                
            }
            else if (!ControlList[2])
                MessageBox.Show("Upload image");
        }

        // График для автоматической кластеризации
        private void button7_Click(object sender, EventArgs e)
        {
            if(ControlList[0])
            try
            {
                int step = 40;
                int countObjects = countOfNumbers;
                int[,] seria = new int[countObjects, step];
                string fileName = "CountOfCluster";

                for (int j = 0; j < countObjects; j++)
                {
                    string[] s = File.ReadAllLines(fileName + j.ToString() + ".txt");
                    ParsingFilling(ref j, ref seria, s);
                }

                chart1.Series[0].Points.Clear();
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                ChartArea chartArea = new ChartArea();
                chartArea.AxisX.Minimum = 0;
                chartArea.AxisX.Maximum = 95;

                chartArea.AxisY.Minimum = 0;
                int x;
                switch (comboBox1.SelectedItem)
                {
                    case "0":                     
                         chartArea.AxisY.Maximum = databaseForRecognition[0].countOfImagesForeachNumberTest;
                         x = 10;
                        for (int i = 0; i < step; i++)
                          {
                             int y = seria[0, i];
                             chart1.Series[0].Points.AddXY(x, y);
                             x = x + 2;
                          } 
                        break;
                    case "1":
                        chartArea.AxisY.Maximum = databaseForRecognition[1].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[1, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "2":
                        chartArea.AxisY.Maximum = databaseForRecognition[2].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[2, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "3":
                        chartArea.AxisY.Maximum = databaseForRecognition[3].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[3, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "4":
                        chartArea.AxisY.Maximum = databaseForRecognition[4].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[4, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "5":
                        chartArea.AxisY.Maximum = databaseForRecognition[5].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[5, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "6":
                        chartArea.AxisY.Maximum = databaseForRecognition[6].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[6, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "7":
                        chartArea.AxisY.Maximum = databaseForRecognition[7].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[7, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "8":
                        chartArea.AxisY.Maximum = databaseForRecognition[8].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[8, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    case "9":
                        chartArea.AxisY.Maximum = databaseForRecognition[9].countOfImagesForeachNumberTest;
                        x = 10;
                        for (int i = 0; i < step; i++)
                        {
                            int y = seria[9, i];
                            chart1.Series[0].Points.AddXY(x, y);
                            x = x + 2;
                        }
                        break;
                    default: break;
                }

                richTextBox1.Text += "Chart for " + comboBox1.SelectedItem + " built\n";
                    logger.Info("Chart for " + comboBox1.SelectedItem + " built");
            }
            catch(Exception ex)
            {
                richTextBox1.Text += ex.ToString() + "\n";
                    logger.Error(ex.ToString());
            }
            else
            {
                MessageBox.Show("Determine the number of clusters");
            }
        }

        public int CheckHopfield1(int label, int num)
        {
            
            List<HopfieldNeuralNetwork> hopfieldNeuralNetworks = new List<HopfieldNeuralNetwork>();
            for (int i = 0; i < countOfNumbers; i++)
            {
                HopfieldNeuralNetwork hopfield = new HopfieldNeuralNetwork(databaseForRecognition[label].arrObjT, num, kohonenNeuralNetwork[i]);
                hopfieldNeuralNetworks.Add(hopfield);
            }
            int[] resInd = new int[countOfNumbers];
            double[] resProcBlack = new double[countOfNumbers];
            double[] resProcWhite = new double[countOfNumbers];
            int[] iter = new int[10];

            List<int[]> pictures = new List<int[]>();
            for (int k = 0; k < countOfNumbers; k++)
            {
                int[] pict = new int[hopfieldNeuralNetworks[k].sizeOfVector];
                hopfieldNeuralNetworks[k].result = resInd[k];
                hopfieldNeuralNetworks[k].resprocBlack = resProcBlack[k]; hopfieldNeuralNetworks[k].resprocWhite = resProcWhite[k];

                hopfieldNeuralNetworks[k].Recognition1(kohonenNeuralNetwork[k].maxClusters, ref iter[k], ref pict);
                pictures.Add(pict);

            }

            // Сортировка
            int maxInd0 = 0, maxInd1 = 0, maxInd2 = 0;
            int old = -1, old2 = -1, old3 = -1;
            double[] values = Sort(1, ref old, ref old2, ref old3, ref maxInd0, ref maxInd1, ref maxInd2, hopfieldNeuralNetworks);

            return old;
        }
        // Проверка 1
        private void button8_Click(object sender, EventArgs e)
        {
            if (ControlList[1])
            {
                const string message = "Do you need to verify network again?";
                const string caption = "Message";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int[,] fMera = new int[countOfNumbers, countOfNumbers];
                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                fMera[i, j] = 0;
                            }
                        }
                        string fileName = "VerifyHopfield1.txt";
                        FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
                        StreamWriter swr = new StreamWriter(aFile);

                        aFile.Seek(0, SeekOrigin.End);

                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < databaseForRecognition[i].countOfImagesForeachNumberTest; j++)
                            {
                                int res = CheckHopfield1(i, j);
                                fMera[i, res]++;
                            }

                        }
                        RecognitionReport report = new RecognitionReport();
                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                swr.WriteLine(fMera[i, j]);

                            }
                            float procOfTrueClusters = ((float)fMera[i, i] / databaseForRecognition[i].countOfImagesForeachNumberTest) * 100;
                            report.networkVerifying.Rows.Add(i.ToString(), procOfTrueClusters.ToString() + " %");
                        }
                        swr.Close();
                        richTextBox1.Text += "Hopfield NN is checked\n";
                        logger.Info("Hopfield NN is checked");
                        report.Show();
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text += ex.ToString()+"\n";
                        logger.Error(ex.ToString());
                    }
                }
                else
                {
                    try
                    {
                        RecognitionReport report = new RecognitionReport();
                        string fileName = "VerifyHopfield1.txt";
                        string[] s = File.ReadAllLines(fileName);
                        int[,] fMera = new int[countOfNumbers, countOfNumbers];
                        for (int i = 0; i < countOfNumbers; i++)
                        {

                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                fMera[i, j] = int.Parse(s[i * (countOfNumbers) + j]);
                            }
                            float procOfTrueClusters = ((float)fMera[i, i] / databaseForRecognition[i].countOfImagesForeachNumberTest) * 100;
                            report.networkVerifying.Rows.Add(i.ToString(), procOfTrueClusters.ToString() + " %");
                        }
                        richTextBox1.Text += "Hopfield neural network checked \n";
                        logger.Info("Hopfield neural network checked");
                        report.Show();
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text += ex.ToString() + "\n";
                        logger.Error(ex.ToString());
                    }
                }
            }
            else MessageBox.Show("Build clusters");
        }

        public int CheckHopfield2(int label,int num)
        {

            List<HopfieldNeuralNetwork> hopfieldNeuralNetworks = new List<HopfieldNeuralNetwork>();
            for (int i = 0; i < countOfNumbers; i++)
            {
                HopfieldNeuralNetwork hopfield = new HopfieldNeuralNetwork(databaseForRecognition[label].arrObjT, num, kohonenNeuralNetwork[i]);
                hopfieldNeuralNetworks.Add(hopfield);
            }
            int[] resInd = new int[countOfNumbers];
            double[] resProcBlack = new double[countOfNumbers];
            double[] resProcWhite = new double[countOfNumbers];
            int[] iter = new int[countOfNumbers];

            List<int[]> pictures = new List<int[]>();
            for (int k = 0; k < countOfNumbers; k++)
            {
                int[] pict = new int[hopfieldNeuralNetworks[k].sizeOfVector];
                hopfieldNeuralNetworks[k].result = resInd[k];
                hopfieldNeuralNetworks[k].resprocBlack = resProcBlack[k]; hopfieldNeuralNetworks[k].resprocWhite = resProcWhite[k];

                hopfieldNeuralNetworks[k].Recognition2(dtb.height, kohonenNeuralNetwork[k].maxClusters, ref iter[k], ref pict);
                pictures.Add(pict);

            }

            // Сортировка
            int maxInd0 = 0, maxInd1 = 0, maxInd2 = 0;
            int old = -1, old2 = -1, old3 = -1;
            double[] values = Sort(2, ref old, ref old2, ref old3, ref maxInd0, ref maxInd1, ref maxInd2, hopfieldNeuralNetworks);

            return old;
        }
        // Проверка 2
        private void button9_Click(object sender, EventArgs e)
        {
            if (ControlList[1])
            {
                const string message = "Do you need to verify network again?";
                const string caption = "Message";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int[,] fMera = new int[countOfNumbers, countOfNumbers];
                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                fMera[i, j] = 0;
                            }
                        }
                        string fileName = "VerifyHopfield2.txt";
                        FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
                        StreamWriter swr = new StreamWriter(aFile);

                        aFile.Seek(0, SeekOrigin.End);

                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < databaseForRecognition[i].countOfImagesForeachNumberTest; j++)
                            {
                                int res = CheckHopfield2(i, j);
                                fMera[i, res]++;
                            }

                        }
                        RecognitionReport report = new RecognitionReport();
                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                swr.WriteLine(fMera[i, j]);

                            }
                            float procOfTrueClusters = ((float)fMera[i, i] / databaseForRecognition[i].countOfImagesForeachNumberTest) * 100;
                            report.networkVerifying.Rows.Add(i.ToString(), procOfTrueClusters.ToString() + " %");
                        }
                        swr.Close();
                        richTextBox1.Text += "Hopfield NN is checked\n";
                        logger.Info("Hopfield NN is checked");
                        report.Show();
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text += ex.ToString()+"\n";
                        logger.Error(ex.ToString());
                    }
                }
                else
                {
                    try
                    {
                        RecognitionReport report = new RecognitionReport();
                        string fileName = "VerifyHopfield2.txt";
                        string[] s = File.ReadAllLines(fileName);
                        int[,] fMera = new int[countOfNumbers, countOfNumbers];
                        for (int i = 0; i < countOfNumbers; i++)
                        {

                            for (int j = 0; j < countOfNumbers; j++)
                            {
                                fMera[i, j] = int.Parse(s[i * (countOfNumbers) + j]);
                            }
                            float procOfTrueClusters = ((float)fMera[i, i] / databaseForRecognition[i].countOfImagesForeachNumberTest) * 100;
                            report.networkVerifying.Rows.Add(i.ToString(), procOfTrueClusters.ToString() + " %");
                        }
                        richTextBox1.Text += "Hopfield neural network checked \n";
                        logger.Info("Hopfield neural network checked");
                        report.Show();
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text += ex.ToString() + "\n";
                        logger.Error(ex.ToString());
                    }
                }
            }
            else MessageBox.Show("Build clusters");
        }

        private void RecognizeByKohonen(ref int res, ref int clst)
        {
            try
            {
                double[] minDist = new double[countOfNumbers];
                int[] minClusters = new int[countOfNumbers];

                for (int j = 0; j < countOfNumbers; j++)
                {
                    if(textBox1.Text=="")
                    {
                        EvclidDistanceForAllClusters(mainNumberForRecognition, databaseForRecognition[mainLabelForRecognition].arrObjT, kohonenNeuralNetwork[j].maxClusters, kohonenNeuralNetwork[j].w, kohonenNeuralNetwork[j].d);
                    }
                    else
                    EvclidDistanceForAllClusters(mainNumberForRecognition, dtb.arrT, kohonenNeuralNetwork[j].maxClusters, kohonenNeuralNetwork[j].w, kohonenNeuralNetwork[j].d);

                    int cluster = MinDistance(kohonenNeuralNetwork[j].d, kohonenNeuralNetwork[j].maxClusters);
                    minDist[j] = kohonenNeuralNetwork[j].d[cluster];
                    minClusters[j] = cluster;
                }

                double min = Double.MaxValue;
                for (int j = 0; j < countOfNumbers; j++)
                {
                    if (minDist[j] < min)
                    {
                        min = minDist[j];
                        res = j;
                        clst = minClusters[j];
                    }
                }
            }
            catch(Exception ex)
            {
                richTextBox1.Text += ex.ToString() + "\n";
                logger.Error(ex.ToString());
            }
        }
        // Распознать нейронной сетью Кохонена
        private void button10_Click(object sender, EventArgs e)
        {
            if (ControlList[2] && ControlList[1])
            {
                try
                {
                    int res = -1; int clst = -1;
                    RecognizeByKohonen(ref res, ref clst);
                    pictureBox1.Image = DrawImage(clst, kohonenNeuralNetwork[res].w);
                    textBox2.Text = res.ToString();
                    richTextBox1.Text += "The image was recognized by the Kohonen neural network" + "\n";
                    logger.Info("The image was recognized by the Kohonen neural network");
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += ex.ToString() + "\n";
                    logger.Error(ex.ToString());
                }
            }
            else if(!ControlList[1])
            {
                MessageBox.Show("Build clusters");
               
            }
            else if (!ControlList[2])
                MessageBox.Show("Upload image");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Image Recognition.chm");
        }
        // Обновить: пользователь обновляет количество кластеров
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                for(int i=0;i<countOfNumbers;i++)
                {
                    kohonenNeuralNetwork[i].maxClusters = int.Parse(InfoAboutClusters[1,i].Value.ToString());
                    
                }
                richTextBox1.Text += "Data was updated successfully" + "\n";
                logger.Info("Data was updated successfully");
                MessageBox.Show("Build new clusters");
                ControlList[1] = false;
            }
            catch(Exception ex)
            {
                richTextBox1.Text += ex.ToString() + "\n";
                logger.Error(ex.ToString());
            }
        }
    }
}
