
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


            public ParameterList(double hdwl,double hdwu,int hl,int hu,int wl,int wu)
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
                    if ((double)rect.Height / rect.Width > pl.HeightDivideWidthLow && (double)rect.Height / rect.Width < pl.HeightDivedeWidthUp
                    && rect.Height > pl.HeightLow && rect.Height < pl.HeightUp
                    && rect.Width > pl.WidthLow && rect.Width < pl.WidthUp)
                    {
                        rects.Add(rect);
                    }
                }
                //保存切分的图片
                int index = 0;
                foreach (Rect rect in rects)
                {
                    Mat roi = new Mat(matIn, rects[index]);
                    Cv2.ImWrite(saveFolderPath + "\\sample_" + i.ToString() + "_" + index.ToString() + ".jpg", roi);
                    index++;
                }
            }
        }
    }
}
