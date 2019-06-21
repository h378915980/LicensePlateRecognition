﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.Extensions;

using static LicensePlateRecognition.PlateLocator;

namespace LicensePlateRecognition
{

   

    public partial class Form1 : Form
    {
        //初始化参数列表
        ParameterList parameterList = new ParameterList(
            hdwl: 0.15f,
            hdwu: 0.70f,
            hl: 10,
            hu: 80,
            wl: 40,
            wu: 180);


        public Form1()
        {
            InitializeComponent();
            InitParamterList();
        }
    
        
        //functions
        private void InitParamterList()
        {
            this.HeightDividWidthLow.ValueChanged -= new System.EventHandler(HeightDividWidthLow_ValueChanged);
            this.HeightDividWidthUp.ValueChanged -= new System.EventHandler(HeightDividWidthUp_ValueChanged);
            this.HeightLow.ValueChanged -= new System.EventHandler(HeightLow_ValueChanged);
            this.HeightUp.ValueChanged -= new System.EventHandler(HeightUp_ValueChanged);
            this.WidthLow.ValueChanged -= new System.EventHandler(WidthLow_ValueChanged);
            this.WidthUp.ValueChanged -= new System.EventHandler(WidthUp_ValueChanged);

            this.HeightDividWidthLow.Value = (decimal)parameterList.HeightDivideWidthLow;
            this.HeightDividWidthUp.Value = (decimal)parameterList.HeightDivedeWidthUp;
            this.HeightLow.Value = parameterList.HeightLow;
            this.HeightUp.Value = parameterList.HeightUp;
            this.WidthLow.Value = parameterList.WidthLow;
            this.WidthUp.Value = parameterList.WidthUp;

            this.HeightDividWidthLow.ValueChanged += new System.EventHandler(HeightDividWidthLow_ValueChanged);
            this.HeightDividWidthUp.ValueChanged += new System.EventHandler(HeightDividWidthUp_ValueChanged);
            this.HeightLow.ValueChanged += new System.EventHandler(HeightLow_ValueChanged);
            this.HeightUp.ValueChanged += new System.EventHandler(HeightUp_ValueChanged);
            this.WidthLow.ValueChanged += new System.EventHandler(WidthLow_ValueChanged);
            this.WidthUp.ValueChanged += new System.EventHandler(WidthUp_ValueChanged);

        }
        private int currentTabCount;
        private void AddTag(string text,Mat image)
        {
            //get page
            TabPage page = null;
            if (tabShowDiffImage.TabPages.Count > currentTabCount)
            {
                page = tabShowDiffImage.TabPages[currentTabCount];
            }
            else
            {
                page = new TabPage();
                tabShowDiffImage.TabPages.Add(page);
            }
            page.Text = text;

            //get picture
            PictureBox pic = null;
            if (page.Controls.Count == 0)
            {
                pic = new PictureBox();
                page.Controls.Add(pic);
                pic.Dock = DockStyle.Fill;
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else pic = (PictureBox)page.Controls[0];
            pic.Image = image.ToBitmap();

            currentTabCount++;
        }

        private void ProcessAndShowImage(Bitmap image,ParameterList pl)
        {

            currentTabCount = 0;

            Mat matIn = image.ToMat();
            AddTag("原图", matIn);

            //转为hsv图片    
            Mat matHsv = matIn.CvtColor(ColorConversionCodes.BGR2HSV);
            AddTag("HSV", matHsv);

            //对v均衡化后在合并
            Mat[] matToHsv = new Mat[3];
            Cv2.Split(matHsv, out matToHsv);
            Cv2.EqualizeHist(matToHsv[2], matToHsv[2]);
            Mat equalizeHistHsv = new Mat();
            Cv2.Merge(matToHsv, equalizeHistHsv);
            AddTag("均衡HSV", equalizeHistHsv);

            //在均衡化后的hsv颜色空间红寻找黄色和蓝色区域
            Mat matYellow = new Mat();
            Mat matBlue = new Mat();
            Scalar yellow_low = new Scalar(15, 70, 70);
            Scalar yellow_up = new Scalar(40, 255, 255);
            Scalar blue_low = new Scalar(100, 70, 70);
            Scalar blue_up = new Scalar(140, 255, 255);
            Cv2.InRange(equalizeHistHsv, yellow_low, yellow_up, matYellow);
            Cv2.InRange(equalizeHistHsv, blue_low, blue_up, matBlue);
            Mat matAll = matBlue + matYellow;
            AddTag("黄区", matYellow);
            AddTag("蓝区", matBlue);
            AddTag("黄蓝区", matAll);

            //使用形态学操作对选定颜色区域进行处理
            Mat matAllDilate = new Mat();
            Mat matAllErode = new Mat();
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 3));
            Cv2.Dilate(matAll, matAllDilate, element);
            Cv2.Erode(matAllDilate, matAllErode, element);
            AddTag("闭操作", matAllErode);


