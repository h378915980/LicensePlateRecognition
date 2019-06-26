using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;

namespace LicensePlateRecognition
{
    class DataPreparingForSVM
    {
        //获得当前路径下的所有文件
        public static string[] getImgFiles(string path)
        {
            string[] dirs = Directory.GetFiles(path, "*.JPG");
            return dirs;
        }
        //工具函数，用于将list转为array
        static List<string> StringList2Array(string[] strs)
        {
            List<string> arrayStr = new List<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                arrayStr.Add(strs[i]);
            }
            return arrayStr;
        }
        //得到样本图片文件
        public static List<string> getSampleFiles(string path)
        {
            string[] imgFiles = getImgFiles(path);
            return StringList2Array(imgFiles);
        }
        //定义样本结构体，用于给图片打上标签
        private struct TrainStruct
        {
            public string file;//正负样本路径
            public int label;
        }

        //得到图片的HOG特征
       
        public static Mat GetCharSvmHOGFeatures(Mat img)
        {
            float[] descriptor = PlateChar_SVM.ComputeHogDescriptors(img);
            Mat feature = Float2Mat(descriptor);
            return feature;
        }

        
        //对字符进行训练
        public static void TrainSVMDataForCharRecog(string path,string savePath)
        {
            Console.WriteLine("preparing for training data!!!!");
            List<TrainStruct> svmData = new List<TrainStruct>();
            List<List<string>> ImgFiles = new List<List<string>>();
            //for(int index=0;index<10;index++)
            //{
            //    string label = "_" + index.ToString();
            //    string filePath = @"C:\Users\faiz\Desktop\AI\车牌-字符样本\车牌-字符样本\chars\" + label;
            //    Console.WriteLine("{0}", filePath);
            //    List<string> files = getSampleFiles(filePath);
            //    ImgFiles.Add(files);
            //}
            for (int index = (int)PlateChar.A; index <= (int)PlateChar._9; index++)
            {
                if (index >= 28 && index <= 37)
                {
                    string labelForNumber = "_" + (index - 28).ToString();
                    string filePath = path + labelForNumber;//@"C:\Users\faiz\Desktop\AI\车牌-字符样本\车牌-字符样本\chars\"
                    Console.WriteLine("{0}", filePath);
                    List<string> files = getSampleFiles(filePath);
                    ImgFiles.Add(files);
                }
                else
                {
                    PlateChar plateChar = (PlateChar)index;
                    string labelForWord = plateChar.ToString();
                    string filePath = path + labelForWord;//@"C:\Users\faiz\Desktop\AI\车牌-字符样本\车牌-字符样本\chars\"
                    Console.WriteLine("{0}", filePath);
                    List<string> files = getSampleFiles(filePath);
                    ImgFiles.Add(files);
                }
            }
            //现在主要是要识别广东车牌
            List<string> exFiles = getSampleFiles(path + PlateChar.粤.ToString());
            ImgFiles.Add(exFiles);

            for (int index1 = 0; index1 < ImgFiles.Count - 1; index1++)
            {
                for (int index2 = 0; index2 < ImgFiles.ElementAt(index1).Count; index2++)
                {
                    TrainStruct trainData;
                    trainData.file = ImgFiles.ElementAt(index1).ElementAt(index2);
                    trainData.label = index1 + 2;
                    svmData.Add(trainData);
                }
            }
            //最后再添加东的数据
            for (int index = 0; index < ImgFiles.ElementAt(ImgFiles.Count - 1).Count; index++)
            {
                TrainStruct trainData;
                trainData.file = ImgFiles.ElementAt(ImgFiles.Count - 1).ElementAt(index);
                trainData.label = (int)PlateChar.粤;
                svmData.Add(trainData);
            }

            Mat samples = new Mat();
            Mat responses = new Mat();
            //读取数据并进行处理
            for (int index = 0; index < svmData.Count; index++)
            {
                //以灰度的方式读取图片
                Mat img = Cv2.ImRead(svmData[index].file, ImreadModes.Grayscale);
                //剔除无用数据
                if (img.Data == null)
                {
                    Console.WriteLine("failed to load image {0}", svmData[index].file);
                }
                //对图片进行二值化
                Mat dst = new Mat();
                Cv2.Threshold(img, dst, 1, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                Mat feature = GetCharSvmHOGFeatures(dst);
                //获取HOG特征
                feature = feature.Reshape(1, 1);
                samples.PushBack(feature);
                responses.PushBack(Int2Mat(svmData[index].label));
            }
            // 训练数据的格式，OpenCV规定 samples 中的数据都是需要32位浮点型
            // 因为TrainData::create 第一个参数是规定死的要cv_32F
            samples.ConvertTo(samples, MatType.CV_32F);
            // samples 将图片和样本标签合并成为一个训练集数据
            // 第二个参数的原因是，我们的samples 中的每个图片数据的排列都是一行
            if (PlateChar_SVM.Train(samples, responses))
            {
                Console.WriteLine("Trained success!!!");
                PlateChar_SVM.Save(savePath+@"\charRecog.xml");
                Console.WriteLine("\".xml\"has been written!!1");
            }
            else
            {
                Console.WriteLine("failed to train data");

            }


        }


        //float 数组转为mat
        public static Mat Float2Mat(float[] list)
        {
            Mat resultMat = Mat.Zeros(1, list.Length, MatType.CV_32FC1);
            for (int index = 0; index < list.Length; index++)
            {
                resultMat.Set<float>(0, index, list[index]);
            }
            return resultMat;
        }

        public static Mat Int2Mat(int integer)
        {
            Mat resultMat = Mat.Zeros(1, 1, MatType.CV_32SC1);

            resultMat.Set<int>(0, 0, integer);
            return resultMat;
        }
    }
}
