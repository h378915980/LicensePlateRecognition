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

        //判断车牌颜色,千万要通过byte获取!!!!!!!
        public static PlateColor GetPlateColor(Mat matPlate)
        {

            Mat matHSV = matPlate.CvtColor(ColorConversionCodes.BGR2HSV); //转为hsv空间

            Mat[] matToHsv = new Mat[3];   //分为三通道
            Cv2.Split(matHSV, out matToHsv);

            Mat matH = matToHsv[0];
            Mat matS = matToHsv[1];
            Mat matV = matToHsv[2];

            int blueNum = 0;
            int YellowNum = 0;

            for(int i = 0; i < matHSV.Rows; i++)
            {
                for(int j=0;j<matHSV.Cols;j++)
                {
                    
                    byte hValue = matH.At<byte>(i, j);  //获取各处的值
                    byte sValue = matS.At<byte>(i, j);
                    byte vValue = matV.At<byte>(i, j);

                    if ((hValue > 90 && hValue<120) && (sValue > 80 && sValue < 220) && (vValue > 80 && vValue < 255))
                        blueNum++;

                    else if ((hValue > 11 && hValue < 34) && (sValue > 43 && sValue < 255) && (vValue > 46 && vValue < 255))
                        YellowNum++;
                }
            }

            Console.WriteLine(blueNum);
            if (blueNum > YellowNum)
                return PlateColor.BLUE;
            else
                return PlateColor.YELLOW;

        }


        //注意！！输入的矩阵必须是单通道的灰度图！
        //只清除了水平边框，垂直边框没有清除的必要
        public static Mat ClearBorder(Mat matIn)
        {
            int rows = matIn.Rows;
            int cols = matIn.Cols;

            int noJumpCountThreshold = (int)(0.15f * cols); //判断是否是边框的阈值

            Mat border = new Mat(rows,1,MatType.CV_8UC1);   //边框，跟原车牌行数相同但只有一列

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

            int minTop = (int)(0.15f * rows);   //为了避免将字抹去，只设置最上面和最下面的两小行范围
            int maxTop = (int)(0.85f * rows);

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

        //根据原图切割，返回该车牌上的字符的信息
        public static List<CharInfo> SplitePlateByOriginal(Mat matOriginal,Mat matPlate,
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
                   // Cv2.Rectangle(matPlate, rect, new Scalar(0, 0, 255), 1);
                }
            }

            rects = RejectInnerRectFromRects(rects);  //去掉矩形内部的矩形
            rects = AdjustRects(rects);        //对矩形大小等进行调整
            rects = GetSafeRects(matOriginal, rects);   //检查安全矩形

            if (rects.Count == 0) return result;

            for (int index = 0; index < rects.Count; index++)
            {
                CharInfo plateCharInfo = new CharInfo(); //字符信息
                Rect rectROI = rects[index];   //字符所在矩形
                Mat matROI = matOriginal.SubMat(rectROI);  //字符的图片
                plateCharInfo.OriginalMat = matROI;
                plateCharInfo.OriginalRect = rectROI;

                result.Add(plateCharInfo);
            }            
            return result;
        }

        public static List<Mat> SplitePlateByOriginal(Mat matPlate,
            int leftLimit = 0, int rightLimit = 0,
            int topLimit = 0, int bottomLimit = 0,
            int minWidth = 2, int maxWidth = 30,
            int minHeight = 10, int maxHeight = 80,
            float minRatio = 0.08f, float maxRatio = 2f)
        {

            List<Mat> matChars = new List<Mat>();
  
            Mat matGray = matPlate.CvtColor(ColorConversionCodes.BGR2GRAY);//先将车牌转为灰度图,注意蓝黄牌会有很大区别
            Mat matClear = ClearMaodingAndBorder(matGray, GetPlateColor(matPlate)); //去除铆钉和边框
            //找轮廓
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchyIndices = null;
            matClear.FindContours(out contours, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            //求轮廓外接最小矩形
            List<Rect> rects = new List<Rect>();
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rect = Cv2.BoundingRect(contours[index]);
                if (VerifyRect(rect, minWidth, maxWidth, minHeight, maxHeight, minRatio, maxRatio) &&
                    NotOnBorder(rect, matPlate.Size(), leftLimit, rightLimit, topLimit, bottomLimit))
                {
                    rects.Add(rect);
                }
            }

            rects = RejectInnerRectFromRects(rects);  //去掉矩形内部的矩形
            rects = AdjustRects(rects);        //对矩形大小等进行调整
            rects = GetSafeRects(matPlate, rects);   //检查安全矩形
            rects = SortLeftRects(rects);      //存排序结果

            if (rects.Count == 0) return matChars;

            for (int index = 0; index < rects.Count; index++)
            {
                Mat roi = new Mat(matPlate,rects[index]);
                matChars.Add(roi);

            }
            return matChars;
        }























        //通过gamma增强原图后在切割
        public static List<CharInfo> SplitePlateByGammaTransform(Mat originalMat,
        PlateColor plateColor,
        float gammaFactor = 0.40f,
        int leftLimit = 0, int rightLimit = 0,
        int topLimit = 0, int bottomLimit = 0,
        int minWidth = 1, int maxWidth = 30,
        int minHeight = 10, int maxHeight = 80,
        float minRatio = 0.08f, float maxRatio = 2f)
        {
            Mat plateMat = Utilities.GammaTransform(originalMat, gammaFactor);

            return SplitePlateByOriginal(originalMat, plateMat, plateColor,
            leftLimit, rightLimit,
            topLimit, bottomLimit,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
        }

        public static void AutoProcessCharSegement(List<string> files,string savePath)
        {

            for(int i = 0; i < files.Count; i++)
            {
                Mat matIn = new Mat(files[i]);
                Mat matGray = matIn.CvtColor(ColorConversionCodes.RGB2GRAY);

                PlateColor plateColor = GetPlateColor(matIn);
                Mat matClear = ClearMaodingAndBorder(matGray, plateColor);

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
                    if (VerifyRect(rect) &&
                        NotOnBorder(rect, matIn.Size()))
                    {
                        rects.Add(rect);
                        Cv2.Rectangle(matContours, rect, new Scalar(0, 0, 255), 1);
                    }
                }

                //去除内部矩形
                Mat matInner = matIn.Clone();
                rects =RejectInnerRectFromRects(rects);
                for (int index = 0; index < rects.Count; index++)
                {
                    Cv2.Rectangle(matInner, rects[index], new Scalar(0, 0, 255), 1);
                }



                //调整矩形大小
                Mat matAdjust = matIn.Clone();
                rects = AdjustRects(rects);
                for (int index = 0; index < rects.Count; index++)
                {
                    Cv2.Rectangle(matAdjust, rects[index], new Scalar(0, 0, 255), 1);
                }
                rects = GetSafeRects(matIn, rects);

                //保存切分的图片
                int j = 0;
                foreach (Rect rect in rects)
                {
                    Mat roi = new Mat(matIn, rects[j]);
                    Cv2.ImWrite(savePath + "\\sample_" + i.ToString() + "_" + j.ToString() + ".jpg", roi);
                    j++;
                }
            }

        }








        //筛选出符合大小条件的矩形
        public static bool VerifyRect(Rect rect,
        int minWidth = 1, int maxWidth = 25,
        int minHeight = 10, int maxHeight = 80,
        float minRatio = 0.0001f, float maxRatio = 2f)
        {
            int width = rect.Width;
            int height = rect.Height;

            if (width == 0 || height == 0) return false;
            float ratio = (float) width / height;

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


        //调整矩形大小
        public static List<Rect> AdjustRects(List<Rect> rects)
        {
            float heightAverage = GetRectsAverageHeight(rects); //高度平均值
            float heightLimit = heightAverage * 0.5f;           //高度限定值
            float highestHeight = GetRectsHighestHeight(rects); //得到矩形最大高度
            float highAveRadio = highestHeight / heightAverage; //最大高度与平均高度的比值

            int topMedian = GetMedianRectsTop(rects);           //顶部中位数
            int BottomMedian = GetMedianRectsBottom(rects);     //底部中位数

            for(int index = rects.Count - 1; index >= 0; index--)
            {
                Rect rect = rects[index];
                float rectAveRadio = heightAverage / rect.Height;
                if ( rect.Height < heightAverage )   
                {
                    int topOffset = Math.Abs(rect.Top - topMedian);             //与顶部差值
                    int bottomOffset = Math.Abs(rect.Bottom - BottomMedian);    //与底部差值

                    //那边偏移值大就选另一边
                    if (topOffset > bottomOffset)      
                    {
                        rect.Y = (int)(rect.Bottom - heightAverage);
                    }
                    
                    rect.Height = (int)(heightAverage +3);
                    rects[index] = rect;
                }
            }
            return rects;
        }
        //去掉矩形的内部矩形和左右的矩形
        public static List<Rect> RejectInnerRectFromRects(List<Rect> rects)
        {
            if (rects.Count == 0)
                return rects;

            for (int index = rects.Count - 1; index >= 0; index--)
            {
                Rect rect = rects[index];
                float limitWidth = 2.0f * rect.Width;               
                for (int i = 0; i < rects.Count; i++)
                {

                    Rect rectTemp = rects[i];
                    int lldis = rect.X - rectTemp.X;
                    int lrdis = rectTemp.Right - rect.X;
                    int rrdis = rect.Right - rectTemp.Right;
                    int limitdis = (int)(0.25 * rectTemp.Width);
                    //去掉内部的矩形
                    if (rect.X == rectTemp.X && rect.Right != rectTemp.Right) 
                    {
                        if (rect.Width <= 5 )
                        {
                            rects.RemoveAt(index);
                            break;
                        }
                        if (rect.Width < rectTemp.Width && rectTemp.Width <= limitWidth) 
                        {
                            rects.RemoveAt(index);
                            break;
                        }
                        if (rect.Width < rectTemp.Width && rectTemp.Width > limitWidth)
                        {
                            rectTemp.X = rect.Right;
                            rectTemp.Width = rectTemp.Width - rect.Width;
                            rects[i] = rectTemp;
                            break;
                        }
                    }

                    if (rect.X > rectTemp.X && rect.X < rectTemp.Right) 
                    {
                        if (rect.Width <= 5) 
                        {
                            rects.RemoveAt(index);
                            break;
                        }
                        //位于左边0.25之内时
                        if (lldis <= limitdis && rectTemp.Width <= limitWidth) 
                        {
                            rects.RemoveAt(index);
                            break;
                        }
                        if (lldis <= limitdis && rectTemp.Width > limitWidth)
                        {
                            rectTemp.X = rect.Right;
                            rectTemp.Width = rectTemp.Width - lldis - rect.Width;
                            rects[i] = rectTemp;
                            break;
                        }
                        //位于右边0.25时
                        if (lrdis <= limitdis && lrdis >= 0.5 * rect.Width && rectTemp.Width <= limitWidth) 
                        {
                            rects.RemoveAt(index);
                            break;
                        }
                        if (lrdis <= limitdis && lrdis < 0.5 * rect.Width && rectTemp.Width <= limitWidth) 
                        {
                            rect.X = rectTemp.Right;
                            rect.Width = rect.Width - lrdis;
                            break;
                        }
                        if (lrdis <= limitdis && rectTemp.Width > limitWidth)
                        {
                            rectTemp.Width = rectTemp.Width - lrdis;
                            rects[i] = rectTemp;
                            break;
                        }
                        //位于中间0.25~0.75部分
                        if (lldis > limitdis && lrdis > limitdis) 
                        {
                            if (rectTemp.Width <= limitWidth && lldis > 0.5 * rectTemp.Width)  
                            {
                                rects.RemoveAt(i);
                                break;
                            }
                            if (rectTemp.Width <= limitWidth && lldis <= 0.5 * rectTemp.Width)
                            {
                                rects.RemoveAt(index);
                                break;
                            }
                            if (rectTemp.Width > limitWidth && lldis >= lrdis) 
                            {
                                rectTemp.Width = rectTemp.Width - lrdis;
                                rects[i] = rectTemp;
                                break;
                            }
                            if (rectTemp.Width > limitWidth && lldis < lrdis)
                            {
                                rectTemp.X = rect.Right;
                                rectTemp.Width = rectTemp.Width - lldis - rect.Width;
                                rects[i] = rectTemp;
                                break;
                            }
                        }

                    }
                                        
                //    if ((rect.X >= rectTemp.X && rect.Y >= rectTemp.Y &&
                //    rect.Right <= rectTemp.Right && rect.Bottom <= rectTemp.Bottom) &&
                //    (rect.Width < rectTemp.Width || rect.Height < rectTemp.Height))      //避免将自己除去
                //    {
                //        rects.RemoveAt(index);
                //        break;
                //    }


                //    if (rect.X >= rectTemp.X && rect.X < rectTemp.Right && rect.Width < rectTemp.Width)
                //    {
                //        rects.RemoveAt(index);
                //        break;
                //    }

                //    if (rect.X > rectTemp.X && rect.Width == rectTemp.Width && rect.X <= (rectTemp.X + 0.6 * rectTemp.Width))
                //    {
                //        rects.RemoveAt(index);
                //        break;
                //    }

                //    if (rect.X > rectTemp.X && rect.Width == rectTemp.Width && rect.X > (rectTemp.X + 0.6 * rectTemp.Width))
                //    {
                //        rect.X = rectTemp.Right;
                //        break;
                //    }
                    

                //    if (rect.X >= rectTemp.X && rect.X <= rectTemp.Right && rect.Width > rectTemp.Width)
                //    {
                //        rect.X = rectTemp.Right;
                //        rect.Width = rect.Width - rectTemp.Right + rect.X;
                //        break;
                //    }
                }
                                   
            }
            rects = SortLeftRects(rects);
            if (rects[0].Width <= 6 && rects[0].Right < 7)
            {
                rects.RemoveAt(0);
            }
            
            

            if (rects.Count!=0&& rects[0].Width <= 2)
                rects.RemoveAt(0);
            
            if (rects.Count > 7)
            {
                for (int i = 7; i < rects.Count; i++)
                    rects.RemoveAt(i);
            }
            return rects;
        }

        //获取安全矩形
        public static List<Rect> GetSafeRects(Mat m,List<Rect> rects)
        {

            List<Rect> result = new List<Rect>();
            for(int index=0;index<rects.Count;index++)
            {
                Rect roi = rects[index];
                if (!(0 <= roi.X && 0 <= roi.Width && roi.X + roi.Width <= m.Cols && 0 <= roi.Y && 0 <= roi.Height && roi.Y + roi.Height <= m.Rows))
                {
                    continue;
                }
                if (roi.Left < 0)
                    roi.Left = 0;
                if (roi.Top < 0)
                    roi.Top = 0;
                if (roi.Right > m.Cols)
                    roi.Width = m.Cols - roi.Left;
                if (roi.Bottom > m.Rows)
                    roi.Height = m.Rows - roi.Top;

                result.Add(roi);
            }
            return result;
        }

        //获得矩形最高高度
        public static float GetRectsHighestHeight(List<Rect> rects)
        {
            float highestHeight = 0f;
            if (rects.Count == 0)
                return highestHeight;
            foreach (var rect in rects)
            {
                if (rect.Height > highestHeight)
                    highestHeight = rect.Height;
            }
            return highestHeight;
        }

        //获得矩形平均高度
        public static float GetRectsAverageHeight(List<Rect> rects)
        {
            float heightTotal = 0f;
            if (rects.Count == 0) return heightTotal;
            foreach (var rect in rects)
            {
                heightTotal += rect.Height;
            }

            return heightTotal / rects.Count;
        }

        //获得矩形最大高度
        public static int GetRectsMaxHeight(List<Rect> rects)
        {
            int maxHeight = 0;
            if (rects.Count == 0) return maxHeight;
            foreach (var rect in rects)
            {
                if (maxHeight < rect.Height) maxHeight = rect.Height;
            }

            return maxHeight;
        }

        //获得矩形顶部中位数
        public static int GetMedianRectsTop(List<Rect> rects)
        {
            if (rects.Count == 0) return 0;

            rects.Sort(new RectTopComparer());
            int midianIndex = rects.Count / 2;

            return rects[midianIndex].Top;
        }

        //获得矩形底部中位数
        public static int GetMedianRectsBottom(List<Rect> rects)
        {
            if (rects.Count == 0) return 0;
            
            rects.Sort(new RectBottomComparer());
            int midianIndex = rects.Count / 2;

            return rects[midianIndex].Bottom;
        }

        //将矩形数组从左到右排列
        public static List<Rect> SortLeftRects(List<Rect> rects)
        {
            if (rects.Count == 0)
                return rects;
            
            rects.Sort(new RectLeftComparer());
            return rects;
        }


        //比较大小
        private class RectTopComparer : IComparer<Rect>
        {
            public int Compare(Rect x, Rect y)
            {
                return x.Top.CompareTo(y.Top);
            }
        }
        private class RectBottomComparer : IComparer<Rect>
        {
            public int Compare(Rect x, Rect y)
            {
                return x.Bottom.CompareTo(y.Bottom);
            }
        }
        private class RectHeightComparer : IComparer<Rect>
        {
            public int Compare(Rect x, Rect y)
            {
                return x.Height.CompareTo(y.Height);
            }
        }
        private class RectLeftComparer : IComparer<Rect>
        {
            public int Compare(Rect x, Rect y)
            {
                return x.X.CompareTo(y.X);
            }
        }
        private class CharInfoLeftComparer : IComparer<CharInfo>
        {
            public int Compare(CharInfo x, CharInfo y)
            {
                return x.OriginalRect.X.CompareTo(y.OriginalRect.X);
            }
        }

    }
}
