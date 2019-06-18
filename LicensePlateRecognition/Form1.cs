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




        Mat imgHsv = new Mat();
        Mat equalizeHistHsv = new Mat();
        Mat matYellow = new Mat();
        Mat matYellow_dilate = new Mat();
        Mat matYellow_erode = new Mat();
        Mat matContours = new Mat();
        Mat matRects = new Mat();
        //选中列表中的文件路径并进行图片处理
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

            //转为hsv图片          
            imgHsv = img_in.CvtColor(ColorConversionCodes.BGR2HSV);
            //对v均衡化后在合并
            Mat[] matToHsv = new Mat[3];
            Cv2.Split(imgHsv, out matToHsv);
            Cv2.EqualizeHist(matToHsv[2], matToHsv[2]);
            Cv2.Merge(matToHsv, equalizeHistHsv);

            //在均衡化后的hsv颜色空间红寻找黄色区域
            Scalar yellow_low = new Scalar(15,95,95);
            Scalar yellow_up = new Scalar(40,255,255);
            
            Cv2.InRange(equalizeHistHsv,yellow_low,yellow_up,matYellow);

            //使用形态学操作对选定颜色区域进行处理
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect,new OpenCvSharp.Size(7,3));           
            Cv2.Dilate(matYellow, matYellow_dilate, element);            
            Cv2.Erode(matYellow_dilate, matYellow_erode, element);

            //寻找轮廓
            OpenCvSharp.Point[][] contours; //vector<vector<Point>> contours;
            HierarchyIndex[] hierarchyIndexes; //vector<Vec4i> hierarchy;
            Cv2.FindContours(
                matYellow_erode,
                out contours,
                out hierarchyIndexes,
                mode: RetrievalModes.Tree,
                method: ContourApproximationModes.ApproxSimple); //求轮廓
            
            matContours = img_in.Clone();
            Cv2.DrawContours(matContours, contours, -1, new Scalar(0, 0, 255), 2); //画轮廓线

            //求轮廓的最小外接矩形          
            matRects = img_in.Clone();
            Rect[] rects = new Rect[contours.Length];
            int rectNum=0;
            for (int index =0;index<contours.Length;index++)
            {
                
                Rect rect = Cv2.BoundingRect(contours[index]);//用矩形包围区域
                if((double)rect.Width/rect.Height>2 && (double)rect.Width/rect.Height<10
                    && rect.Height>25 && rect.Height<125
                    && rect.Width>100 && rect.Width<400)
                {
                    
                    rects[index] = rect;
                    Cv2.Rectangle(matRects, rect, new Scalar(255, 0, 0), 1);
                    rectNum++;
                }
                else
                {
                    rects[index] = Rect.Empty;
                }       
            }
            Rect[] filterRect = new Rect[rectNum];
            int fRectNum = 0;
            for (int index = 0; index < rects.Length; index++)
            {
                if(rects[index]!=Rect.Empty)
                {
                    filterRect[fRectNum] = rects[index];
                    fRectNum++;
                }
              
            }

            // MessageBox.Show(rectNum.ToString());
            this.ShowMatSplitResult(img_in, filterRect);
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
                    case 1://灰度图
                        Mat img_out = new Mat();
                        img_out = img_in.CvtColor(ColorConversionCodes.BGR2GRAY);
                        this.pictureBox1.Image = img_out.ToBitmap();
                        img_out.Release();
                        break;
                    case 2: 
                        this.pictureBox1.Image = imgHsv.ToBitmap();
                        break;

                    case 3:  
                        this.pictureBox1.Image = equalizeHistHsv.ToBitmap();
                        break;

                    case 4:
                        this.pictureBox1.Image = matYellow.ToBitmap();
                        break;
                    case 5:
                        this.pictureBox1.Image = matYellow_erode.ToBitmap();
                        break;
                    case 6:
                        this.pictureBox1.Image = matContours.ToBitmap();
                        break;
                    case 7:
                        this.pictureBox1.Image = matRects.ToBitmap();
                        break;
                    case 8:
                        this.pictureBox1.Image = matContours.ToBitmap();
                        break;
                }
            }
        }



        //显示切分的图片
        void ShowMatSplitResult(Mat mat, Rect[] rects)
        {
            if (mat.Empty()) return;

            this.listView2.Items.Clear();
            this.imageList1.Images.Clear();


            for (int index=0;index<rects.Length;index++)
            {
                if(rects[index]!=Rect.Empty)
                {
                    Mat roi = new Mat(mat, rects[index]);

                    this.imageList1.Images.Add(roi.ToBitmap());
                    this.listView2.Items.Add(index.ToString());
                    //this.listView2.Items[index].ImageList.ImageSize = new System.Drawing.Size(rects[index].Width, rects[index].Height);
                    this.listView2.Items[index].ImageIndex = index;
                    
                }

            }


        }


    }
}
