using OpenCvSharp;
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

            if (PlateCategorySVM.Train(files, "C:\\Users\\hepeiyuan\\Desktop\\MyPlateData\\svmdata.xml"))
            {
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
    }
}
