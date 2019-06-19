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
        

        public Form1()
        {
            InitializeComponent();
        }


        //functions
        private void OpenFile(string[] filePath)
        {
            foreach(string f in filePath)
            {
                if(
                    f.EndsWith(".jpg") ||
                    f.EndsWith(".png") ||
                    f.EndsWith(".bmp"))
                {

                    listInputImage.Items.Add(f);
                }
            }
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
        

        private void ProcessAndShowImage(Bitmap image,
            double heightDivideWidthLow=0.15f,
            double heightDivedeWidthUp=0.7f,
            int heightLow=10,
            int heightUp=80,
            int widthLow=40,
            int widthUp=180)
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
                if ((double)rect.Height / rect.Width > 0.15f && (double)rect.Height / rect.Width < 0.7f
                    && rect.Height > 10 && rect.Height < 80
                    && rect.Width > 40 && rect.Width < 180)
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

        //这里看看能不能实现一个进度框
        private void AutoProcessImage(
            double heightDivideWidthLow = 0.15f,
            double heightDivedeWidthUp = 0.7f,
            int heightLow = 10,
            int heightUp = 80,
            int widthLow = 40,
            int widthUp = 180)
        {

            if(this.listInputImage.Items.Count==0)
            {
                MessageBox.Show("请先导入图片");
                return;
            }

            MessageBox.Show(" 请选择保存路径，处理过程中请不要进行其他操作！");

            string savePath;

            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                savePath = this.inputImageFolder.SelectedPath+"\\sample_";
                //MessageBox.Show(savePath);
                
            }
            else return;


            for(int i=0;i<this.listInputImage.Items.Count;i++)
            {
                //MessageBox.Show(this.listInputImage.Items[i].Text);
                Mat matIn = new Mat(this.listInputImage.Items[i].Text);
                //转为hsv图片    
                Mat matHsv = matIn.CvtColor(ColorConversionCodes.BGR2HSV);
                //对v均衡化后在合并
                Mat[] matToHsv = new Mat[3];
                Cv2.Split(matHsv, out matToHsv);
                Cv2.EqualizeHist(matToHsv[2], matToHsv[2]);
                Mat equalizeHistHsv = new Mat();
                Cv2.Merge(matToHsv, equalizeHistHsv);

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

                //使用形态学操作对选定颜色区域进行处理
                Mat matAllDilate = new Mat();
                Mat matAllErode = new Mat();
                Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 3));
                Cv2.Dilate(matAll, matAllDilate, element);
                Cv2.Erode(matAllDilate, matAllErode, element);


                //寻找轮廓
                OpenCvSharp.Point[][] contours; //vector<vector<Point>> contours;
                HierarchyIndex[] hierarchyIndexes; //vector<Vec4i> hierarchy;
                Cv2.FindContours(
                    matAllErode,
                    out contours,
                    out hierarchyIndexes,
                    mode: RetrievalModes.Tree,
                    method: ContourApproximationModes.ApproxSimple); //求轮廓
                //求轮廓的最小外接矩形          
                List<Rect> rects = new List<Rect>();
                foreach (OpenCvSharp.Point[] p in contours)
                {
                    Rect rect = Cv2.BoundingRect(p);
                    if ((double)rect.Height / rect.Width > 0.15f && (double)rect.Height / rect.Width < 0.7f
                        && rect.Height > 10 && rect.Height < 80
                        && rect.Width > 40 && rect.Width < 180)
                    {
                        rects.Add(rect);
                    }
                }
                //保存切分的图片
                int index = 0;
                foreach (Rect rect in rects)
                {
                    Mat roi = new Mat(matIn, rects[index]);
                    Cv2.ImWrite(savePath+i.ToString()+"_"+index.ToString()+".jpg",roi);
                    index++;
                }


            }
            MessageBox.Show("处理完成");
        }

        //events
        //选择文件夹中的图片添加到列表
        private void InputImage(object sender, EventArgs e)
        {
            
            if(this.inputImageFolder.ShowDialog()==DialogResult.OK)
            {
                this.listInputImage.Clear();
                this.OpenFile(System.IO.Directory.GetFiles(inputImageFolder.SelectedPath));
            }
        }

        
        //选中列表中的文件路径并进行图片处理
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listInputImage.SelectedItems.Count !=0)
            {
                ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text));              
            }

        }

        private void butSaveSplitImage_Click(object sender, EventArgs e)
        {
            this.AutoProcessImage();
        }
    }
}