            //寻找轮廓
            OpenCvSharp.Point[][] contours; //vector<vector<Point>> contours;
            HierarchyIndex[] hierarchyIndexes; //vector<Vec4i> hierarchy;
            Cv2.FindContours(
                matAllErode,
                out contours,
                out hierarchyIndexes,
                mode: RetrievalModes.Tree,
                method: ContourApproximationModes.ApproxSimple); //求轮廓

            Mat matContours = matIn.Clone();
            Cv2.DrawContours(matContours, contours, -1, new Scalar(0, 0, 255), 2); //画轮廓线
            AddTag("轮廓", matContours);

            //求轮廓的最小外接矩形          
            Mat matRects = matIn.Clone();
            List<Rect> rects = new List<Rect>();
            foreach(OpenCvSharp.Point[] p in contours)
            {
                Rect rect = Cv2.BoundingRect(p);
                if ((double)rect.Height / rect.Width > pl.HeightDivideWidthLow && (double)rect.Height / rect.Width < pl.HeightDivedeWidthUp
                    && rect.Height > pl.HeightLow && rect.Height < pl.HeightUp
                    && rect.Width > pl.WidthLow && rect.Width < pl.WidthUp)
                {
                    rects.Add(rect);
                    Cv2.Rectangle(matRects, rect, new Scalar(0, 0, 255), 3);
                }
            }
            AddTag("外接矩形", matRects);

            //显示切分的图片
            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();
            int index = 0;
            foreach (Rect rect in rects)
            {
                Mat roi = new Mat(matIn, rects[index]);

                this.imgListSplitImage.Images.Add(roi.ToBitmap());
                this.listShowSplitImage.Items.Add(index.ToString());
                //this.listShowSplitImage.Items[index].ImageList.ImageSize = new System.Drawing.Size(rects[index].Width, rects[index].Height);
                this.listShowSplitImage.Items[index].ImageIndex = index;
                index++;
            }

        }


        //events
        //选择文件夹中的图片添加到列表
        private void InputImage(object sender, EventArgs e)
        {
            
            if(this.inputImageFolder.ShowDialog()==DialogResult.OK)
            {
                this.listInputImage.Clear();
                List<string> files = FileIO.OpenFile(inputImageFolder.SelectedPath);
                foreach(string f in files)
                {
                    listInputImage.Items.Add(f);
                }
            }
        }

        
        //选中列表中的文件路径并进行图片处理
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listInputImage.SelectedItems.Count !=0)
            {
                this.groupBox1.Enabled = true;  
                this.ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text),parameterList);
            }
            else
            {
                this.groupBox1.Enabled = false;
            }

        }

        private void butSaveSplitImage_Click(object sender, EventArgs e)
        {
            if (this.listInputImage.Items.Count == 0)
            {
                MessageBox.Show("请先导入图片");
                return;
            }

            MessageBox.Show(" 请选择保存路径，处理过程中请不要进行其他操作！");

            string savePath;

            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                savePath = this.inputImageFolder.SelectedPath;
            }
            else return;

            List<string> files = new List<string>();
            for (int i = 0; i < this.listInputImage.Items.Count; i++)
            {
                files.Add(this.listInputImage.Items[i].Text);
            }

            AutoProcessImageByColor(files, parameterList, savePath);

            MessageBox.Show("处理完成");
       
        }

        private void HeightDividWidthLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightDivideWidthLow = (double)this.HeightDividWidthLow.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightDividWidthUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightDivedeWidthUp= (double)this.HeightDividWidthUp.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void WidthLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.WidthLow = (int)this.WidthLow.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void WidthUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.WidthUp= (int)this.WidthUp.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightLow = (int)this.HeightLow.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightUp = (int)this.HeightUp.Value;
            ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text), parameterList);
        }

    }
}
