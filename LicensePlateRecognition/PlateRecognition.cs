using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;

namespace LicensePlateRecognition
{
    class PlateRecognition
    {

        //车牌识别
        public static string PlateRecognite(Mat matIn)
        {
            string plate=null;

            if (!PlateCategorySVM.IsReady)
                return "车牌识别库没有准备好";

            if (!CharCategorySVM.IsReady)
                return "字符识别库没有准备好";


            //获取车牌
            List<Mat> roiPlates = PlateLocator.PlateLocateByColor(matIn); //疑似区域
            List<Mat> matPlates = new List<Mat>(); //车牌区域
            for(int index = 0; index < roiPlates.Count; index++)
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
                return "无识别出车牌";

            //下面根据识别出的车牌进行字符识别
            for(int index = 0; index < matPlates.Count; index++)
            {
                Mat mat = matPlates[index];
                List<Mat> roiChars = new List<Mat>();
                roiChars = CharSegement.SplitePlateByOriginal(mat);

                if (roiChars.Count == 0)
                    return "无识别字符";

                for (int i=0;i<roiChars.Count;i++)
                {
                    plate = plate + CharCategorySVM.Test(roiChars[i]).ToString();
                }

            }

            plate = plate.Replace("_", "");
            plate = plate.Replace("非字符", "");

            return plate;
        }
        










    }
}
