using System;
using System.Collections.Generic;
using System.IO;
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

        //生成文件目录
        public static bool PrepareTrainningDirectory(string path)
        {
            bool success = true;
            try
            {
                success = Directory.Exists(path);

                string charsDirectory = path + @"\chars";
                if (Directory.Exists(charsDirectory) == false)
                    Directory.CreateDirectory(charsDirectory);

                string[] plateCharNames = Enum.GetNames(typeof(PlateChar));

                for (int index_PlateChar = 0; index_PlateChar < plateCharNames.Length; index_PlateChar++)

                {
                    string plateChar = plateCharNames[index_PlateChar];

                    string plateCharDirectory = charsDirectory + @"\" + plateChar;
                    if (Directory.Exists(plateCharDirectory) == false)
                        Directory.CreateDirectory(plateCharDirectory);
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
