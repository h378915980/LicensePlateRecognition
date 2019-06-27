using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.ML;


namespace LicensePlateRecognition
{
    class CharCategorySVM
    {
        public static bool IsReady = false;

        public static OpenCvSharp.Size HOGWinSize = new Size(16, 32);
        public static OpenCvSharp.Size HOGBlockSize = new Size(16, 16);
        public static OpenCvSharp.Size HOGBlockStride = new Size(8, 8);
        public static OpenCvSharp.Size HOGCellSize = new Size(8, 8);
        public static int HOGNBits = 9;

        private static SVM svm = null;
        public struct SVMFileInfo
        {
            public string FilePath;//样本路径
            public int Label;      //样本标识

            public SVMFileInfo(string fp, int lab)
            {
                FilePath = fp;
                Label = lab;
            }
        }


        //HOGDescriptor
        public static float[] ComputeHogDescriptors(Mat image)
        {
            Mat matToHog = image.Resize(HOGWinSize);
            //初始化一个自定义的hog描述子
            HOGDescriptor hog = new HOGDescriptor(HOGWinSize, HOGBlockSize, HOGBlockStride, HOGCellSize, HOGNBits);

            return hog.Compute(matToHog, new OpenCvSharp.Size(1, 1), new OpenCvSharp.Size(0, 0));
        }



        //分类文件与标签，参数为文件列表与标签结构体
        public static bool Train(List<SVMFileInfo> file)
        {
            svm = SVM.Create();
            svm.Type = SVM.Types.CSvc;
            svm.KernelType = SVM.KernelTypes.Linear;
            svm.TermCriteria = new TermCriteria(CriteriaType.MaxIter, 1000, 1e-10);

            CharCategorySVM.IsReady = true;

            Mat samples = new Mat();  //特征矩阵

            Mat responses = new Mat(); //标签矩阵

            foreach (SVMFileInfo fi in file)
            {
                List<string> fileNames = FileIO.OpenFile(fi.FilePath); //读到需要的文件路径列表

                if (fileNames == null || fileNames.Count <= 0)  //如果文件夹中一个图片都没有就返回
                    return false;

                //处理每个文件
                foreach (string s in fileNames)
                {
                    Mat matImg = new Mat(s, ImreadModes.Grayscale);  //get image

                    Cv2.Threshold(matImg, matImg, 0, 255, ThresholdTypes.Otsu); //二值化

                    float[] feature = ComputeHogDescriptors(matImg);  //提取图片HOG特征

                    samples.PushBack(TypeConvert.Float2Mat(feature));//向特征矩阵中添加一行特征向量
                    responses.PushBack(TypeConvert.Int2Mat(fi.Label)); //向标签矩阵中添加一行标签

                }
            }

            samples.ConvertTo(samples, MatType.CV_32FC1); //训练数据的格式必须是32位浮点型

            if (svm.Train(samples, SampleTypes.RowSample, responses))
            {
                return true;
            }

            return false;
        }

        //保存
        public static void Save(string ﬁleName)
        {
            if (IsReady == false || svm == null)
                return;
            svm.Save(ﬁleName + @"\charSVM.xml");
        }

        //测试
        public static PlateChar Test(Mat matTest)
        {
            try
            {
                if (IsReady == false || svm == null)
                {
                    throw new Exception("训练数据为空，请重新训练字符类型识别或加载数据");
                }

                PlateChar result = PlateChar.非字符;

                if (IsReady == false || svm == null) return result;

                float[] descriptor = ComputeHogDescriptors(matTest);

                Mat testDescriptor = TypeConvert.Float2Mat(descriptor);

                float predict = svm.Predict(testDescriptor);

                result = (PlateChar)((int)predict);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







    }
}
