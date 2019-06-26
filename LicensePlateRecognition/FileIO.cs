using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LicensePlateRecognition.PlateCategorySVM;

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

        //生成字符文件目录
        public static bool PrepareTrainningCharDirectory(string path)
        {
            bool success = true;
            try
            {
                success = Directory.Exists(path);

                string charsDirectory = path + @"\chars";
                if (Directory.Exists(charsDirectory) == false)
                    Directory.CreateDirectory(charsDirectory);   //不存在就创建

                string[] plateCharNames = Enum.GetNames(typeof(PlateChar)); //获取所有枚举类型名

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
        //生成车牌文件目录，返回标签信息
        public static List<SVMFileInfo> PrepareTrainningPlateDirectory(string path)
        {
            List<SVMFileInfo> fileInfos = new List<SVMFileInfo>();

            try
            {
                string charsDirectory = path + @"\plates";  //车牌目录

                if (Directory.Exists(charsDirectory) == false)
                    Directory.CreateDirectory(charsDirectory);   //不存在就创建

                string[] plateCategory = Enum.GetNames(typeof(PlateCategory)); //获取所有枚举类型名

                for (int index_Plate = 0; index_Plate < plateCategory.Length; index_Plate++)
                {
                    SVMFileInfo fileInfo = new SVMFileInfo();
                    string plateChar = plateCategory[index_Plate];
                    string plateCharDirectory = charsDirectory + @"\" + plateChar;

                    fileInfo.FilePath = plateCharDirectory;
                    fileInfo.Label = index_Plate;   //希望是按照枚举顺序来的

                    fileInfos.Add(fileInfo);
                    if (Directory.Exists(plateCharDirectory) == false)
                        Directory.CreateDirectory(plateCharDirectory);
                }

            }
            catch (IOException)
            {
                throw new Exception("路径错误");
            }

            return fileInfos;
        }








    }
}
