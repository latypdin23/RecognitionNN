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

namespace RecognitionNN
{
    public partial class DatabaseInformation : Form
    {
        string databaseFile;

        
        public DatabaseInformation()
        {
            InitializeComponent();
            string fileName = "InstructionEng";
            string[] s = File.ReadAllLines(fileName + ".txt");
            foreach (string st in s)
              richTextBox1.Text += st;

        }

        private void LoadMNIST_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The download may take some time, please wait");
            string pixelFileTrain = @"train-images.idx3-ubyte";
            string labelFileTrain = @"train-labels.idx1-ubyte";
            string pixelFileTest = @"t10k-images.idx3-ubyte";
            string labelFileTest = @"t10k-labels.idx1-ubyte";
            Form1 form1 = new Form1();
            form1.dtb = new InfoDtb(60000, 10000, 28, 28);
            form1.DownloadMNISTDatabase( pixelFileTrain,  labelFileTrain,  pixelFileTest,  labelFileTest);
            form1.ShowDialog();
            this.Close();
        }

        private void LoadDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                AddingInfoAboutDtb info = new AddingInfoAboutDtb();
                info.ShowDialog();

                MessageBox.Show("The download may take some time, please wait");

                Form1 form1 = new Form1();

                string pixelFileTrain = @"fashiontrain-images.idx3-ubyte";
                string labelFileTrain = @"fashiontrain-labels.idx1-ubyte";
                string pixelFileTest = @"fashiont10k-images.idx3-ubyte";
                string labelFileTest = @"fashiont10k-labels.idx1-ubyte";


                form1.dtb = new InfoDtb(info.trainCount, info.testCount, info.height, info.width);
                form1.DownloadFashionMNISTDatabase(pixelFileTrain, labelFileTrain, pixelFileTest, labelFileTest);

                form1.ShowDialog();
                this.Close();

            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message + "\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Image Recognition English.chm");
        }
    }
}
