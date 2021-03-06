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

namespace LicensePlateRecognition
{
    public partial class Form1 : Form
    {
        //初始化参数列表
        PlateLocator.ParameterList parameterList = new PlateLocator.ParameterList(
            hdwl: 0.15f,
            hdwu: 0.70f,
            hl: 10,
            hu: 80,
            wl: 40,
            wu: 180);


        public Form1()
        {          
            InitializeComponent();
            Form form = new Welcome();
            form.ShowDialog();
            InitAll();
        }

        Sunisoft.IrisSkin.SkinEngine skinEngine1;
        UserSetting.ShowTypes showTypes=UserSetting.ShowTypes.UNKNOW;


        //init
        private void InitAll()
        {

            //C#皮肤文件加载
            this.Text = "studio";
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.skinEngine1.SkinFile = Application.StartupPath + "//OmegaSkin.ssk";

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
            AddCharCategoryForComboBox();

            this.groupBoxForPlateLabel.Enabled = false;
            this.groupBoxForPlateParameter.Enabled = false;
            this.groupBoxForChar.Enabled = false;

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
        //添加字符标签选项
        private void AddCharCategoryForComboBox()
        {
            string[] plateCategory = Enum.GetNames(typeof(PlateChar));
            for (int index = 0; index < plateCategory.Length; index++)
            {
                this.comboBoxChar.Items.Add(plateCategory[index]);
            }
        }



        //显示部分
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
        private void ProcessAndShowImage(Bitmap image,PlateLocator.ParameterList pl)
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
            matClear.FindContours(out contours, out hierarchyIndices, RetrievalModes.External,
                 ContourApproximationModes.ApproxNone);
            
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

            //得到安全矩形
            rects = CharSegement.GetSafeRects(matIn,rects); ;

            //展示切割结果
            ShowSpliteImage(rects, matIn);
        }
        //展示最终结果
        private void ProcessAndShowResult(Mat matIn)
        {
            currentTabCount = 0;

            AddTag("原图", matIn);
            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();
            this.imgListSplitImage.ImageSize = new System.Drawing.Size(192, 64);



            List<Mat> roiPlates = PlateLocator.PlateLocateByColor(matIn); //疑似区域
            List<Mat> matPlates = new List<Mat>(); //车牌区域
            for (int index = 0; index < roiPlates.Count; index++)
            {
                Mat mat = roiPlates[index];
                if (PlateCategory.车牌 != PlateCategorySVM.Test(mat))
                    continue;
                else
                    matPlates.Add(mat);

            }
            //颜色不行就用sobel
            if (matPlates.Count == 0)
            {
                roiPlates.Clear();
                roiPlates = PlateLocator.PlateLocateBySobel(matIn);
                for (int index = 0; index < roiPlates.Count; index++)
                {
                    Mat mat = roiPlates[index];
                    if (PlateCategory.车牌 != PlateCategorySVM.Test(mat))
                        continue;
                    else
                        matPlates.Add(mat);
                }
            }

            if (matPlates.Count == 0)
            {
                MessageBox.Show("没有识别出车牌");
                return;
            }

            


            //下面根据识别出的车牌进行字符识别
            for (int index = 0; index < matPlates.Count; index++)
            {
                string plate = null;
                Mat mat = matPlates[index];

                List<Mat> roiChars = new List<Mat>();
                roiChars = CharSegement.SplitePlateByOriginal(mat);

                if (roiChars.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < roiChars.Count; i++)
                {
                    plate = plate + CharCategorySVM.Test(roiChars[i]).ToString();
                }
                plate = plate.Replace("_", "");
                plate = plate.Replace("非字符", "");

                this.imgListSplitImage.Images.Add(mat.ToBitmap());
                this.listShowSplitImage.Items.Add(plate);
                this.listShowSplitImage.Items[index].ImageIndex = index;
            }

           //MessageBox.Show(PlateRecognition.PlateRecognite(matIn));
        }
        //显示图片和相关信息
        private void ShowSpliteImage(List<Rect> rects, Mat matIn)
        {
            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();


            if (this.showTypes == UserSetting.ShowTypes.PLATE)
            {
                this.imgListSplitImage.ImageSize = new System.Drawing.Size(96, 32);

                int index = 0;
                foreach (Rect rect in rects)
                {
                    Mat roi = new Mat(matIn, rects[index]);

                    if (!UserSetting.isAutoProcessPlate)
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
                return;
            }

            if (this.showTypes == UserSetting.ShowTypes.CHAR)
            {
                this.imgListSplitImage.ImageSize = new System.Drawing.Size(16, 32);
                int index = 0;

                rects = CharSegement.SortLeftRects(rects);

                foreach (Rect rect in rects)
                {
                    Mat roi = new Mat(matIn, rects[index]);

                    if (!UserSetting.isAutoProcessChar)
                    {
                        this.imgListSplitImage.Images.Add(roi.ToBitmap());
                        this.listShowSplitImage.Items.Add(index.ToString());
                        this.listShowSplitImage.Items[index].ImageIndex = index;  //这三条顺序还还不能换，佛了
                    }
                    else
                    {

                        this.imgListSplitImage.Images.Add(roi.ToBitmap());
                        this.listShowSplitImage.Items.Add(CharCategorySVM.Test(roi).ToString());
                        this.listShowSplitImage.Items[index].ImageIndex = index;  //这三条顺序还还不能换，佛了
                    }

                    index++;
                }
                return; 
            }

            if (this.showTypes == UserSetting.ShowTypes.All)
            {


            }
        }



        //events
        //选择车牌文件夹中的图片添加到列表       
        private void OpenImageFolder(object sender, EventArgs e)
        {
            
            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                this.listInputImage.Clear();
                List<string> files = FileIO.OpenFile(inputImageFolder.SelectedPath);
                foreach (string f in files)
                {
                    listInputImage.Items.Add(Path.GetFileName(f));
                }
            }
            else
            {
                return;
            }

            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();

            this.showTypes = UserSetting.ShowTypes.PLATE;
            this.tabControl1.TabPages[0].Enabled = true;
            this.tabControl1.TabPages[1].Enabled = false;

            this.tabControl1.SelectedTab = this.tabControl1.TabPages[0];


            //初始化一下tab界面
            for (int i = this.tabShowDiffImage.TabPages.Count - 1; i >= 0; i--)
            {
                this.tabShowDiffImage.TabPages.RemoveAt(i);
            }
        }
        //选择字符文件夹中的图片添加到列表
        private void butOpenCharFolder(object sender, EventArgs e)
        {
            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                this.listInputImage.Clear();
                List<string> files = FileIO.OpenFile(inputImageFolder.SelectedPath);
                foreach (string f in files)
                {
                    listInputImage.Items.Add(Path.GetFileName(f));
                }
            }
            else
            {
                return;
            }

            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();

