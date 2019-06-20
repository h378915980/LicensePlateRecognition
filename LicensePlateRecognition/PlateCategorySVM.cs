using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.ML;

namespace LicensePlateRecognition
{
    class PlateCategorySVM
    {
        public static bool IsReady = false;

        public static OpenCvSharp.Size HOGWinSize = new Size(96,32);
        public static OpenCvSharp.Size HOGBlockSize = new Size(16,16);
        public static OpenCvSharp.Size HOGBlockStride = new Size(8,8);
        public static OpenCvSharp.Size HOGCellSize = new Size(8,8);
        public static int HOGNBits = 9;

        private static SVM svm = null;


        //HOGDescriptor
        public static float[] ComputeHogDescriptors(Mat image)
        {
            Mat matToHog = image.Resize(HOGWinSize);
            //初始化一个自定义的hog描述子
            HOGDescriptor hog = new HOGDescriptor(HOGWinSize,HOGBlockSize,HOGBlockStride,HOGCellSize,HOGNBits);

            return hog.Compute(matToHog,new OpenCvSharp.Size(1,1),new OpenCvSharp.Size(0,0));
        }


        




        //train
        public static bool Train(Mat samples,Mat responses)
        {
            svm = SVM.Create();
            svm.Type = SVM.Types.CSvc;
            svm.KernelType = SVM.KernelTypes.Linear;
            svm.TermCriteria = new TermCriteria(CriteriaType.MaxIter,1000,1e-10);
            
            IsReady = true;
            return svm.Train(samples,SampleTypes.RowSample,responses);
        }

        //加载svm模型
        public static void Load(string fileName)
        {
            try
            {
                svm = SVM.Load(fileName);
                IsReady = true;
            }
            catch (Exception)
            {
                throw new Exception("ERROR：请检查路径是否正确");
            }

        }
        //保存
        public static void Save(string ﬁleName)
        {
            if (IsReady == false || svm == null)
                return;
            svm.Save(ﬁleName);
        }



    }
}
