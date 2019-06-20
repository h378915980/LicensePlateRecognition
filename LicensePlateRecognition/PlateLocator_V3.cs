using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using System.IO;

namespace LicensePlateRecognition
{
    class PlateLocator_V3
    {
        //提前过滤长宽以及长宽比不符合的疑似车牌部分
        public static bool VerifyPlateSize(OpenCvSharp.Size size,
                                           int minWidth = 60, int maxWidth = 180,
                                           int minHeight = 18, int maxHeight = 80,
                                           float minRatio = 0.15f, float maxRatio = 0.70f)
        {
            int width = size.Width;
            int height = size.Height;
            if (width == 0 || height == 0) return false;

            float ratio = (float)height / width;
            bool result = (width > minWidth && width < maxWidth) &&
            (height > minHeight && height < maxHeight) &&
            (ratio > minRatio && ratio < maxRatio);
            return result;
        }
        //使用sobel算子过滤车牌大小不符合的
        public static List<PlateInfo> LocatePlatesForCameraAdjust(Mat matSource,
            out Mat matProcess,
            int blur_Size = 5,
            int sobel_Scale = 1,
            int sobel_Delta = 0,
            int sobel_X_Weight = 1,
            int sobel_Y_Weight = 0,
            int morph_Size_Width = 17,
            int morph_Size_Height = 3,
            int minWidth = 60, int maxWidth = 250,
            int minHeight = 18, int maxHeight = 100,
            float minRatio = 0.15f, float maxRatio = 0.70f)
            {
                List<PlateInfo> plateInfos = LocatePlatesForAutoSample(matSource,
                out matProcess,
                blur_Size,
                sobel_Scale,
                sobel_Delta,
                sobel_X_Weight,
                sobel_Y_Weight,
                morph_Size_Width,
                morph_Size_Height,
                minWidth, maxWidth,
                minHeight, maxHeight,
                minRatio, maxRatio);
                for (int index = plateInfos.Count - 1; index >= 0; index--)
                {
                    if (plateInfos[index].PlateCategory == PlateCategory.非车牌) plateInfos.RemoveAt(index);
                }
                return plateInfos;
            }
        //先判断颜色法，查询不到再使用sobel算法（简易）
        public static List<PlateInfo> LocatePlatesForAutoSample(Mat matSource,
                                                                out Mat matProcess,
                                                                int blur_Size = 5,
                                                                int sobel_Scale = 1,
                                                                int sobel_Delta = 0,
                                                                int sobel_X_Weight = 1,
                                                                int sobel_Y_Weight = 0,
                                                                int morph_Size_Width = 17,
                                                                int morph_Size_Height = 3,
                                                                int minWidth = 60, int maxWidth = 180,
                                                                int minHeight = 18, int maxHeight = 80,
                                                                float minRatio = 0.15f, float maxRatio = 0.70f)
        {
            List<PlateInfo> plateInfos = new List<PlateInfo>();
            //优先使用颜色法定位可能是车牌的区域，如果没有发现车牌，再使用Sobel法
            Mat gray = null;
            Mat blur = null;
            if (matSource.Empty() || matSource.Rows == 0 || matSource.Cols == 0)
            {
                matProcess = new Mat(0, 0, MatType.CV_8UC1);
                return plateInfos;
            }
            //灰度图
            gray = matSource.CvtColor(ColorConversionCodes.BGR2GRAY);
            //高斯模糊图
            blur = gray.GaussianBlur(new OpenCvSharp.Size(blur_Size, blur_Size), 0, 0,BorderTypes.Default);
            //HSV图
            Mat hsv = matSource.CvtColor(ColorConversionCodes.BGR2HSV);
            //将HSV图分成H、S、V通道图
            Mat[] hsvSplits = hsv.Split();
            hsvSplits[2] = hsvSplits[2].EqualizeHist();
            Mat hsvEqualizeHist = new Mat();
            //合并通道图
            Cv2.Merge(hsvSplits, hsvEqualizeHist);

            //提取蓝色
            Scalar blueStart = new Scalar(100, 70, 70);
            Scalar blueEnd = new Scalar(140, 255, 255);
            Mat blue = hsvEqualizeHist.InRange(blueStart, blueEnd);
            //提取黄色
            Scalar yellowStart = new Scalar(15, 70, 70);
            Scalar yellowEnd = new Scalar(40, 255, 255);
            Mat yellow = hsvEqualizeHist.InRange(yellowStart, yellowEnd);

            Mat add = blue + yellow;
            //二值化
            Mat threshold = add.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect,
                              new OpenCvSharp.Size(morph_Size_Width,morph_Size_Height));

