using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    class Utilities
    {
        //拉普拉斯变化
        public static Mat LaplaceTransform(Mat matIn)
        {
            Mat kernel = new Mat(3, 3, MatType.CV_32FC1);
            kernel.Set<float>(0, 0, 0);
            kernel.Set<float>(0, 1, -1);
            kernel.Set<float>(0, 2, 0);
            kernel.Set<float>(1, 0, 0);
            kernel.Set<float>(1, 1, 5);
            kernel.Set<float>(1, 2, 0);
            kernel.Set<float>(2, 0, 0);
            kernel.Set<float>(2, 1, -1);
            kernel.Set<float>(2, 2, 0);

            Mat result = matIn.Filter2D(MatType.CV_8UC3, kernel);

            return result;
        }
        //gamma变化
        public static Mat GammaTransform(Mat source,float gammaFactor=0.4f)
        {
            int[] lut = new int[256];
            for (int index = 0; index < 256; index++)
            {
                float f = (index + 0.5f) / 255;
                f = (float)Math.Pow(f, gammaFactor);
                lut[index] = (int)(f * 255.0f - 0.5f);
                if (lut[index] > 255) lut[index] = 255;
            }

            Mat result = source.Clone();
            if (source.Channels() == 1)
            {
                for (int rowIndex = 0; rowIndex < result.Rows; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < result.Cols; colIndex++)
                    {
                        int temp = result.At<int>(rowIndex, colIndex);
                        result.Set<int>(rowIndex, colIndex, lut[temp]);
                    }
                }
            }
            else
            {
                for (int rowIndex = 0; rowIndex < result.Rows; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < result.Cols; colIndex++)
                    {
                        Vec3b temp = result.At<Vec3b>(rowIndex, colIndex);
                        result.Set<Vec3b>(rowIndex, colIndex, new Vec3b(
                         (byte)lut[temp[0]],
                         (byte)lut[temp[1]],
                         (byte)lut[temp[2]]));
                    }
                }
            }

            return result;

        }

        //指数变换
        public static Mat IndexTransform(Mat source)
        {
            Mat result = new Mat(source.Size(), source.Type());
            int rows = source.Rows;
            int cols = source.Cols;

            double k = 1 / 255f;
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < cols; colIndex++)
                {
                    Vec3b color = source.At<Vec3b>(rowIndex, colIndex);
                    byte B = color[0];
                    byte G = color[1];
                    byte R = color[2];

                    B = (byte)(k * B * B);
                    G = (byte)(k * G * G);
                    R = (byte)(k * R * R);
                    color = new Vec3b(B, G, R);
                    result.Set<Vec3b>(rowIndex, colIndex, color);
                }
            }
            return result;
        }

        //对数变换
        public static Mat LogTransform(Mat source)
        {
            Mat result = new Mat(source.Size(), source.Type());
            int rows = source.Rows;
            int cols = source.Cols;

            double k = 255 / Math.Log10(256.0);
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < cols; colIndex++)
                {
                    Vec3b color = source.At<Vec3b>(rowIndex, colIndex);
                    byte B = color[0];
                    byte G = color[1];
                    byte R = color[2];

                    B = (byte)(k * Math.Log10(B + 1));
                    G = (byte)(k * Math.Log10(G + 1));
                    R = (byte)(k * Math.Log10(R + 1));
                    color = new Vec3b(B, G, R);
                    result.Set<Vec3b>(rowIndex, colIndex, color);
                }
            }

            return result;
        }



    }
}
