
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//用于各种类型的互相转化
namespace LicensePlateRecognition
{
    class TypeConvert
    {


        public static Mat Float2Mat(float[] f)
        {
            Mat matResult = Mat.Zeros(1, f.Length, MatType.CV_32FC1);
            for(int i=0;i<f.Length;i++)
            {
                matResult.Set<float>(0, i, f[i]);
            }

            return matResult;
        }

        public static Mat Int2Mat(int i)
        {
            Mat matResult = Mat.Zeros(1, 1, MatType.CV_32SC1);
            matResult.Set<int>(0, 0, i);
            return matResult;
        }


    }
}
