using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;

namespace LicensePlateRecognition
{
    class PlateCategory_SVM
    {
        public static bool IsReady = false;
        public static OpenCvSharp.Size HOGWinSize = new OpenCvSharp.Size(96, 32);
        public static OpenCvSharp.Size HOGBlockSize = new OpenCvSharp.Size(16, 16);
        public static OpenCvSharp.Size HOGBlockStride = new OpenCvSharp.Size(8, 8);
        public static OpenCvSharp.Size HOGCellSize = new OpenCvSharp.Size(8, 8);
        public static int HOGNBits = 9;
        private static SVM svm = null;
        private static Random random = new Random();

        static PlateCategory_SVM()
        {

        }
        //保存图片的方式
        public static void SavePlateSample((PlateInfo plateInfo, string fileName)
        {
            plateInfo.OriginalMat.SaveImage(fileName);
        }

        public static void SavePlateSample(Mat matPlate, PlateCategory plateCategory, string libPath)
        {
            DateTime now = DateTime.Now;
            string name = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}-{6:000000}",
            now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second,
            random.Next(100000));
            string fileName = string.Format(@"{0}\plates\{1}\{2}.jpg", libPath, plateCategory, name);
            matPlate.SaveImage(fileName);
        }

        public static void SavePlateSample(Mat matPlate, PlateCategory plateCategory, string libPath, string shortFileNameNoExt)
        {
            DateTime now = DateTime.Now;
            string fileName = string.Format(@"{0}\plates\{1}\{2}.jpg", libPath, plateCategory,
            shortFileNameNoExt);
            matPlate.SaveImage(fileName);
        }

        //HOG特征提取
        public static float[] ComputeHogDescriptors(Mat image)
        {
            Mat matToHog = image.Resize(HOGWinSize);
            /*  第一个参数：窗口大小          第二个参数：块大小
             *  第三个参数：块的滑动步长      第四个参数：细胞大小
             *  第五个参数：分箱个数n，即每次转动360/n度
             */
            HOGDescriptor hog = new HOGDescriptor(HOGWinSize,
                                                  HOGBlockSize,
                                                  HOGBlockStride,
                                                  HOGCellSize,
                                                  HOGNBits);

            return hog.Compute(matToHog, new OpenCvSharp.Size(1, 1), new OpenCvSharp.Size(0, 0));
        }

        //SVM训练
        public static bool Train(Mat samples, Mat responses)
        {
            svm = SVM.Create();
            svm.Type = SVM.Types.CSvc;
            svm.KernelType = SVM.KernelTypes.Linear;
            svm.TermCriteria = new TermCriteria(CriteriaType.MaxIter, 10000, 1e-10);
            IsReady = true;
            return svm.Train(samples, SampleTypes.RowSample, responses);
        }

        //SVM保存
        public static void Save(string fileName)
        {
            if (IsReady == false || svm == null) return;
            svm.Save(fileName);
        }
        //导入SVM
        public static void Load(string fileName)
        {
            try
            {
                svm = SVM.Load(fileName);
                IsReady = true;
            }
            catch (Exception)
            {
                throw new Exception("字符识别库加载异常，请检查存放路路径");
            }
        }
        //判断是否是正确的培训目录
        public static bool IsCorrectTrainngDirectory(string path)
        {
            bool isCorrect = true;
            string[] plateCategoryNames = Enum.GetNames(typeof(plateCategory));
            for (int index = 0; index < plateCategoryNames.Length; index++)
            {
                string plateCategoryName = plateCategoryNames[index];
                string platePath = string.Format(@"{0}\{1}", path, plateCategoryName);
                if (Directory.Exists(platePath) == false)
                {
                    isCorrect = false;
                    break;
                }
            }
            return isCorrect;
        }
        //测试集
        public static PlateCategory Test(Mat matTest)
        {
            try
            {
                if (IsReady == false || svm == null)
                {
                    throw new Exception("训练数据为空，请重新训练?车牌类型识别或加载数据");
                }
                PlateCategory result = PlateCategory.? 非 ? 车牌;
                if (IsReady == false || svm == null) return result;
                float[] descriptor = ComputeHogDescriptors(matTest);
                Mat testDescriptor = Mat.Zeros(1, descriptor.Length, MatType.CV_32FC1);
                for (int index = 0; index < descriptor.Length; index++)
                {
                    testDescriptor.Set<float>(0, index, descriptor[index]);
                }
                float predict = svm.Predict(testDescriptor);
                result = (PlateCategory)((int)predict);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PlateCategory Test(string fileName)
        {
            Mat matTest = Cv2.ImRead(fileName, ImreadModes.Grayscale);
            return Test(matTest);
        }

        public static bool PreparePlateTrainningDirectory(string path)
        {
            bool success = true;
            try
            {
                string platesDirectory = path + @"\plates";
                if (Directory.Exists(platesDirectory) == false) Directory.CreateDirectory(platesDirectory);
                string[] plateCategoryNames = Enum.GetNames(typeof(PlateCategory));
                for (int index_PlateCategory = 0; index_PlateCategory < plateCategoryNames.Length;
                index_PlateCategory++)
                {
                    string plateCategoryDirectory = string.Format(@"{0}\{1}", platesDirectory,
                    plateCategoryNames[index_PlateCategory]);
                    if (Directory.Exists(plateCategoryDirectory) == false)
                        Directory.CreateDirectory(plateCategoryDirectory);
                }
            }
            catch (IOException)
            {
                success = false;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }


    }
}
