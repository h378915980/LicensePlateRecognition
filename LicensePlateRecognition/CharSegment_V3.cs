using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using OpenCvSharp;

namespace LicensePlateRecognition
{
    class CharSegment_V3
    {
        //清除铆钉
        public static Mat ClearMaoding(Mat threshold)
        {
            List<float> jumps = new List<float>();
            Mat jump = new Mat(threshold.Rows, 1, MatType.CV_32F);
            for (int rowIndex = 0; rowIndex < threshold.Rows; rowIndex++)
            {
                int jumpCount = 0;
                for (int colIndex = 0; colIndex < threshold.Cols - 1; colIndex++)
                {
                    if (threshold.At<byte>(rowIndex, colIndex) != threshold.At<byte>(rowIndex, colIndex + 1))
                        jumpCount++;
                }
                jump.Set<float>(rowIndex, 0, (float)jumpCount);
            }
            int x = 7;
            Mat result = threshold.Clone();
            for (int rowIndex = 0; rowIndex < threshold.Rows; rowIndex++)
            {
                if (jump.At<float>(rowIndex) <= x)
                {
                    for (int colIndex = 0; colIndex < threshold.Cols; colIndex++)
                    {
                        result.Set<byte>(rowIndex, colIndex, 0);
                    }
                }
            }
            return result;
        }
        //清除边界
        public static Mat ClearBorder(Mat threshold)
        {
            int rows = threshold.Rows;
            int cols = threshold.Cols;
            int noJumpCountThresh = (int)(0.15f * cols);
            Mat border = new Mat(rows, 1, MatType.CV_8UC1);
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                int noJumpCount = 0;
                byte isBorder = 0;
                for (int colIndex = 0; colIndex < cols - 1; colIndex++)
                {
                    if (threshold.At<byte>(rowIndex, colIndex) == threshold.At<byte>(rowIndex, colIndex + 1))
                        noJumpCount++;
                    if (noJumpCount > noJumpCountThresh)
                    {
                        noJumpCount = 0;
                        isBorder = 1;
                        break;
                    }
                }
                border.Set<byte>(rowIndex, 0, isBorder);
            }
            int minTop = (int)(0.1f * rows);
            int maxTop = (int)(0.9f * rows);
            Mat result = threshold.Clone();
            for (int rowIndex = 0; rowIndex < minTop; rowIndex++)
            {
                if (border.At<byte>(rowIndex, 0) == 1)
                {
                    for (int colIndex = 0; colIndex < cols; colIndex++)
                    {
                        result.Set<byte>(rowIndex, colIndex, 0);
                    }
                }
            }
            for (int rowIndex = rows - 1; rowIndex > maxTop; rowIndex--)
            {
                if (border.At<byte>(rowIndex, 0) == 1)
                {
                    for (int colIndex = 0; colIndex < cols; colIndex++)
                    {
                        result.Set<byte>(rowIndex, colIndex, 0);
                    }
                }
            }
            return result;
        }
        //清除铆钉和边界
        public static Mat ClearMaodingAndBorder(Mat gray, PlateColor plateColor)
        {
            Mat threshold = null;
            switch (plateColor)
            {
                case PlateColor.蓝牌:
                case PlateColor.黑牌:
                    threshold = gray.Threshold(1, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                    break;
                case PlateColor.黄牌:
                case PlateColor.白牌:
                case PlateColor.绿牌:
                    threshold = gray.Threshold(1, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
                    break;
                case PlateColor.未知:
                    threshold = gray.Threshold(1, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                    break;
            }
            Mat matOfClearBoder = ClearBorder(threshold);
            Mat matOfClearMaodingAndBorder = ClearMaoding(matOfClearBoder);
            return matOfClearMaodingAndBorder;
        }
        //分割字符
        public static List<CharInfo> SpliteCharsInPlateMat(Mat plateMat, List<Rect> rects)
        {
            if (PlateChar_SVM.IsReady == false)
            {
                throw new Exception("字符识别库没有准备好");
            }
            List<CharInfo> result = new List<CharInfo>();
            for (int index = 0; index < rects.Count; index++)
            {
                Rect rect = rects[index];
                rect = Utilities.GetSafeRect(rect, plateMat);
                CharInfo charInfo = new CharInfo();
                Mat originalMat = plateMat.SubMat(rect);
                charInfo.OriginalMat = originalMat;
                charInfo.OriginalRect = rect;
                charInfo.PlateChar = PlateChar_SVM.Test(originalMat);
                result.Add(charInfo);
            }
            result.Sort(new CharInfoLeftComparer());
            return result;
        }
        //自动样品分割
        public static List<CharInfo> SplitePlateForAutoSample(Mat plateMat)
        {
            List<CharInfo> result = new List<CharInfo>();
            List<CharInfo> charInfos_Original_Blue = SplitePlateByOriginal(plateMat, plateMat, PlateColor.蓝牌);
            List<CharInfo> charInfos_IndexTransform_Blue = SplitePlateByIndexTransform(plateMat,
            PlateColor.蓝牌);
            List<CharInfo> charInfos_GammaTransform_Blue = SplitePlateByGammaTransform(plateMat,
            PlateColor.蓝牌);
            List<CharInfo> charInfos_LogTransform_Blue = SplitePlateByLogTransform(plateMat, PlateColor.蓝牌);
            List<CharInfo> charInfos_Blue = new List<CharInfo>();
            charInfos_Blue.AddRange(charInfos_Original_Blue.ToArray());
            charInfos_Blue.AddRange(charInfos_IndexTransform_Blue.ToArray());
            charInfos_Blue.AddRange(charInfos_GammaTransform_Blue.ToArray());
            charInfos_Blue.AddRange(charInfos_LogTransform_Blue.ToArray());
            int isCharCount = 0;
            for (int index = 0; index < charInfos_Blue.Count; index++)
            {
                CharInfo charInfo = charInfos_Blue[index];
                charInfo.PlateChar = PlateChar_SVM.Test(charInfo.OriginalMat);
                if (charInfo.PlateChar != PlateChar.非字符) isCharCount++;
            }
            if (isCharCount >= 15) return charInfos_Blue;
            //如果用蓝色切分字符，少于15个，就再用黄色尝试切分
            List<CharInfo> charInfos_Original_Yellow = SplitePlateByOriginal(plateMat, plateMat, PlateColor.黄牌);
            List<CharInfo> charInfos_IndexTransform_Yellow = SplitePlateByIndexTransform(plateMat,
            PlateColor.黄牌);
            List<CharInfo> charInfos_GammaTransform_Yellow = SplitePlateByGammaTransform(plateMat,
            PlateColor.黄牌);
            List<CharInfo> charInfos_LogTransform_Yellow = SplitePlateByLogTransform(plateMat,
            PlateColor.黄牌);
            List<CharInfo> charInfos_Yellow = new List<CharInfo>();
            charInfos_Yellow.AddRange(charInfos_Original_Yellow.ToArray());
            charInfos_Yellow.AddRange(charInfos_IndexTransform_Yellow.ToArray());
            charInfos_Yellow.AddRange(charInfos_GammaTransform_Yellow.ToArray());
            charInfos_Yellow.AddRange(charInfos_LogTransform_Yellow.ToArray());
            isCharCount = 0;
            for (int index = 0; index < charInfos_Yellow.Count; index++)
            {
                CharInfo charInfo = charInfos_Yellow[index];
                charInfo.PlateChar = PlateChar_SVM.Test(charInfo.OriginalMat);
                if (charInfo.PlateChar != PlateChar.非字符) isCharCount++;
            }
            if (isCharCount >= 15) return charInfos_Yellow;
            return new List<CharInfo>(); //返回?长度为零的集合
        }
        //通过索引变换分割
        public static List<CharInfo> SplitePlateByIndexTransform(Mat originalMat,
                                                                    PlateColor plateColor,
                                                                    int leftLimit = 0, int rightLimit = 0,
                                                                    int topLimit = 0, int bottomLimit = 0,
                                                                    int minWidth = 2, int maxWidth = 30,
                                                                    int minHeight = 10, int maxHeight = 80,
                                                                    float minRatio = 0.08f, float maxRatio = 2f)
        {
            Mat plateMat = Utilities.IndexTransform(originalMat);
            return SplitePlateByOriginal(originalMat, plateMat, plateColor,
            CharSplitMethod.指数,
            leftLimit, rightLimit,
            topLimit, bottomLimit,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
        }
        //通过对数变换分割
        public static List<CharInfo> SplitePlateByLogTransform(Mat originalMat,
                                                            PlateColor plateColor,
                                                            int leftLimit = 0, int rightLimit = 0,
                                                            int topLimit = 0, int bottomLimit = 0,
                                                            int minWidth = 2, int maxWidth = 30,
                                                            int minHeight = 10, int maxHeight = 80,
                                                            float minRatio = 0.08f, float maxRatio = 2f)
        {
            Mat plateMat = Utilities.LogTransform(originalMat);
            return SplitePlateByOriginal(originalMat, plateMat, plateColor,
            CharSplitMethod.对数,
            leftLimit, rightLimit,
            topLimit, bottomLimit,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
        }
        //利用伽马变换分割
        public static List<CharInfo> SplitePlateByGammaTransform(Mat originalMat,
                                                                PlateColor plateColor,
                                                                float gammaFactor = 0.40f,
                                                                int leftLimit = 0, int rightLimit = 0,
                                                                int topLimit = 0, int bottomLimit = 0,
                                                                int minWidth = 2, int maxWidth = 30,
                                                                int minHeight = 10, int maxHeight = 80,
                                                                float minRatio = 0.08f, float maxRatio = 2f)
        {
            Mat plateMat = Utilities.GammaTransform(originalMat, gammaFactor);
            return SplitePlateByOriginal(originalMat, plateMat, plateColor,
            CharSplitMethod.伽马,
            leftLimit, rightLimit,
            topLimit, bottomLimit,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
        }
        //原始分割
        public static List<CharInfo> SplitePlateByOriginal(Mat originalMat, Mat plateMat,
                                            PlateColor plateColor,
                                            CharSplitMethod charSplitMethod = CharSplitMethod.原图,
                                            int leftLimit = 0, int rightLimit = 0,
                                            int topLimit = 0, int bottomLimit = 0,
                                            int minWidth = 2, int maxWidth = 30,
                                            int minHeight = 10, int maxHeight = 80,
                                            float minRatio = 0.08f, float maxRatio = 2f)
        {
            List<CharInfo> result = new List<CharInfo>();
            Mat gray = plateMat.CvtColor(ColorConversionCodes.BGR2GRAY);
            Mat matOfClearMaodingAndBorder = ClearMaodingAndBorder(gray, plateColor);
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchyIndices = null;
            matOfClearMaodingAndBorder.FindContours(out contours, out hierarchyIndices,
            RetrievalModes.External, ContourApproximationModes.ApproxNone);
            List<Rect> rects = new List<Rect>();
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rect = Cv2.BoundingRect(contours[index]);
                if (NotOnBorder(rect, plateMat.Size(), leftLimit, rightLimit, topLimit, bottomLimit)
                    && VerifyRect(rect, minWidth, maxWidth, minHeight, maxHeight, minRatio, maxRatio))
                {
                    rects.Add(rect);
                }
            }
            rects = RejectInnerRectFromRects(rects);
            //rects = RejectOutFromRects(rects);
            rects = AdjustRects(rects);
            if (rects.Count == 0) return result;
            for (int index = 0; index < rects.Count; index++)
            {
                CharInfo plateCharInfo = new CharInfo();
                rects[index] = Utilities.GetSafeRect(rects[index], originalMat);
                Rect rectROI = rects[index];
                Mat matROI = originalMat.SubMat(rectROI);
                plateCharInfo.OriginalMat = matROI;
                plateCharInfo.OriginalRect = rectROI;
                plateCharInfo.CharSplitMethod = charSplitMethod;
                result.Add(plateCharInfo);
            }
            result.Sort(new CharInfoLeftComparer());
            return result;
        }
        //校验矩形
        public static bool VerifyRect(Rect rect,
                                        int minWidth = 2, int maxWidth = 30,
                                        int minHeight = 10, int maxHeight = 80,
                                        float minRatio = 0.08f, float maxRatio = 2f)
        {
            int width = rect.Width;
            int height = rect.Height;
            if (width == 0 || height == 0) return false;
            float ratio = (float)width / height;
            float area = width * height;
            return ((width > minWidth && width < maxWidth) &&
            (height > minHeight && height < maxHeight) &&
            (ratio > minRatio && ratio < maxRatio));
        }
        //校验是否在边界
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
        //按照Left_ASC排序矩形
        public static void SortRectsByLeft_ASC(List<Rect> rects)
        {
            rects.Sort(new RectLeftComparer());
        }
        //按照Height_ASC排序矩形
        public static void SortRectsByHeight_ASC(List<Rect> rects)
        {
            rects.Sort(new RectHeightComparer());
        }
        //合并矩形
        public static Rect MergeRect(Rect A, Rect B)
        {
            Rect result = new Rect();
            int minX = (A.X <= B.X) ? A.X : B.X;
            int minY = (A.Y <= B.Y) ? A.Y : B.Y;
            int maxRight = (A.Right >= B.Right) ? A.Right : B.Right;
            int maxBottom = (A.Bottom >= B.Bottom) ? A.Bottom : B.Bottom;
            result.X = minX;
            result.Y = minY;
            result.Width = maxRight - minX;
            result.Height = maxBottom - minY;
            return result;
        }
        //调整矩形
        public static List<Rect> AdjustRects(List<Rect> rects)
        {
            float averageHeight = GetRectsAverageHeight(rects);
            float heightLimit = averageHeight * 0.5f;
            int medianTop = GetMedianRectsTop(rects);
            int medianBottom = GetMedianRectsBottom(rects);
            for (int index = rects.Count - 1; index >= 0; index--)
            {
                Rect rect = rects[index];
                if (rect.Height >= heightLimit && rect.Height < averageHeight)
                {
                    int offsetTop = Math.Abs(rect.Top - medianTop);
                    int offsetBottom = Math.Abs(rect.Bottom - medianBottom);
                    if (offsetTop > offsetBottom)
                    {
                        rect.Y = (int)(rect.Bottom - averageHeight);
                    }
                    rect.Height = (int)averageHeight + 5;
                    rects[index] = rect;
                }
            }
            return rects;
        }
        //合并矩形数组
        public static List<Rect> MergeRects(List<Rect> rects)
        {
            List<int> indexesOfMerge = new List<int>();
            List<int> indexesBeMerged = new List<int>();
            int maxHeight = GetRectsMaxHeight(rects);
            float averageHeight = GetRectsAverageHeight(rects);
            float hightLimit = averageHeight * 0.5f;
            for (int index = rects.Count - 1; index >= 0; index--)
            {
                if (indexesBeMerged.Contains(index)) continue;
                if (indexesOfMerge.Contains(index)) continue;
                Rect A = rects[index];
                if (A.Height < hightLimit) continue;
                for (int i = rects.Count - 1; i >= 0; i--)
                {
                    if (i == index) continue;
                    Rect B = rects[i];
                    if (B.Height > hightLimit) continue;
                    if ((A.Left >= B.Left && A.Right >= B.Right) || (A.Left <= B.Left && A.Right <= B.Right))
                    {
                        Rect rectMerge = MergeRect(A, B);
                        if (VerifyRect(rectMerge))
                        {
                            indexesBeMerged.Add(i);
                            rects[index] = rectMerge;
                            indexesOfMerge.Add(index);
                        }
                    }
                }
            }
            List<Rect> result = new List<Rect>();
            for (int index = 0; index < rects.Count; index++)
            {
                if (indexesBeMerged.Contains(index) == false)
                {
                    result.Add(rects[index]);
                }
            }
            return result;
        }

        //public static List<Rect> RejectOutFromRects(List<Rect> rects)
        //{
        // int medianTop = GetMedianRectsTop(rects);
        // int medianBottom = GetMedianRectsBottom(rects);
        // float heightLimit = GetRectsAverageHeight(rects) / 4;
        // for (int index = rects.Count - 1; index >= 0; index--)
        // {
        // Rect rect = rects[index];
        // if ((rect.Bottom < medianTop || rect.Top > medianBottom) && rect.Height < heightLimit)
        // {
        // rects.RemoveAt(index);
        // }
        // }
        // return rects;
        //}
        //拒绝内部矩形
        public static List<Rect> RejectInnerRectFromRects(List<Rect> rects)
        {
            for (int index = rects.Count - 1; index >= 0; index--)
            {
                Rect rect = rects[index];
                for (int i = 0; i < rects.Count; i++)
                {
                    Rect rectTemp = rects[i];
                    if ((rect.X >= rectTemp.X && rect.Y >= rectTemp.Y &&
                    rect.Right <= rectTemp.Right && rect.Bottom <= rectTemp.Bottom) &&
                    (rect.Width < rectTemp.Width || rect.Height < rectTemp.Height))
                    {
                        rects.RemoveAt(index);
                        break;
                    }
                }
            }
            return rects;
        }
        //得到直线平均的高度
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
        //得到矩形最大高度
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
        //获取矩形上方的中位数
        public static int GetMedianRectsTop(List<Rect> rects)
        {
            if (rects.Count == 0) return 0;
            rects.Sort(new RectTopComparer());
            int midianIndex = rects.Count / 2;
            return rects[midianIndex].Top;
        }
        //获取矩形下方的中位数
        public static int GetMedianRectsBottom(List<Rect> rects)
        {
            if (rects.Count == 0) return 0;
            rects.Sort(new RectBottomComparer());
            int midianIndex = rects.Count / 2;
            return rects[midianIndex].Bottom;
        }

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
