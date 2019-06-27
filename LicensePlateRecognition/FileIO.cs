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

        //生成字符文件目录
        public static List<CharCategorySVM.SVMFileInfo> PrepareTrainningCharDirectory(string path)
        {
            List<CharCategorySVM.SVMFileInfo> fileInfos = new List<CharCategorySVM.SVMFileInfo>();

            try
            {
                string charsDirectory = path + @"\chars";
                if (Directory.Exists(charsDirectory) == false)
                    Directory.CreateDirectory(charsDirectory);   //不存在就创建

                string[] plateCharNames = Enum.GetNames(typeof(PlateChar)); //获取所有枚举类型名

                for (int index_PlateChar = 0; index_PlateChar < plateCharNames.Length; index_PlateChar++)
                {
                    CharCategorySVM.SVMFileInfo fileInfo = new CharCategorySVM.SVMFileInfo();

                    string plateChar = plateCharNames[index_PlateChar];
                    string plateCharDirectory = charsDirectory + @"\" + plateChar;

                    fileInfo.FilePath = plateCharDirectory;
                    fileInfo.Label = index_PlateChar;   //希望是按照枚举顺序来的
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
        //生成车牌文件目录，返回标签信息
        public static List<PlateCategorySVM.SVMFileInfo> PrepareTrainningPlateDirectory(string path)
        {
            List<PlateCategorySVM.SVMFileInfo> fileInfos = new List<PlateCategorySVM.SVMFileInfo>();

            try
            {
                string charsDirectory = path + @"\plates";  //车牌目录

                if (Directory.Exists(charsDirectory) == false)
                    Directory.CreateDirectory(charsDirectory);   //不存在就创建

                string[] plateCategory = Enum.GetNames(typeof(PlateCategory)); //获取所有枚举类型名

                for (int index_Plate = 0; index_Plate < plateCategory.Length; index_Plate++)
                {
                    PlateCategorySVM.SVMFileInfo fileInfo = new PlateCategorySVM.SVMFileInfo();
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
