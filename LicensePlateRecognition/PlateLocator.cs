
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    class PlateLocator
    {
        //车牌定位参数列表
        public struct ParameterList
        {
            public double HeightDivideWidthLow;
            public double HeightDivedeWidthUp;
            public int HeightLow;
            public int HeightUp;
            public int WidthLow;
            public int WidthUp;


            public ParameterList(double hdwl=0.15f,double hdwu=0.70f,int hl=10,int hu=80,int wl=40,int wu=180)
            {
                HeightDivideWidthLow = hdwl;
                HeightDivedeWidthUp = hdwu;
                HeightLow = hl;
                HeightUp = hu;
                WidthLow = wl;
                WidthUp = wu;

            }

        }

        //使用颜色法自动分割图片,参数为图片路径列表，处理参数列表，保存文件夹路径
        public static void AutoProcessImageByColor(List<string> files,ParameterList pl,string saveFolderPath)
        {
            for (int i = 0; i < files.Count; i++)
            {
                Mat matIn = new Mat(files[i]);
                List<Mat> mats = PlateLocateByColor(matIn,pl);

                for(int index = 0; index < mats.Count; index++)
                {
                    Cv2.ImWrite(saveFolderPath + "\\sample_" + i.ToString() + "_" + index.ToString() + ".jpg", mats[index]);
                }

            }

        }

        //车牌颜色定位
        public static List<Mat> PlateLocateByColor(Mat matIn, ParameterList pl)
        {
            List<Mat> matPlates = new List<Mat>();


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
                if ((double)rect.Height / rect.Width > pl.HeightDivideWidthLow && (double)rect.Height / rect.Width < pl.HeightDivedeWidthUp
                && rect.Height > pl.HeightLow && rect.Height < pl.HeightUp
                && rect.Width > pl.WidthLow && rect.Width < pl.WidthUp)
                {
                    rects.Add(rect);
                }
            }

            if (rects.Count == 0)
                return matPlates;

            for(int index = 0; index < rects.Count; index++)
            {
                Mat roi = new Mat(matIn, rects[index]);
                matPlates.Add(roi);
            }
            return matPlates;

        }
        public static List<Mat> PlateLocateByColor(Mat matIn)
        {
            List<Mat> matPlates = new List<Mat>();


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
                if ((double)rect.Height / rect.Width > 0.15f && (double)rect.Height / rect.Width < 0.70f
                && rect.Height > 10 && rect.Height < 80
                && rect.Width > 40 && rect.Width < 180)
                {
                    rects.Add(rect);
                }
            }

            if (rects.Count == 0)
                return matPlates;

            for (int index = 0; index < rects.Count; index++)
            {
                Mat roi = new Mat(matIn, rects[index]);
                matPlates.Add(roi);
            }
            return matPlates;

        }

        //车牌sobel定位
        public static List<Mat> PlateLocateBySobel(Mat matIn,ParameterList pl,
            int blur_Size = 5,
            int sobel_Scale = 1,
            int sobel_Delta = 0,
            int sobel_X_Weight = 1,
            int sobel_Y_Weight = 0,
            int morph_Size_Width = 17,
            int morph_Size_Height = 3)
        {
            List<Mat> matPlates = new List<Mat>();

            if (matIn.Empty()) return matPlates;

            Mat blur = matIn.GaussianBlur(new OpenCvSharp.Size(blur_Size, blur_Size), 0, 0, BorderTypes.Default); //高斯模糊
            Mat gray = blur.CvtColor(ColorConversionCodes.BGR2GRAY);
            //对图像进行sobel运算，得到的是图像的一阶水平方向导数
            MatType ddepth = MatType.CV_16S;
            Mat gradX = gray.Sobel(ddepth, 1, 0, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat absGradX = gradX.ConvertScaleAbs();
            Mat gradY = gray.Sobel(ddepth, 0, 1, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat absGradY = gradY.ConvertScaleAbs();

            Mat grad = new Mat();
            Cv2.AddWeighted(absGradX, sobel_X_Weight, absGradY, sobel_Y_Weight, 0, grad);

            //将图像二值化
            Mat threshold = grad.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

            //使用闭操作
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(morph_Size_Width, morph_Size_Height));
            Mat thresholdClose = threshold.MorphologyEx(MorphTypes.Close, element);

            Mat elementErode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
            Mat thresholdErode = thresholdClose.Erode(elementErode);

            //求轮廓
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchys = null;
            Cv2.FindContours(thresholdErode, out contours, out hierarchys, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            //求轮廓外接矩形
            List<Rect> rects = new List<Rect>();
            foreach (OpenCvSharp.Point[] p in contours)
            {
                Rect rect = Cv2.BoundingRect(p);
                if ((double)rect.Height / rect.Width > pl.HeightDivideWidthLow && (double)rect.Height / rect.Width < pl.HeightDivedeWidthUp
                && rect.Height > pl.HeightLow && rect.Height < pl.HeightUp
                && rect.Width > pl.WidthLow && rect.Width < pl.WidthUp)
                {
                    rects.Add(rect);
                }
            }

            if (rects.Count == 0)
                return matPlates;

            for (int index = 0; index < rects.Count; index++)
            {
                Mat roi = new Mat(matIn, rects[index]);
                matPlates.Add(roi);
            }

            return matPlates;
        }
        public static List<Mat> PlateLocateBySobel(Mat matIn,
            int blur_Size = 5,
            int sobel_Scale = 1,
            int sobel_Delta = 0,
            int sobel_X_Weight = 1,
            int sobel_Y_Weight = 0,
            int morph_Size_Width = 17,
            int morph_Size_Height = 3)
        {
            List<Mat> matPlates = new List<Mat>();

            if (matIn.Empty()) return matPlates;

            Mat blur = matIn.GaussianBlur(new OpenCvSharp.Size(blur_Size, blur_Size), 0, 0, BorderTypes.Default); //高斯模糊
            Mat gray = blur.CvtColor(ColorConversionCodes.BGR2GRAY);
            //对图像进行sobel运算，得到的是图像的一阶水平方向导数
            MatType ddepth = MatType.CV_16S;
            Mat gradX = gray.Sobel(ddepth, 1, 0, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat absGradX = gradX.ConvertScaleAbs();
            Mat gradY = gray.Sobel(ddepth, 0, 1, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat absGradY = gradY.ConvertScaleAbs();

            Mat grad = new Mat();
            Cv2.AddWeighted(absGradX, sobel_X_Weight, absGradY, sobel_Y_Weight, 0, grad);

            //将图像二值化
            Mat threshold = grad.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);

            //使用闭操作
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(morph_Size_Width, morph_Size_Height));
            Mat thresholdClose = threshold.MorphologyEx(MorphTypes.Close, element);

            Mat elementErode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
            Mat thresholdErode = thresholdClose.Erode(elementErode);

            //求轮廓
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchys = null;
            Cv2.FindContours(thresholdErode, out contours, out hierarchys, RetrievalModes.External, ContourApproximationModes.ApproxNone);

            //求轮廓外接矩形
            List<Rect> rects = new List<Rect>();
            foreach (OpenCvSharp.Point[] p in contours)
            {
                Rect rect = Cv2.BoundingRect(p);
                if ((double)rect.Height / rect.Width > 0.15f && (double)rect.Height / rect.Width < 0.70f
                && rect.Height > 10 && rect.Height < 80
                && rect.Width > 40 && rect.Width < 180)
                {
                    rects.Add(rect);
                }
            }

            if (rects.Count == 0)
                return matPlates;

            for (int index = 0; index < rects.Count; index++)
            {
                Mat roi = new Mat(matIn, rects[index]);
                matPlates.Add(roi);
            }

            return matPlates;
        }
    }
}