            this.showTypes = UserSetting.ShowTypes.CHAR;
            this.tabControl1.TabPages[0].Enabled = false;
            this.tabControl1.TabPages[1].Enabled = true;

            this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];



            //初始化一下tab界面
            for (int i = this.tabShowDiffImage.TabPages.Count - 1; i >= 0; i--)
            {
                this.tabShowDiffImage.TabPages.RemoveAt(i);
            }
        }
        //选择待识别文件夹中的图片添加到列表
        private void butOpenPlateFolder(object sender, EventArgs e)
        {

            if(UserSetting.plateSVMPath==null || UserSetting.charSVMPath==null)
            {
                MessageBox.Show("请先在设置中添加识别库");
                return;
            }

            PlateCategorySVM.Load(UserSetting.plateSVMPath);
            CharCategorySVM.Load(UserSetting.charSVMPath);


            if (this.inputImageFolder.ShowDialog() == DialogResult.OK)
            {
                this.listInputImage.Clear();
                List<string> files = FileIO.OpenFile(inputImageFolder.SelectedPath);
                foreach (string f in files)
                {
                    listInputImage.Items.Add(Path.GetFileName(f));
                }
            }
            else
            {
                return;
            }

            this.listShowSplitImage.Items.Clear();
            this.imgListSplitImage.Images.Clear();

            this.showTypes = UserSetting.ShowTypes.All;
            this.tabControl1.TabPages[0].Enabled = false;
            this.tabControl1.TabPages[1].Enabled = false;

           
            //初始化一下tab界面
            for (int i = this.tabShowDiffImage.TabPages.Count - 1; i >= 0; i--)
            {
                this.tabShowDiffImage.TabPages.RemoveAt(i);
            }
        }


        //选中列表中的文件路径并进行图片处理
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.showTypes==UserSetting.ShowTypes.UNKNOW)
            {
                MessageBox.Show("文件夹类型错误");
                return; 
            }

            if (this.showTypes == UserSetting.ShowTypes.PLATE)
            {
                if (listInputImage.SelectedItems.Count != 0)
                {
                    this.groupBoxForPlateParameter.Enabled = true;
                    this.ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath+@"\"+this.listInputImage.SelectedItems[0].Text), parameterList);
                }
                else
                {
                    this.groupBoxForPlateParameter.Enabled = false;
                }
                return;
            }

            if (this.showTypes == UserSetting.ShowTypes.CHAR)
            {
                if (listInputImage.SelectedItems.Count != 0)
                {
                    this.ProcessAndShowChars(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text));
                }
                else
                {

                }

                return;
            }
            
            if (this.showTypes == UserSetting.ShowTypes.All)
            {
                if (listInputImage.SelectedItems.Count != 0)
                {
                    ProcessAndShowResult(new Mat(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text));

                }
                else
                {

                }

                return;
            }

        }
        //选择切出来的图片进行处理
        private void listShowSplitImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listShowSplitImage.SelectedItems.Count != 0)
            {
                this.groupBoxForPlateLabel.Enabled = true;
                this.groupBoxForChar.Enabled = true;
            }
            else
            {
                this.groupBoxForChar.Enabled = false;
                this.groupBoxForPlateLabel.Enabled = false;

            }

        }
        //


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

            if (UserSetting.isPlateFolderReady == false)
            {
                FileIO.PrepareTrainningPlateDirectory(UserSetting.savePath);  //添加文件夹
                UserSetting.isPlateFolderReady = true;
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

            }

            for (int index = this.listShowSplitImage.SelectedItems.Count-1; index >=0 ; index--)
            {
                this.listShowSplitImage.SelectedItems[0].Remove();

            }



            if (this.listShowSplitImage.Items.Count == 0)
            {

                if (this.listInputImage.SelectedItems.Count == 0)  //没有了
                    return;

                if (this.listInputImage.SelectedItems[0].Index + 1 >= this.listInputImage.Items.Count) //最后一个
                {
                    this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Remove();
                    return;
                }


                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index + 1].Selected = true;
                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Selected = false;
                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index - 1].Remove();
            }

        }
        private void butSaveCharCategory_Click(object sender, EventArgs e)
        {
            if (listShowSplitImage.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先选择图片");
                return;
            }

            if (UserSetting.savePath == null)
            {
                MessageBox.Show("请先在设置中添加保存路径");
                return;
            }

            if (this.comboBoxChar.Text == "")
            {
                MessageBox.Show("请先为图片添加一个标签");
                return;
            }

            if (UserSetting.isCharFolderReady == false)
            {
                FileIO.PrepareTrainningCharDirectory(UserSetting.savePath);  //添加文件夹
                UserSetting.isCharFolderReady = true;
            }



            for (int index = 0; index < this.listShowSplitImage.SelectedItems.Count; index++)
            {
                Bitmap bitmap = (Bitmap)(this.imgListSplitImage.Images[this.listShowSplitImage.SelectedItems[index].ImageIndex]);
                DateTime now = DateTime.Now;
                string time = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}-{6:000000}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, new Random().Next(1000));
                string sp = UserSetting.savePath + @"\chars\" + this.comboBoxChar.Text + @"\" + time + ".jpg";//保存路径
                //MessageBox.Show(sp);
                Cv2.ImWrite(sp, bitmap.ToMat()); //添加图片

            }

            for (int index = this.listShowSplitImage.SelectedItems.Count - 1; index >= 0; index--)
            {
                this.listShowSplitImage.SelectedItems[0].Remove();

            }


            if (this.listShowSplitImage.Items.Count == 0)
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
        }


        //全部保存
        private void butAutoSavePlateCategory(object sender, EventArgs e)
        {
            if (!UserSetting.isAutoProcessPlate)
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
        private void butAutoSaveCharCategory(object sender, EventArgs e)
        {
            if (!UserSetting.isAutoProcessChar)
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

                string sp = UserSetting.savePath + @"\chars\" + this.listShowSplitImage.Items[index].Text + @"\" + time + ".jpg";//保存路径
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
        private void comboBoxChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int index = 0; index < this.listShowSplitImage.SelectedItems.Count; index++)
            {
                this.listShowSplitImage.SelectedItems[index].Text = this.comboBoxChar.Text;

            }
        }
        //丢弃
        private void butDrop_Click(object sender, EventArgs e)
        {
            if (this.listInputImage.SelectedItems.Count == 0)
            {
                this.listShowSplitImage.Clear();
                return;
            }
                

            if (this.listInputImage.SelectedItems[0].Index + 1 >= this.listInputImage.Items.Count)
            {
                this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Remove();
                return;
            }


            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index + 1].Selected = true;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index].Selected = false;
            this.listInputImage.Items[this.listInputImage.SelectedItems[0].Index - 1].Remove();
        }

        //批量处理列表文件
        private void AutoProcessImage(object sender, EventArgs e)
        {


            if(this.showTypes!=UserSetting.ShowTypes.PLATE)
            {
                MessageBox.Show("当前列表打开图片不对！");
                return;
            }


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

            PlateLocator.AutoProcessImageByColor(files, parameterList, savePath);

            MessageBox.Show("处理完成");
        }
        private void AutoProcessChar(object sender, EventArgs e)
        {
            if (this.showTypes != UserSetting.ShowTypes.CHAR)
            {
                MessageBox.Show("当前列表打开图片不对！");
                return;
            }

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

            CharSegement.AutoProcessCharSegement(files,savePath);

            MessageBox.Show("处理完成");


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

            UserSetting.isPlateFolderReady = true;

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
        private void TrainChar(object sender, EventArgs e)
        {
            if (UserSetting.savePath == null)
            {
                MessageBox.Show("请先在设置中设置训练样本保存路径");
                return;
            }

            List<CharCategorySVM.SVMFileInfo> fileInfos = FileIO.PrepareTrainningCharDirectory(UserSetting.savePath);

            UserSetting.isPlateFolderReady = true;

            if (CharCategorySVM.Train(fileInfos))
            {
                CharCategorySVM.Save(UserSetting.savePath);
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
        //帮助
        private void OpenHelpForm(object sender, EventArgs e)
        {
            Form form = new HelpWeb();
            form.ShowDialog();
        }

        //测试用按钮
        private void button7_Click(object sender, EventArgs e)
        {
            //Thread progressthread = new Thread(new ParameterizedThreadStart(ThreadForProcessBar));
            //object maxValue = 500;
            //progressthread.Start(maxValue);
            if (this.listInputImage.SelectedItems.Count == 0)
                return;

            string fp = this.listInputImage.SelectedItems[0].Text;
            string fn = Path.GetFileName(fp);
            string sp = @"C:\Users\18446\Desktop\Plat_test\" + fn;
            Mat mat = new Mat(fp);
            Cv2.ImWrite(sp, mat);
            

        }


        //进度条管理
        private void ThreadForProcessBar(object length)
        {
            ProgressBar progressBar = new ProgressBar();
            progressBar.progressBar1.Maximum = (int)length;
            progressBar.Show();

            for(int i = 0; i < (int)length; i++)
            {

                progressBar.AddProcess();
                Thread.Sleep(50);
            }
            progressBar.Close();
        }




        //动态修改
        private void HeightDividWidthLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightDivideWidthLow = (double)this.HeightDividWidthLow.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightDividWidthUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightDivedeWidthUp = (double)this.HeightDividWidthUp.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void WidthLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.WidthLow = (int)this.WidthLow.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void WidthUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.WidthUp = (int)this.WidthUp.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightLow_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightLow = (int)this.HeightLow.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

        private void HeightUp_ValueChanged(object sender, EventArgs e)
        {
            this.parameterList.HeightUp = (int)this.HeightUp.Value;
            ProcessAndShowImage(new Bitmap(this.inputImageFolder.SelectedPath + @"\" + this.listInputImage.SelectedItems[0].Text), parameterList);
        }

     
    }
}
