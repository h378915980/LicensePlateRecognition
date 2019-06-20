using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LicensePlateRecognition
{

    public enum PlateCategory
    {
        UNKNOW=0,
        NORMALPLATE=1
    }

    public enum PlateColor
    {
        UNKNOW=0,
        BLUE=1,
        YELLOW=2,

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

    








}
