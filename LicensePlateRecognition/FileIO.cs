using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicensePlateRecognition
{
    class FileIO
    {
        //待定：返回指定类型的文件列表，这里只能返回图片
        public static List<string> OpenFile(string fileFolder)
        {
            string[] files = System.IO.Directory.GetFiles(fileFolder);
            List<string> useFiles=new List<string>();
            foreach(string f in files)
            {
                if (
                    f.EndsWith(".jpg") ||
                    f.EndsWith(".png") ||
                    f.EndsWith(".bmp"))
                {

                   useFiles.Add(f);
                }
            }
            return useFiles;
        }

        //









    }
}
