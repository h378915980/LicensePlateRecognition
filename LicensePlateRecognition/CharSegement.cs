using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    class CharSegement
    {

        //只清除了水平边框，垂直边框没有清除的必要
        public static Mat ClearBorder(Mat matIn)
        {
            int rows = matIn.Rows;
            int cols = matIn.Cols;

            int noJumpCountThreshold = (int)(0.15f * cols);  //判断是否是边框的阈值

            Mat border = new Mat(rows,1,MatType.CV_8UC1); //边框，跟原车牌行数相同但只有一列

            //清除水平边框，思路是只要有连续的一长段（0.15cols）相同就将这一行当作边框
            for(int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                int noJumpCount = 0;  
                byte isBorder = 0;

                for(int colIndex = 0; colIndex < cols-1; colIndex++)
                {
                    if (matIn.At<byte>(rowIndex, colIndex) == matIn.At<byte>(rowIndex, colIndex + 1))  //每一列值与后一列比较
                        noJumpCount++;  

                    if (noJumpCount > noJumpCountThreshold)
                    {
                        noJumpCount = 0;
                        isBorder = 1;
                        break;
                    }
                }
                border.Set<byte>(rowIndex, 0, isBorder); //得出哪一行是边框
            }

            int minTop = (int)(0.1f * rows);   //为了避免将字抹去，只设置最上面和最下面的两小行范围
            int maxTop = (int)(0.9f * rows);

            Mat matResult = matIn.Clone();

            for(int rowIndex = 0; rowIndex < minTop; rowIndex++)
            {
                if(border.At<byte>(rowIndex,0)==1) //说明这一列是矩形边框
                {
                    for(int colIndex=0;colIndex<cols;colIndex++)
                    {
                        matResult.Set<byte>(rowIndex, colIndex, 0);
                    }

                }
            }

            for (int rowIndex = rows - 1; rowIndex > maxTop; rowIndex--)
            {
                if (border.At<byte>(rowIndex, 0) == 1)
                {
                    for (int colIndex = 0; colIndex < cols; colIndex++)
                    {
                        matResult.Set<byte>(rowIndex, colIndex, 0);
                    }
                }
            }

            return matResult;
        }


        //清除车牌上的铆钉
        public static Mat ClearMaoding(Mat matIn)
        {
            Mat matResult = matIn.Clone();

            List<float> jumps = new List<float>();
            Mat jump = new Mat(matIn.Rows, 1, MatType.CV_32F); //一列矩阵

            for(int rowIndex = 0; rowIndex < matIn.Rows; rowIndex++)
            {
                int jumpCount = 0;
                for(int colIndex=0;colIndex<matIn.Cols-1;colIndex++)
                {
                    if (matIn.At<byte>(rowIndex, colIndex) != matIn.At<byte>(rowIndex, colIndex + 1)) 
                        jumpCount++; //每一列与后一列比较

                }

                jump.Set<float>(rowIndex, 0, (float)jumpCount); //记录每一行的跳变数
            }

            int thresholdJump = 7;   //跳变阈值(7个字）
            for(int rowIndex = 0; rowIndex < matIn.Rows; rowIndex++)
            {
                if (jump.At<float>(rowIndex) <= thresholdJump)
                {
                    for(int colIndex = 0; colIndex < matIn.Cols; colIndex++)
                    {
                        matResult.Set<byte>(rowIndex, colIndex, 0);
                    }
                }
            }

            return matResult;
        }

        //清除铆钉和边框
        public static Mat ClearMaodingAndBorder(Mat matGray,PlateColor plateColor)
        {
            Mat threshold = null;
            //注意后面的thresholdTypes参数，将黄牌的白底黑字转为和蓝牌一样的黑底白字
            switch(plateColor)
            {
                case PlateColor.BLUE:
                    threshold = matGray.Threshold(1,255,ThresholdTypes.Otsu|ThresholdTypes.Binary);
                    break;
                case PlateColor.YELLOW:
                    threshold = matGray.Threshold(1, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
                    break;
                case PlateColor.UNKNOW:
                    threshold = matGray.Threshold(1, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                    break;

            }

            Mat matOfClearBoder = ClearBorder(threshold);
            Mat matOfClearMaodingAndBorder = ClearMaoding(matOfClearBoder);

            return matOfClearMaodingAndBorder;
        }

        public static List<CharInfo> SpliteCharsInPlateMat(Mat matPlate,List<Rect> rects)
        {
            List<CharInfo> result = new List<CharInfo>();


            return result;
        }

        public static List<CharInfo> SplitePlateForAutoSample(Mat matPlate)
        {
            List<CharInfo> result = new List<CharInfo>();





            return result;
        }


        public static Mat SplitePlateByOriginal(Mat matOriginal,Mat matPlate,
            PlateColor plateColor,
            int leftLimit = 0, int rightLimit = 0,
            int topLimit = 0, int bottomLimit = 0,
            int minWidth = 2, int maxWidth = 30,
            int minHeight = 10, int maxHeight = 80,
            float minRatio = 0.08f, float maxRatio = 2f)
        {
            List<CharInfo> result = new List<CharInfo>();

            Mat matGray = matPlate.CvtColor(ColorConversionCodes.BGR2GRAY);//先将车牌转为灰度图,注意蓝黄牌会有很大区别
            Mat matClear = ClearMaodingAndBorder(matGray, plateColor); //去除铆钉和边框
            //找轮廓
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchyIndices = null;
            matClear.FindContours(out contours, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            //求轮廓外接最小矩形
            List<Rect> rects = new List<Rect>();
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rect = Cv2.BoundingRect(contours[index]);
                if (VerifyRect(rect,minWidth,maxWidth,minHeight,maxHeight,minRatio,maxRatio)&&
                    NotOnBorder(rect,matPlate.Size(),leftLimit,rightLimit,topLimit,bottomLimit))
                {
                    rects.Add(rect);
                    Cv2.Rectangle(matPlate, rect, new Scalar(0, 0, 255), 1);
                }
            }

            rects = RejectInnerRectFromRects(rects);  //去掉矩形内部的矩形
            
            return matPlate;
        }



        //筛选出符合大小条件的矩形
        public static bool VerifyRect(Rect rect,
        int minWidth = 2, int maxWidth = 30,
        int minHeight = 10, int maxHeight = 80,
        float minRatio = 0.08f, float maxRatio = 2f)
        {
            int width = rect.Width;
            int height = rect.Height;

            if (width == 0 || height == 0) return false;
            float ratio = (float)width / height;

            return ((width > minWidth && width < maxWidth) &&
            (height > minHeight && height < maxHeight) &&
            (ratio > minRatio && ratio < maxRatio));
        }

        //判断是否在规定的边界框范围内
        public static bool NotOnBorder(Rect rectToJudge,
        OpenCvSharp.Size borderSize,
        int leftLimit = 0,
        int rightLimit = 0,
        int topLimit = 0,
        int bottomLimit = 0)
        {
            float leftPercent = leftLimit / 100f;
            float rightPercent = rightLimit / 100f;
            float topPercent = topLimit / 100f;
            float bottomPercent = bottomLimit / 100f;

            float widthPercent = 1f - leftPercent - rightPercent;
            float heightPercent = 1f - topPercent - bottomPercent;

            int xLimit = (int)(borderSize.Width * leftPercent);
            int yLimit = (int)(borderSize.Height * topPercent);
            int widthLimit = (int)(borderSize.Width * widthPercent);
            int heightLimit = (int)(borderSize.Height * heightPercent);

            Rect rectLimit = new Rect();
            rectLimit.X = xLimit;
            rectLimit.Y = yLimit;
            rectLimit.Width = widthLimit;
            rectLimit.Height = heightLimit;

            return rectLimit.Contains(rectToJudge);

        }


        private static List<Rect> AdjustRects(List<Rect> rects)
        {
            throw new NotImplementedException();
        }

        private static List<Rect> RejectInnerRectFromRects(List<Rect> rects)
        {
            throw new NotImplementedException();
        }



    }
}
