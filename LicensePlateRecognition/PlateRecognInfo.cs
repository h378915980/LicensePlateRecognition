using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{
    //车牌类型
    public enum PlateCategory
    {
        非车牌=0,
        车牌=1
    }
    //车牌颜色
    public enum PlateColor
    {
        UNKNOW=0,
        BLUE=1,
        YELLOW=2,

    }
    //车牌字符
    public enum PlateChar
    {
		非字符 = 0,
        穗 = 1,
        A = 2,
        B = 3,
        C = 4,
        D = 5,
        E = 6,
        F = 7,
        G = 8,
        H = 9,
        I = 10,
        J = 11,
        K = 12,
        L = 13,
        M = 14,
        N = 15,
        O = 16,
        P = 17,
        Q = 18,
        R = 19,
        S = 20,
        T = 21,
        U = 22,
        V = 23,
        W = 24,
        X = 25,
        Y = 26,
        Z = 27,
        _0 = 28,
        _1 = 29,
        _2 = 30,
        _3 = 31,
        _4 = 32,
        _5 = 33,
        _6 = 34,
        _7 = 35,
        _8 = 36,
        _9 = 37,
        粤 = 38,
  //      京 = 39,
  //      津 = 40,
  //      沪 = 41,
  //      渝 = 42,
  //      蒙 = 43,
  //      新 = 44,
  //      藏 = 45,
  //      宁 = 46,
  //      桂 = 47,
  //      港 = 48,
  //      澳 = 49,
  //      黑 = 50,
  //      吉 = 51,
  //      辽 = 52,
  //      晋 = 53,
  //      冀 = 54,
		//青 = 55,
  //      鲁 = 56,
  //      豫 = 57,
  //      苏 = 58,
  //      皖 = 59,
  //      浙 = 60,
  //      闽 = 61,
  //      赣 = 62,
  //      湘 = 63,
  //      鄂 = 64,
  //      点 = 65,
  //      琼 = 66,
		//甘 = 67,
  //      陕 = 68,
  //      贵 = 69,
  //      云 = 70,
  //      川 = 71,
  //      警 = 72
    }

    //车牌定位法
    public enum PlateLocateMethod
    {
        未知 = 0,
        颜色法_蓝黑 = 1,
        颜色法_黄白 = 2,
        颜色法 = 3,
        Sobel法 = 4,
        MSER法 = 5
    }
    //车牌切割法
    public enum CharSplitMethod
    {
        UNKNOW = 0,
        原图 = 1,
        伽马 = 2,
        指数 = 3,
        对数 = 4
    }
    //车牌图片属性结构体
    public struct PlateImage
    {
        public string FileName;
        public string Name;

        public PlateCategory PlateCategory;

        public OpenCvSharp.Size MatSize;
        public PlateImage(string ﬁleName, string name, PlateCategory plateCategory, OpenCvSharp.Size matSize)
        {
            this.FileName = ﬁleName;
            this.Name = name;

            this.PlateCategory = plateCategory;
            this.MatSize = matSize;
        }
    }

    //单个字符信息
    public class CharInfo
    {
        public PlateChar PlateChar;  //字符
        public Mat OriginalMat; //字符的原始图片
        public Rect OriginalRect; //字符的位置
        public PlateLocateMethod PlateLocateMethod;
        public CharSplitMethod CharSplitMethod;
        public string Info
        {
            get
            {
                return string.Format("字符:{0} \r\n宽:{1} \r\n⾼:{2} \r\n宽⾼⽐:{3:0.00} \r\n 左:{4} \r\n右:{5} \r\n上:{6}\r\n下:{7} \r\n⻋牌定位: {8} \r\n字符切分: {9} \r\n",
                    this.PlateChar,
                    this.OriginalRect.Width,
                    this.OriginalRect.Height,
                    (float)this.OriginalRect.Width / this.OriginalRect.Height,
                    this.OriginalRect.Left,
                    this.OriginalRect.Right,
                    this.OriginalRect.Top,
                    this.OriginalRect.Bottom,
                    this.PlateLocateMethod,
                    this.CharSplitMethod);
            }
        }

        public CharInfo() { }

        public CharInfo(PlateChar plateChar, Mat originalMat, Rect originalRect, PlateLocateMethod plateLocateMethod, CharSplitMethod charSplitMethod)
        {
            this.PlateChar = plateChar;
            this.OriginalMat = originalMat;
            this.OriginalRect = originalRect;
            this.PlateLocateMethod = plateLocateMethod;
            this.CharSplitMethod = charSplitMethod;
        }
        //返回字符值
        public override string ToString()
        {
            return this.PlateChar.ToString().Replace("_", ""); ;
        }
    }


    //车牌信息
    public class PlateInfo
    {

        public PlateCategory PlateCategory;
        public PlateColor PlateColor = PlateColor.UNKNOW;
        public PlateLocateMethod PlateLocateMethod;

        public RotatedRect RotatedRect;
        public Rect OriginalRect;
        public Mat OriginalMat;


        public List<CharInfo> CharInfos; //车牌上的所有字符图片

        public string Info
        {
            get
            {
                return string.Format("类型:{0} \r\n颜⾊:{1} \r\n字符:{2} \r\n宽:{3} \r\n⾼:{4} \r\n宽⾼⽐:{5:0.00} \r\n 左:{6} \r\n右: {7} \r\n上: {8} \r\n下: {9} \r\n⻋牌定位: {10}\r\n",
                    this.PlateCategory,
                    this.PlateColor,
                    this.ToString(),
                    this.OriginalRect.Width,
                    this.OriginalRect.Height,
                    (float)this.OriginalRect.Width / this.OriginalRect.Height,
                    this.OriginalRect.Left,
                    this.OriginalRect.Right,
                    this.OriginalRect.Top,
                    this.OriginalRect.Bottom,
                    this.PlateLocateMethod);
            }

        }

        public PlateInfo() { }
        public PlateInfo(PlateCategory plateCategory,
            Rect originalRect, 
            Mat originalMat,
            List<CharInfo> charInfos,
            PlateLocateMethod plateLocateMethod)
        {
            this.PlateCategory = plateCategory;
            this.OriginalRect = originalRect;
            this.OriginalMat = originalMat;
            this.CharInfos = charInfos;
            this.PlateLocateMethod = plateLocateMethod;
        }

        public override string ToString()
        {
            if (this.CharInfos == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(this.PlateCategory.ToString());
            //stringBuilder.Append(" ");
            foreach (CharInfo charInfo in this.CharInfos)
            {
                stringBuilder.Append(charInfo.ToString());
            }
            string result = stringBuilder.ToString();
            result = result.Replace("非字符", "");
            return result;
        }

    }





}
