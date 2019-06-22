﻿using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensePlateRecognition
{
    public partial class TrainForm : Form
    {
        public TrainForm()
        {
            InitializeComponent();
        }

        PlateCategorySVM.FileInfo f1=new PlateCategorySVM.FileInfo("C:\\Users\\hepeiyuan\\Desktop\\MyPlateData\\nonplate", (int)PlateCategory.UNKNOW);
        PlateCategorySVM.FileInfo f2=new PlateCategorySVM.FileInfo("C:\\Users\\hepeiyuan\\Desktop\\MyPlateData\\plate",(int)PlateCategory.NORMALPLATE);


        private void button1_Click(object sender, EventArgs e)
        {
            List<PlateCategorySVM.FileInfo> files = new List<PlateCategorySVM.FileInfo>();
            files.Add(f1);
            files.Add(f2);

            if (PlateCategorySVM.Train(files))
            {
                PlateCategorySVM.Save("C:\\Users\\hepeiyuan\\Desktop\\MyPlateData\\svmdata.xml");
                MessageBox.Show("train success");
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listView1.Clear();
            List<string> testFile = FileIO.OpenFile("C:\\Users\\hepeiyuan\\Desktop\\testsample");
            foreach(string tf in testFile)
            {
                Mat mat = new Mat(tf);
                PlateCategory plateCategory = PlateCategorySVM.Test(mat);
                this.listView1.Items.Add(plateCategory.ToString());
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();//可以自由增加的字符串

            stringBuilder.Append("asd");
            stringBuilder.Append("asd");
            stringBuilder.Append("asd");
            MessageBox.Show(stringBuilder.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mat mat = new Mat(@"C:\Users\hepeiyuan\Desktop\chartest\2019-03-12-14-40-38-034156_颜色法_269.jpg");
            mat = CharSegement.ClearMaodingAndBorder(mat.CvtColor(ColorConversionCodes.BGR2GRAY), PlateColor.BLUE);
            this.pictureBox1.Image = mat.ToBitmap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Mat mat = new Mat(@"C:\Users\hepeiyuan\Desktop\chartest\sample_118_0.jpg");
            mat = CharSegement.SplitePlateByOriginal(mat,mat,PlateColor.YELLOW);
            this.pictureBox1.Image = mat.ToBitmap();
        }
    }
}