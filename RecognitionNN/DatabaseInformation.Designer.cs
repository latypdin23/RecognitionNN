namespace RecognitionNN
{
    partial class DatabaseInformation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LoadMNIST = new System.Windows.Forms.Button();
            this.LoadDatabase = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 42);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(435, 341);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image recognition using neural networks";
            // 
            // LoadMNIST
            // 
            this.LoadMNIST.BackColor = System.Drawing.Color.Snow;
            this.LoadMNIST.Location = new System.Drawing.Point(12, 389);
            this.LoadMNIST.Name = "LoadMNIST";
            this.LoadMNIST.Size = new System.Drawing.Size(136, 49);
            this.LoadMNIST.TabIndex = 2;
            this.LoadMNIST.Text = "Download MNIST";
            this.LoadMNIST.UseVisualStyleBackColor = false;
            this.LoadMNIST.Click += new System.EventHandler(this.LoadMNIST_Click);
            // 
            // LoadDatabase
            // 
            this.LoadDatabase.BackColor = System.Drawing.Color.Snow;
            this.LoadDatabase.Location = new System.Drawing.Point(163, 389);
            this.LoadDatabase.Name = "LoadDatabase";
            this.LoadDatabase.Size = new System.Drawing.Size(136, 48);
            this.LoadDatabase.TabIndex = 3;
            this.LoadDatabase.Text = "Download Database";
            this.LoadDatabase.UseVisualStyleBackColor = false;
            this.LoadDatabase.Click += new System.EventHandler(this.LoadDatabase_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.AliceBlue;
            this.button1.Location = new System.Drawing.Point(372, 410);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DatabaseInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(467, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LoadDatabase);
            this.Controls.Add(this.LoadMNIST);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "DatabaseInformation";
            this.Text = "DatabaseInformation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LoadMNIST;
        private System.Windows.Forms.Button LoadDatabase;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
    }
}