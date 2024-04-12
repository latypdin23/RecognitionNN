using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionNN
{
    public class MNIST
    {
        public DigitImage[] trainImages = null;
        public DigitImage[] testImages = null;

        public int[,] arr;
        public int[,] arrT;
        public MNIST(int train,int test,int N)
        {
            arr = new int[train, N];
            arrT = new int[test, N];
        }

       // public int[,] arr = new int[60000, 784];
        //public int[,] arrT = new int[10000, 784];
        //public int trainCount = 60000;
        //public int testCount = 10000;

        public static int ReverseBytes(int v)
        {
            byte[] intAsBytes = BitConverter.GetBytes(v);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }
        public static Bitmap MakeBitmap(DigitImage dImage, int mag)
        {
            int width = dImage.width * mag;
            int height = dImage.height * mag;
            Bitmap result = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(result);
            for (int i = 0; i < dImage.height; ++i)
            {
                for (int j = 0; j < dImage.width; ++j)
                {
                    int pixelColor = 255 - dImage.pixels[i][j];

                    Color c = Color.FromArgb(pixelColor, pixelColor, pixelColor);

                    SolidBrush sb = new SolidBrush(c);
                    gr.FillRectangle(sb, j * mag, i * mag, mag, mag);
                }
            }
            return result;
        }
        public static DigitImage[] LoadData(int numImages, string pixelFile, string labelFile, int[,] temp)
        {

            DigitImage[] result = new DigitImage[numImages];

            byte[][] pixels = new byte[28][];
            for (int i = 0; i < pixels.Length; ++i)
                pixels[i] = new byte[28];

            FileStream ifsPixels = new FileStream(pixelFile, FileMode.Open);
            FileStream ifsLabels = new FileStream(labelFile, FileMode.Open);

            BinaryReader brImages = new BinaryReader(ifsPixels);
            BinaryReader brLabels = new BinaryReader(ifsLabels);

            int magic1 = brImages.ReadInt32(); // stored as Big Endian
            magic1 = ReverseBytes(magic1); // convert to Intel format

            int imageCount = brImages.ReadInt32();
            imageCount = ReverseBytes(imageCount);

            int numRows = brImages.ReadInt32();
            numRows = ReverseBytes(numRows);
            int numCols = brImages.ReadInt32();
            numCols = ReverseBytes(numCols);

            int magic2 = brLabels.ReadInt32();
            magic2 = ReverseBytes(magic2);

            int numLabels = brLabels.ReadInt32();
            numLabels = ReverseBytes(numLabels);

            // each image

            for (int di = 0; di < numImages; ++di)
            {
                for (int i = 0; i < 28; ++i) // get 28x28 pixel values
                {
                    for (int j = 0; j < 28; ++j)
                    {
                        byte b = brImages.ReadByte();
                        pixels[i][j] = b;

                        temp[di, i * 28 + j] = b;

                    }
                }

                byte lbl = brLabels.ReadByte(); // get the label
                DigitImage dImage = new DigitImage(28, 28, pixels, lbl);
                result[di] = dImage;
            }

            ifsPixels.Close(); brImages.Close();
            ifsLabels.Close(); brLabels.Close();

            return result;
        }
    }
}
