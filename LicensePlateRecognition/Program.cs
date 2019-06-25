using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensePlateRecognition
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new TrainForm());
            //Application.Run(new MyWindow());
            //Application.Run(new UIResource());
            //Application.Run(new progressForm());
        }


    }
}
