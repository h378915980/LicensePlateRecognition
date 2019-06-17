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

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    public partial class Form1 : Form
    {
        
        public string selectPath;
        public string imgPath;
        public Mat img_in = null;


        public Form1()
        {
            InitializeComponent();
        }

        //选择文件夹中的图片添加到列表
        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();

            if (this.folderBrowserDialog1.SelectedPath == string.Empty)
            {
                return;
            }


            selectPath = this.folderBrowserDialog1.SelectedPath;


            string[] files = Directory.GetFiles(selectPath, "*.*");
            List<string> lists = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                string fileNamei = files[i].ToLower();
                if (fileNamei.EndsWith(".jpg") || fileNamei.EndsWith(".png") || fileNamei.EndsWith(".bmp"))
                {
                    lists.Add(fileNamei);
                }
            }
            Array array = Array.CreateInstance(typeof(string), lists.Count);
            lists.CopyTo((string[])array, 0);

            //添加到列表中
            foreach (string arr in array)
            {
                this.listView1.Items.Add(arr);
            }
        }

        //选中列表中的文件路径
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            //显示原图
            imgPath = this.listView1.SelectedItems[0].SubItems[0].Text;
            this.pictureBox1.Load(imgPath);

            //添加原图
            this.img_in = new Mat(imgPath);
        }

        //显示各种处理
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imgPath != string.Empty && img_in != null)
            {

                switch (this.tabControl1.SelectedIndex)
                {
                    case 0:
                        this.pictureBox1.Load(imgPath);
                        break;
                    case 1:
                        Mat img_out = new Mat();
                        img_out = img_in.CvtColor(ColorConversionCodes.BGR2GRAY);
                        this.pictureBox1.Image = img_out.ToBitmap();
                        img_out.Release();
                        break;
                    case 2:
                        Mat imgHsv = new Mat();
                        imgHsv = img_in.CvtColor(ColorConversionCodes.BGR2HSV);
                        Mat[] matToHsv = new Mat[3];
                        Cv2.Split(imgHsv,out matToHsv);
                        
                        this.pictureBox1.Image = matToHsv[2].ToBitmap();

                        break;
                }
            }
        }
    }
}
