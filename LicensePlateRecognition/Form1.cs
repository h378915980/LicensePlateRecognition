using System;
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
            InitAll();
        }


        


        //init
        private void InitAll()
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


            AddPlateCategoryForComboBox();

            //this.groupBoxForPlateLabel.Enabled = false;
            this.groupBoxForPlateParameter.Enabled = false;
            //this.comboBoxPlate.Text = this.comboBoxPlate.Items[1].ToString();

        }
        //添加车牌标签选项
        private void AddPlateCategoryForComboBox()
        {
            string[] plateCategory = Enum.GetNames(typeof(PlateCategory));
            for(int index = 0; index < plateCategory.Length; index++)
            {
                this.comboBoxPlate.Items.Add(plateCategory[index]);
            }

        }



        //functions
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
        //展示车牌
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


            ShowSpliteImage(rects, matIn);
            

        }

        //显示图片和相关信息
        private void ShowSpliteImage(List<Rect> rects,Mat matIn)
        {
            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();
            int index = 0;
            foreach (Rect rect in rects)
            {
                Mat roi = new Mat(matIn, rects[index]);
             
                if(!UserSetting.isAutoProcess)
                {
                    this.imgListSplitImage.Images.Add(roi.ToBitmap());
                    this.listShowSplitImage.Items.Add(index.ToString());
                    this.listShowSplitImage.Items[index].ImageIndex = index;  //这三条顺序还还不能换，佛了
                }
                else
                {
                    
                    this.imgListSplitImage.Images.Add(roi.ToBitmap());
                    this.listShowSplitImage.Items.Add(PlateCategorySVM.Test(roi).ToString());
                    this.listShowSplitImage.Items[index].ImageIndex = index;  //这三条顺序还还不能换，佛了
                }

                index++;
            }
        }

        //






        //展示字符
        private void ProcessAndShowChars(Bitmap plate)
        {
            currentTabCount = 0;

            Mat matIn = plate.ToMat();           
            AddTag("原图", matIn);

            // matIn = Utilities.GammaTransform(matIn);
            //AddTag("gamma增强", matIn);

            //matIn = Utilities.IndexTransform(matIn);
            //AddTag("指数增强", matIn);

            //matIn = Utilities.LogTransform(matIn);
            //AddTag("指数增强", matIn);

            //matIn = Utilities.LaplaceTransform(matIn);
            //AddTag("拉普拉斯",matIn);

            Mat matGray = matIn.CvtColor(ColorConversionCodes.RGB2GRAY);
            AddTag("灰度图", matGray);

            PlateColor plateColor = CharSegement.GetPlateColor(matIn);
            Mat matClear = CharSegement.ClearMaodingAndBorder(matGray,plateColor);
            AddTag("去边框与铆钉", matClear);

            //找轮廓
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchyIndices = null;
            matClear.FindContours(out contours, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            
            //求轮廓外接最小矩形
            List<Rect> rects = new List<Rect>();
            Mat matContours = matIn.Clone();
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rect = Cv2.BoundingRect(contours[index]);
                if (CharSegement.VerifyRect(rect) &&
                    CharSegement.NotOnBorder(rect,matIn.Size()))
                {
                    rects.Add(rect);
                    Cv2.Rectangle(matContours, rect, new Scalar(0, 0, 255), 1);
                }
            }
            AddTag("外接矩形",matContours);

            //去除内部矩形
            Mat matInner = matIn.Clone();
            rects = CharSegement.RejectInnerRectFromRects(rects);
            for(int index = 0; index < rects.Count; index++)
            {
                Cv2.Rectangle(matInner, rects[index], new Scalar(0, 0, 255), 1);
            }
            AddTag("去内部矩形", matInner);



            //调整矩形大小
            Mat matAdjust = matIn.Clone();
            rects = CharSegement.AdjustRects(rects);
            for (int index = 0; index < rects.Count; index++)
            {
                Cv2.Rectangle(matAdjust, rects[index], new Scalar(0, 0, 255), 1);
            }
            AddTag("调整大小",matAdjust);

            //展示切割结果
            rects = CharSegement.GetSafeRects(matIn,rects);
            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();
            this.imgListSplitImage.ImageSize = new System.Drawing.Size(16,32);
            int i = 0;
            rects = CharSegement.SortLeftRects(rects);
            foreach (Rect rect in rects)
            {
                Mat roi = new Mat(matIn, rects[i]);
                
                this.imgListSplitImage.Images.Add(roi.ToBitmap());
                this.listShowSplitImage.Items.Add(i.ToString());
                //this.listShowSplitImage.Items[index].ImageList.ImageSize = new System.Drawing.Size(rects[index].Width, rects[index].Height);
                this.listShowSplitImage.Items[i].ImageIndex = i;
                i++;
            }
        }



        //events
        //选择文件夹中的图片添加到列表       
        private void OpenImageFolder(object sender, EventArgs e)
        {
            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                this.listInputImage.Clear();
                List<string> files = FileIO.OpenFile(inputImageFolder.SelectedPath);
                foreach (string f in files)
                {
                    listInputImage.Items.Add(f);
                }
            }
        }
        //选中列表中的文件路径并进行图片处理
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.groupBoxForPlateLabel.Enabled = false;  //选择图片时先将打标签功能禁用
            

            if (listInputImage.SelectedItems.Count !=0)
            {
                
                this.groupBoxForPlateParameter.Enabled = true;
                //this.ProcessAndShowImage(new Bitmap(this.listInputImage.SelectedItems[0].Text),parameterList);
                this.ProcessAndShowChars(new Bitmap(this.listInputImage.SelectedItems[0].Text));
            }
            else
            {
                this.groupBoxForPlateParameter.Enabled = false;
            }

        }
        //选择切出来的图片进行处理
        private void listShowSplitImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listShowSplitImage.SelectedItems.Count != 0)
            //{
            //    this.groupBoxForPlateLabel.Enabled = true;
            //    Bitmap bitmap = (Bitmap)(this.imgListSplitImage.Images[this.listShowSplitImage.SelectedItems[0].ImageIndex]);
            //    selectedImage = bitmap;  //这里不太好
            //}
            //else
            //{
            //    this.groupBoxForPlateLabel.Enabled = false;
            //}
        }
        //单个保存
        private void butSavePlateCategory_Click(object sender, EventArgs e)
        {
            if(listShowSplitImage.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先选择图片");
                return;
            }

            if (UserSetting.savePath == null)
            {
                MessageBox.Show("请先在设置中添加保存路径");
                return;
            }

            if (this.comboBoxPlate.Text == "")
            {
                MessageBox.Show("请先为图片添加一个标签");
                return;
            }

            if (UserSetting.isFolderReady == false)
            {
                FileIO.PrepareTrainningPlateDirectory(UserSetting.savePath);  //添加文件夹
                UserSetting.isFolderReady = true;
            }

           
           
            for(int index = 0; index < this.listShowSplitImage.SelectedItems.Count; index++)
            {
                Bitmap bitmap = (Bitmap)(this.imgListSplitImage.Images[this.listShowSplitImage.SelectedItems[index].ImageIndex]);
                DateTime now = DateTime.Now;
                string time = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}-{6:000000}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, new Random().Next(1000));
                string sp = UserSetting.savePath + @"\plates\" + this.comboBoxPlate.Text + @"\" + time + ".jpg";//保存路径
                //MessageBox.Show(sp);
                Cv2.ImWrite(sp, bitmap.ToMat()); //添加图片

                this.listShowSplitImage.SelectedItems[index].Remove();  
            }

        }
        //全部保存
        private void butAutoSavePlateCategory(object sender, EventArgs e)
        {
            if (!UserSetting.isAutoProcess)
            {
                MessageBox.Show("请先在设置中开启自动处理功能");
                return;
            }

            for (int index = 0; index < this.listShowSplitImage.Items.Count; index++)
            {
                Bitmap bitmap = (Bitmap)(this.imgListSplitImage.Images[this.listShowSplitImage.Items[index].ImageIndex]);
                DateTime now = DateTime.Now;
                string time = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}-{6:000000}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, new Random().Next(1000));

                string sp = UserSetting.savePath + @"\plates\" + this.listShowSplitImage.Items[index].Text + @"\" + time + ".jpg";//保存路径
                //MessageBox.Show(sp);
                Cv2.ImWrite(sp, bitmap.ToMat()); //添加图片             
            }
            this.listShowSplitImage.Clear();

            if (this.listInputImage.SelectedItems.Count == 0)
                return;

            if (this.listInputImage.SelectedItems[0].Index + 1 >= this.listInputImage.Items.Count)
            {
                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Remove();
                return;
            }


            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index + 1].Selected = true;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Selected = false;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index - 1].Remove();


        }
        //修改标签
        private void comboBoxPlate_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int index = 0; index < this.listShowSplitImage.SelectedItems.Count; index++)
            {
                this.listShowSplitImage.SelectedItems[index].Text = this.comboBoxPlate.Text;
                

            }
        }


        //批量处理列表文件
        private void AutoProcessImage(object sender, EventArgs e)
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


        //测试用按钮
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.listInputImage.SelectedItems.Count == 0)
                return;

            if (this.listInputImage.SelectedItems[0].Index + 1 >= this.listInputImage.Items.Count)
            {
                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Remove();
                return;
            }
                

            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index + 1].Selected = true;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Selected = false;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index - 1].Remove();

        }

        //训练
        private void TrainPlate(object sender, EventArgs e)
        {
            if(UserSetting.savePath==null)
            {
                MessageBox.Show("请先在设置中设置训练样本保存路径");
                return;
            }

            List<PlateCategorySVM.SVMFileInfo> fileInfos = FileIO.PrepareTrainningPlateDirectory(UserSetting.savePath);

            UserSetting.isFolderReady = true;

            if (PlateCategorySVM.Train(fileInfos))
            {
                PlateCategorySVM.Save(UserSetting.savePath);
                MessageBox.Show("svm已经准备好");
            }
            else
            {
                MessageBox.Show("训练文件夹中无图片，请先手动添加一部分");
                return;
            }
            


        }

        //打开设置界面
        private void OpenUserSetting(object sender, EventArgs e)
        {
            Form form = new UserSetting();
            form.ShowDialog();
        }

        
    }
}