            //闭操作
            Mat threshold_Close = threshold.MorphologyEx(MorphTypes.Close, element);
            Mat element_Erode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3,
            3));
            
            Mat threshold_Erode = threshold_Close.Erode(element_Erode);
            matProcess = threshold_Erode;
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchys = null;
            Cv2.FindContours(threshold_Erode, out contours, out hierarchys,
            RetrievalModes.External, ContourApproximationModes.ApproxNone);
            int isPlateCount = 0;
            for (int index = 0; index < contours.Length; index++)
            {
                RotatedRect rotatedRect = Cv2.MinAreaRect(contours[index]);
                Rect rectROI = Cv2.BoundingRect(contours[index]);
                if (VerifyPlateSize(rectROI.Size,
                minWidth,
                maxWidth,
                minHeight,
                maxHeight,
                minRatio,
                maxRatio))
                {
                    Mat matROI = matSource.SubMat(rectROI);
                    PlateCategory plateCategory = PlateCategory_SVM.Test(matROI);
                    if (plateCategory != PlateCategory.非车牌) isPlateCount++;
                    PlateInfo plateInfo = new PlateInfo();
                    plateInfo.RotatedRect = rotatedRect;
                    plateInfo.OriginalRect = rectROI;
                    plateInfo.OriginalMat = matROI;
                    plateInfo.PlateCategory = plateCategory;
                    plateInfo.PlateLocateMethod = PlateLocateMethod.颜色法;
                    plateInfos.Add(plateInfo);
                }
            }
            if (isPlateCount > 0) return plateInfos;
            blur = matSource.GaussianBlur(new OpenCvSharp.Size(blur_Size, blur_Size), 0, 0,
                                            BorderTypes.Default);
            gray = blur.CvtColor(ColorConversionCodes.BGR2GRAY);
            // 对图像进行Sobel 运算，得到的是图像的一阶水平方向导数。
            // Generate grad_x and grad_y
            MatType ddepth = MatType.CV_16S;
            Mat grad_x = gray.Sobel(ddepth, 1, 0, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat abs_grad_x = grad_x.ConvertScaleAbs();
            Mat grad_y = gray.Sobel(ddepth, 0, 1, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat abs_grad_y = grad_y.ConvertScaleAbs();
            Mat grad = new Mat();
            Cv2.AddWeighted(abs_grad_x, sobel_X_Weight, abs_grad_y, sobel_Y_Weight, 0, grad);
            // 对图像进行二值化。将灰度图像（每个像素点有256 个取值可能）转化为二值图像（每个像素点仅有1 和0 两个取值可能）。
            threshold = grad.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            // 使用闭操作。对图像进?行行闭操作以后，可以看到车牌区域被连接成一个矩形装的区域。
            element = Cv2.GetStructuringElement(MorphShapes.Rect,
                                                new OpenCvSharp.Size(morph_Size_Width,
                                                morph_Size_Height));
            threshold_Close = threshold.MorphologyEx(MorphTypes.Close, element);
            element_Erode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
            threshold_Erode = threshold_Close.Erode(element_Erode);
            matProcess = threshold_Erode;
            // Find 轮廓 of possibles plates 求轮廓。求出图中所有的轮廓。这个算法会把全图的轮廓都计算出来，因此要进? 行行筛选。
            contours = null;
            hierarchys = null;

            Cv2.FindContours(threshold_Erode, out contours, out hierarchys,
                             RetrievalModes.External, ContourApproximationModes.ApproxNone);
            // 筛选。对轮廓求最?小外接矩形，然后验证，不不满?足条件的淘汰。
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rectROI = Cv2.BoundingRect(contours[index]);
                if (VerifyPlateSize(rectROI.Size,
                minWidth,
                maxWidth,
                minHeight,
                maxHeight,
                minRatio,
                maxRatio))
                {
                    RotatedRect rotatedRect = Cv2.MinAreaRect(contours[index]);
                    Mat matROI = matSource.SubMat(rectROI);
                    PlateCategory plateCategory = PlateCategory_SVM.Test(matROI);
                    PlateInfo plateInfo = new PlateInfo();
                    plateInfo.RotatedRect = rotatedRect;
                    plateInfo.OriginalRect = rectROI;
                    plateInfo.OriginalMat = matROI;
                    plateInfo.PlateCategory = plateCategory;
                    plateInfo.PlateLocateMethod = PlateLocateMethod.Sobel法;
                    plateInfos.Add(plateInfo);
                }
            }
            return plateInfos;
        }
        //使用颜色法先查询，查询不到再使用sobel算法
        public static List<PlateInfo> LocatePlates( Mat matSource,
                                                    int blur_Size = 5,
                                                    int sobel_Scale = 1,
                                                    int sobel_Delta = 0,
                                                    int sobel_X_Weight = 1,
                                                    int sobel_Y_Weight = 0,
                                                    int morph_Size_Width = 17,
                                                    int morph_Size_Height = 3,
                                                    int minWidth = 60, int maxWidth = 180,
                                                    int minHeight = 18, int maxHeight = 80,
                                                    float minRatio = 0.15f, float maxRatio = 0.70f)
        {
            List<PlateInfo> plateInfos = LocatePlatesByColor(matSource,
            blur_Size,
            morph_Size_Width,
            morph_Size_Height,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
            if (plateInfos.Count > 0) return plateInfos;
            plateInfos = LocatePlatesBySobel(matSource,
            blur_Size,
            sobel_Scale,
            sobel_Delta,
            sobel_X_Weight,
            sobel_Y_Weight,
            morph_Size_Width,
            morph_Size_Height,
            minWidth, maxWidth,
            minHeight, maxHeight,
            minRatio, maxRatio);
            return plateInfos;
        }
        //颜色法
        private static List<PlateInfo> LocatePlatesByColor( Mat matSource,
                                                            int blur_Size = 5,
                                                            int morph_Size_Width = 17,
                                                            int morph_Size_Height = 3,
                                                            int minWidth = 60, int maxWidth = 180,
                                                            int minHeight = 18, int maxHeight = 80,
                                                            float minRatio = 0.15f, float maxRatio = 0.70f)
        {
            List<PlateInfo> plateInfos = new List<PlateInfo>();
            if (matSource.Empty()) return plateInfos;
            Mat hsv = matSource.CvtColor(ColorConversionCodes.BGR2HSV);
            Mat[] hsvSplits = hsv.Split();
            hsvSplits[2] = hsvSplits[2].EqualizeHist();
            Mat hsvEqualizeHist = new Mat();
            Cv2.Merge(hsvSplits, hsvEqualizeHist);
            Scalar blueStart = new Scalar(100, 70, 70);
            Scalar blueEnd = new Scalar(140, 255, 255);
            Mat blue = hsvEqualizeHist.InRange(blueStart, blueEnd);
            Scalar yellowStart = new Scalar(15, 70, 70);
            Scalar yellowEnd = new Scalar(40, 255, 255);
            Mat yellow = hsvEqualizeHist.InRange(yellowStart, yellowEnd);
            Mat add = blue + yellow;
            Mat threshold = add.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect,
            new OpenCvSharp.Size(morph_Size_Width,
            morph_Size_Height));
            Mat threshold_Close = threshold.MorphologyEx(MorphTypes.Close, element);
            Mat element_Erode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3,
            3));
            Mat threshold_Erode = threshold_Close.Erode(element_Erode);
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchys = null;
            Cv2.FindContours(threshold_Erode, out contours, out hierarchys,
            RetrievalModes.External, ContourApproximationModes.ApproxNone);
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rectROI = Cv2.BoundingRect(contours[index]);
                if (VerifyPlateSize(rectROI.Size,
                minWidth,
                maxWidth,
                minHeight,
                maxHeight,
                minRatio,
                maxRatio))
                {
                    Mat matROI = matSource.SubMat(rectROI);
                    PlateCategory plateCategory = PlateCategory_SVM.Test(matROI);
                    if (plateCategory == PlateCategory.非车牌) continue;
                    PlateInfo plateInfo = new PlateInfo();
                    plateInfo.OriginalRect = rectROI;
                    plateInfo.OriginalMat = matROI;
                    plateInfo.PlateCategory = plateCategory;
                    plateInfo.PlateLocateMethod = PlateLocateMethod.颜色法;
                    plateInfos.Add(plateInfo);
                }
            }
            return plateInfos;
        }

        private static List<PlateInfo> LocatePlatesBySobel( Mat matSource,
                                                            int blur_Size = 5,
                                                            int sobel_Scale = 1,
                                                            int sobel_Delta = 0,
                                                            int sobel_X_Weight = 1,
                                                            int sobel_Y_Weight = 0,
                                                            int morph_Size_Width = 17,
                                                            int morph_Size_Height = 3,
                                                            int minWidth = 60, int maxWidth = 180,
                                                            int minHeight = 18, int maxHeight = 80,
                                                            float minRatio = 0.15f, float maxRatio = 0.70f)
        {
            List<PlateInfo> plateInfos = new List<PlateInfo>();
            if (matSource.Empty()) return plateInfos;
            Mat blur = null;
            Mat gray = null;
            blur = matSource.GaussianBlur(new OpenCvSharp.Size(blur_Size, blur_Size), 0, 0,
            BorderTypes.Default);
            gray = blur.CvtColor(ColorConversionCodes.BGR2GRAY);
            // 对图像进行Sobel 运算，得到的是图像的一阶水平方向导数。
            // Generate grad_x and grad_y
            MatType ddepth = MatType.CV_16S;
            Mat grad_x = gray.Sobel(ddepth, 1, 0, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat abs_grad_x = grad_x.ConvertScaleAbs();
            Mat grad_y = gray.Sobel(ddepth, 0, 1, 3, sobel_Scale, sobel_Delta, BorderTypes.Default);
            Mat abs_grad_y = grad_y.ConvertScaleAbs();
            Mat grad = new Mat();
            Cv2.AddWeighted(abs_grad_x, sobel_X_Weight, abs_grad_y, sobel_Y_Weight, 0, grad);
            // 对图像进行二值化。将灰度图像（每个像素点有256 个取值可能）转化为二值图像（每个像素点仅有1 和0 两个取值可能）。
            Mat threshold = grad.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            // 使用闭操作。对图像进行闭操作以后，可以看到车牌区域被连接成一个矩形装的区域。
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect,
            new OpenCvSharp.Size(morph_Size_Width,
            morph_Size_Height));
            Mat threshold_Close = threshold.MorphologyEx(MorphTypes.Close, element);
            Mat element_Erode = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5,
            5));
            Mat threshold_Erode = threshold_Close.Erode(element_Erode);
            // Find 轮廓 of possibles plates 求轮廓。求出图中所有的轮廓。这个算法会把全图的轮廓都计算出来，因此要进行筛选。
            OpenCvSharp.Point[][] contours = null;
            HierarchyIndex[] hierarchys = null;
            Cv2.FindContours(threshold_Erode, out contours, out hierarchys,
            RetrievalModes.External, ContourApproximationModes.ApproxNone);
            // 筛选。对轮廓求最小外接矩形，然后验证，不满足条件的淘汰。
            for (int index = 0; index < contours.Length; index++)
            {
                Rect rectROI = Cv2.BoundingRect(contours[index]);
                if (VerifyPlateSize(rectROI.Size,
                minWidth,
                maxWidth,
                minHeight,
                maxHeight,
                minRatio,
                maxRatio))
                {
                    Mat matROI = matSource.SubMat(rectROI);
                    PlateCategory plateCategory = PlateCategory_SVM.Test(matROI);
                    if (plateCategory == PlateCategory.非车牌) continue;
                    PlateInfo plateInfo = new PlateInfo();
                    plateInfo.OriginalRect = rectROI;
                    plateInfo.OriginalMat = matROI;
                    plateInfo.PlateCategory = plateCategory;
                    plateInfo.PlateLocateMethod = PlateLocateMethod.Sobel法;
                    plateInfos.Add(plateInfo);
                }
            }
            return plateInfos;
        }






    }
}
