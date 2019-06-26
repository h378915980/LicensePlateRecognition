using OpenCvSharp;
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



        private void button1_Click(object sender, EventArgs e)
        {
           
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
            FileIO.PrepareTrainningCharDirectory(@"C:\Users\hepeiyuan\Desktop\新建文件夹");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mat mat = new Mat(@"C:\Users\hepeiyuan\Desktop\chartest\2019-03-12-14-40-38-034156_颜色法_269.jpg");
            //mat = CharSegement.SplitePlateByOriginal(mat, PlateColor.BLUE);
            this.pictureBox1.Image = mat.ToBitmap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Mat mat = new Mat(@"C:\Users\hepeiyuan\Desktop\chartest\2019-03-12-14-52-59-029965_颜色法_273.jpg");
           // mat = CharSegement.SplitePlateByOriginal(mat,PlateColor.YELLOW);
            this.pictureBox1.Image = mat.ToBitmap();
        }
    }
}
