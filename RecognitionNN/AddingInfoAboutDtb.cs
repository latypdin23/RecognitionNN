using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecognitionNN
{
    public partial class AddingInfoAboutDtb : Form
    {
        public int trainCount;
        public int testCount;
        public int height;
        public int width;
        public AddingInfoAboutDtb()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trainCount = int.Parse(textBox1.Text);
            testCount = int.Parse(textBox2.Text);
            height = int.Parse(textBox3.Text);
            width = int.Parse(textBox4.Text);

            Hide();
        }
    }
}
